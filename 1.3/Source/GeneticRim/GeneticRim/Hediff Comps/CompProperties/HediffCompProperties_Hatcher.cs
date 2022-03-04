using Verse;

namespace GeneticRim
{
    public class HediffCompProperties_Hatcher : HediffCompProperties
    {

        public float hatcherDaystoHatch = 1f;
        public int baseAmount = 5;
        public ThingDef thingToHatch;


        public HediffCompProperties_Hatcher()
        {
            this.compClass = typeof(HediffComp_Hatcher);
        }
    }
}
