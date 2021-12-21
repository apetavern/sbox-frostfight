using Sandbox;

namespace FrostFight.Weapons
{
	[Library( "weapon_icepick" )]
	public partial class IcePick : BaseWeapon
	{
		public override string ViewModelPath => "models/weapons/pickaxe/pickaxe_view.vmdl";
		public override float PrimaryRate => 0.6f;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/weapons/pickaxe/pickaxe_world.vmdl" );
		}

		public override void AttackPrimary()
		{
			SetAnimBool( "fire", true );

			base.AttackPrimary();
		}

		public override void SimulateAnimator( PawnAnimator anim )
		{
			anim.SetParam( "holdtype", 4 );
			anim.SetParam( "holdtype_handedness", 1 );
			anim.SetParam( "holdtype_pose_hand", 0.07f );
		}
	}
}
