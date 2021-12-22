using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FrostFight.UI.Elements
{
	[UseTemplate]
	public class WeaponSwitcher : Panel
	{
		public WeaponSwitcher()
		{
			for ( int i = 0; i < 2; ++i )
			{
				var weaponElement = new WeaponElement( "Ice Pick", "tools/images/common/generic_hud_warning.png", i );

				weaponElement.Parent = this;
			}
		}

		public class WeaponElement : Panel
		{
			public WeaponElement( string weaponName, string weaponIcon, int slot )
			{
				Add.Image( weaponIcon, "weapon-icon" );
				Add.Label( weaponName, "weapon-name" );
				BindClass( "active", () =>
				{
					var localInv = Local.Pawn.Inventory;
					return slot == localInv.GetActiveSlot();
				} );
			}
		}
	}
}
