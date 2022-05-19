
using RimWorld;
using Verse;
namespace GeneticRim
{
    public class SpecialThingFilterWorker_Unextracted : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Corpse pawn = t as Corpse;
            if (pawn != null)
            {
                if (pawn.InnerPawn.health.hediffSet.HasHediff(InternalDefOf.GR_ExtractedBrain))
                {
                    return false;
                }
                else return true;
            }
            return false;

           
        }

       
       
    }
}
