using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace GeneticRim
{

    public class DamageWorker_Double : DamageWorker_Cut
    {
        float chance = 0.25f;

        protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
        {
            base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);


            if (Rand.Chance(chance)) {

                
                    pawn.TakeDamage(dinfo);
                
            }
            

        }
    }
}