using Sandbox;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrostFight.UI
{
	public class NameTags : Panel
	{
		Dictionary<Player, BaseNameTag> ActiveTags = new Dictionary<Player, BaseNameTag>();

		public float MaxDrawDistance = 400;
		public int MaxTagsToShow = 5;

		public NameTags()
		{
			StyleSheet.Load( "/UI/NameTags.scss" );
		}

		public BaseNameTag CreateNameTag( Player player )
		{
			if ( player.Client == null )
				return null;

			var tag = new FrostNameTag( player );
			tag.Parent = this;
			return tag;
		}

		public override void Tick()
		{
			base.Tick();

			var deleteList = new List<Player>();
			deleteList.AddRange( ActiveTags.Keys );

			int count = 0;
			foreach ( var player in Entity.All.OfType<Player>().OrderBy( x => Vector3.DistanceBetween( x.Position, CurrentView.Position ) ) )
			{
				if ( UpdateNameTag( player ) )
				{
					deleteList.Remove( player );
					count++;
				}

				if ( count >= MaxTagsToShow )
					break;
			}

			foreach ( var player in deleteList )
			{
				ActiveTags[player].Delete();
				ActiveTags.Remove( player );
			}

		}

		public bool UpdateNameTag( Player player )
		{
			// Don't draw local player
			if ( player == Local.Pawn )
				return false;

			if ( player.LifeState != LifeState.Alive )
				return false;

			//
			// Where we putting the label, in world coords
			//
			var head = player.GetAttachment( "hat" ) ?? new Transform( player.EyePos );
			var labelPos = head.Position + Vector3.Up * 8;

			//
			// Are we too far away?
			//
			float dist = labelPos.Distance( CurrentView.Position );
			if ( dist > MaxDrawDistance )
				return false;

			//
			// Are we looking in this direction?
			//
			var lookDir = (labelPos - CurrentView.Position).Normal;
			if ( CurrentView.Rotation.Forward.Dot( lookDir ) < 0.5 )
				return false;

			//
			// Can we see them?
			//
			var tr = Trace.Ray( Local.Pawn.EyePos, labelPos ).WorldOnly().Run();
			if ( tr.Hit )
				return false;

			var alpha = dist.LerpInverse( MaxDrawDistance, MaxDrawDistance * 0.1f, true );

			// If I understood this I'd make it proper function
			var objectSize = 0.05f / dist / (2.0f * MathF.Tan( (CurrentView.FieldOfView / 2.0f).DegreeToRadian() )) * 1500f;

			objectSize = objectSize.Clamp( 0.25f, 2.0f );

			if ( !ActiveTags.TryGetValue( player, out var tag ) )
			{
				tag = CreateNameTag( player );
				if ( tag != null )
				{
					ActiveTags[player] = tag;
				}
			}

			if ( tag == null )
				return false;

			tag.UpdateFromPlayer( player );

			var screenPos = labelPos.ToScreen();

			tag.Style.Left = Length.Fraction( screenPos.x );
			tag.Style.Top = Length.Fraction( screenPos.y );
			tag.Style.Opacity = alpha;

			var transform = new PanelTransform();
			transform.AddTranslateY( Length.Fraction( -1.0f ) );
			transform.AddScale( objectSize );
			transform.AddTranslateX( Length.Fraction( -0.5f ) );

			tag.Style.Transform = transform;
			tag.Style.Dirty();

			return true;
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
