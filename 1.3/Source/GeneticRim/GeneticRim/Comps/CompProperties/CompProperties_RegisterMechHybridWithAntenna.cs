using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GeneticRim
{
    
 
    public class CompProperties_RegisterMechHybridWithAntenna : CompProperties
    {
        public int timer = 1000;

        public CompProperties_RegisterMechHybridWithAntenna()
        {
            this.compClass = typeof(CompRegisterMechHybridWithAntenna);
        }
    }
}
