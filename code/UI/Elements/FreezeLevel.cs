using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FrostFight.UI.Elements
{
	public class FreezeLevel : Panel
	{
		Panel innerPanel;
		Label innerLabel;

		public FreezeLevel()
		{
			StyleSheet.Load( "/UI/Elements/FreezeLevel.scss" );
			Add.Icon( "ac_unit", "icon" );

			innerLabel = Add.Label( "0%", "player-health" );
			innerPanel = Add.Panel( "inner" );
		}

		public override void Tick()
		{
			if ( Local.Pawn is not FrostPlayer player )
				return;

			innerLabel.Text = $"{player.CurrentFreezeAmount.CeilToInt()}%";
			innerPanel.Style.Width = Length.Percent( player.CurrentFreezeAmount );
		}
	}
}
