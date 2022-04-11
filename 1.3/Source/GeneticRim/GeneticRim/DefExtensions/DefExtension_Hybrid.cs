using System.Collections.Generic;
using Verse;

namespace GeneticRim
{


    public class DefExtension_Hybrid : DefModExtension
    {
        public ThingDef dominantGenome;
        public ThingDef secondaryGenome;
        public bool carryingIncrease = false;
        public float carryingFactor = 1.5f;
    }
}
