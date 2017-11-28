using System;
using Game.Enums;
using UnityEngine.Events;

namespace Game.Data
{
	public class Health
	{
		private readonly Actor _actor;
		public int MaxHp { get; private set; }
		public int Hp { get; private set; }
		public float Percent { get { return MaxHp == 0 ? 0.0f : (float) Hp / MaxHp; } }
		public bool IsDead { get { return Hp <= 0; } }

		#region Events
		// TODO: rewrite events with C# 6.0 feature 'auto property initializer'.
		// public UnityEvent SomeEvent { get; private set;} = new UnityEvent();

		private readonly UnityEventActor _deathEvent = new UnityEventActor();
		public UnityEventActor DeathEvent { get { return _deathEvent; } }

		private readonly UnityEvent _damageEvent = new UnityEvent();
		public UnityEvent DamageEvent { get { return _damageEvent; } }

		private readonly UnityEvent _healEvent = new UnityEvent();
		public UnityEvent HealEvent { get { return _healEvent; } }

		private readonly UnityEvent _reviveEvent = new UnityEvent();
		public UnityEvent ReviveEvent { get { return _reviveEvent; } }
		#endregion

		public Health(Actor actor, int maxHp)
		{
			_actor = actor;
			MaxHp = maxHp;
			MaximizeHp();
		}

		public void IncreaseMaxHp(int amount)
		{
			MaxHp += amount;
			MaximizeHp();
			HealEvent.Invoke();
		}

		public void Revive()
		{
			MaximizeHp();
			ReviveEvent.Invoke();
		}

		public void Heal(int amount)
		{
			ChangeHealth(Math.Abs(amount));
		}

		public void Damage(int amount)
		{
			if (_actor.StatusEffectsController != null && _actor.StatusEffectsController.IsAffectedBy(StatusEffectType.Immune))
				return;

			ChangeHealth(-Math.Abs(amount));
		}

		private void ChangeHealth(int amount)
		{
			if (IsDead)
				return;

			var oldHealth = Hp;
			Hp = Math.Min(Math.Max(Hp + amount, 0), MaxHp);

			if (IsDead)
				DeathEvent.Invoke(_actor);
			else if (Hp < oldHealth)
				DamageEvent.Invoke();
			else
				HealEvent.Invoke();
		}

		private void MaximizeHp()
		{
			Hp = MaxHp;
		}
	}
}
