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

			if ( Health <= 0 )
				OnKilled();
		}

		public override void OnKilled()
		{
			(Owner as FrostPlayer)?.ClearFreeze();
			Delete();
		}

		[Event.Tick.Server]
		public void OnTick()
		{
			DebugOverlay.Text( Position + Vector3.Up * 80f, $"Block health: {Health}", Color.Red, 0 );

			Rotation = Owner.Rotation;
		}
	}
}
