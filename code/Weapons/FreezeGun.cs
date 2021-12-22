﻿using Sandbox;

namespace FrostFight.Weapons
{
	[Library( "weapon_freezegun" )]
	public partial class FreezeGun : BaseWeapon
	{
		public override string ViewModelPath => "models/weapons/freezegun/freezegun_view.vmdl";
		public override float PrimaryRate => 0.6f;
		public float FreezeReach = 200f;
		public TimeSince TimeSinceAreaCreated { get; set; }
		public float AreaCreationInterval { get; set; } = 0.07f;
		public Particles IceParticle { get; set; }

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/weapons/freezegun/freezegun_world.vmdl" );
		}

		public override void Simulate( Client player )
		{
			if ( CanReload() )
			{
				Reload();
			}

			if ( Input.Down( InputButton.Attack1 ) )
			{
				TimeSincePrimaryAttack = 0;

				if ( IsServer )
				{
					// Trace for freeze
					var trace = Trace.Ray( Owner.EyePos, Owner.EyePos + Owner.EyeRot.Forward * FreezeReach ).Ignore( Owner ).Run();

					if ( TimeSinceAreaCreated > AreaCreationInterval )
					{
						var freezeArea = new FreezeArea() { Position = trace.EndPos };
						TimeSinceAreaCreated = 0;
					}
				}

				// Client stuff
				if ( !IsClient )
					return;

				if ( IceParticle is null )
					IceParticle = Particles.Create( "particles/frostpuff.vpcf", EffectEntity, "muzzle" );

				ViewModelEntity?.SetAnimBool( "fire", true );
			}
			else
			{
				if ( IceParticle is not null )
				{
					IceParticle.Destroy();
					IceParticle = null;
				}

				ViewModelEntity?.SetAnimBool( "fire", false );
			}
		}

		public override void SimulateAnimator( PawnAnimator anim )
		{
			anim.SetParam( "holdtype", 4 );
			anim.SetParam( "holdtype_handedness", 1 );
			anim.SetParam( "holdtype_pose_hand", 0.07f );
		}
	}
}
