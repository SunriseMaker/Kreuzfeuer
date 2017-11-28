using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(Animator))]
	public class Robot : Character
	{
		public RobotCharacterAnimator Animator { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			Animator = new RobotCharacterAnimator(GetComponent<Animator>());
		}

		protected override void InitializeMover(out IMover mover)
		{
			mover = new RobotMover(this, false, BaseMovementSpeed, BaseRotationSpeed, true);
		}

		public sealed class RobotCharacterAnimator
		{
			private readonly Animator _animator;
			private readonly int _moveId;
			private readonly int _aimId;
			private readonly int _idleId;

			public RobotCharacterAnimator(Animator animator)
			{
				_animator = animator;
				_moveId = UnityEngine.Animator.StringToHash("Move");
				_aimId = UnityEngine.Animator.StringToHash("Aim");
				_idleId = UnityEngine.Animator.StringToHash("Idle");
			}

			public void SetMove(bool value)
			{
				_animator.SetBool(_moveId, value);
			}

			public void SetAim(bool value)
			{
				_animator.SetBool(_aimId, value);
			}

			public void SetIdle()
			{
				SetMove(false);
				SetAim(false);
				_animator.SetTrigger(_idleId);
			}
		}
	}
}
