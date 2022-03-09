using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace GeneticRim
{

    public class DamageWorker_DevastatingBite : DamageWorker_Bite
    {
        float chance = 0.01f;

        protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
        {
            base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);


            if (Rand.Chance(chance))
            {


                pawn.Kill(null);

            }


        }
    }
}