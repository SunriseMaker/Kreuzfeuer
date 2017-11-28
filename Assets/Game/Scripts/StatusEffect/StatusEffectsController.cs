using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.Enums;

namespace Game
{
	public sealed class StatusEffectsController : MonoBehaviour
	{
		private readonly Dictionary<StatusEffectType, StatusEffect> _statusEffects = new Dictionary<StatusEffectType, StatusEffect>();

		public void AddEffect(StatusEffect statusEffect, bool replace)
		{
			if (IsAffectedBy(statusEffect.Type))
			{
				if (!replace)
					return;
				
				_statusEffects.Remove(statusEffect.Type);
			}
			
			_statusEffects.Add(statusEffect.Type, statusEffect);
		}

		public void RemoveEffect(StatusEffectType statusEffectType)
		{
			if (!IsAffectedBy(statusEffectType))
				return;

			_statusEffects.Remove(statusEffectType);
		}

		public bool IsAffectedBy(StatusEffectType statusEffectType)
		{
			return _statusEffects.ContainsKey(statusEffectType);
		}

		public bool IsAffectedByAny(params StatusEffectType[] statusEffectTypes)
		{
			return statusEffectTypes.Any(IsAffectedBy);
		}

		public float GetValue(StatusEffectType statusEffectType, float defaultValue)
		{
			return IsAffectedBy(statusEffectType) ? _statusEffects[statusEffectType].Value : defaultValue;
		}

		private void FixedUpdate()
		{
			foreach (var statusEffectKey in _statusEffects.Keys.ToList())
			{
				var statusEffect = _statusEffects[statusEffectKey];

				if (statusEffect.IsPermanent)
					continue;
					
				statusEffect.UpdateDuration(Time.deltaTime);

				if (statusEffect.Duration <= 0)
					_statusEffects.Remove(statusEffect.Type);
			}
		}
	}
}