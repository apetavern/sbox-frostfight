using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FrostFight.UI.Elements
{
	public class WinnerScreen : Panel
	{
		Label winningTeamLabel;

		public WinnerScreen()
		{
			StyleSheet.Load( "/UI/Elements/WinnerScreen.scss" );
			winningTeamLabel = Add.Label( "Winning team" );
			Add.Panel( "center" );

			BindClass( "visible", () =>
			{
				return Game.Instance.State == Game.GameState.GameOver;
			} );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Game.Instance.WinningTeam == Game.Teams.None )
				winningTeamLabel.Text = $"Tie!";
			else
				winningTeamLabel.Text = $"{Game.Instance.WinningTeam} win!";
		}
	}
}
