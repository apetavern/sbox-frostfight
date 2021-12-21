using FrostFight.Weapons;
using Sandbox;

namespace FrostFight
{
	public partial class FrostPlayer : Player
	{
		public bool IsFreezer { get; private set; } = false;
		public bool IsFrozen { get; private set; } = false;

		public FrostPlayer()
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

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( Input.ActiveChild != null )
			{
				ActiveChild = Input.ActiveChild;
			}

			if ( LifeState != LifeState.Alive )
				return;

			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );
		}

		private void SetLoadout()
		{
			Inventory.DeleteContents();

			if ( IsFreezer )
				Inventory.Add( new FreezeGun(), true );
			else
				Inventory.Add( new IcePick(), true );
		}

	}
}
