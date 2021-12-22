using Sandbox;
using System;
using System.Collections.Generic;

namespace FrostFight
{
	partial class Game
	{
		public enum GameState
		{
			Waiting,
			Playing,
			GameOver
		}

		public enum Teams
		{
			Freezers,
			Runners
		}

		[Net] public GameState State { get; set; }
		[Net] public IList<FrostPlayer> Players { get; set; }
		[Net] public float WaitingTimer { get; set; } = -1;
		[Net] public float PlayingTimer { get; set; } = -1;
		[Net] public Teams WinningTeam { get; set; }

		[Event.Tick]
		private void OnTick()
		{
			if ( IsClient ) return;

			switch ( State )
			{
				case GameState.Waiting:
					TickWaiting();
					break;
				case GameState.Playing:
					TickPlaying();
					break;
				case GameState.GameOver:
					TickGameOver();
					break;
			}
		}

		private void ChangeState( GameState newState )
		{
			State = newState;
		}

		private void TickWaiting()
		{
			if ( WaitingTimer == -1 )
				WaitingTimer = 30;

			WaitingTimer -= Time.Delta;

			if ( WaitingTimer <= 0 )
			{
				if ( Client.All.Count >= 4 )
				{
					AssignRoles();
					ChangeState( GameState.Playing );
				}
			}
		}

		private void TickPlaying()
		{
			if ( PlayingTimer == -1 )
				PlayingTimer = 300;

			PlayingTimer -= Time.Delta;

			// Check if all Non-Freezers are Frozen.
			var winConditionMet = true;
			foreach ( FrostPlayer player in Players )
			{
				if ( player.IsFreezer ) continue;

				if ( !player.IsFrozen ) winConditionMet = false;

				if ( !winConditionMet ) break;
			}

			if ( winConditionMet )
			{
				WinningTeam = Teams.Freezers;
				ChangeState( GameState.GameOver );
			}

			if ( PlayingTimer <= 0 )
			{
				WinningTeam = Teams.Runners;
				ChangeState( GameState.GameOver );
			}
		}

		private void TickGameOver()
		{
			if ( WinningTeam == Teams.Runners )
			{

			}
			else
			{

			}
		}

		private void AssignRoles()
		{
			var freezersCount = MathX.CeilToInt( Players.Count * 0.15f );
			var playersCount = Players.Count;

			foreach ( FrostPlayer player in Players )
			{
				var selectionChance = (float)freezersCount / playersCount;
				var rand = Rand.Float( 0, 1 );

				if ( rand <= selectionChance )
				{
					player.IsFreezer = true;
					freezersCount--;
				}

				player.Respawn();
				playersCount--;
			}
		}
	}
}
