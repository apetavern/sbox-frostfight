using Sandbox;

namespace FrostFight.Weapons
{
	[Library( "weapon_freezegun" )]
	public partial class FreezeGun : BaseWeapon
	{
		public override string ViewModelPath => "models/weapons/freezegun/freezegun_view.vmdl";
		public override float PrimaryRate => 0.6f;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/weapons/freezegun/freezegun_world.vmdl" );
		}

		public override void AttackPrimary()
		{
			(Owner as AnimEntity).SetAnimBool( "fire", true );

			ShootEffects();
			base.AttackPrimary();
		}

		[ClientRpc]
		public void SetFire()
		{
		}

		[ClientRpc]
		public void ShootEffects()
		{
			Host.AssertClient();

			Particles.Create( "particles/frostpuff.vpcf", EffectEntity, "muzzle" );
		}

		public override void SimulateAnimator( PawnAnimator anim )
		{
			anim.SetParam( "holdtype", 4 );
			anim.SetParam( "holdtype_handedness", 1 );
			anim.SetParam( "holdtype_pose_hand", 0.07f );
		}
	}
}
