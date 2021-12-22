using FrostFight.UI.Elements.Generic;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FrostFight.UI.Elements
{
	public class Stamina : ProgressBar
	{
		public Stamina()
		{
			StyleSheet.Load( "/UI/Elements/Stamina.scss" );
			Add.Icon( "bolt", "icon" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Local.Pawn is not FrostPlayer player )
				return;

			innerPanel.Style.Width = Length.Percent( player.Stamina );
		}
	}
}
