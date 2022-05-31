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

    [HarmonyPatch(typeof(Recipe_RemoveImplant))]
    [HarmonyPatch("ApplyOnPawn")]
    public static class VanillaGeneticsExpanded_Recipe_RemoveImplant_ApplyOnPawn_Prefix
    {

        public static Pair<ThingDef, QualityCategory> implantQuality;

        [HarmonyPrefix]
        public static void AddQualityToImplant(RecipeWorker __instance, Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {

            if (__instance.recipe?.removesHediff != null)
            {
                if (!pawn.health?.hediffSet?.GetNotMissingParts().Contains(part) ?? false)
                {
                    return;
                }
                Hediff hediff = pawn.health?.hediffSet?.hediffs?.FirstOrDefault((Hediff x) => x.def == __instance.recipe.removesHediff);
                if (hediff != null)
                {
                    if (hediff.def.spawnThingOnRemoved != null)
                    {
                        var comp = hediff.TryGetComp<HediffCompImplantQuality>();
                        if (comp != null)
                        {
                            implantQuality = new Pair<ThingDef, QualityCategory>(hediff.def.spawnThingOnRemoved, comp.quality);
                        }
                    }
                }
            }




        }

    }
    [HarmonyPatch(typeof(ThingWithComps))]
    [HarmonyPatch("SpawnSetup")]

    public class VanillaGeneticsExpanded_ThingWithComps_SpawnSetup
    {
        public static void Postfix(ThingWithComps __instance)
        {
            if (__instance.def == VanillaGeneticsExpanded_Recipe_RemoveImplant_ApplyOnPawn_Prefix.implantQuality.First) {
                var comp = __instance.TryGetComp<CompQuality>();
                if (comp != null)
                {
                    comp.SetQuality(VanillaGeneticsExpanded_Recipe_RemoveImplant_ApplyOnPawn_Prefix.implantQuality.Second, ArtGenerationContext.Colony);

                }
            }
            

        }
    }
}



