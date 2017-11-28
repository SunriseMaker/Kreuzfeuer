using System;
using Game.Enums;

namespace Game.Data
{
	[Serializable]
	public class MindStateDuration
	{
		public MindState MindState;
		public float Duration;

		public MindStateDuration(MindState mindState, float duration)
		{
			MindState = mindState;
			Duration = duration;
		}
	}
}
