using Game.Data;
using UnityEngine;
using Game.Helpers;

namespace Game
{
	public sealed class ProjectileWeapon: Weapon
	{
		protected override void FireImplementation(Aim aim)
		{
			var direction = aim.TargetActor ? MathEx.Direction(BulletSpawnNode.position, aim.TargetActor.SelfAimPoint) : aim.Direction;
			var rotation = Quaternion.LookRotation(direction, Vector3.up);
			
			for (var i = 1; i <= Stats.BulletsPerShot; i++)
			{
				var offset = BulletOffset(i);
				var instance = Instantiate(Bullet, BulletSpawnNode.position + offset, rotation);
				instance.GetComponent<Bullet>().Initialize(Stats.Damage, Stats.BulletLifeTime, aim);
				instance.GetComponent<Rigidbody>().AddForce(direction * Stats.Force);
			}
		}
	}
}
