using UnityEngine;
using Verse;

namespace VEFTweaks
{
    public class VEFTWeaks_Mod : Mod
    {
        public static VEFTweaks_ModSettings settings;

        public VEFTWeaks_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<VEFTweaks_ModSettings>();
        }
        public override string SettingsCategory() => "VEF Tweaks";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("VEFTweaks_EnablePlaceOnWallTweaks".Translate(), ref settings.VEFTweaks_EnablePlaceOnWallTweaks, "VEFTweaks_EnablePlaceOnWallTweaks_Tooltip".Translate());
            listingStandard.Gap();


            listingStandard.GapLine();
            listingStandard.Gap();


            Rect rectDefault = listingStandard.GetRect(30f);
            TooltipHandler.TipRegion(rectDefault, "VEFTweaks_ResetSettings".Translate());
            if (Widgets.ButtonText(rectDefault, "VEFTweaks_ResetSettings".Translate(), true, true, true))
            {
                settings.ResetSettings();
            }

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

    }
}
