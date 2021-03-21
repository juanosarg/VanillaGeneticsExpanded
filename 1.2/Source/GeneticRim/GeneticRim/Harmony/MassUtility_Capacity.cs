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

    /*This Harmony Prefix makes the creature carry more weight
*/
    [HarmonyPatch(typeof(MassUtility))]
    [HarmonyPatch("Capacity")]
    public static class GeneticRim_MassUtility_Capacity_Patch
    {
        [HarmonyPostfix]
        public static void MakeThemCarryMore(Pawn p, ref float __result)

        {
            bool flagIsCreatureMine = p.Faction != null && p.Faction.IsPlayer;
            bool flagIsCreatureDraftable = DraftingList.draftable_animals.ContainsKey(p);
            bool flagIsAnimalControlHubBuilt = DraftingList.numberOfAnimalControlHubsBuilt > 0;
            bool flagCanCreatureCarryMore = false;

            if (flagIsCreatureDraftable && flagIsAnimalControlHubBuilt)
            {             
                  flagCanCreatureCarryMore = DraftingList.draftable_animals[p][3];                              
            }

            if (flagIsCreatureDraftable && flagIsCreatureMine && flagCanCreatureCarryMore)
            {
                __result = p.BodySize * 50f;
            }

        }
    }


}
