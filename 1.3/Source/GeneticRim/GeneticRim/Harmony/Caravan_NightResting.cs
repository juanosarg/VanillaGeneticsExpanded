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

   
    [HarmonyPatch(typeof(Caravan))]
    [HarmonyPatch("NightResting", MethodType.Getter)]

    static class GeneticRim_Caravan_NightResting_Patch
    {
        [HarmonyPostfix]
        public static void DecreaseNighttime(Caravan __instance, ref bool __result)
        {
            

            foreach (Pawn p in __instance.PawnsListForReading)
            {
                
                if (StaticCollectionsClass.horse_hybrids.Contains(p))
                {
                  
                    if (!__instance.Spawned)
                    {
                        
                    }else
                    if (__instance.pather.Moving && __instance.pather.nextTile == __instance.pather.Destination && Caravan_PathFollower.IsValidFinalPushDestination(__instance.pather.Destination) && Mathf.CeilToInt(__instance.pather.nextTileCostLeft / 1f) <= 10000)
                    {

                    }
                    else
                    {
                        __result = RestingNowAtImproved(__instance.Tile);
                        break;
                    }




                }

            }

        }

        public static bool RestingNowAtImproved(int tile)
        {
            float num = GenDate.HourFloat(GenTicks.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
            if (!(num < 4f))
            {
                return num > 23.5f;
            }
            return true;
        }
    }

}
