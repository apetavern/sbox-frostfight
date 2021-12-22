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

		[Net] public GameState State { get; set; }
		[Net] public IList<FrostPlayer> Players { get; set; }

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
			// Check for min players count:
			if ( Client.All.Count >= 4 )
			{
				AssignRoles();
				ChangeState( GameState.Playing );
			}
		}

		private void TickPlaying()
		{

		}

		private void TickGameOver()
		{

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
