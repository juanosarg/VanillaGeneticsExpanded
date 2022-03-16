

using RimWorld;
using Verse;
using Verse.Sound;
using System;
using Verse.AI;
using System.Linq;

namespace GeneticRim
{
    public class HediffComp_Vomiting : HediffComp
    {
        int checkDownCounter=0;

        public HediffCompProperties_Vomiting Props
        {
            get
            {
                return (HediffCompProperties_Vomiting)this.props;
            }
        }



        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            checkDownCounter++;

            if (checkDownCounter > Props.mtbVomitingBlood)
            {
                if(!parent.pawn.InMentalState && !parent.pawn.Downed)
                {

                    parent.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: true);

                    BodyPartRecord lung = parent.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).
                                   FirstOrDefault((BodyPartRecord x) => x.def == DefDatabase<BodyPartDef>.GetNamedSilentFail("Lung"));
                    if (lung != null)
                    {
                        DamageInfo damageInfo = new DamageInfo(DamageDefOf.Cut, Props.woundDamage, 999f, -1f, null, lung, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
                        damageInfo.SetAllowDamagePropagation(false);
                        parent.pawn.TakeDamage(damageInfo);
                    }

                }
                checkDownCounter = 0;
            }







        }
    }
}

