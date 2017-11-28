using System;
using System.Collections.Generic;
using Game.Data;
using UnityEngine.Events;

namespace Game
{
	public sealed class CharacterProgressController
	{
		private const int FIRST_LEVEL = 1;
		
		public int CurrentLevel { get; private set; }

		private readonly int _maxLevel;
		private int _currentExperience;

		private readonly Dictionary<int, int> _levelExperience = new Dictionary<int, int>();

		private readonly UnityEvent _levelUpEvent = new UnityEvent();
		public UnityEvent LevelUpEvent { get { return _levelUpEvent; }}

		public CharacterProgressController(int[] levelExperience, UnityEventActor enemyDeathEvent)
		{
			enemyDeathEvent.AddListener(enemy => AddExperience(enemy.ExperiencePoints));
			CurrentLevel = FIRST_LEVEL;

			var level = 0;

			foreach (var experience in levelExperience)
				_levelExperience.Add(++level, experience);

			_maxLevel = level;
		}

		public void AddExperience(int amount)
		{
			while (CurrentLevel < _maxLevel)
			{
				var levelExperience = _levelExperience[CurrentLevel];
				var delta = Math.Min(amount, levelExperience - _currentExperience);
				amount -= delta;
				_currentExperience += delta;

				if (_currentExperience < levelExperience)
					break;

				_currentExperience = 0;
				CurrentLevel++;
				LevelUpEvent.Invoke();
			}
		}
	}
}
