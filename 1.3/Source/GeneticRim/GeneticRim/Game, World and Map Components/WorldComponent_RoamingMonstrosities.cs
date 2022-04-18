using RimWorld;
using RimWorld.Planet;
using Verse;

namespace GeneticRim
{



    public class WorldComponent_RoamingMonstrosities : WorldComponent
    {
        public int tickCounter;
        public int ticksToNextAssault = 60000 * 15;


        public WorldComponent_RoamingMonstrosities(World world) : base(world)
        {
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();



        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();


            if (!GeneticRim_Mod.settings.GR_DisableHybridRaids)
            {
                if (Current.Game.storyteller.difficultyDef != DifficultyDefOf.Peaceful)
                {


                    if (tickCounter > ticksToNextAssault)
                    {
                        if (Find.FactionManager.FirstFactionOfDef(InternalDefOf.GR_RoamingMonstrosities) != null)
                        {
                            IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, this.world);
                            parms.target = Find.AnyPlayerHomeMap;
                            IncidentDef def = InternalDefOf.GR_ManhunterMonstrosities;
                            def.Worker.TryExecute(parms);
                            
                            ticksToNextAssault = (int)(60000 * Rand.RangeInclusive(10, 30) * GeneticRim_Mod.settings.GR_RaidsRate);
                            tickCounter = 0;
                        }



                    }
                    tickCounter++;
                }
            }
            



            
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));
            Scribe_Values.Look(ref this.ticksToNextAssault, nameof(this.ticksToNextAssault));
        }
    }
}
