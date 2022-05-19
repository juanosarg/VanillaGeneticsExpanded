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
                if (!GeneticRim_Mod.settings.GR_DisableGreaterScariaRotting)
                {
                    comp.RotImmediately();

                }
                else
                {
                    if (Rand.Chance(Find.Storyteller.difficulty.scariaRotChance))
                    {
                        comp.RotImmediately();
                    }

                }
            }

        }


    }
}
