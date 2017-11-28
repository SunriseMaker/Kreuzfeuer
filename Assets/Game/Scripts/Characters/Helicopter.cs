using Game.Data;
using Game.Helpers;
using UnityEngine;

namespace Game
{
	public sealed class Helicopter : Character
	{
		[Header("Helicopter")]
		[SerializeField] private PrefabData _criticalHealthEffect;
		private Weapon _primaryWeapon1;
		private Weapon _primaryWeapon2;
		private Weapon _specialWeapon;

		private byte _hpStatus = 2;

		protected override void OnDamage()
		{
			base.OnDamage();
			CheckHealth();
		}
		
		private void CheckHealth()
		{
			var percent = Health.Percent;

			if (percent < 0.66f && _hpStatus > 1
			    || percent < 0.33f && _hpStatus > 0)
			{
				_hpStatus--;
				StartCoroutine(Prefabs.SpawnPrefabs(transform, _criticalHealthEffect));
			}
		}

		protected override void InitializeMover(out IMover mover)
		{
			mover = new AirCharacterMover(this, true, BaseMovementSpeed, BaseRotationSpeed, true);
		}

		protected override void InitializeWeapons()
		{
			_primaryWeapon1 = WeaponsNode.Find("PrimaryWeapon1").GetComponent<Weapon>();
			_primaryWeapon2 = WeaponsNode.Find("PrimaryWeapon2").GetComponent<Weapon>();
			_specialWeapon = WeaponsNode.Find("SpecialWeapon").GetComponent<Weapon>();
		}

		public override void Attack(Aim aim)
		{
			_primaryWeapon1.Fire(aim);
			_primaryWeapon2.Fire(aim);
		}

		public void SpecialAttack(Aim aim)
		{
			_specialWeapon.Fire(aim);
		}
	}
}
