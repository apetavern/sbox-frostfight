using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FrostFight.UI.Elements
{
	public class Stamina : Panel
	{
		Panel innerPanel;

		public Stamina()
		{
			StyleSheet.Load( "/UI/Elements/Stamina.scss" );
			Add.Icon( "bolt", "icon" );

			innerPanel = Add.Panel( "inner" );
		}

		public override void Tick()
		{
			if ( Local.Pawn is not FrostPlayer player )
				return;

			innerPanel.Style.Width = Length.Percent( player.Stamina );
		}
	}
}
