using HarmonyLib;
using Verse;

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace GeneticRim
{

   


    [HarmonyPatch(typeof(Recipe_InstallArtificialBodyPart))]
    [HarmonyPatch("ApplyOnPawn")]
    public static class VanillaGeneticsExpanded_Recipe_InstallArtificialBodyPart_ApplyOnPawn
    {
        [HarmonyPostfix]
        public static void AddQualityToHediff(RecipeWorker __instance, Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            
            if (__instance.recipe?.addsHediff != null && ingredients != null)
            {
                var hediff = pawn.health?.hediffSet?.hediffs?.FindLast(x => x.def == __instance.recipe.addsHediff);
               
                if (hediff != null)
                {
                    var comp = hediff.TryGetComp<HediffCompImplantQuality>();
                    if (comp != null)
                    {
                        foreach (var ingredient in ingredients)
                        {
                            if (ingredient != null && hediff.def.spawnThingOnRemoved == ingredient.def && ingredient.TryGetQuality(out var qualityCategory))
                            {
                                comp.quality = qualityCategory;
                                break;
                            }
                        }
                    }
                }
            }




        }

    }
}