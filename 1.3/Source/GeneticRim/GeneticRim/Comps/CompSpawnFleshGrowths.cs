
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace GeneticRim
{
    class CompSpawnFleshGrowths : ThingComp
    {


        public CompProperties_SpawnFleshGrowths Props
        {
            get
            {
                return (CompProperties_SpawnFleshGrowths)this.props;
            }
        }



        public override void CompTick()
        {
            base.CompTick();
       
            if (this.parent.Map != null)
            {

                CellRect rectWomb = GenAdj.OccupiedRect(parent.Position, parent.Rotation, IntVec2.One);
                rectWomb = rectWomb.ExpandedBy(1);
                foreach(IntVec3 currentWombCell in rectWomb.Cells)
                {
                    if (currentWombCell.InBounds(parent.Map))
                    {

                        List<Thing> list = parent.Map.thingGrid.ThingsListAt(currentWombCell);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if ((list[i].def == InternalDefOf.GR_ElectroWomb)|| (list[i].def == InternalDefOf.GR_LargeElectroWomb))
                            {
                                list[i].Kill();
                            }
                        }


                    }

                }


                CellRect rect = GenAdj.OccupiedRect(parent.Position, parent.Rotation, IntVec2.One);
                rect = rect.ExpandedBy(6);
                int totalCreated = 0;
                IntRange totalToMakeRange = new IntRange(5, 10);
                int totalToMake = totalToMakeRange.RandomInRange;

                foreach (IntVec3 current in rect.Cells.InRandomOrder())
                {
                    Room room = current.GetRoom(this.parent.Map);
                    if (current.InBounds(parent.Map) && room?.OutdoorsForWork==false)
                    {

                        if (totalCreated < totalToMake)
                        {
                            Thing thing = ThingMaker.MakeThing(InternalDefOf.GR_FleshGrowth_Building, null);
                            thing.Rotation = Rot4.North;
                            thing.Position = current;

                            thing.SpawnSetup(parent.Map, false);

                            totalCreated++;
                        }


                    }

                }
                this.parent.Destroy();




            }
        }
    }
}
