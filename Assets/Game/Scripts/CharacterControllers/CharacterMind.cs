using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Enums;
using Game.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
	public abstract class CharacterMind : MonoBehaviour
	{
		[Tooltip("True: 100% accuracy. False: accuracy depends on character rotation towards target.")]
		[SerializeField] private bool _instantAim;
		[SerializeField] protected MindStateDuration[] MindStateDurations;
		
		protected Character Character;
		protected Actor Enemy;
		protected MindState CurrentMindState;
		private List<MindStateAct> _acts;
		protected Level Level;
		protected Collider CombatZoneCollider;
		protected Vector3 CombatZonePosition;
		protected Aim Aim;
		protected UnityEvent StopCycleEvent = new UnityEvent();

		protected virtual void Awake()
		{
			Character = GetComponent<Character>();
			Debug.Assert(Character);

			Enemy = Finders.GetPlayerCharacter();
			Aim = new Aim(Character.transform.forward, _instantAim ? Enemy : null);

			Level = Finders.GetLevel();
			CombatZoneCollider = Level.ZoneCollider(Zone.Enemy);

			InitializeStateTransitions();
		}

		private void Start()
		{
			if (Enemy)
				Character.Mover.RotateTowards(Enemy.SelfAimPoint, true);

			Debug.Assert(_acts.Count > 0);

			if(_acts.Count> 0)
				StartCoroutine(CycleStates());
		}

		private void InitializeStateTransitions()
		{
			_acts = new List<MindStateAct>();

			foreach (var mindStateDuration in MindStateDurations)
			{
				var mindState = mindStateDuration.MindState;
				var act = new MindStateAct(
					ActBefore(mindState),
					ActEssence(mindState),
					ActAfter(mindState),
					mindStateDuration
				);
				_acts.Add(act);
			}
		}

		protected virtual Action ActBefore(MindState mindState)
		{
			switch (mindState)
			{
				case MindState.Move:
					return ChangeCombatZonePosition;
			}

			return null;
		}

		protected virtual Action ActEssence(MindState mindState)
		{
			switch (mindState)
			{
				case MindState.Aim: return LookAtEnemy;
				case MindState.Attack: return Attack;
				case MindState.Move: return MoveToCombatZonePosition;
				case MindState.MoveAndAttack: return () => { MoveToCombatZonePosition(); Attack(); };
			}

			return null;
		}

		protected virtual Action ActAfter(MindState mindState)
		{
			return null;
		}

		private IEnumerator CycleStates()
		{
			while (CanContinue)
			{
				foreach (var transition in _acts)
				{
					CurrentMindState = transition.MindState;
					transition.Before.SafeInvoke();

					var started = Time.timeSinceLevelLoad;

					do
					{
						transition.Essence.SafeInvoke();
						yield return new WaitForFixedUpdate();
					}
					while (CanContinue && Time.timeSinceLevelLoad - started <= transition.Duration);
					
					transition.After.SafeInvoke();
				}
			}

			StopCycleEvent.Invoke();
		}

		private bool CanContinue { get { return Character && !Character.Health.IsDead && Enemy && !Enemy.Health.IsDead; } }

		protected virtual void LookAtEnemy()
		{
			if(Enemy)
				Character.Mover.RotateTowards(Enemy.SelfAimPoint, false);
		}

		protected virtual void MoveToCombatZonePosition()
		{
			Character.Mover.MoveTowards(CombatZonePosition, Time.deltaTime);
		}

		protected virtual void ChangeCombatZonePosition()
		{
			const float MIN_DISTANCE = 1.0f;

			do
			{
				CombatZonePosition = Level.RandomZonePosition(Zone.Enemy, Character.Mover.CanFly);
			}
			while (Vector3.Distance(Character.transform.position, CombatZonePosition) < MIN_DISTANCE);
		}

		protected abstract void Attack();
	}
}
