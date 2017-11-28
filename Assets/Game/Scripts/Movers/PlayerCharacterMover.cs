using System;
using UnityEngine;

namespace Game
{
	public sealed class PlayerCharacterMover : MoverBase
	{
		private readonly PlayerCharacter _playerCharacter;
		private readonly CapsuleCollider _collider;

		public PlayerCharacterMover(PlayerCharacter playerCharacter, bool flying, float baseMovementSpeed, float baseRotationSpeed, bool onlyYrotation, CapsuleCollider collider) :
			base(playerCharacter, flying, baseMovementSpeed, baseRotationSpeed, onlyYrotation)
		{
			_playerCharacter = playerCharacter;
			_collider = collider;
		}

		public override bool Move(float forwardAmount, float rightAmount, float upAmount, float timeAmount)
		{
			if (!CanMove)
				return false;

			const float RAY_DISTANCE = 0.1f;
			var rightSign = Math.Sign(rightAmount);
			var direction = rightSign * Vector3.right;

			var rayStart = Character.transform.position + _collider.center + Vector3.right * _collider.radius * rightSign * 1.01f;

			if (Physics.Raycast(rayStart, direction, RAY_DISTANCE))
				rightAmount = 0;
			else
				Character.transform.Translate(Vector3.right * rightAmount * BaseMovementSpeed * timeAmount);

			_playerCharacter.Animator.SetHorizontal(rightAmount);
			return true;
		}

		public override bool MoveTowards(Vector3 targetPosition, float timeAmount)
		{
			return false;
		}
	}
}
