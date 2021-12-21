using FrostFight.Weapons;
using Sandbox;

namespace FrostFight
{
	public partial class Player : Sandbox.Player
	{
		public bool IsFreezer { get; private set; } = false;
		public bool IsFrozen { get; private set; } = false;

		public Player()
		{
			Inventory = new BaseInventory( this );
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			// Set to new PlayerController() if/when we decide to use it.
			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			SetLoadout();

			base.Respawn();
		}

		private void SetLoadout()
		{
			Inventory.DeleteContents();
			Inventory.Add( new FreezeGun(), true );
		}

	}
}
