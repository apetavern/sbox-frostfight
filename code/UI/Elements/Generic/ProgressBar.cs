using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FrostFight.UI.Elements.Generic
{
	public class ProgressBar : Panel
	{
		protected Panel innerPanel;

		public ProgressBar()
		{
			AddClass( "progress-bar" );
			StyleSheet.Load( "/UI/Elements/Generic/ProgressBar.scss" );
			innerPanel = Add.Panel( "inner" );
		}

		public override void Tick()
		{
			base.Tick();

			//
			// Hide inner panel entirely if value is almost 0
			//

			float borderSize = 8f; // Our borders are 4px thick, and we have them on both sides = 8px
			if ( innerPanel.Box.Rect.width.AlmostEqual( 0, borderSize ) )
				innerPanel.Style.Opacity = 0;
			else
				innerPanel.Style.Opacity = 1;
		}
	}
}
