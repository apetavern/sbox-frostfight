using Sandbox;
using Sandbox.UI;

namespace FrostFight.UI.Elements
{
	public class Vitals : Panel
	{
		FreezeLevel freezeLevel;

		public Vitals()
		{
			StyleSheet.Load( "/UI/Elements/Vitals.scss" );
			AddChild<Stamina>();
			freezeLevel = AddChild<FreezeLevel>();
		}

		public override void Tick()
		{
			base.Tick();

			if ( Local.Pawn is FrostPlayer { IsFreezer: true } )
				freezeLevel.Style.Display = DisplayMode.None;
			else
				freezeLevel.Style.Display = DisplayMode.Flex;
		}
	}
}
