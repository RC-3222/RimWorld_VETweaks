using HarmonyLib;
using System.Collections.Generic;
using VanillaFurnitureExpanded;
using VEFTweaks.DefModExtensions;
using Verse;

namespace VEFTweaks.Harmony;

[HarmonyPatch(typeof(PlaceWorker_OnWall))]
[HarmonyPatch(nameof(PlaceWorker_OnWall.AllowsPlacing))]
public static class PlaceWorker_OnWall_Patch
{
    private static bool IsValidRotation(bool isDoubleSided, Building existingBuilding, Rot4 rot)
    {
        if (existingBuilding.def.GetModExtension<PlaceableOnWallDefExtension>() is { IsDoubleSided:true })
        {
            return existingBuilding.Rotation != rot && existingBuilding.Rotation.Opposite != rot;
        }

        return isDoubleSided ? existingBuilding.Rotation != rot && existingBuilding.Rotation != rot.Opposite : existingBuilding.Rotation != rot;
    }

    // Should think about making this into transpiler
    [HarmonyPostfix]
    public static void AllowPlacing_Postfix(ref BuildableDef checkingDef, ref IntVec3 loc, ref Rot4 rot, ref Map map, ref AcceptanceReport __result)
    {
        //Log.Warning($"Checking placement... setting is enabled: {VEFTWeaks_Mod.settings.VEFTweaks_EnablePlaceOnWallTweaks}, Building is doublesided: {checkingDef.GetModExtension<PlaceableOnWallDefExtension>() is { IsDoubleSided: true }}");

        // no applying if the setting is not enabled
        if (!VEFTweaks_Mod.Settings.VEFTweaks_EnablePlaceOnWallTweaks) return;

        // no need to apply if the result is already false
        if (!__result) return;

        var isDoubleSided = checkingDef.GetModExtension<PlaceableOnWallDefExtension>() is { IsDoubleSided: true };

        if (isDoubleSided)
        {
            var facingCellOpposite = loc + rot.Opposite.FacingCell;
            if (facingCellOpposite.InBounds(map))
            {
                var edifice = facingCellOpposite.GetEdifice(map);
                if (edifice != null && (edifice.def.defName.ToLower().Contains("wall") || edifice.def.IsSmoothed))
                {
                    __result = false;
                    return;
                }
            }
        }

        if (loc.GetThingList(map) is { Count: > 0 } existingThings) {
            foreach (var existingThing in existingThings)
            {
                if (
                    existingThing is Building { def.placeWorkers: not null } existingBuilding
                    && existingBuilding.def.placeWorkers.Contains(typeof(PlaceWorker_OnWall))
                    && !IsValidRotation(isDoubleSided, existingBuilding, rot)
                    )
                {
                    __result = false;
                    return;
                }
            }
        }

    }
}