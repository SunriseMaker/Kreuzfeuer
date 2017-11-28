using System;
using System.Diagnostics;
using Game.Enums;

namespace Game
{
	public sealed class DroneMind : CharacterMind
	{
		private Drone _drone;

		protected override void Awake()
		{
			base.Awake();

			_drone = GetComponent<Drone>();
			Debug.Assert(_drone);
		}

		protected override Action ActBefore(MindState mindState)
		{
			switch (mindState)
			{
				case MindState.MoveAndAttack:
					return ChangeCombatZonePosition;
			}

			return base.ActBefore(mindState);
		}

		protected override void MoveToCombatZonePosition()
		{
			LookAtEnemy();

			base.MoveToCombatZonePosition();
		}

		protected override void Attack()
		{
			LookAtEnemy();
			Aim.Direction = Character.transform.forward;
			Character.Attack(Aim);
		}
	}
}
