using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			SetModel( "models/christmas/snowball.vmdl_c" );
		}

		public void FireTowards( Vector3 targetPos )
		{
			OriginPos = Owner.EyePos + Owner.EyeRot.Forward * 30f;
			TargetPos = targetPos;
			Distance = Vector3.DistanceBetween( OriginPos, TargetPos );
			TimeSinceFired = 0;
		}

		[Event.Tick.Server]
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

				Delete();
			}
		}
	}
}
