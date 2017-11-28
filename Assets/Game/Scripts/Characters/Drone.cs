namespace Game
{
	public class Drone : Character
	{
		protected override void InitializeMover(out IMover mover)
		{
			mover = new AirCharacterMover(this, true, BaseMovementSpeed, BaseRotationSpeed, false);
		}

		protected override void InitializeWeapons()
		{
			base.InitializeWeapons();

			SelectRandomWeapon();
		}
	}
}
