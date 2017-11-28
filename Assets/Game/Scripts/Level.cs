using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data;
using Game.Enums;
using UnityEngine;
using Game.Helpers;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game
{
	/// <summary>
	/// Enemy waves, Level zones, statistics, core events.
	/// </summary>
	public sealed class Level : MonoBehaviour
	{
		private const float LEVEL_END_DELAY = 5;
		private const float SHOW_GREETING_SECONDS = 1.5f;
		private const float SHOW_WAVE_SECONDS = 1.5f;
		private const float FIREWORKS_INTERVAL = 0.1f;

		[SerializeField] private PlayerCharacter _playerCharacterA;
		[SerializeField] private PlayerCharacter _playerCharacterB;
		private PlayerCharacter PlayerCharacter { get { return SaveController.Get<int>(Save.CharacterIndex) == 0 ? _playerCharacterA : _playerCharacterB; } }
		[SerializeField] private GameObject _fireworksEffect;

		[Header("Messages")]
		[SerializeField] private UiMessage _uiMessage;
		private UiMessage _message;
		[SerializeField] private string _greetingText;
		[SerializeField] private string _levelCompletedText;
		[SerializeField] private string _playerDefeatedText;

		[Header("Zones")]
		[SerializeField] private Collider _enemyZone;
		[SerializeField] private Collider _playerZone;

		[Header("Zone Y")]
		[SerializeField] private float _airY;
		[SerializeField] private float _groundY;
		public float GroundY { get { return _groundY; } }

		[SerializeField] private List<EnemyWave> _enemyWaves;
		private EnemyWave _currentEnemyWave;
		private int _currentWaveNumber;
		private bool _loopWaves;
		private bool _allWavesCleared;
		private int _enemiesKilled;

		private bool _timerEnabled;
		private float _levelStartTime;
		public float TimeElapsed { get; private set; }
		
		public int Score { get; private set; }

		#region Events
		private readonly UnityEventFloat _timeChangedEvent = new UnityEventFloat();
		public UnityEventFloat TimeChangedEvent { get { return _timeChangedEvent; } }

		private readonly UnityEvent _levelStartEvent = new UnityEvent();
		public UnityEvent LevelStartEvent { get { return _levelStartEvent; } }

		private readonly UnityEvent _levelCompletedEvent = new UnityEvent();
		public UnityEvent LevelCompletedEvent { get { return _levelCompletedEvent; } }

		private readonly UnityEventActor _enemyDeathEvent = new UnityEventActor();
		public UnityEventActor EnemyDeathEvent { get { return _enemyDeathEvent; } }

		private readonly UnityEventInt _scoreChangedEvent = new UnityEventInt();
		public UnityEventInt ScoreChangedEvent { get { return _scoreChangedEvent; } }
		#endregion

		private void Awake()
		{
			Asserts();

			_loopWaves = SaveController.Get<int>(Save.InfiniteWaves).ToBool();
			_message = Instantiate(_uiMessage);
			SpawnPlayer();
			LevelStartEvent.AddListener(OnLevelStart);
			LevelCompletedEvent.AddListener(OnLevelCompleted);
		}

		private void Asserts()
		{
			Debug.Assert(_enemyZone);
			Debug.Assert(_playerZone);
			Debug.Assert(_uiMessage);
			Debug.Assert(_fireworksEffect);

			int? prevWaveIndex = null;

			foreach (var waveIndex in _enemyWaves.Select(x => x.WaveIndex).OrderBy(x => x))
			{
				Debug.Assert(waveIndex > 0);

				if (prevWaveIndex.HasValue)
					Debug.Assert(waveIndex - prevWaveIndex <= 1);
				else
					Debug.Assert(waveIndex == 1);

				prevWaveIndex = waveIndex;
			}
		}

		private void FixedUpdate()
		{
			if (Input.GetButtonDown(AxisNames.Cancel))
				SceneManager.LoadScene(Scenes.MainMenu);
			
			if (!_timerEnabled || Time.timeSinceLevelLoad - _levelStartTime - TimeElapsed < 1)
				return;

			TimeElapsed = Time.timeSinceLevelLoad - _levelStartTime;
			TimeChangedEvent.Invoke(TimeElapsed);
		}

		private void Start()
		{
			if (_enemyWaves.Count == 0)
				return;

			_message.Display(_greetingText, SHOW_GREETING_SECONDS);
			StartCoroutine(StartLevel());
		}

		private IEnumerator StartLevel()
		{
			yield return new WaitForSeconds(SHOW_GREETING_SECONDS);
			LevelStartEvent.Invoke();
			StartCoroutine(StartWave());
		}

		private void SpawnPlayer()
		{
			Debug.Assert(_playerCharacterA);
			Debug.Assert(_playerCharacterB);
			var position = ZoneCollider(Zone.Player).bounds.center;
			position = new Vector3(position.x, GroundY, position.z);
			var instance = Instantiate(PlayerCharacter, position, Quaternion.identity);
			instance.GetComponent<PlayerCharacter>().Health.DeathEvent.AddListener(OnPlayerDeath);
		}

		private IEnumerator StartWave()
		{
			_currentEnemyWave.WaveIndex++;
			_enemiesKilled = 0;
			_currentEnemyWave.EnemyCount = 0;
			var waveParts = _enemyWaves.Where(x => x.WaveIndex == _currentEnemyWave.WaveIndex).ToList();

			if (waveParts.Count == 0)
				AllWavesCleared();
			else
			{
				_currentWaveNumber++;
				_message.Display("WAVE " + _currentWaveNumber, SHOW_WAVE_SECONDS);
				
				foreach (var wavePart in waveParts)
				{
					var enemyCount = wavePart.EnemyCount;
					_currentEnemyWave.EnemyCount += enemyCount;

					for (var i = 0; i < enemyCount; i++)
					{
						var instance = Instantiate(wavePart.EnemyCharacter).GetComponent<Character>();
						instance.transform.position = RandomZonePosition(Zone.Enemy, instance.Mover.CanFly);
						instance.Health.DeathEvent.AddListener(OnEnemyDeath);

						if (wavePart.SpawnDelay > 0)
							yield return new WaitForSeconds(wavePart.SpawnDelay);
					}
				}
			}
		}

		private void AllWavesCleared()
		{
			if (_allWavesCleared)
				return;

			if (_loopWaves)
			{
				_currentEnemyWave.WaveIndex = 0;
				StartCoroutine(StartWave());
				return;
			}

			_allWavesCleared = true;
			LevelCompletedEvent.Invoke();
		}

		private IEnumerator Fireworks()
		{
			while (true)
			{
				Instantiate(_fireworksEffect, RandomZonePosition(Zone.Enemy, true), Quaternion.identity);
				yield return new WaitForSeconds(FIREWORKS_INTERVAL);
			}
		}

		private void OnLevelStart()
		{
			_levelStartTime = Time.timeSinceLevelLoad;
			_timerEnabled = true;
		}

		private void OnLevelCompleted()
		{
			_timerEnabled = false;
			_message.Display(_levelCompletedText);
			StartCoroutine(Scenes.StartNextLevel(LEVEL_END_DELAY));
		}

		private void OnPlayerDeath(Actor player)
		{
			_timerEnabled = false;

			if (_loopWaves)
				WaveResults();
			else
				_message.Display(_playerDefeatedText);

			StartCoroutine(Scenes.RestartLevel(LEVEL_END_DELAY));
		}

		private void WaveResults()
		{
			bool betterScore, betterWave, betterTime;
			SaveController.SetIfBetter(Score, _currentWaveNumber, TimeElapsed, out betterScore, out betterWave, out betterTime);

			if (betterScore || betterWave || betterTime)
			{
				var msg = new StringBuilder(Colors.AddColorTag("NEW RECORD!\n", Colors.Red));

				if (betterScore)
				{
					msg.Append("SCORE: " + Score);
					msg.AppendLine();
				}

				if (betterTime)
				{
					msg.Append("TIME: " + TimeEx.FormatElapsedTime(TimeElapsed));
					msg.AppendLine();
				}

				if (betterWave)
				{
					msg.Append("WAVES: " + _currentWaveNumber);
				}

				_message.Display(msg.ToString());
				StartCoroutine(Fireworks());
			}
			else
			{
				_message.Display(_playerDefeatedText);
			}
		}

		private void OnEnemyDeath(Actor enemy)
		{
			_enemiesKilled++;

			Score += enemy.ScorePoints;
			ScoreChangedEvent.Invoke(Score);

			EnemyDeathEvent.Invoke(enemy);

			if (_enemiesKilled >= _currentEnemyWave.EnemyCount)
				StartCoroutine(StartWave());
		}

		public Vector3 RandomZonePosition(Zone zone, bool isFlying)
		{
			var zoneCollider = ZoneCollider(zone);
			var randomPosition = MathEx.RandomVector(zoneCollider.bounds.min, zoneCollider.bounds.max);

			if (isFlying)
			{
				if (randomPosition.y < _airY)
					randomPosition = new Vector3(randomPosition.x, _airY, randomPosition.z);
			}
			else
				randomPosition = new Vector3(randomPosition.x, _groundY, randomPosition.z);

			return randomPosition;
		}

		public Collider ZoneCollider(Zone zone)
		{
			Collider zoneCollider = null;

			switch (zone)
			{
				case Zone.Enemy: zoneCollider = _enemyZone; break;
				case Zone.Player: zoneCollider = _playerZone; break;
			}

			return zoneCollider;
		}
	}
}
