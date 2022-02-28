using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GeneticRim
{
    

    public class CompProperties_RandomHybridSpawner : CompProperties
    {
       

        public CompProperties_RandomHybridSpawner()
        {
            this.compClass = typeof(CompRandomHybridSpawner);
        }
    }
}
