using RimWorld;
using Verse;
using Verse.Sound;
using System;

namespace GeneticRim
{
    public class HediffComp_RotWhenDead : HediffComp
    {


        public HediffCompProperties_RotWhenDead Props
        {
            get
            {
                return (HediffCompProperties_RotWhenDead)this.props;
            }
        }

        public override void Notify_PawnDied()
        {
          
            CompRottable comp = this.parent.pawn.Corpse.GetComp<CompRottable>();
            if (comp != null)
            {
                comp.RotImmediately();
            }

        }


    }
}
