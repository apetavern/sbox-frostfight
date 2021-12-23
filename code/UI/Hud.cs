﻿using FrostFight.UI.Elements;
using Sandbox;
using Sandbox.UI;

namespace FrostFight.UI
{
	public partial class Hud : HudEntity<RootPanel>
	{
		public Hud()
		{
			if ( IsClient )
			{
				RootPanel.StyleSheet.Load( "/Code/UI/Hud.scss" );

				RootPanel.AddChild<Crosshair>();
				RootPanel.AddChild<CurrentWeapon>();
				RootPanel.AddChild<WinnerScreen>();
				RootPanel.AddChild<Vitals>();
				RootPanel.AddChild<GameState>();

				// TODO - if we have time - custom variants:
				RootPanel.AddChild<NameTags>();
				RootPanel.AddChild<ChatBox>();
				RootPanel.AddChild<VoiceList>();
				RootPanel.AddChild<KillFeed>();
				RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			}
		}
	}
}
