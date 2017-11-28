using System;
using Game.Data;
using Game.Enums;
using UnityEngine;

namespace Game
{
	public sealed class HelicopterMind : CharacterMind
	{
		private Helicopter _helicopter;
		private Vector3 _leftPosition;
		private Vector3 _rightPosition;
		private bool _moveRight;
		private const int ATTACK_COUNT = 2;
		private Action<Aim> CurrentAttack;
		private int _attackIndex;

		protected override void Awake()
		{
			base.Awake();

			_helicopter = GetComponent<Helicopter>();
			Debug.Assert(_helicopter);

			var centerPosition = CombatZoneCollider.bounds.center + CombatZoneCollider.bounds.extents.y * Vector3.up * 0.5f;
			var rightOffset = Vector3.right * CombatZoneCollider.bounds.extents.x;
			_leftPosition = centerPosition - rightOffset;
			_rightPosition = centerPosition + rightOffset;
		}

		protected override Action ActBefore(MindState mindState)
		{
			switch (mindState)
			{
				case MindState.Attack: return BeforeAttack;
				case MindState.MoveAndAttack: return () => { ChangeCombatZonePosition(); BeforeAttack(); };
			}

			return base.ActBefore(mindState);
		}

		private void BeforeAttack()
		{
			_attackIndex = _attackIndex == ATTACK_COUNT - 1 ? 0 : _attackIndex + 1;

			switch (_attackIndex)
			{
				case 1: CurrentAttack = _helicopter.SpecialAttack; break;
				default: CurrentAttack = Character.Attack; break;
			}
		}

		protected override void ChangeCombatZonePosition()
		{
			_moveRight = !_moveRight;
			CombatZonePosition = _moveRight ? _leftPosition : _rightPosition;
		}

		protected override void MoveToCombatZonePosition()
		{
			LookAtEnemy();

			base.MoveToCombatZonePosition();
		}

		protected override void Attack()
		{
			Aim.Direction = Character.transform.forward;
			CurrentAttack.Invoke(Aim);
		}
	}
}
