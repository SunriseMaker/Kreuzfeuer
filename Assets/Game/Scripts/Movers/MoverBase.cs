using System.Runtime.InteropServices;
using Game.Enums;
using Game.Helpers;
using UnityEngine;

namespace Game
{
	public abstract class MoverBase : IMover
	{
		protected readonly Character Character;
		protected readonly bool Flying;
		protected readonly float BaseMovementSpeed;
		protected readonly float BaseRotationSpeed;
		protected readonly bool OnlyYrotation;

		protected MoverBase(Character character, bool flying, float baseMovementSpeed, float baseRotationSpeed, bool onlyYrotation)
		{
			Character = character;
			Flying = flying;
			BaseMovementSpeed = baseMovementSpeed;
			BaseRotationSpeed = baseRotationSpeed;
			OnlyYrotation = onlyYrotation;
		}

		public abstract bool Move(float forwardAmount, float rightAmount, float upAmount, float timeAmount);

		public abstract bool MoveTowards(Vector3 targetPosition, float timeAmount);

		public bool CanMove { get { return !(Character.Health.IsDead || Character.StatusEffectsController.IsAffectedByAny(StatusEffectType.Immovable, StatusEffectType.Stunned)); } }

		public bool CanFly{ get { return Flying; } }

		public void RotateTowards(Vector3 targetPosition, bool instantly)
		{
			var direction = MathEx.Direction(Character.transform.position, targetPosition);
			var lookRotation = Quaternion.LookRotation(direction);

			if (OnlyYrotation)
				lookRotation = Quaternion.Euler(new Vector3(0, lookRotation.eulerAngles.y, 0));

			Character.transform.rotation = instantly
				? lookRotation
				: Quaternion.Slerp(Character.transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
		}

		protected float MovementSpeed { get { return BaseMovementSpeed * Character.StatusEffectsController.GetValue(StatusEffectType.SpeedBuff, 1); } }

		protected float RotationSpeed { get { return BaseRotationSpeed; } }
	}
}
