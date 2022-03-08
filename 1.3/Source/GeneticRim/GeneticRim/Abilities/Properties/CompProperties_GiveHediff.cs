using RimWorld;
using Verse;

namespace GeneticRim
{


    public class CompProperties_GiveHediff : CompProperties_AbilityEffect
    {

        public HediffDef hediffDef;
       

        public CompProperties_GiveHediff()
        {
            this.compClass = typeof(CompGiveHediff);
        }
    }
}