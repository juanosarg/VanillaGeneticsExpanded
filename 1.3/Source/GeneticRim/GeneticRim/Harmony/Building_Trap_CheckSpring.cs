using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Verse.AI;
using RimWorld.Planet;



namespace GeneticRim
{



    [HarmonyPatch(typeof(Building_Trap))]
    [HarmonyPatch("CheckSpring")]
    public static class GeneticRim_Building_Trap_CheckSpring_Patch
    {
        [HarmonyPrefix]
        public static bool ManalopesDontTriggerTraps(Pawn p)

        {

            if (p.def == InternalDefOf.GR_Manalope)
            {
                return false;
            }
            return true;


        }
    }













}

