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



    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch("Ingested")]
    public static class GeneticRim_Thing_Ingested_Patch
    {
        [HarmonyPrefix]
        public static void DetectSargSyndrome(Pawn ingester, float nutritionWanted, Thing __instance)

        {

            if (ingester?.health?.hediffSet?.HasHediff(InternalDefOf.GR_SargSyndrome) == true) {

                Corpse corpse = __instance as Corpse;
               
                if ((corpse != null && corpse.InnerPawn == null)||!FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(__instance))
                {
                    ingester.needs.food.CurLevel -= ingester.needs.food.MaxLevel * 0.5f;
                    ingester?.jobs?.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: true);
                   
                    
                }
                
            }
            

        }
    }













}

