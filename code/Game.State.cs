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
			Runners,
			None
		}

		[Net] public GameState State { get; set; }
		[Net] public IList<FrostPlayer> Players { get; set; }
		[Net] public IList<FrostPlayer> Spectators { get; set; }
		[Net] public float WaitingTimer { get; set; } = -1;
		[Net] public float PlayingTimer { get; set; } = -1;
		[Net] public float GameOverTimer { get; set; } = -1;
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
				PlayingTimer = 120;

			PlayingTimer -= Time.Delta;

			// Check if all Non-Freezers are Frozen.
			var winConditionMet = true;
			foreach ( var player in Players )
			{
				if ( player.IsFreezer ) continue;

				if ( !player.IsFrozen ) winConditionMet = false;

				if ( !winConditionMet ) break;
			}

			if ( winConditionMet )
			{
				WinningTeam = Teams.Freezers;
				ChangeState( GameState.GameOver );
				return;
			}

			if ( PlayingTimer <= 0 )
			{
				WinningTeam = Teams.Runners;
				ChangeState( GameState.GameOver );
				return;
			}

			CheckGameStillValid();
		}

		private void TickGameOver()
		{
			if ( GameOverTimer == -1 )
				GameOverTimer = 10;

			GameOverTimer -= Time.Delta;

			if ( GameOverTimer <= 0 )
			{
				ResetGame();
			}
		}

		private void AssignRoles()
		{
			var freezersCount = MathX.CeilToInt( Players.Count * 0.15f );
			var playersCount = Players.Count;

			foreach ( var player in Players )
			{
				var selectionChance = (float)freezersCount / playersCount;
				var rand = Rand.Float( 0, 1 );

				if ( rand <= selectionChance )
				{
					player.IsFreezer = true;
					freezersCount--;
				}

				player.Respawn();
				player.Ready();

				playersCount--;
			}
		}

		private void CheckGameStillValid()
		{
			var freezerCount = 0;
			var runnerCount = 0;

			foreach ( var player in Players )
			{
				if ( player.IsFreezer )
					freezerCount++;
				else
					runnerCount++;
			}

			if ( freezerCount == 0 || runnerCount == 0 )
			{
				WinningTeam = Teams.None;
				ChangeState( GameState.GameOver );
			}
		}

		private void ResetGame()
		{
			// Reset Timers
			WaitingTimer = 10;
			PlayingTimer = -1;
			GameOverTimer = -1;

			// Reset Players
			foreach ( var player in Players )
			{
				player.MovementDisabled = true;
				player.ClearFreeze();
				player.IsFreezer = false;
				player.Stamina = 100;
			}

			// Add Spectators to Players
			foreach ( var player in Spectators )
			{
				Players.Add( player );
			}
			Spectators.Clear();

			// Set to Waiting State with reduced waiting time
			ChangeState( GameState.Waiting );
		}
	}
}
