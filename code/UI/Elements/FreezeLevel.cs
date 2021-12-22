using FrostFight.UI.Elements.Generic;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FrostFight.UI.Elements
{
	public class FreezeLevel : ProgressBar
	{
		public FreezeLevel()
		{
			StyleSheet.Load( "/UI/Elements/FreezeLevel.scss" );
			Add.Icon( "ac_unit", "icon" );
		}

		public override void Tick()
		{
			if ( Local.Pawn is not FrostPlayer player )
				return;

			innerPanel.Style.Width = Length.Percent( player.CurrentFreezeAmount );
		}
	}
}
