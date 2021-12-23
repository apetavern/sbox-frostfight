using FrostFight.Weapons;
using Sandbox;
using Sandbox.UI;

namespace FrostFight.UI.Elements
{
	[UseTemplate]
	public class Crosshair : Panel
	{
		public Panel CooldownBar { get; set; }

		public Crosshair()
		{
			BindClass( "visible", () =>
			{
				return !(Local.Pawn as FrostPlayer).MovementDisabled;
			} );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Local.Pawn is not FrostPlayer { ActiveChild: FreezeGun freezeGun } )
			{
				CooldownBar.RemoveClass( "visible" );
				return;
			}

			float t = freezeGun.TimeSinceSecondaryAttack / 3f;
			t = t.Clamp( 0, 1f );
			CooldownBar.SetClass( "visible", !t.AlmostEqual( 1 ) );

			CooldownBar.Style.Width = Length.Fraction( t * 2f );
		}
	}
}
