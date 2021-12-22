using FrostFight.UI;
using Sandbox;

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

			var player = new FrostPlayer();
			cl.Pawn = player;

			player.Respawn();
		}

		[ClientCmd]
		public static void RecreateHud()
		{
			Instance.Hud?.Delete();
			Instance.Hud = new Hud();

			Log.Trace( "Created new hud element" );
		}
	}
}
