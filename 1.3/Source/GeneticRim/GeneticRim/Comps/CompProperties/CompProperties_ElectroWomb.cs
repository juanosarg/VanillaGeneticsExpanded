using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticRim
{
    using Verse;

    public class CompProperties_ElectroWomb : CompProperties
    {
        public float maxBodySize = float.MaxValue;

        public CompProperties_ElectroWomb()
        {
            this.compClass = typeof(CompElectroWomb);
        }
    }
}
