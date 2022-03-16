using RimWorld;
using Verse;
using Verse.Sound;
using System.Collections.Generic;

namespace GeneticRim

{
    public class CompApplyAgeDiseases : ThingComp, AnimalBehaviours.PawnGizmoProvider
    {
        public int tickCounter = 0;

        public const int ticksToApply = 10800000;  // 3 years
        public const int ticksToReapply = ticksToApply-900000;  // 3 years - 15 days

        List<HediffDef> hediffsToApply = new List<HediffDef>() { InternalDefOf.GR_MuscleNecrosis, InternalDefOf.GR_AnimalTuberculosis, InternalDefOf.GR_AnimalAbasia };

        public override void PostExposeData()
        {
            base.PostExposeData();

          
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));

        }

        public override void CompTick()
        {
            base.CompTick();
            tickCounter++;

            if (tickCounter >= ticksToApply)
            {
                Pawn pawn = this.parent as Pawn;

                if (pawn != null && pawn.Map != null)
                {
                    HediffDef randomHediff = hediffsToApply.RandomElement();
                    Hediff hediff = null;
                    foreach (HediffDef hediffPresent in hediffsToApply)
                    {
                        hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffPresent);
                        if (hediff != null)
                        {
                            break;
                        }
                    }
                  
                    if (hediff == null)
                    {
                        pawn.health.AddHediff(randomHediff);
                    }


                }
                tickCounter = ticksToReapply;
            }
        }

        public IEnumerable<Gizmo> GetGizmos()
        {

            if (Prefs.DevMode)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "DEBUG: Give age related diseases";
                command_Action.icon = TexCommand.DesirePower;
                command_Action.action = delegate
                {
                    tickCounter = ticksToApply - 10;
                };
                yield return command_Action;
            }
        }



    }
}
