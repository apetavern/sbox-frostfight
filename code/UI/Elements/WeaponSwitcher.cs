using FrostFight.Weapons;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Collections.Generic;

namespace FrostFight.UI.Elements
{
	[UseTemplate]
	public class WeaponSwitcher : Panel
	{
		List<WeaponElement> weapons;

		public WeaponSwitcher()
		{
			weapons = new();
		}

		public override void Tick()
		{
			base.Tick();

			Update();
		}

		public void Update()
		{
			var inventorySlot = Local.Pawn.Inventory.GetSlot( 0 );

			if ( inventorySlot is Weapons.BaseWeapon inventoryWeapon &&
				(weapons.Count == 0 || weapons[0].Weapon != inventoryWeapon) )
			{
				var weaponElement = new WeaponElement( inventoryWeapon.UIName, "tools/images/common/generic_hud_warning.png", 0, inventoryWeapon );
				weaponElement.Parent = this;
				weapons.Add( weaponElement );
			}
		}

		public class WeaponElement : Panel
		{
			public Weapons.BaseWeapon Weapon;

			public WeaponElement( string weaponName, string weaponIcon, int slot, Weapons.BaseWeapon weapon )
			{
				Add.Image( weaponIcon, "weapon-icon" );
				Add.Label( weaponName, "weapon-name" );
				BindClass( "active", () =>
				{
					var localInv = Local.Pawn.Inventory;
					return slot == localInv.GetActiveSlot();
				} );

				Weapon = weapon;
			}
		}
	}
}
