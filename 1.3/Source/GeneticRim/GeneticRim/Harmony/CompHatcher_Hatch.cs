using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;
using Verse.AI;
using RimWorld.Planet;



namespace GeneticRim
{

   
    [HarmonyPatch(typeof(CompHatcher))]
    [HarmonyPatch("Hatch")]

    public static class GeneticRim_CompHatcher_Hatch_Patch
    {

        public static Pawn fatherStored;
        public static Pawn motherStored;

        [HarmonyPrefix]

        public static void AddQuality(CompHatcher __instance)
        {
            GeneticRim_CompHatcher_Hatch_Patch.motherStored = __instance.hatcheeParent;

            GeneticRim_CompHatcher_Hatch_Patch.fatherStored = __instance.otherParent;

        }
    }

   

    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("TrySpawnHatchedOrBornPawn")]

    public class GeneticRim_PawnUtility_TrySpawnHatchedOrBornPawn_ForEggs_Patch
    {


        [HarmonyPostfix]

        public static void AddQualityToEgg(Pawn pawn)
        {
            CompHybrid compMother = GeneticRim_CompHatcher_Hatch_Patch.motherStored?.TryGetComp<CompHybrid>();
            CompHybrid compFather = GeneticRim_CompHatcher_Hatch_Patch.fatherStored?.TryGetComp<CompHybrid>();         

            if (compMother != null && compFather != null)
            {
                QualityCategory qualityMother = compMother.quality;
                QualityCategory qualityFather = compFather.quality;

                CompHybrid compHybrid = pawn.TryGetComp<CompHybrid>();
                if (compHybrid != null)
                {
                    compHybrid.quality = (QualityCategory)Math.Min((sbyte)qualityMother, (sbyte)qualityFather);

                }
                
            }

        }
    }


}
