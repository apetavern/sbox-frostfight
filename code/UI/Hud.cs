using FrostFight.UI.Elements;
using Sandbox;
using Sandbox.UI;
using System.ComponentModel;

namespace FrostFight.UI
{
	public partial class Hud : HudEntity<RootPanel>
	{
		public static Hud Instance { get; set; }

		public Hud()
		{
			if ( IsClient )
			{
				Instance = this;
				RootPanel.StyleSheet.Load( "/UI/Hud.scss" );

				AddOtherElements();
			}
		}

		public void AddPlayingElements()
		{
			RootPanel.AddChild<Vitals>();
			RootPanel.AddChild<Crosshair>();
			RootPanel.AddChild<CurrentWeapon>();
		}

		public void AddOtherElements()
		{
			RootPanel.AddChild<WinnerScreen>();
			RootPanel.AddChild<PlayerList>();
			RootPanel.AddChild<GameState>();

			// TODO - if we have time - custom variants:
			RootPanel.AddChild<NameTags>();
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<VoiceList>();
		}

		[ClientRpc]
		public static void SetHudState( bool playing )
		{
			Instance?.RootPanel.DeleteChildren();

			if ( playing )
			{
				Instance?.AddPlayingElements();
				Instance?.AddOtherElements();
			}

			else
			{
				Instance?.AddOtherElements();
			}
		}
	}
}
