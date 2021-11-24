
using Verse;

namespace GeneticRim
{
    public class CompProperties_Exploder : CompProperties
    {


        public float wickTimeSeconds;
        public int wickTimeVariance;
        public float explosionForce;


        public CompProperties_Exploder()
        {
            
            this.compClass = typeof(CompExploder);
        }
    }
}