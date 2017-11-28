using Game.Data;
using Game.Helpers;
using UnityEngine;

namespace Game
{
	public class BombardmentWeapon : Weapon
	{
		[SerializeField] private float _spawnHeight;
		[SerializeField] private float _nearRange;
		[SerializeField] private float _farRange;
		[SerializeField] private int _spawnLinesCount;

		private float _spawnStep;
		private readonly Quaternion _rotation = Quaternion.Euler(Vector3.right * 90);

		protected override void ActualizeCache()
		{
			base.ActualizeCache();

			_spawnStep = (_farRange - _nearRange) / _spawnLinesCount;
		}

		protected override void FireImplementation(Aim aim)
		{
			var direction = aim.TargetActor ? MathEx.Direction(BulletSpawnNode.position, aim.TargetActor.SelfAimPoint) : aim.Direction;

			for (var j = 1; j <= _spawnLinesCount; j++)
			{
				var spawnHeight = new Vector3(0, _spawnHeight + j, 0);
				var currentPosition = BulletSpawnNode.position + direction * _nearRange + direction * _spawnStep * j;

				for (var i = 0; i <= Stats.BulletsPerShot; i++)
				{
					var offset = BulletOffset(i);
					var position = currentPosition + offset + spawnHeight;
					var instance = Instantiate(Bullet, position, _rotation);
					instance.GetComponent<Bullet>().Initialize(Stats.Damage, Stats.BulletLifeTime, aim);
				}
			}
		}
	}
}
