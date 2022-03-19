using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Verse.AI;
using RimWorld.Planet;



namespace GeneticRim
{



    [HarmonyPatch(typeof(JobDriver_Nuzzle))]
    [HarmonyPatch("MakeNewToils")]
    public static class GeneticRim_JobDriver_Nuzzle_MakeNewToils_Patch
    {
        [HarmonyPostfix]
        public static void NotifyNuzzled(JobDriver_Nuzzle __instance)

        {
            CompDieUnlessReset comp = __instance.pawn.TryGetComp<CompDieUnlessReset>();
            if (comp != null && __instance.pawn.def==InternalDefOf.GR_Fleshling)
            {
                comp.ResetTimer();
            }
           

        }
    }













}

