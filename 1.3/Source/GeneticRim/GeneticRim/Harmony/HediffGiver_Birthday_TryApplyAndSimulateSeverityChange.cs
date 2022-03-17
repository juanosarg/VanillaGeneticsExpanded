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



    [HarmonyPatch(typeof(HediffGiver_Birthday))]
    [HarmonyPatch("TryApplyAndSimulateSeverityChange")]
    public static class GeneticRim_HediffGiver_Birthday_TryApplyAndSimulateSeverityChange_Patch
    {
        [HarmonyPrefix]
        public static bool NoBasegameAgeDiseasesForHybrids(Pawn pawn)

        {

            if (pawn.TryGetComp<CompHybrid>()!=null)
            {
                return false;
            }
            return true;


        }
    }













}

