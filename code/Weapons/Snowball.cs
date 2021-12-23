using Sandbox;
using System.Linq;

namespace FrostFight.Weapons
{
	public partial class Snowball : ModelEntity
	{
		private Vector3 OriginPos { get; set; }
		private Vector3 TargetPos { get; set; }
		private float Distance { get; set; }
		private float Speed { get; set; } = 550f;
		private TimeSince TimeSinceFired { get; set; }

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/christmas/snowball.vmdl" );
		}

		public void FireTowards( Vector3 targetPos )
		{
			OriginPos = Owner.EyePos + Owner.EyeRot.Forward * 30f;
			TargetPos = targetPos;
			Distance = Vector3.DistanceBetween( OriginPos, TargetPos );
			TimeSinceFired = 0;
		}

		[Event.Tick]
		public void OnTick()
		{
			if ( OriginPos == Vector3.Zero )
				return;

			var distTravelled = TimeSinceFired * Speed;
			var currPos = distTravelled / Distance;

			Position = Vector3.Lerp( OriginPos, TargetPos, currPos );

			if ( Position.IsNearlyEqual( TargetPos ) )
			{
				var hitPlayer = Physics.GetEntitiesInSphere( Position, 5f )
					.Where( ent => ent is FrostPlayer fPlayer && !fPlayer.IsFreezer )
					.Cast<FrostPlayer>().FirstOrDefault();

				hitPlayer?.AddFreezeWithStun( 25, Owner );

				if ( hitPlayer == null )
				{
					var hitParticles = Particles.Create( "particles/impact.generic.smokepuff.vpcf", Position );
					hitParticles.SetForward( 0, (OriginPos - TargetPos).Normal );
					Sound.FromWorld( "snowball_hit", Position );
				}
				else
				{
					Sound.FromWorld( "snowball_hit", Position ).SetVolume( 0.25f );
				}

				Delete();
			}
		}
	}
}
