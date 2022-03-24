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



    [HarmonyPatch(typeof(ResurrectionUtility))]
    [HarmonyPatch("Resurrect")]
    public static class GeneticRim_ResurrectionUtility_Resurrect_Patch
    {
        [HarmonyPrefix]
        public static bool DisallowResurrection(Pawn pawn)

        {

            if (pawn.health.hediffSet.HasHediff(InternalDefOf.GR_ExtractedBrain))
            {
                Messages.Message("GR_CantResurrect".Translate(), pawn, MessageTypeDefOf.NegativeEvent);
                return false;
            }
            return true;


        }
    }

    [HarmonyPatch(typeof(ResurrectionUtility))]
    [HarmonyPatch("ResurrectWithSideEffects")]
    public static class GeneticRim_ResurrectionUtility_ResurrectWithSideEffects_Patch
    {
        [HarmonyPrefix]
        public static bool DisallowResurrection(Pawn pawn)

        {

            if (pawn.health.hediffSet.HasHediff(InternalDefOf.GR_ExtractedBrain))
            {
                Messages.Message("GR_CantResurrect".Translate(), pawn, MessageTypeDefOf.NegativeEvent);
                return false;
            }
            return true;


        }
    }













}

