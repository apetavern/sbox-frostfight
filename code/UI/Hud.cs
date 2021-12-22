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
				RootPanel.AddChild<WeaponSwitcher>();
				RootPanel.AddChild<FreezeLevel>();

				// TODO:
				RootPanel.AddChild<ChatBox>();
				RootPanel.AddChild<KillFeed>();
				RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			}
		}
	}
}
