
using Verse;
using RimWorld;

namespace GeneticRim
{
    class HediffComp_Draftable : HediffComp
    {

        public int tickCounter = 0;

        public HediffCompProperties_Draftable Props
        {
            get
            {
                return (HediffCompProperties_Draftable)this.props;
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            tickCounter++;
            if (tickCounter> Props.checkingInterval)
            {
                if (this.parent.pawn.drafter == null) { this.parent.pawn.drafter = new Pawn_DraftController(this.parent.pawn); }
                if (this.parent.pawn.equipment == null) { this.parent.pawn.equipment = new Pawn_EquipmentTracker(this.parent.pawn); }
                DraftingList.AddAnimalToList(this.parent.pawn, new bool[14]);
                tickCounter = 0;
            }
        }

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            if (this.parent.pawn.drafter == null) { this.parent.pawn.drafter = new Pawn_DraftController(this.parent.pawn); }
            if (this.parent.pawn.equipment == null) { this.parent.pawn.equipment = new Pawn_EquipmentTracker(this.parent.pawn); }
            DraftingList.AddAnimalToList(this.parent.pawn, new bool[14]);
        }

        public override void CompPostPostRemoved()
        {
            DraftingList.RemoveAnimalFromList(this.parent.pawn);
        }

        public override void Notify_PawnDied()
        {
            DraftingList.RemoveAnimalFromList(this.parent.pawn);
        }

        public override void Notify_PawnKilled()
        {
            DraftingList.RemoveAnimalFromList(this.parent.pawn);
        }
    }
}
