using System.Diagnostics;
using Game.Enums;

namespace Game
{
	public class StatusEffect
	{
		public const float PermanentDuration = float.PositiveInfinity;
		public StatusEffectType Type { get; private set; }
		public float Duration { get; private set; }
		public bool IsPermanent { get; private set; }
		public float Value { get; private set; }

		public StatusEffect(StatusEffectType type, float value = 0, float duration = PermanentDuration)
		{
			Debug.Assert(duration > 0);
			
			Type = type;
			Value = value;
			Duration = duration;
			IsPermanent = float.IsPositiveInfinity(duration);
		}

		public void UpdateDuration(float deltaTime)
		{
			Duration -= deltaTime;
		}
	}
}
