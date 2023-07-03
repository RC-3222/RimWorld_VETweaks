using UnityEngine;
using Verse;

namespace VEFTweaks
{
    public class VEFTweaks_Mod : Mod
    {
        public static VEFTweaks_ModSettings Settings;

        public VEFTweaks_Mod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<VEFTweaks_ModSettings>();
        }
        public override string SettingsCategory() => "VEF Tweaks";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("VEFTweaks_EnablePlaceOnWallTweaks".Translate(), ref Settings.VEFTweaks_EnablePlaceOnWallTweaks, "VEFTweaks_EnablePlaceOnWallTweaks_Tooltip".Translate());
            listingStandard.Gap();


            listingStandard.GapLine();
            listingStandard.Gap();


            var rectDefault = listingStandard.GetRect(30f);
            TooltipHandler.TipRegion(rectDefault, "VEFTweaks_ResetSettings".Translate());
            if (Widgets.ButtonText(rectDefault, "VEFTweaks_ResetSettings".Translate(), true, true, true))
            {
                Settings.ResetSettings();
            }

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

    }
}
