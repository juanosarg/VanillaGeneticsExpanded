using Verse;

namespace GeneticRim
{
    public class CompProperties_DieUnlessReset : CompProperties
    {

      

        public int timeToDieInTicks = 60000;
        public bool manhunterButNotDie = false;
        public bool effect = false;
        public ThingDef effectFilth;
        public string message;

        public CompProperties_DieUnlessReset()
        {
            this.compClass = typeof(CompDieUnlessReset);
        }
    }
}
