using RimWorld;
using RimWorld.Planet;
using Verse;

namespace GeneticRim
{



    public class WorldComponent_FailuresChecker : WorldComponent
    {
        public int tickCounter;
        public int checkInterval = 10000;


        public WorldComponent_FailuresChecker(World world) : base(world)
        {
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();




            if (tickCounter > checkInterval)
            {
                foreach(Thing thing in StaticCollectionsClass.failures_in_map)
                {
                    Pawn pawn = thing as Pawn;

                    if(pawn!=null && pawn.Dead)
                    {
                        StaticCollectionsClass.RemoveFailuresFromList(thing);
                    }

                    if (thing.Map == null)
                    {
                        StaticCollectionsClass.RemoveFailuresFromList(thing);
                    }

                }



            }
            tickCounter++;




        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));

        }
    }
}
