using Sandbox;

namespace FrostFight
{
	public partial class IceBlock : ModelEntity
	{
		float DamagePerHit { get; set; } = 25f;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/objects/iceblock/iceblock.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Static );

			Health = 100;
		}

		public void TakeDamage()
		{
			Health -= DamagePerHit;

			UpdateModelFromHealth();

			if ( Health <= 0 )
				OnKilled();
		}

		public override void OnKilled()
		{
			(Owner as FrostPlayer)?.ClearFreeze();
			Delete();
		}

		private void UpdateModelFromHealth()
		{
			if ( Health <= 25 )
			{
				SetBodyGroup( 0, 3 );
				return;
			}

			if ( Health <= 50 )
			{
				SetBodyGroup( 0, 2 );
				return;
			}

			if ( Health <= 75 )
			{
				SetBodyGroup( 0, 1 );
				return;
			}
		}

		[Event.Tick.Server]
		public void OnTick()
		{
			Rotation = Owner.Rotation;
		}
	}
}
