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



    [HarmonyPatch(typeof(Verb_CastAbility))]
    [HarmonyPatch("DrawRadius")]
    public static class GeneticRim_Verb_CastAbility_DrawRadius_Patch
    {
        [HarmonyPrefix]
        public static bool DisableRadius(Verb_CastAbility __instance)

        {

            if (__instance.ability.def == InternalDefOf.GR_DeathRay)
            {
               
                return false;
            }
            return true;


        }
    }

   













}

