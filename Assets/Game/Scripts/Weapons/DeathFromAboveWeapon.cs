using Game.Data;
using UnityEngine;

namespace Game
{
	public sealed class DeathFromAboveWeapon : Weapon
	{
		[SerializeField] private float _baseSpawnHeight;
		[SerializeField] private float _maxRange;

		private Vector3 _spawnHeight;
		private readonly Quaternion _rotation = Quaternion.Euler(Vector3.right * 90);

		protected override void ActualizeCache()
		{
			base.ActualizeCache();

			_spawnHeight = new Vector3(0, _baseSpawnHeight, 0);
		}

		protected override void FireImplementation(Aim aim)
		{
			var spawnPosition = aim.TargetActor ? aim.TargetActor.SelfAimPoint : aim.Direction * _maxRange;

			for (var i = 0; i <= Stats.BulletsPerShot; i++)
			{
				var offset = BulletOffset(i);
				var position = spawnPosition + offset + _spawnHeight;
				var instance = Instantiate(Bullet, position, _rotation);
				instance.GetComponent<Bullet>().Initialize(Stats.Damage, Stats.BulletLifeTime, aim);
			}
		}
	}
}
