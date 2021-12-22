using FrostFight.UI.Elements;
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
				RootPanel.AddChild<Crosshair>();
				RootPanel.AddChild<WeaponSwitcher>();
				RootPanel.AddChild<FreezeLevel>();

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
