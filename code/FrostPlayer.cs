using FrostFight.UI.Elements;
using FrostFight.Weapons;
using Sandbox;

namespace FrostFight
{
	public partial class FrostPlayer : Player
	{
		public const float MaxFreezeAmount = 100;
		[Net] public bool IsFreezer { get; set; } = false;
		[Net] public float CurrentFreezeAmount { get; private set; }
		public IceBlock IceBlock { get; set; }
		public bool IsFrozen => CurrentFreezeAmount >= MaxFreezeAmount;

		public FrostPlayer()
		{
			Inventory = new BaseInventory( this );
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

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

		public void OnDisconnect()
		{
			IceBlock?.Delete();
		}

		public override void Simulate( Client cl )
		{
			if ( IsFrozen )
				return;

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
				DebugOverlay.Text( Position + Vector3.Up * 90f, $"Frozen amount: {CurrentFreezeAmount}" );
		}

		public void AddFreeze( float amount, Entity attacker )
		{
			CreateHitmarker( To.Single( attacker ) );

			CurrentFreezeAmount += amount;
			CurrentFreezeAmount = CurrentFreezeAmount.Clamp( 0, MaxFreezeAmount );

			(GetActiveController() as PlayerController)?.ScaleMovementSpeedsByFreeze( CurrentFreezeAmount );

			if ( CurrentFreezeAmount >= MaxFreezeAmount && !IceBlock.IsValid() )
			{
				IceBlock = new();
				IceBlock.Position = Position;
				IceBlock.Owner = this;
			}
		}

		public void ClearFreeze()
		{
			CurrentFreezeAmount = 0;
			(GetActiveController() as PlayerController)?.ScaleMovementSpeedsByFreeze( CurrentFreezeAmount );
			IceBlock = null;
		}

		private void SetLoadout()
		{
			Inventory.DeleteContents();

			if ( IsFreezer )
				Inventory.Add( new FreezeGun(), true );
			else
				Inventory.Add( new IcePick(), true );
		}

		[ClientRpc]
		public static void CreateHitmarker()
		{
			HitmarkerContainer.OnHit();
		}

		[AdminCmd]
		public static void GivePick()
		{
			var player = ConsoleSystem.Caller.Pawn;

			player?.Inventory.DeleteContents();
			player?.Inventory.Add( new IcePick(), true );
		}

		[AdminCmd]
		public static void GiveGun()
		{
			var player = ConsoleSystem.Caller.Pawn;

			player?.Inventory.DeleteContents();
			player?.Inventory.Add( new FreezeGun(), true );
		}
	}
}
