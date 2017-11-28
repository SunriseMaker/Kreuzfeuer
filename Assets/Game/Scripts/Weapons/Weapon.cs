using System;
using Game.Data;
using Game.Helpers;
using UnityEngine;

namespace Game
{
	public abstract class Weapon : MonoBehaviour
	{
		public string Name;
		public int Level { get; private set; }

		[SerializeField] protected GameObject Bullet;
		[SerializeField] protected WeaponStats Stats;
		[SerializeField] protected WeaponStats StatsModifiers;
		[SerializeField] protected bool InfiniteAmmo;
		[SerializeField] protected Distribution BulletDistribution;
		[SerializeField] private Transform _bulletSpawnNode;
		public Transform BulletSpawnNode { get { return _bulletSpawnNode; } }
		[NonSerialized] public int Ammo;
		
		protected float LastFireTime { get; private set; }

		protected virtual bool CanFire { get { return HasAmmo && Time.time - LastFireTime >= FireRate; } }

		public bool HasAmmo { get { return InfiniteAmmo || Ammo > 0; } }

		protected float FireRate;
		private float _radiansStep;
		private Vector3 _dispersionStart;
		private Vector3 _dispersionStep;

		protected virtual void Awake()
		{
			Debug.Assert(Bullet);
			Debug.Assert(Stats.Damage > 0);
			Debug.Assert(Stats.BulletsPerShot > 0);
			Debug.Assert(Stats.RoundsPerSecond > 0);
			Debug.Assert(Stats.BulletLifeTime > 0);

			ActualizeCache();
		}

		protected virtual void ActualizeCache()
		{
			FireRate = 1 / Stats.RoundsPerSecond;
			_radiansStep = MathEx.Degrees360InRadians / Stats.BulletsPerShot;
			_dispersionStart = -(Stats.BulletDispersion / 2);
			_dispersionStep = Stats.BulletsPerShot == 1 ? Vector3.zero : Stats.BulletDispersion / (Stats.BulletsPerShot - 1);
		}

		public void Fire(Aim aim)
		{
			if(!CanFire)
				return;

			if (!InfiniteAmmo)
				Ammo--;

			LastFireTime = Time.time;

			FireImplementation(aim);
		}

		protected abstract void FireImplementation(Aim aim);

		public void Upgrade()
		{
			Level++;
			Stats.Modify(StatsModifiers);
			ActualizeCache();
		}

		protected Vector3 BulletOffset(int i)
		{
			var offset = Vector3.zero;

			switch (BulletDistribution)
			{
				case Distribution.Line:
					if (Stats.BulletsPerShot > 1)
						offset = _dispersionStart + i * _dispersionStep - _dispersionStep;
					break;

				case Distribution.Circle:
					var currentRadians = _radiansStep * i;

					offset = new Vector3(
					    (float) Math.Cos(currentRadians) * Stats.BulletDispersion.x,
					    (float) Math.Sin(currentRadians) * Stats.BulletDispersion.y,
					    (float) Math.Sin(currentRadians) * Stats.BulletDispersion.z
					);
					break;

				case Distribution.Random:
					offset = new VectorBuilder(Vector3.zero)
						.RandomizeWithVector(Stats.BulletDispersion)
						.RandomizeSigns()
						.Build();
					break;
			}

			return offset;
		}
	}
}
