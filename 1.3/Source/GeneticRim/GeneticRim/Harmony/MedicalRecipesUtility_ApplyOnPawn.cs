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
    [HarmonyPatch(typeof(MedicalRecipesUtility))]
    [HarmonyPatch("SpawnThingsFromHediffs")]
    public static class VanillaGeneticsExpanded_MedicalRecipesUtility_SpawnThingsFromHediffs_Prefix
    {
        

        [HarmonyPrefix]
        public static void AddQualityToImplant(Pawn pawn, BodyPartRecord part)
        {           
            foreach (Hediff hediff in from x in pawn.health.hediffSet.hediffs
            where x.Part == part
            select x)
            if (hediff != null)
            {
                if (hediff.def.spawnThingOnRemoved != null)
                {
                    var comp = hediff.TryGetComp<HediffCompImplantQuality>();
                    if (comp != null)
                    {
                        VanillaGeneticsExpanded_Recipe_RemoveImplant_ApplyOnPawn_Prefix.implantQuality = new Pair<ThingDef, QualityCategory>(hediff.def.spawnThingOnRemoved, comp.quality); //Setting other patches pair to not have to duplicate spawn postfix
                    }
                }
            }            

        }

    }    
}
