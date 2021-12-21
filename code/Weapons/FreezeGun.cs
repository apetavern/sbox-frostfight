using Sandbox;

namespace FrostFight.Weapons
{
	[Library( "weapon_freezegun" )]
	public partial class FreezeGun : BaseWeapon
	{
		public override string ViewModelPath => "weapons/freezegun/freezegun_view.vmdl";
		public override float PrimaryRate => 0.6f;

		public override void Spawn()
		{
			base.Spawn();
			SetModel( "weapons/freezegun/freezegun_world.vmdl" );
		}
		public override bool CanPrimaryAttack()
		{
			if ( !Input.Pressed( InputButton.Attack1 ) )
				return false;

			if ( Owner.Health <= 0 )
				return false;

			return base.CanPrimaryAttack();
		}

		public override void AttackPrimary()
		{
			base.AttackPrimary();
		}
	}
}
