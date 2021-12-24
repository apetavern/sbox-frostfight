using Sandbox;

namespace FrostFight.Weapons
{
	[Library( "weapon_freezegun" )]
	public partial class FreezeGun : BaseWeapon
	{
		public override string UIName => "Freeze Gun";
		public override string UIImage => "/textures/ui/freeze-gun.png";
		public override string ViewModelPath => "models/weapons/freezegun/freezegun_view.vmdl";
		public override float PrimaryRate => 0.6f;
		public override float SecondaryRate => 1.5f;
		public float FreezeReach = 200f;
		public TimeSince TimeSinceAreaCreated { get; set; }
		public float AreaCreationInterval { get; set; } = 0.07f;
		public Particles IceParticle { get; set; }
		public Sound SpraySound { get; set; }
		public bool IsSpraySoundPlaying { get; set; }

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

			// Primary
			if ( Input.Down( InputButton.Attack1 ) )
			{
				TimeSincePrimaryAttack = 0;

				(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );

				if ( IsServer )
				{
					// Trace for freeze
					var trace = Trace.Ray( Owner.EyePos, Owner.EyePos + Owner.EyeRot.Forward * FreezeReach ).Ignore( Owner ).Run();

					if ( TimeSinceAreaCreated > AreaCreationInterval )
					{
						var freezeArea = new FreezeArea() { Position = trace.EndPos, Owner = Owner };
						TimeSinceAreaCreated = 0;
					}

					using ( Prediction.Off() )
					{
						if ( !IsSpraySoundPlaying )
						{
							SpraySound = Sound.FromEntity( "freezegun_spray", this );
							IsSpraySoundPlaying = true;
						}
					}
				}

				CreateEffects();

				// Client stuff
				if ( !IsClient )
					return;

				if ( IceParticle is null )
					IceParticle = Particles.Create( "particles/frostpuff.vpcf", EffectEntity, "muzzle" );

				ViewModelEntity?.SetAnimBool( "fire", true );
			}

			// Primary release
			if ( Input.Released( InputButton.Attack1 ) )
			{
				DestroyEffects();

				if ( IsServer )
				{
					SpraySound.Stop();
					IsSpraySoundPlaying = false;
				}

				if ( !IsClient )
					return;

				ViewModelEntity?.SetAnimBool( "fire", false );
			}

			// Secondary
			if ( Input.Down( InputButton.Attack2 ) && !Input.Down( InputButton.Attack1 ) )
			{
				if ( TimeSinceSecondaryAttack < SecondaryRate )
					return;

				TimeSinceSecondaryAttack = 0;

				(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );
				ViewModelEntity?.SetAnimBool( "fire_snowball", true );
				Sound.FromEntity( "snowball_fire", this );

				if ( !IsServer )
					return;

				var trace = Trace.Ray( Owner.EyePos, Owner.EyePos + Owner.EyeRot.Forward * 1000 ).Ignore( Owner ).Run();

				SnowballFiredEffects();

				var snowball = new Snowball() { Owner = Owner };
				snowball.FireTowards( trace.EndPos );
			}
		}

		public override void ActiveEnd( Entity ent, bool dropped )
		{
			base.ActiveEnd( ent, dropped );
			SpraySound.Stop();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			SpraySound.Stop();
		}

		[ClientRpc]
		public void CreateEffects()
		{
			if ( IceParticle is null )
				IceParticle = Particles.Create( "particles/frostpuff.vpcf", EffectEntity, "muzzle" );
		}

		[ClientRpc]
		public void DestroyEffects()
		{
			if ( IceParticle is not null )
			{
				IceParticle.Destroy();
				IceParticle = null;
			}
		}

		[ClientRpc]
		public void SnowballFiredEffects()
		{
			if ( IsLocalPawn )
			{
				_ = new Sandbox.ScreenShake.Perlin();
			}
		}

		public override void SimulateAnimator( PawnAnimator anim )
		{
			anim.SetParam( "holdtype", 1 );
			anim.SetParam( "holdtype_handedness", 1 );
		}
	}
}
