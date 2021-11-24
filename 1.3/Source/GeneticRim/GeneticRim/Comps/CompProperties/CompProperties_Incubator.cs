using Verse;

namespace GeneticRim
{
    public class CompProperties_Incubator : CompProperties
    {
        public float hatcherDaystoHatch = 1f;

        public PawnKindDef hatcherPawn;
        public PawnKindDef hatcherPawnSecondary = null;


        public CompProperties_Incubator()
        {
            this.compClass = typeof(CompIncubator);
        }
    }
}