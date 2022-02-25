using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticRim
{
    using Verse;

    public class CompProperties_RandomHybridSpawner : CompProperties
    {
       

        public CompProperties_RandomHybridSpawner()
        {
            this.compClass = typeof(CompRandomHybridSpawner);
        }
    }
}
