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
		[Net] public float Stamina { get; set; } = 100;
		public TimeSince TimeSinceLastFroze { get; set; }
		[Net] public TimeSince TimeSinceStunned { get; set; }
		[Net] public IceBlock IceBlock { get; set; }
		public bool IsFrozen => CurrentFreezeAmount >= MaxFreezeAmount;
		public Clothing.Container Clothing = new();

		public FrostPlayer()
		{
			Inventory = new BaseInventory( this );
		}

		public FrostPlayer( Client cl ) : this()
		{
			Clothing.LoadFromClient( cl );
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

			Clothing.DressEntity( this );

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
			base.Simulate( cl );

			// Update movement speed based on CurrentFreezeAmount
			(GetActiveController() as PlayerController)?.ScaleMovementSpeedsByFreeze( CurrentFreezeAmount );

			if ( CurrentFreezeAmount > 0 && TimeSinceLastFroze > 1 && !IsFrozen )
				CurrentFreezeAmount--;

			if ( Input.ActiveChild != null )
			{
				ActiveChild = Input.ActiveChild;
			}

			if ( LifeState != LifeState.Alive )
				return;

			if ( IsFrozen )
				return;

			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );
		}

		public void AddFreezeWithStun( float amount, Entity attacker )
		{
			TimeSinceStunned = 0;
			AddFreeze( amount, attacker );
		}

		public void AddFreeze( float amount, Entity attacker )
		{
			CurrentFreezeAmount += amount;
			CurrentFreezeAmount = CurrentFreezeAmount.Clamp( 0, MaxFreezeAmount );

			if ( CurrentFreezeAmount >= MaxFreezeAmount && !IceBlock.IsValid() )
			{
				IceBlock = new()
				{
					Position = Position,
					Owner = this,
					Parent = this
				};

				Camera = new SpectateBlockCamera() { Target = this };
			}

			TimeSinceLastFroze = 0;
		}

		public void ClearFreeze()
		{
			CurrentFreezeAmount = 0;
			IceBlock = null;

			Camera = new FirstPersonCamera();
		}

		private void SetLoadout()
		{
			Inventory.DeleteContents();

			if ( IsFreezer )
				Inventory.Add( new FreezeGun(), true );
			else
				Inventory.Add( new IcePick(), true );
		}

		[AdminCmd]
		public static void GivePick()
		{
			var player = ConsoleSystem.Caller.Pawn;

			player?.Inventory.DeleteContents();
			player?.Inventory.Add( new IcePick(), true );
		}

		[AdminCmd]
		public static void Freeze()
		{
			var player = ConsoleSystem.Caller.Pawn as FrostPlayer;

			player?.AddFreeze( 100, null );
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
