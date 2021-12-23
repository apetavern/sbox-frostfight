using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Collections.Generic;
using System.Linq;

namespace FrostFight.UI.Elements
{
	public class PlayerList : Panel
	{
		private Dictionary<long, Panel> Players { get; set; } = new();

		public PlayerList()
		{
			StyleSheet.Load( "/UI/Elements/PlayerList.scss" );
		}

		public void Update()
		{
			foreach ( var client in Client.All )
			{
				if ( client.Pawn is FrostPlayer frostPlayer && !Players.ContainsKey( client.PlayerId ) && !frostPlayer.IsFreezer )
				{
					Panel avatar = Add.Panel( "avatar" );
					avatar.Style.SetBackgroundImage( $"avatar:{client.PlayerId}" );
					avatar.BindClass( "frozen", () => frostPlayer.IsFrozen );
					avatar.Add.Icon( "ac_unit", "frozen-icon" );
					Players.Add( client.PlayerId, avatar );
				}
			}

			var playersCopy = Players.ToArray();
			foreach ( var player in playersCopy )
			{
				var client = Client.All.FirstOrDefault( c => c.PlayerId == player.Key );
				if ( client == null || client.Pawn is FrostPlayer { IsFreezer: true } )
				{
					player.Value.Delete();
					Players.Remove( player.Key );
				}
			}
		}

		public override void Tick()
		{
			base.Tick();
			Update();
		}
	}
}
