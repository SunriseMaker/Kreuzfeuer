using System.Linq;
using System.Collections.Generic;
using Game.Data;
using Game.Helpers;
using UnityEngine;

namespace Game
{
	public abstract class Character : Actor
	{
		[Header("Character")]
		[SerializeField] protected float BaseMovementSpeed;
		[SerializeField] protected float BaseRotationSpeed;
		[SerializeField] protected Transform WeaponsNode;

		private IMover _mover;
		public IMover Mover { get { return _mover; } }

		public List<Weapon> Weapons { get; private set; }
		public Weapon CurrentWeapon { get; private set; }
		private int _currentWeaponIndex;

		protected override void Awake()
		{
			base.Awake();

			Debug.Assert(WeaponsNode);
			Debug.Assert(BaseMovementSpeed > 0);
			Debug.Assert(BaseRotationSpeed > 0);

			InitializeWeapons();
			InitializeMover(out _mover);
		}

		protected abstract void InitializeMover(out IMover mover);

		protected virtual void InitializeWeapons()
		{
			Weapons = WeaponsNode.GetComponents<Weapon>().ToList();
			Debug.Assert(Weapons.Count > 0);
			CurrentWeapon = Weapons[0];
		}

		public void SelectNextWeapon()
		{
			do
			{
				_currentWeaponIndex = _currentWeaponIndex == Weapons.Count - 1 ? 0 : _currentWeaponIndex + 1;
				CurrentWeapon = Weapons[_currentWeaponIndex];
				if (CurrentWeapon.HasAmmo)
					break;
			}
			while (_currentWeaponIndex > 0);
		}

		public void SelectRandomWeapon()
		{
			CurrentWeapon = Weapons.Where(x => x.HasAmmo).RandomElement();
			_currentWeaponIndex = Weapons.IndexOf(CurrentWeapon);
		}

		public virtual void Attack(Aim aim)
		{
			CurrentWeapon.Fire(aim);
		}
	}
}
