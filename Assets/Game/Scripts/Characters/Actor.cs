using Game.Data;
using Game.Helpers;
using UnityEngine;

namespace Game
{
	public class Actor : MonoBehaviour
	{
		private const float DISTANCE_ERROR = 0.1f;

		[Header("Actor")]
		[SerializeField] private int _maxHealth;
		public Health Health { get; private set; }

		[SerializeField] protected PrefabData[] PrefabsOnDeath;
		[SerializeField] protected PrefabData[] PrefabsOnStart;

		[SerializeField] protected float SecondsBeforeDestroy;
		[SerializeField] private int _baseScorePoints;
		[SerializeField] private int _experiencePoints;

		public int ScorePoints { get { return _baseScorePoints; } }
		public int ExperiencePoints { get { return _experiencePoints; } }

		[SerializeField] private Vector3 _forceOnDeath;
		protected Rigidbody Rigidbody { get; private set; }

		[SerializeField] protected Transform AimPoint;
		public virtual Vector3 SelfAimPoint { get { return AimPoint.position; } }

		private BlinkEffect _blinkEffect;
		public StatusEffectsController StatusEffectsController { get; private set; }
		public bool AtPosition(Vector3 position)
		{
			return Vector3.Distance(transform.position, position) < DISTANCE_ERROR;
		}

		protected virtual void Awake()
		{
			StatusEffectsController = gameObject.AddComponent<StatusEffectsController>();

			Debug.Assert(_maxHealth > 0);
			Health = new Health(this, _maxHealth);

			Health.DeathEvent.AddListener(OnDeath);
			Health.DamageEvent.AddListener(OnDamage);
			Health.ReviveEvent.AddListener(OnRevive);

			Rigidbody = GetComponent<Rigidbody>();
			Debug.Assert(Rigidbody);

			Debug.Assert(AimPoint);
			_blinkEffect = new BlinkEffect(Color.gray, 5, 0.08f, gameObject, this);
		}

		protected virtual void Start()
		{
			StartCoroutine(Prefabs.SpawnPrefabs(AimPoint, PrefabsOnStart));
		}

		private void AddForceOnDeath()
		{
			Rigidbody.isKinematic = false;
			var force = new VectorBuilder(_forceOnDeath).RandomizeSigns(randomY: false).Build();
			var positionOffset = new VectorBuilder(Vector3.one).RandomizeSigns().Build();
			var forcePosition = transform.position + positionOffset;
			Rigidbody.AddForceAtPosition(force, forcePosition);
		}

		protected virtual void OnDeath(Actor character)
		{
			_blinkEffect.Stop();

			StartCoroutine(Prefabs.SpawnPrefabs(transform, PrefabsOnDeath));

			if (SecondsBeforeDestroy == 0)
			{
				Destroy(gameObject);
				return;
			}

			if (SecondsBeforeDestroy > 0)
				Destroy(gameObject, SecondsBeforeDestroy);

			AddForceOnDeath();

			gameObject.SetLayer(Layers.LimboIndex);
		}

		protected virtual void OnDamage()
		{
			_blinkEffect.Start(true);
		}

		protected virtual void OnRevive()
		{
			gameObject.SetLayer(Layers.DefaultIndex);
		}
	}
}
