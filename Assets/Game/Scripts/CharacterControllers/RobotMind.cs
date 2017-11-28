using System;
using Game.Enums;
using UnityEngine;

namespace Game
{
	public sealed class RobotMind : CharacterMind
	{
		private Robot _robot;

		protected override void Awake()
		{
			base.Awake();

			_robot = GetComponent<Robot>();
			Debug.Assert(_robot);

			StopCycleEvent.AddListener(() => _robot.Animator.SetIdle());
		}

		protected override Action ActBefore(MindState mindState)
		{
			switch (mindState)
			{
				case MindState.Aim: return ()=> _robot.Animator.SetAim(true);
			}

			return base.ActBefore(mindState);
		}

		protected override Action ActAfter(MindState mindState)
		{
			if(mindState!= MindState.Aim)
				return () => _robot.Animator.SetAim(false);

			return base.ActAfter(mindState);
		}

		protected override void Attack()
		{
			if(!Enemy)
				return;

			Character.Mover.RotateTowards(Enemy.SelfAimPoint, false);
			Aim.Direction = Character.transform.forward;
			Character.CurrentWeapon.Fire(Aim);
		}
	}
}
