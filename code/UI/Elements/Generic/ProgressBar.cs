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
	}
}
