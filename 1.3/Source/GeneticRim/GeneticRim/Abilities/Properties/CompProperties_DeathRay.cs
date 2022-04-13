using RimWorld;
using Verse;

namespace GeneticRim
{


    public class CompProperties_DeathRay : CompProperties_AbilityEffect
    {

        public int duration;
     


        public CompProperties_DeathRay()
        {
            this.compClass = typeof(CompDeathRay);
        }
    }
}