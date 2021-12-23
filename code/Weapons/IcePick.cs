using Sandbox;

namespace FrostFight.Weapons
{
	[Library( "weapon_icepick" )]
	public partial class IcePick : BaseWeapon
	{
		public override string UIName => "Ice Pick";
		public override string UIImage => "/textures/ui/ice-pick.png";
		public override string ViewModelPath => "models/weapons/pickaxe/pickaxe_view.vmdl";
		public override float PrimaryRate => 1.2f;
		public float WeaponReach { get; set; } = 65f;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/weapons/pickaxe/pickaxe_world.vmdl" );
		}

		public override void AttackPrimary()
		{
			base.AttackPrimary();

			ViewModelEntity?.SetAnimBool( "fire", true );
			(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );

			if ( !IsServer )
				return;

			var trace = Trace.Ray( Owner.EyePos, Owner.EyePos + Owner.EyeRot.Forward * WeaponReach ).Ignore( Owner ).Run();

			if ( trace.Entity is IceBlock block )
				DoDamage( block, trace.EndPos );
		}

		public async void DoDamage( IceBlock block, Vector3 hitPos )
		{
			await GameTask.DelaySeconds( 0.2f );

			ImpactEffects( hitPos );
			Sound.FromWorld( "icepick_hit", hitPos );

			block.TakeDamage();
		}

		[ClientRpc]
		public void ImpactEffects( Vector3 hitPos )
		{
			if ( IsLocalPawn )
			{
				_ = new Sandbox.ScreenShake.Perlin();
			}

			var impactEffect = Particles.Create( "particles/impact.ice.vpcf", hitPos );
			impactEffect.SetForward( 0, hitPos.Normal );

			DebugOverlay.Sphere( hitPos, 2f, Color.Red, false, 2f );
			DebugOverlay.Line( hitPos, hitPos + hitPos.Normal, 2f );
		}

		[ClientRpc]
		public void OnAttackEffects()
		{
			// Do world model attack
			(Owner as AnimEntity).SetAnimBool( "b_attack", true );
		}

		public override void SimulateAnimator( PawnAnimator anim )
		{
			anim.SetParam( "holdtype", 4 );
			anim.SetParam( "holdtype_handedness", 1 );
			anim.SetParam( "holdtype_pose_hand", 0.07f );
		}
	}
}
