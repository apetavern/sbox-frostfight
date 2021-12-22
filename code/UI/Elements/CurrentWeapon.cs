using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FrostFight.UI.Elements
{
	[UseTemplate]
	public class CurrentWeapon : Panel
	{
		Image weaponIcon;
		Label weaponName;

		public CurrentWeapon()
		{
			AddClass( "active" );
			weaponIcon = Add.Image( "", "weapon-icon" );
			weaponName = Add.Label( "", "weapon-name" );
		}

		public override void Tick()
		{
			base.Tick();

			Update();
		}

		public void Update()
		{
			var active = Local.Pawn.Inventory.Active;

			if ( active is Weapons.BaseWeapon inventoryWeapon )
			{
				weaponIcon.SetTexture( "tools/images/common/generic_hud_warning.png" );
				weaponName.Text = inventoryWeapon.UIName;
			}
		}
	}
}
