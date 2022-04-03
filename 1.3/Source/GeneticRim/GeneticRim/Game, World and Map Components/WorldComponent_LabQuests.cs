using RimWorld;
using RimWorld.Planet;
using Verse;
using RimWorld.QuestGen;

namespace GeneticRim
{



    public class WorldComponent_LabQuests : WorldComponent
    {
        public int tickCounter;
        public int ticksToNextQuest = 60000 * 10;


        public WorldComponent_LabQuests(World world) : base(world)
        {
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();



        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();



            if (tickCounter > ticksToNextQuest)
            {

                Slate slate = new Slate();
                Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(InternalDefOf.GR_OpportunitySite_AbandonedLab, slate);

                QuestUtility.SendLetterQuestAvailable(quest);
                ticksToNextQuest = (int)(60000 * Rand.RangeInclusive(10, 30) * GeneticRim_Mod.settings.GR_QuestRate);
                tickCounter = 0;




            }
            tickCounter++;





        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));
            Scribe_Values.Look(ref this.ticksToNextQuest, nameof(this.ticksToNextQuest));
        }
    }
}
