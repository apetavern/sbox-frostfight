using FrostFight.UI;
using Sandbox;
using System.Linq;

namespace FrostFight
{
	public partial class Game : Sandbox.Game
	{
		public Hud Hud { get; set; }

		public static Game Instance => Current as Game;

		public Game()
		{
			if ( !IsServer )
				return;

			Hud = new Hud();
			AssetPrecache.DoPrecache();
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			Player player;

			if ( State is GameState.Playing )
			{
				player = new SpectatorPlayer();
				Spectators.Add( player as SpectatorPlayer );
			}
			else
			{
				player = new FrostPlayer( cl );
				Players.Add( player as FrostPlayer );
			}

			cl.Pawn = player;
			player.Respawn();
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			var frostPlayer = cl.Pawn as FrostPlayer;
			frostPlayer?.OnDisconnect();

			var playerToRemove = Players.SingleOrDefault( p => p == frostPlayer );
			if ( playerToRemove != null )
				Players.Remove( playerToRemove );

			base.ClientDisconnect( cl, reason );
		}

		[ServerCmd]
		public static void RecreateHud()
		{
			Instance.Hud?.Delete();
			Instance.Hud = new Hud();

			Log.Trace( "Created new hud element" );
		}
	}
}
