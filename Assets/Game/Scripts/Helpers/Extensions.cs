using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Helpers
{
	public static class Extensions
	{
		public static void SafeInvoke(this Action action)
		{
			if (action != null)
				action.Invoke();
		}

		public static T RandomElement<T>(this IEnumerable<T> enumerable)
		{
			var array = enumerable.ToArray();

			if (array.Length == 0)
				return default(T);

			var index = array.Length > 1 ? MathEx.RandomInt(0, array.Length) : 0;
			return array[index];
		}

		public static int ToInt(this bool b)
		{
			return b ? 1 : 0;
		}

		public static bool ToBool(this int i)
		{
			return i != 0;
		}
	}
}
