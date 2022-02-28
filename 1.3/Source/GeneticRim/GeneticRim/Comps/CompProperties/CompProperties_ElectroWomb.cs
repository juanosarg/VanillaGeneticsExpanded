using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GeneticRim
{
    

    public class CompProperties_ElectroWomb : CompProperties
    {
        public float maxBodySize = float.MaxValue;

        public bool isLarge = false;

        public int hoursProcess = 1;

        public CompProperties_ElectroWomb()
        {
            this.compClass = typeof(CompElectroWomb);
        }
    }
}
