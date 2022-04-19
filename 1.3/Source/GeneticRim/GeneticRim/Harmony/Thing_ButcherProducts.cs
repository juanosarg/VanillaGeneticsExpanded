using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;



namespace GeneticRim
{
    /*This patch makes some creatures not drop regular meat, and scales the amount of butcherproducts depending on missing body parts*/


    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch("ButcherProducts")]
    public static class GeneticRim_Thing_ButcherProducts_Patch
    {
        [HarmonyPostfix]
        static void ChangeMeatAmountByAge(Thing __instance, float efficiency, ref IEnumerable<Thing> __result)
        {

            if (__instance.GetType() == typeof(Pawn))
            {

                var thingies = __result.ToList();
                var pawn = (Pawn)__instance;

                if ((__instance.def.butcherProducts != null) && (__instance.def.defName == "GR_Manchicken") )
                {
                    
                    int baseCalculation = 140;
                   
                    ThingDefCountClass ta = __instance.def.butcherProducts[0];
                    float num = pawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(pawn.RaceProps.body.corePart);
                    int count = GenMath.RoundRandom((pawn.BodySize * baseCalculation * efficiency * num));
                    if (count > 0)
                    {
                        Thing t = ThingMaker.MakeThing(ta.thingDef, null);
                        t.stackCount = count;
                        thingies.Insert(1, t);

                        __result = thingies;
                    }


                }
            }




        }
    }








}
