using UnityEngine;

namespace Game.Helpers
{
	public static class MathEx
	{
		public static readonly System.Random Rnd = new System.Random();
		public const float Degrees360InRadians = 360 * Mathf.Deg2Rad;

		public static bool RandomBool()
		{
			return Rnd.NextDouble() > 0.5d;
		}

		public static int RandomSign()
		{
			return RandomBool() ? 1 : -1;
		}

		public static float RandomFloat(float val1, float val2)
		{
			return (float) Rnd.NextDouble() * (val2 - val1) + val1;
		}

		public static int RandomInt(int val1, int val2)
		{
			return Rnd.Next(val1, val2);
		}

		public static Vector3 RandomVector(Vector3 v1, Vector3 v2)
		{
			return new Vector3(RandomFloat(v1.x, v2.x), RandomFloat(v1.y, v2.y), RandomFloat(v1.z, v2.z));
		}

		public static Vector3 Direction(Vector3 fromPos, Vector3 toPos)
		{
			return (toPos - fromPos).normalized;
		}
	}
}
