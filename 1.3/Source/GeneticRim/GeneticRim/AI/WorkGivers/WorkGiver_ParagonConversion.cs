using System;
using Verse;
using Verse.AI;
using RimWorld;
using System.Collections.Generic;

namespace GeneticRim
{
    public class WorkGiver_ParagonConversion : WorkGiver_Scanner
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {

            return pawn.Map?.GetComponent<ArchotechExtractableAnimals_MapComponent>()?.paragonsToCarry?.Keys;


        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {

            bool result;
            if (t == null || t.IsBurning() )
            {

                result = false;
            }

            else
            {

                if (!t.IsForbidden(pawn))
                {
                    LocalTargetInfo target = t;
                    if (pawn.CanReserve(target, 1, -1, null, forced))
                    {
                        result = true;
                        return result;
                    }
                }
                result = false;
            }
            return result;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            LocalTargetInfo building = pawn.Map.GetComponent<ArchotechExtractableAnimals_MapComponent>().paragonsToCarry[(Pawn)t];

            return new Job(InternalDefOf.GR_ParagonConversionJob, t, building);
        }
    }
}
