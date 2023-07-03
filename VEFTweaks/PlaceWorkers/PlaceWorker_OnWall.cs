﻿using RimWorld;
using VEFTweaks.DefModExtensions;
using Verse;

// TODO: Think some more...

namespace VEFTweaks.PlaceWorkers
{
    public class PlaceWorker_OnWall : PlaceWorker
    {
        private static bool IsValidRotation(bool isDoubleSided, Building building, Rot4 rot)
        {
            if (building.def.GetModExtension<PlaceableOnWallDefExtension>() is { IsDoubleSided: true })
            {
                return building.Rotation != rot && building.Rotation.Opposite != rot;
            }

            return isDoubleSided ? building.Rotation != rot && building.Rotation != rot.Opposite : building.Rotation != rot;
        }

        private static bool CellHasWall(IntVec3 cell, Map map)
        {
            if (cell.InBounds(map))
            {
                var edifice = cell.GetEdifice(map);
                if (edifice != null && (edifice.def.defName.ToLower().Contains("wall") || edifice.def.IsSmoothed))
                {
                    return true;
                }
            }

            return false;
        }

        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            var facingCell = loc + rot.FacingCell;
            if (CellHasWall(facingCell, map))
            {
                return false;
            }

            var isDoubleSided = checkingDef.GetModExtension<PlaceableOnWallDefExtension>() is { IsDoubleSided: true };

            if (isDoubleSided)
            {
                var facingCellOpposite = loc + rot.Opposite.FacingCell;
                if (CellHasWall(facingCellOpposite, map))
                {
                    return false;
                }
            }


            if (loc.InBounds(map)) {

                if (loc.GetThingList(map) is { Count: > 0 } existingThings)
                {
                    foreach (var existingStuff in existingThings)
                    {
                        if (
                            existingStuff is Building { def.placeWorkers: not null } building
                            && building.def.placeWorkers.Contains(typeof(PlaceWorker_OnWall))
                            && !IsValidRotation(isDoubleSided, building, rot)
                            )
                        {
                            return false;
                        }
                    }
                }

                return true; 
            }

            return false;
        }
    }
}