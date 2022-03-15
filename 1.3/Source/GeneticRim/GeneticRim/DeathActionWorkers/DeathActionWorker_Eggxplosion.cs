using RimWorld;
using Verse;
using System;
using System.Threading;




namespace GeneticRim
{
    public class DeathActionWorker_Eggxplosion : DeathActionWorker
    {



        public override void PawnDied(Corpse corpse)
        {
           
            

            CellRect rect = GenAdj.OccupiedRect(corpse.Position, corpse.Rotation, IntVec2.One);
            rect = rect.ExpandedBy(1);
            int total = 3;
            int totalCreated = 0;

            foreach (IntVec3 current in rect.Cells.InRandomOrder())
            {
                if (current.InBounds(corpse.Map))
                {
                    if (totalCreated < total) {
                        Thing thing = ThingMaker.MakeThing(InternalDefOf.GR_EggBomb, null);
                        thing.Rotation = Rot4.North;
                        thing.Position = current;
                        thing.SpawnSetup(corpse.Map, false);
                    }
                    totalCreated++;
                }
            }
        }

    }


}
