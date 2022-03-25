
using Verse;
using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;




namespace GeneticRim
{
    class CompJump : CompAbilityEffect
    {

        new public CompProperties_Jump Props
        {
            get
            {
                return (CompProperties_Jump)this.props;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Map map = parent.pawn.Map;
            PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ThingDefOf.PawnJumper, parent.pawn, target.Cell);
            if (pawnFlyer != null)
            {
                GenSpawn.Spawn(pawnFlyer, target.Cell, map);
              
            }
        }






    }
}
