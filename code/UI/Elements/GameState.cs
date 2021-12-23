using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

namespace FrostFight.UI.Elements
{
	public class GameState : Panel
	{
		Label stateNameLabel;
		Label stateTimeLabel;

		public GameState()
		{
			StyleSheet.Load( "/Code/UI/Elements/GameState.scss" );

			var stateName = Add.Panel( "state-name" );
			stateName.Add.Icon( "priority_high" );
			stateNameLabel = stateName.Add.Label( "State name" );

			var stateTime = Add.Panel( "state-time" );
			stateTime.Add.Icon( "schedule" );
			stateTimeLabel = stateTime.Add.Label( "00:00" );
		}

		public override void Tick()
		{
			base.Tick();

			var stateName = "";
			var stateTime = 0f;
			switch ( Game.Instance.State )
			{
				case Game.GameState.Waiting:
					stateTime = Game.Instance.WaitingTimer;
					stateName = "Waiting for players...";
					break;
				case Game.GameState.Playing:
					stateTime = Game.Instance.PlayingTimer;
					stateName = "Playing";
					break;
				case Game.GameState.GameOver:
					stateTime = Game.Instance.GameOverTimer;
					stateName = "Game over";
					break;
			}

			TimeSpan time = TimeSpan.FromSeconds( stateTime );
			stateTimeLabel.Text = time.ToString( "mm':'ss" );
			stateNameLabel.Text = stateName;
		}
	}
}
