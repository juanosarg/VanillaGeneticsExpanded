using RimWorld;
using Verse;
using Verse.Sound;
using System;

namespace GeneticRim
{
    public class HediffComp_BloodFrenzy : HediffComp
    {


        public HediffCompProperties_BloodFrenzy Props
        {
            get
            {
                return (HediffCompProperties_BloodFrenzy)this.props;
            }
        }

        public override void Notify_PawnDied()
        {

            Map map = this.parent.pawn.Corpse.Map;
            if (map != null)
            {
                foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.parent.pawn.Corpse.Position, this.parent.pawn.Corpse.Map, 10, true))
                {
                    Pawn pawn = thing as Pawn;
                    if (pawn != null && pawn.def== InternalDefOf.GR_Wolfchicken)
                    {
                        pawn.health.AddHediff(InternalDefOf.GR_Frenzied);
                    }
                }



            }

        }


    }
}
