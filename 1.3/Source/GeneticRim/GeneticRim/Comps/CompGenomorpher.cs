using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GeneticRim
{
    public class CompGenomorpher : ThingComp
    {
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_Action
            {
                defaultLabel = "GR_DesignateGrowthCell".Translate(),
                defaultDesc = "GR_DesignateGrowthCellDesc".Translate(),
                action = delegate
                {
                    Find.WindowStack.Add(new Window_DesignateGrowthCell(this));
                }
            };
        }
    }
}
