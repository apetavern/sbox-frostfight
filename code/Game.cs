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
			if ( IsServer )
				Hud = new Hud();
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new FrostPlayer( cl );
			cl.Pawn = player;

			if ( State is GameState.Playing )
				Spectators.Add( player );
			else
				Players.Add( player );
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
