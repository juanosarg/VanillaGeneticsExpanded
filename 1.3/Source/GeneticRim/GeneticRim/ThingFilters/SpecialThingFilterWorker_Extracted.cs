
using RimWorld;
using Verse;
namespace GeneticRim
{
    public class SpecialThingFilterWorker_Extracted : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Corpse pawn = t as Corpse;
            if (pawn != null)
            {
                if (pawn.InnerPawn.health.hediffSet.HasHediff(InternalDefOf.GR_ExtractedBrain))
                {
                    return true;
                }
                else return false;
            }
            return false;


        }



    }
}
