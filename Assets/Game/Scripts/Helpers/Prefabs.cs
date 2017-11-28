using Game.Data;
using UnityEngine;

namespace Game.Helpers
{
	public static class Prefabs
	{
		public static System.Collections.IEnumerator SpawnPrefabs(Transform transform, params PrefabData[] prefabs)
		{
			foreach (var prefab in prefabs)
			{
				for (var i = 0; i < prefab.Count; i++)
				{
					var position = transform.position;

					if(prefab.PositionOffset != Vector3.zero)
						position += new VectorBuilder(prefab.PositionOffset).RandomizeSigns().Build();

					Object.Instantiate(prefab.Prefab, position, Quaternion.identity, prefab.BindToParent ? transform : null);

					if (prefab.Interval > 0)
						yield return new WaitForSeconds(prefab.Interval);
				}
			}
		}
	}
}
