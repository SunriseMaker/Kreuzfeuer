using System;

namespace Game.Data
{
	public sealed class MindStateAct : MindStateDuration
	{
		public Action Before { get; private set; }
		public Action Essence { get; private set; }
		public Action After { get; private set; }

		public MindStateAct(Action before, Action essence, Action after, MindStateDuration mindStateDuration)
			: base(mindStateDuration.MindState, mindStateDuration.Duration)
		{
			Before = before;
			Essence = essence;
			After = after;
		}
	}
}
