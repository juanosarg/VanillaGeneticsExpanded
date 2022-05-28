using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace GeneticRim
{


    public class CompArchotechGrowthCell : ThingComp
    {
       

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
           
            List<Building> list = this?.parent?.Map?.listerBuildings?.allBuildingsColonist;
            if (list != null)
            {
                foreach (Building building in list)
                {
                    Building_ArchoWomb womb = building as Building_ArchoWomb;

                    if (womb != null)
                    {
                        yield return new FloatMenuOption("GR_ArchoGrowthCell_InsertInElectroWomb".Translate(building.LabelCap), () =>
                                                                                                                           {
                                                                                                                               Job makeJob = JobMaker.MakeJob(InternalDefOf.GR_InsertArchotechGrowthCell, womb,
                                                                                                                                   this.parent);
                                                                                                                               makeJob.haulMode = HaulMode.ToCellNonStorage;
                                                                                                                               makeJob.count = 1;
                                                                                                                               selPawn.jobs?.StartJob(makeJob, JobCondition.InterruptForced);
                                                                                                                           });
                    }
                }
            }
        }

       
    }
}
