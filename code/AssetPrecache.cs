using Sandbox;

namespace FrostFight
{
	public static class AssetPrecache
	{
		public static void DoPrecache()
		{
			foreach ( var resource in Resources )
			{
				Precache.Add( resource );
			}
		}

		static string[] Resources =
		{
			// Sounds
			"sounds/soundeffects/icepick/icepick_hit1.vsnd",
			"sounds/soundeffects/icepick/icepick_hit2.vsnd",
			"sounds/soundeffects/icepick/icepick_hit3.vsnd",
			"sounds/soundeffects/icepick/icepick_hit4.vsnd",
			"sounds/soundeffects/icepick/icepick_hit5.vsnd",
			"sounds/soundeffects/icepick/icepick_hit6.vsnd",
			"sounds/soundeffects/icepick/icepick_hit7.vsnd",
			"sounds/soundeffects/icepick/icepick_hit8.vsnd",
			"sounds/soundeffects/icepick/icepick_hit9.vsnd",
			"sounds/soundeffects/icepick/icepick_hit10.vsnd",
			"sounds/soundeffects/icepick/icepick_miss.vsnd",
			"sounds/soundeffects/freezegun/snowball_fire.vsnd",
			"sounds/soundeffects/freezegun/freezegun_spray.vsnd",
			"sounds/surfaces/snow/snow_footstep_01.vsnd",
			"sounds/surfaces/snow/snow_footstep_02.vsnd",
			"sounds/surfaces/snow/snow_footstep_03.vsnd",
			"sounds/surfaces/snow/snow_footstep_04.vsnd",
			"sounds/surfaces/snow/snow_footstep_05.vsnd",
			"sounds/soundeffects/misc/become_frozen.vsnd",
			

			// Models
			"models/cosmetics/santahat/santahat.vmdl",
			"models/objects/iceblock/iceblock.vmdl",
			"models/weapons/freezegun/freezegun_view.vmdl",
			"models/weapons/freezegun/freezegun_world.vmdl",
			"models/weapons/pickaxe/pickaxe_view.vmdl",
			"models/weapons/pickaxe/pickaxe_world.vmdl",
			"models/christmas/snowball.vmdl"
		};
	}
}
