using Verse;
namespace GeneticRim
{
    

    public class DefExtension_HybridFailure : DefModExtension
    {
        public int failureMin = 0;
        public int failureMax = int.MaxValue;

        public bool InRange(float failureChance) => 
            this.failureMin <= failureChance && failureChance <= this.failureMax;
    }
}
