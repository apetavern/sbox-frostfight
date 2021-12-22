using Sandbox;
using Sandbox.UI;

namespace FrostFight.UI
{
	public class NameTags : Sandbox.UI.NameTags
	{
		public NameTags()
		{
			StyleSheet.Load( "/Code/UI/NameTags.scss" );
		}

		public override BaseNameTag CreateNameTag( Player player )
		{
			if ( player.Client == null )
				return null;

			var tag = new FrostNameTag( player );
			tag.Parent = this;
			return tag;
		}
	}

	public class FrostNameTag : BaseNameTag
	{
		Panel freezePanel;

		public FrostNameTag( Player player ) : base( player )
		{
			freezePanel = Add.Panel( "freeze-level" );

			BindClass( "frozen", () =>
			{
				return (player is FrostPlayer { IsFrozen: true });
			} );
		}

		public override void UpdateFromPlayer( Player player )
		{
			// Nothing to do unless we're showing health and shit
			if ( player is not FrostPlayer frostPlayer )
				return;

			if ( frostPlayer.IsFrozen )
				freezePanel.Style.Width = Length.Percent( frostPlayer.IceBlock.Health );
			else
				freezePanel.Style.Width = Length.Percent( frostPlayer.CurrentFreezeAmount );
		}
	}
}
