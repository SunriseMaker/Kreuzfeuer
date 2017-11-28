using System.Collections;
using Game.Data;
using Game.Enums;
using UnityEngine;
using Game.Helpers;

namespace Game
{
	[RequireComponent(typeof(Animator))]
	public sealed class PlayerCharacter : Character
	{
		[Header("PlayerCharacter")]
		[SerializeField] private float _immuneOnReviveSeconds;
		[SerializeField] private float _immuneOnDamageSeconds;
		[SerializeField] private float _immuneOnEvadeSeconds;
		[SerializeField] private PlayerHud _playerHud;

		public PlayerCharacterAnimator Animator { get; private set; }
		[SerializeField] private int[] levelExperience;
		public CharacterProgressController CharacterProgressController;

		private bool _evading;
		private CapsuleCollider _capsuleCollider;

		protected override void Awake()
		{
			_capsuleCollider = GetComponent<CapsuleCollider>();
			Animator = new PlayerCharacterAnimator(GetComponent<Animator>());

			Debug.Assert(_playerHud);
			Instantiate(_playerHud);

			Debug.Assert(levelExperience.Length > 0);

			var level = Finders.GetLevel();
			level.LevelCompletedEvent.AddListener(OnLevelEnd);
			CharacterProgressController = new CharacterProgressController(levelExperience, level.EnemyDeathEvent);
			CharacterProgressController.LevelUpEvent.AddListener(OnLevelUp);

			base.Awake();
		}

		protected override void InitializeMover(out IMover mover)
		{
			mover = new PlayerCharacterMover(this, false, BaseMovementSpeed, BaseRotationSpeed, true, _capsuleCollider);
		}

		public override void Attack(Aim aim)
		{
			if (_evading || StatusEffectsController.IsAffectedBy(StatusEffectType.Stunned))
				return;

			base.Attack(aim);
		}

		public void Evade()
		{
			if(_evading)
				return;

			_evading = true;
			StartCoroutine(EvadeCr());
		}

		public IEnumerator EvadeCr()
		{
			Animator.SetEvade();
			gameObject.SetLayer(Layers.LimboIndex);

			yield return new WaitForSeconds(_immuneOnEvadeSeconds);

			Animator.SetHorizontal(0);
			gameObject.SetLayer(Layers.DefaultIndex);
			_evading = false;
		}

		private void OnLevelEnd()
		{
			var stunned = new StatusEffect(StatusEffectType.Stunned);
			StatusEffectsController.AddEffect(stunned, true);

			var immune = new StatusEffect(StatusEffectType.Immune);
			StatusEffectsController.AddEffect(immune, true);

			Animator.SetWin();
		}

		protected override void OnDamage()
		{
			base.OnDamage();

			var temporaryImmunity = new StatusEffect(StatusEffectType.Immune, 0, _immuneOnDamageSeconds);
			StatusEffectsController.AddEffect(temporaryImmunity, false);
		}

		protected override void OnDeath(Actor character)
		{
			base.OnDeath(character);

			Animator.SetHorizontal(0);
		}

		protected override void OnRevive()
		{
			base.OnRevive();

			var temporaryImmunity = new StatusEffect(StatusEffectType.Immune, 0, _immuneOnReviveSeconds);
			StatusEffectsController.AddEffect(temporaryImmunity, true);
		}

		private void OnLevelUp()
		{
			Health.IncreaseMaxHp(1);

			foreach (var weapon in Weapons)
				weapon.Upgrade();
		}

		public sealed class PlayerCharacterAnimator
		{
			private readonly Animator _animator;
			private readonly int _horizontalId;
			private readonly int _verticalId;
			private readonly int _evadeId;
			private readonly int _winId;

			public PlayerCharacterAnimator(Animator animator)
			{
				_animator = animator;
				_horizontalId = UnityEngine.Animator.StringToHash(AxisNames.Horizontal);
				_verticalId = UnityEngine.Animator.StringToHash(AxisNames.Vertical);
				_evadeId = UnityEngine.Animator.StringToHash(AxisNames.Evade);
				_winId = UnityEngine.Animator.StringToHash("Win");
			}

			public void SetHorizontal(float amount)
			{
				_animator.SetFloat(_horizontalId, amount);
			}

			public void SetVertical(float amount)
			{
				_animator.SetFloat(_verticalId, amount);
			}

			public void SetEvade()
			{
				_animator.SetTrigger(_evadeId);
			}

			public void SetWin()
			{
				_animator.SetTrigger(_winId);
			}
		}
	}
}
