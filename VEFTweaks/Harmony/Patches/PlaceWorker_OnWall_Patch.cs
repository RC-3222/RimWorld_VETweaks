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
    private static bool IsValidRotation(bool isDoubleSided, Building building, Rot4 rot)
    {
        if (building.def.GetModExtension<PlaceableOnWallDefExtension>() is { IsDoubleSided:true })
        {
            return building.Rotation != rot && building.Rotation.Opposite != rot;
        }

        return isDoubleSided ? building.Rotation != rot && building.Rotation != rot.Opposite : building.Rotation != rot;
    }

    // Should think about making this into transpiler
    [HarmonyPostfix]
    public static void AllowPlacing_Postfix(ref BuildableDef checkingDef, ref IntVec3 loc, ref Rot4 rot, ref Map map, ref AcceptanceReport __result)
    {
        //Log.Warning($"Checking placement... setting is enabled: {VEFTWeaks_Mod.settings.VEFTweaks_EnablePlaceOnWallTweaks}, Building is doublesided: {checkingDef.GetModExtension<PlaceableOnWallDefExtension>() is { IsDoubleSided: true }}");

        // no applying if the setting is not enabled
        if (!VEFTweaks_Mod.settings.VEFTweaks_EnablePlaceOnWallTweaks) return;

        // no need to apply if the result is already false
        if (!__result) return;

        var isDoubleSided = checkingDef.GetModExtension<PlaceableOnWallDefExtension>() is { IsDoubleSided: true };

        if (isDoubleSided)
        {
            IntVec3 facingCellOpposite = loc + rot.Opposite.FacingCell;
            if (facingCellOpposite.InBounds(map))
            {
                Building edifice = facingCellOpposite.GetEdifice(map);
                if (edifice != null && (edifice.def.defName.ToLower().Contains("wall") || edifice.def.IsSmoothed))
                {
                    __result = false;
                    return;
                }
            }
        }

        if (loc.GetThingList(map) is List<Thing> { Count: > 0 } existingThings) {
            foreach (var existingStuff in existingThings)
            {
                if (
                    existingStuff is Building { def.placeWorkers: not null } building
                    && building.def.placeWorkers.Contains(typeof(PlaceWorker_OnWall))
                    && !IsValidRotation(isDoubleSided, building, rot)
                    )
                {
                    __result = false;
                    return;
                }
            }
        }

    }
}