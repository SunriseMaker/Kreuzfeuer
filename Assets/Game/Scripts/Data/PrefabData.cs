using UnityEngine;

namespace Game.Data
{
	[System.Serializable]
	public sealed class PrefabData
	{
		public GameObject Prefab;
		public byte Count;
		public float Interval;
		public bool BindToParent;
		public Vector3 PositionOffset;
	}
}
