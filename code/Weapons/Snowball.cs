using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostFight.Weapons
{
	public class Snowball : ModelEntity
	{
		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/christmas/snowball.vmdl_c" );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );

			DeleteAsync( 10 );
		}

		protected override void OnPhysicsCollision( CollisionEventData eventData )
		{
			base.OnPhysicsCollision( eventData );

			Delete();

			if ( eventData.Entity is FrostPlayer player )
				player.AddFreezeWithStun( 25, Owner );
		}
	}
}
