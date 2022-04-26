using System;
using Verse;
using RimWorld;

namespace GeneticRim
{
    public class PlaceWorker_MustBeRoofed : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {

            Room room = loc.GetRoom(map);
            if (room != null)
            {
                if (room.OutdoorsForWork || (!map.roofGrid.Roofed(loc)))
                {
                    return new AcceptanceReport("GR_MustPlaceRoofed".Translate());
                }
            }




            return true;
        }
    }
}