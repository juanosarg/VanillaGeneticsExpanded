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

   
    [HarmonyPatch(typeof(GenRecipe))]
    [HarmonyPatch("MakeRecipeProducts")]

    public static class GeneticRim_GenRecipe_MakeRecipeProducts
    {

        public static List<Thing> ingredients;

       
        [HarmonyPostfix]
        public static void AddQualityToImplants(List<Thing> ingredients)
        {
            GeneticRim_GenRecipe_MakeRecipeProducts.ingredients = ingredients;
        }
    }

    [HarmonyPatch(typeof(GenRecipe))]
    [HarmonyPatch("PostProcessProduct")]

    public static class GeneticRim_GenRecipe_PostProcessProduct
    {

       

        [HarmonyPostfix]
        public static void AddQualityToImplants(ref Thing product, RecipeDef recipeDef, Pawn worker)
        {
            if (product.HasThingCategory(InternalDefOf.GR_ImplantCategory))
            {
                foreach(Thing ingredient in GeneticRim_GenRecipe_MakeRecipeProducts.ingredients)
                {
                    if (ingredient.HasThingCategory(InternalDefOf.GR_Genoframes))
                    {
                        QualityCategory quality = ingredient.def.GetModExtension<DefExtension_Quality>().quality;
                        product.TryGetComp<CompQuality>().SetQuality(quality, ArtGenerationContext.Colony);
                    }

                }
            }
        }
    }


}
