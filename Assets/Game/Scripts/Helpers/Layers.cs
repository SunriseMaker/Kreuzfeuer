using System.Linq;
using UnityEngine;

namespace Game.Helpers
{
	public static class Layers
	{
		private const string DEFAULT = "Default";
		private const string LIMBO = "Limbo";

		public static int DefaultIndex;
		public static int LimboIndex;

		static Layers()
		{
			DefaultIndex = LayerMask.NameToLayer(DEFAULT);
			LimboIndex = LayerMask.NameToLayer(LIMBO);
		}

		public static void SetLayer(this GameObject gameObject, int layerIndex)
		{
			var gameObjects = gameObject.transform.GetComponentsInChildren<Transform>().ToList();
			gameObjects.Add(gameObject.transform);

			foreach (var go in gameObjects)
				go.gameObject.layer = layerIndex;
		}
	}
}
