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



    [HarmonyPatch(typeof(ForagedFoodPerDayCalculator))]
    [HarmonyPatch("GetForagedFoodCountPerInterval")]
    [HarmonyPatch(new Type[] { typeof(List<Pawn>), typeof(BiomeDef), typeof(Faction), typeof(StringBuilder) })] 
    public static class GeneticRim_ForagedFoodPerDayCalculator_GetForagedFoodCountPerInterval_Patch
    {
        [HarmonyPostfix]
        public static void MultiplyForaging(List<Pawn> pawns, ref float __result)

        {

            foreach (Pawn pawn in pawns)
            {
                if(pawn.def == InternalDefOf.GR_Catffalo)
                {
                    __result *= 1.1f;
                }
            }
            
			
            

        }
    }













}

