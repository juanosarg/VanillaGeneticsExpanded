
using Verse;
using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;




namespace GeneticRim
{
    class CompDeathRay : CompAbilityEffect
    {

        new public CompProperties_DeathRay Props
        {
            get
            {
                return (CompProperties_DeathRay)this.props;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);

            PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, target.Cell, parent.pawn.Map, WipeMode.Vanish);
            powerBeam.duration = Props.duration;
            powerBeam.instigator = parent.pawn;
            powerBeam.weaponDef = null;
            powerBeam.StartStrike();

        }

        



    }
}
