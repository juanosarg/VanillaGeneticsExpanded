using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld.Planet;



// So, let's comment this code, since it uses Harmony and has moderate complexity

namespace GeneticRim
{

    /*This Harmony Prefix makes jobs not return an error if the player right clicks something with a drafted animal
         */
    [HarmonyPatch(typeof(FloatMenuMakerMap))]
    [HarmonyPatch("AddUndraftedOrders")]
    public static class GeneticRim_FloatMenuMakerMap_AddUndraftedOrders_Patch
    {
        [HarmonyPrefix]
        public static bool AvoidGeneralErrorIfPawnIsAnimal(Pawn pawn)

        {
            bool flagIsCreatureDraftable = DraftingList.draftable_animals.ContainsKey(pawn);

            if (flagIsCreatureDraftable)
            {
                return false;
            }
            else return true;

        }
    }


}
