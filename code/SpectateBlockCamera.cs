using Sandbox;

namespace FrostFight
{
	public class SpectateBlockCamera : Camera
	{
		public AnimEntity Target { get; set; }

		private Angles OrbitAngles;
		private float OrbitDistance { get; set; } = 320;

		public override void Update()
		{
			var pawn = Target ?? Local.Pawn as AnimEntity;

			if ( pawn == null )
				return;

			Position = pawn.Position;
			Vector3 targetPos;

			Position += Vector3.Up * (40 * pawn.Scale);
			Rotation = Rotation.From( OrbitAngles );

			targetPos = Position + Rotation.Backward * OrbitDistance;

			var tr = Trace.Ray( Position, targetPos ).WorldOnly()
				.Radius( 8 )
				.Run();

			Position = tr.EndPos;
			FieldOfView = 70;

			Viewer = null;
		}

		public override void BuildInput( InputBuilder input )
		{
			OrbitDistance -= input.MouseWheel * 10;
			OrbitDistance = OrbitDistance.Clamp( 0, 1000 );

			OrbitAngles.yaw += input.AnalogLook.yaw;
			OrbitAngles.pitch += input.AnalogLook.pitch;
			OrbitAngles = OrbitAngles.Normal;
			OrbitAngles.pitch = OrbitAngles.pitch.Clamp( -89, 89 );

			input.ViewAngles = OrbitAngles;
			input.AnalogMove = -input.AnalogMove;

			input.Clear();
			input.StopProcessing = true;

			base.BuildInput( input );
		}
	}
}
