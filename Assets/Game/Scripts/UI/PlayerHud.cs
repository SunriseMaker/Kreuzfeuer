using System.Collections;
using System.Text;
using Game.Data;
using Game.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class PlayerHud : MonoBehaviour
	{
		[SerializeField] private RectTransform _crosshair;
		[SerializeField] private char _heartSymbol;
		[SerializeField] private Text _healthBar;
		[SerializeField] private Text _score;
		[SerializeField] private Text _timer;

		private int _hudScore;
		private int _animatedScore;
		private bool _animatingScore;
		private Health _characterHealth;

		private void Start()
		{
			_characterHealth = Finders.GetPlayerCharacter().Health;
			_characterHealth.DeathEvent.AddListener(x => { RefreshHealthBar(); });
			_characterHealth.DamageEvent.AddListener(RefreshHealthBar);
			_characterHealth.HealEvent.AddListener(RefreshHealthBar);
			_characterHealth.ReviveEvent.AddListener(RefreshHealthBar);
			
			var level = Finders.GetLevel();
			level.ScoreChangedEvent.AddListener(RefreshScore);
			level.TimeChangedEvent.AddListener(RefreshTime);

			RefreshHealthBar();
			RefreshScore(level.Score);
			RefreshTime(level.TimeElapsed);
		}

		private void RefreshTime(float timeElapsed)
		{
			_timer.text = TimeEx.FormatElapsedTime(timeElapsed);
		}

		private void RefreshHealthBar()
		{
			var stringBuilder = new StringBuilder();

			if (_characterHealth.Hp > 0)
				stringBuilder.Append(ColoredSymbols(_heartSymbol, _characterHealth.Hp, Colors.Red));

			var delta = _characterHealth.MaxHp - _characterHealth.Hp;

			if (delta > 0)
				stringBuilder.Append(ColoredSymbols(_heartSymbol, delta, Colors.Grey));

			_healthBar.text = stringBuilder.ToString();
		}

		public void SetCrossHairPosition(Vector3 position)
		{
			_crosshair.position = position;
		}

		private static string ColoredSymbols(char symbol, int count, string color)
		{
			return Colors.AddColorTag(new string(symbol, count), color);
		}

		private void RefreshScore(int score)
		{
			_hudScore = score;

			if (_animatingScore)
				return;
			
			_animatingScore = true;
			StartCoroutine(AnimateScore());
		}

		private IEnumerator AnimateScore()
		{
			const float INTERVAL = 0.1f;
			const int STEPS = 10;

			var increment = (_hudScore - _animatedScore) / STEPS;

			for (var i = 0; i < STEPS; i++)
			{
				_animatedScore += increment;
				_score.text = _animatedScore.ToString();
				yield return new WaitForSeconds(INTERVAL);
			}

			_score.text = _hudScore.ToString();
			_animatingScore = false;
		}
	}
}
