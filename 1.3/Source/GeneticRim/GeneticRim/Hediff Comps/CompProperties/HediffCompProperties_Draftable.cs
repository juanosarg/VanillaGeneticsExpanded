using Verse;

namespace GeneticRim
{
    public class HediffCompProperties_Draftable : HediffCompProperties
    {

        public int checkingInterval = 500;

        public HediffCompProperties_Draftable()
        {
            this.compClass = typeof(HediffComp_Draftable);
        }
    }
}

