using FrostFight.UI;
using Sandbox;

namespace FrostFight
{
	public partial class Game : Sandbox.Game
	{
		public Hud Hud { get; set; }

		public Game()
		{
			if ( IsServer )
				Hud = new Hud();
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new Player();
			cl.Pawn = player;

			player.Respawn();
		}
	}
}
