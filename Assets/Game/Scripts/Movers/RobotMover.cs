using UnityEngine;

namespace Game
{
	public class RobotMover : GroundCharacterMover
	{
		private readonly Robot _robot;

		public RobotMover(Robot robot, bool flying, float baseMovementSpeed, float baseRotationSpeed, bool onlyYrotation) :
			base(robot, flying, baseMovementSpeed, baseRotationSpeed, onlyYrotation)
		{
			_robot = robot;
		}

		public override bool Move(float forwardAmount, float rightAmount, float upAmount, float timeAmount)
		{
			var positionChanged = base.Move(forwardAmount, rightAmount, upAmount, timeAmount);
			_robot.Animator.SetMove(positionChanged);
			return positionChanged;
		}

		public override bool MoveTowards(Vector3 targetPosition, float timeAmount)
		{
			var positionChanged = base.MoveTowards(targetPosition, timeAmount);
			_robot.Animator.SetMove(positionChanged);
			return positionChanged;
		}
	}
}
