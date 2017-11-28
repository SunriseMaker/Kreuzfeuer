using System;
using UnityEngine;

namespace Game.Data
{
	[Serializable]
	public sealed class WeaponStats
	{
		public int Damage;
		public float BulletLifeTime;
		public int BulletsPerShot;
		public float RoundsPerSecond;
		public float Force;
		public Vector3 BulletDispersion;

		public void Modify(WeaponStats modifiers)
		{
			Damage += modifiers.Damage;
			BulletLifeTime += modifiers.BulletLifeTime;
			BulletsPerShot += modifiers.BulletsPerShot;
			RoundsPerSecond += modifiers.RoundsPerSecond;
			Force += modifiers.Force;
			BulletDispersion += modifiers.BulletDispersion;
		}
	}
}
