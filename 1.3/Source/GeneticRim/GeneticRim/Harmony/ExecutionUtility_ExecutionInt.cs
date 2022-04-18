using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld.Planet;



namespace GeneticRim
{


    [HarmonyPatch(typeof(ExecutionUtility))]
    [HarmonyPatch("ExecutionInt")]

    static class GeneticRim_ExecutionUtility_ExecutionInt_Patch
    {
        [HarmonyPostfix]
        public static void GiveHediffIfLizardman(Pawn executioner)
        {

            if (executioner.def == InternalDefOf.GR_Lizardman)
            {
                executioner.health.AddHediff(InternalDefOf.GR_SadisticAdrenaline);
                executioner.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.GR_SadisticAdrenaline).Severity = 1;

            }

        }

        
    }

}
