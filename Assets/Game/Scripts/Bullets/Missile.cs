using Game.Helpers;
using UnityEngine;

namespace Game
{
	public sealed class Missile : Bullet
	{
		[SerializeField] private float _rotationSpeed;
		[SerializeField] private float _velocity;

		private Rigidbody _rigidbody;
		private bool pursue;

		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			pursue = Aim.TargetActor;
		}

		protected override void FixedUpdate()
		{
			if (pursue)
				PursueTarget();
			else
				FlyForward();

			base.FixedUpdate();
		}

		private void PursueTarget()
		{
			if (Aim.TargetActor.Health.IsDead)
				pursue = false;
			else
			{
				var targetPoint = Aim.TargetActor.SelfAimPoint;
				var direction = MathEx.Direction(transform.position, targetPoint);
				var rotation = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * _rotationSpeed);
			}

			_rigidbody.velocity = transform.forward * _velocity;
		}

		private void FlyForward()
		{
			_rigidbody.AddForce(Aim.Direction * _velocity * Time.deltaTime, ForceMode.Impulse);
		}
	}
}
