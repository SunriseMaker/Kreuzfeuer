using System;
using System.Collections.Generic;
using Game.Enums;
using UnityEngine;

namespace Game.Data
{
	public static class SaveController
	{
		private static readonly Dictionary<Save, string> StatsMap = new Dictionary<Save, string>
		{
			{Save.CharacterIndex, "CHARACTER_INDEX"},
			{Save.MaxTime, "MAX_TIME"},
			{Save.MaxScore, "MAX_SCORE"},
			{Save.MaxWave, "MAX_WAVE"},
			{Save.InfiniteWaves, "INFINITE_WAVES"}
		};

		public static T Get<T>(Save save)
		{
			var key = StatsMap[save];

			if (!PlayerPrefs.HasKey(key))
				Set(save, 0);

			object result = null;

			if (typeof(T) == typeof(float))
				result = PlayerPrefs.GetFloat(key);

			if (typeof(T) == typeof(int))
				result = PlayerPrefs.GetInt(key);

			if (typeof(T) == typeof(int))
				result = PlayerPrefs.GetInt(key);

			if(result!=null)
				return (T) result;

			throw new ArgumentException();
		}

		public static void Set<T>(Save save, T value)
		{
			var key = StatsMap[save];

			if (value is float)
			{
				var val = (float)(object)value;
				PlayerPrefs.SetFloat(key, val);
			}
			else if (value is int)
			{
				var val = (int)(object)value;
				PlayerPrefs.SetInt(key, val);
			} else if (value is string)
			{
				var val = (string)(object)value;
				PlayerPrefs.SetString(key, val);
			}
			else
				throw new ArgumentException();
		}

		public static void SetIfBetter(int score, int wave, float time, out bool betterScore, out bool betterWave, out bool betterTime)
		{
			betterScore = betterWave = betterTime = false;

			if (Get<int>(Save.MaxScore) < score)
			{
				Set(Save.MaxScore, score);
				betterScore = true;
			}

			if (Get<int>(Save.MaxWave) < wave)
			{
				Set(Save.MaxWave, wave);
				betterWave = true;
			}

			if (Get<float>(Save.MaxTime) < time)
			{
				Set(Save.MaxTime, time);
				betterTime = true;
			}
		}

		public static void Clear()
		{
			Set(Save.MaxWave, 0);
			Set(Save.MaxTime, 0);
			Set(Save.MaxScore, 0);
		}
	}
}
