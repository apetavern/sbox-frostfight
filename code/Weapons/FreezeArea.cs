using Sandbox;
using System.Linq;

namespace FrostFight.Weapons
{
	public class FreezeArea : Entity
	{
		public float LifeSpan = 0.5f;
		public float FreezeTickRate = 0.25f;
		public TimeSince TimeSinceCreated { get; set; }
		public TimeSince TimeSinceLastTick { get; set; }

		public override void Spawn()
		{
			base.Spawn();

			TimeSinceCreated = 0;
		}

		[Event.Tick.Server]
		public void OnTick()
		{
			if ( TimeSinceCreated > LifeSpan )
			{
				Delete();
			}

			if ( TimeSinceLastTick > FreezeTickRate )
			{
				var hits = Physics.GetEntitiesInSphere( Position, 10f ).Where( ent => ent is FrostPlayer fPlayer && !fPlayer.IsFreezer ).Cast<FrostPlayer>();
				if ( hits.Any() )
				{
					hits.ToList().ForEach( ent => ent.AddFreeze( 0.1f ) );
					DebugOverlay.Sphere( Position, 10f, Color.Red );
				}
				else
				{
					DebugOverlay.Sphere( Position, 10f, Color.White );
				}
			}
		}
	}
}
