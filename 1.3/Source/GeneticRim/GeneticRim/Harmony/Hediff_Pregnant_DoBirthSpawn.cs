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

   
    [HarmonyPatch(typeof(Hediff_Pregnant))]
    [HarmonyPatch("DoBirthSpawn")]

    public static class GeneticRim_Hediff_Pregnant_DoBirthSpawn_Patch
    {

        public static Pawn fatherStored;
     

        [HarmonyPrefix]

        public static void AddQuality(Pawn father)
        {
            GeneticRim_Hediff_Pregnant_DoBirthSpawn_Patch.fatherStored = father;
           
        }
    }

   

    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("TrySpawnHatchedOrBornPawn")]

    public class GeneticRim_PawnUtility_TrySpawnHatchedOrBornPawn_Patch
    {


        [HarmonyPostfix]

        public static void AddQuality(Pawn pawn, Thing motherOrEgg)
        {
            CompHybrid compMother = motherOrEgg?.TryGetComp<CompHybrid>();
            CompHybrid compFather = GeneticRim_Hediff_Pregnant_DoBirthSpawn_Patch.fatherStored?.TryGetComp<CompHybrid>();
           
          

            if (compMother != null)
            {
                QualityCategory qualityMother = compMother.quality;
                QualityCategory qualityFather = QualityCategory.Awful;
                if (compFather != null) { qualityFather = compFather.quality; }
                

                CompHybrid compHybrid = pawn.TryGetComp<CompHybrid>();
                if (compHybrid != null)
                {
                    if (compFather != null) { compHybrid.quality = (QualityCategory)Math.Min((sbyte)qualityMother, (sbyte)qualityFather); } else
                    {
                        compHybrid.quality = qualityMother;
                    }
                    

                }
                
            }

        }
    }


}
