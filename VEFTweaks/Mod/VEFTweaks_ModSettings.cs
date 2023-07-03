using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VEFTweaks
{
    public class VEFTweaks_ModSettings : ModSettings

    {
        public bool VEFTweaks_EnablePlaceOnWallTweaks = false;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref VEFTweaks_EnablePlaceOnWallTweaks, "VEFTweaks_EnablePlaceOnWallTweaks", false);

        }

        public void ResetSettings()
        {
            VEFTweaks_EnablePlaceOnWallTweaks = false;
        }
    }
}
