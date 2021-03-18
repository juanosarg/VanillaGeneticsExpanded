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

    /*This first Harmony postfix deals with adding a Pawn_DraftController if it detects the creature
     * belongs to the player and to the custom class CompDraftable. It also adds a Pawn_EquipmentTracker
     * because some ugly errors are produced otherwise, though it is basically unused
     * 
     */
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("IsColonistPlayerControlled", MethodType.Getter)]
    public static class GeneticRim_Pawn_IsColonistPlayerControlled_Patch
    {
        [HarmonyPostfix]
        static void AddAnimalAsColonist(Pawn __instance, ref bool __result)
        {
            bool flagIsCreatureDraftable = (__instance.TryGetComp<CompDraftable>() != null);
            if (flagIsCreatureDraftable) {
                foreach (Thing t in __instance.Map.listerThings.ThingsOfDef(ThingDef.Named("GR_AnimalControlHub")))
                {
                    Thing mindcontrolhub = t as Thing;
                    if (t != null)
                    {
                        __result = __instance.Spawned && __instance.HostFaction == null;
                        break;
                    }
                }
                
            }
            
        }
    }



}
