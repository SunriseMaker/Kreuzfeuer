using UnityEngine;

namespace Game
{
	public class AirCharacterMover : MoverBase
	{
		public AirCharacterMover(Character character, bool flying, float baseMovementSpeed, float baseRotationSpeed, bool onlyYrotation) :
			base(character, flying, baseMovementSpeed, baseRotationSpeed, onlyYrotation)
		{
		}

		public override bool Move(float forwardAmount, float rightAmount, float upAmount, float timeAmount)
		{
			var translation = new Vector3(forwardAmount, upAmount, rightAmount);
			Character.transform.Translate(translation * MovementSpeed * timeAmount, Space.World);
			return true;
		}

		public override bool MoveTowards(Vector3 targetPosition, float timeAmount)
		{
			if (Character.AtPosition(targetPosition))
				return false;

			Character.transform.position = Vector3.MoveTowards(Character.transform.position, targetPosition, MovementSpeed * timeAmount);
			return true;
		}
	}
}
