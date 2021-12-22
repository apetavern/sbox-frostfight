using Sandbox.UI;
using System.Threading.Tasks;

namespace FrostFight.UI.Elements
{
	public class HitmarkerContainer : Panel
	{
		public static HitmarkerContainer Instance { get; private set; }

		public HitmarkerContainer()
		{
			Instance = this;
			StyleSheet.Load( "/Code/UI/Elements/HitmarkerContainer.scss" );
		}

		public static void OnHit()
		{
			Log.Trace( "Hitmarker onHit" );
			var hitmarker = new Hitmarker();
			hitmarker.Parent = Instance;
		}
	}

	public class Hitmarker : Panel
	{
		public Hitmarker()
		{
			StyleSheet.Load( "/Code/UI/Elements/HitmarkerContainer.scss" );

			_ = Lifetime();
		}

		private async Task Lifetime()
		{
			await Task.DelaySeconds( 0.5f );
			this.Delete();
		}
	}
}
