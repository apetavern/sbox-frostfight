using Sandbox;
using Sandbox.UI;

namespace FrostFight.UI.Elements
{
	[UseTemplate]
	public class Crosshair : Panel
	{
		public Crosshair()
		{
			BindClass( "active", () =>
			{
				return Input.Down( InputButton.Attack1 );
			} );

			BindClass( "visible", () =>
			{
				return !(Local.Pawn as FrostPlayer).MovementDisabled;
			} );
		}
	}
}
