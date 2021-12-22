using FrostFight.Weapons;
using Sandbox;

namespace FrostFight
{
	public partial class FrostPlayer : Player
	{
		public const float MaxFreezeAmount = 100;
		[Net] public bool IsFreezer { get; private set; } = true;
		[Net] public float CurrentFreezeAmount { get; private set; }
		public ModelEntity IceBlock { get; set; }

		public bool IsFrozen => CurrentFreezeAmount >= MaxFreezeAmount;

		public FrostPlayer()
		{
			Inventory = new BaseInventory( this );
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			// Set to new PlayerController() if/when we decide to use it.
			Controller = new PlayerController();
			Animator = new StandardPlayerAnimator();
			Camera = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			CurrentFreezeAmount = 0;

			SetLoadout();

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			if ( !IsFrozen )
				base.Simulate( cl );

			if ( Input.ActiveChild != null )
			{
				ActiveChild = Input.ActiveChild;
			}

			if ( LifeState != LifeState.Alive )
				return;

			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );

			if ( IsServer )
				DebugOverlay.Text( Position + Vector3.Up * 80f, $"Frozen amount: {CurrentFreezeAmount}" );
		}

		public void AddFreeze( float amount )
		{
			CurrentFreezeAmount += amount;
			CurrentFreezeAmount = CurrentFreezeAmount.Clamp( 0, MaxFreezeAmount );

			(GetActiveController() as PlayerController)?.ScaleMovementSpeedsByFreeze( CurrentFreezeAmount );

			if ( CurrentFreezeAmount >= MaxFreezeAmount && !IceBlock.IsValid() )
			{
				// TODO: Turn this into it's own entity with health, so that players can destroy it and release this player.
				IceBlock = new ModelEntity( "models/objects/iceblock/iceblock.vmdl" );
				IceBlock.Position = Position;
			}
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
