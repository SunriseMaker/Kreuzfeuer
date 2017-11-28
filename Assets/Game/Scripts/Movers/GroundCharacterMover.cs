using Game.Helpers;
using UnityEngine;

namespace Game
{
	public class GroundCharacterMover : MoverBase
	{
		public GroundCharacterMover(Character character, bool flying, float baseMovementSpeed, float baseRotationSpeed, bool onlyYrotation) :
			base(character, flying, baseMovementSpeed, baseRotationSpeed, onlyYrotation)
		{
		}

		public override bool Move(float forwardAmount, float rightAmount, float upAmount, float timeAmount)
		{
			if (!CanMove)
				return false;

			var translation = Vector3.zero;

			if (Mathf.Abs(rightAmount) > 0)
				translation = Character.transform.right * rightAmount * MovementSpeed * timeAmount;

			if (Mathf.Abs(forwardAmount) > 0)
				translation += Character.transform.forward * forwardAmount * MovementSpeed * timeAmount;

			if (translation != Vector3.zero)
				Character.transform.Translate(translation, Space.World);

			return true;
		}

		public override bool MoveTowards(Vector3 targetPosition, float timeAmount)
		{
			if (!CanMove || Character.AtPosition(targetPosition))
				return false;

			Move(1, 0, 0, Time.deltaTime);

			var direction = MathEx.Direction(Character.transform.position, targetPosition);
			var rotation = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;
			rotation = new Vector3(0, rotation.y, 0);

			Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation, Quaternion.Euler(rotation), timeAmount * RotationSpeed);

			return true;
		}
	}
}
