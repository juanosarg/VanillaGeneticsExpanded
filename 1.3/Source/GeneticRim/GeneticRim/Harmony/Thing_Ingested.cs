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
                    ingester?.jobs?.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, resumeCurJobAfterwards: true);
                    float numTaken;
                    float nutritionIngested;
                    numTaken = Mathf.CeilToInt(nutritionWanted / __instance.GetStatValue(StatDefOf.Nutrition));
                    numTaken = Mathf.Min(numTaken, __instance.def.ingestible.maxNumToIngestAtOnce, __instance.stackCount);
                    numTaken = Mathf.Max(numTaken, 1);
                    nutritionIngested = (float)numTaken * __instance.GetStatValue(StatDefOf.Nutrition);
                    ingester.needs.food.CurLevel -= nutritionIngested;
                }
                
            }
            

        }
    }













}

