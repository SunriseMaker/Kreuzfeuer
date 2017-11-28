using UnityEngine;

namespace Game.Data
{
	public class Aim
	{
		public Vector3 Direction;
		public Actor TargetActor;

		public Aim()
		{
		}

		public Aim(Vector3 direction, Actor targetActor)
		{
			Direction = direction;
			TargetActor = targetActor;
		}

		public Aim Copy()
		{
			return new Aim(Direction, TargetActor);
		}
	}
}
