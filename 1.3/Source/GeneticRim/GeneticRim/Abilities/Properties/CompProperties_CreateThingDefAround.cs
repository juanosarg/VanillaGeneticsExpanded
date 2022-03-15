using RimWorld;
using Verse;

namespace GeneticRim
{


    public class CompProperties_CreateThingDefAround : CompProperties_AbilityEffect
    {
       
        public int radius;     
        public ThingDef thingCreated = null;
        public float thingCreatedChance = 0;
        public int count =0;

        public CompProperties_CreateThingDefAround()
        {
            this.compClass = typeof(CompCreateThingDefAround);
        }
    }
}