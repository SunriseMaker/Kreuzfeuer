using System.Linq;
using UnityEngine;

namespace Game.Helpers
{
	public static class Finders
	{
		private static PlayerCharacter _playerCharacter;
		private static Level _level;

		public static T FirstObjectOfType<T>() where T : Object
		{
			return Object.FindObjectsOfType<T>().FirstOrDefault();
		}

		public static PlayerCharacter GetPlayerCharacter()
		{
			if(!_playerCharacter)
				_playerCharacter = FirstObjectOfType<PlayerCharacter>();
			
			return _playerCharacter;
		}

		public static Level GetLevel()
		{
			if (!_level)
				_level = FirstObjectOfType<Level>();

			return _level;
		}
	}
}
