

using RimWorld;
using Verse;
using Verse.Sound;
using System;

namespace GeneticRim
{
    public class HediffComp_PeriodicWounds : HediffComp
    {
        public int checkDownCounter = 0;

        private System.Random rand = new System.Random();

        public HediffCompProperties_PeriodicWounds Props
        {
            get
            {
                return (HediffCompProperties_PeriodicWounds)this.props;
            }
        }



        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            checkDownCounter++;

            if (this.parent.Severity > Props.severityThirdStage)
            {
                if (checkDownCounter > Props.mtbWoundsThirdStage)
                {
                    if (rand.NextDouble() < Props.chanceCutThirdStage)
                    {
                        this.parent.pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, 2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));

                    }
                    checkDownCounter = 0;
                }
            }
            else if (this.parent.Severity > Props.severitySecondStage)
            {
                if (checkDownCounter > Props.mtbWoundsSecondStage)
                {
                    if (rand.NextDouble() < Props.chanceCutSecondStage)
                    {
                        this.parent.pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, 1, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));

                    }
                    checkDownCounter = 0;
                }
            }







        }
    }
}

