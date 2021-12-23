using Sandbox;

namespace FrostFight
{
	public class SpectatorPlayer : Player
	{
		public override void Respawn()
		{
			Controller = new NoclipController();
			Camera = new FirstPersonCamera();

			Position = Vector3.Zero + Vector3.Up * 150;
		}

		public override void OnKilled()
		{
			// Do nothing.
		}
	}
}
