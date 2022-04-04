
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace GeneticRim
{

    public static class StaticCollectionsClass
    {

        // A list of humanoid hybrids in the colony
        public static HashSet<Thing> humanoid_hybrids = new HashSet<Thing>();

        // A list of horse hybrids in the colony
        public static HashSet<Pawn> horse_hybrids = new HashSet<Pawn>();

        // A list of genetic failures in the map
        public static HashSet<Thing> failures_in_map = new HashSet<Thing>();

        public static void AddHumanoidHybridToList(Thing thing)
        {

            if (!humanoid_hybrids.Contains(thing))
            {
                humanoid_hybrids.Add(thing);
            }
        }

        public static void RemoveHumanoidHybridFromList(Thing thing)
        {
            if (humanoid_hybrids.Contains(thing))
            {
                humanoid_hybrids.Remove(thing);
            }

        }

        public static void AddHorseHybridToList(Pawn thing)
        {
            
            if (!horse_hybrids.Contains(thing))
            {
                horse_hybrids.Add(thing);
             
            }
        }

        public static void RemoveHorseHybridFromList(Pawn thing)
        {
            if (horse_hybrids.Contains(thing))
            {
                horse_hybrids.Remove(thing);
            }

        }

        public static void AddFailuresToList(Thing thing)
        {

            if (!failures_in_map.Contains(thing))
            {
                failures_in_map.Add(thing);
            }
        }

        public static void RemoveFailuresFromList(Thing thing)
        {
            if (failures_in_map.Contains(thing))
            {
                failures_in_map.Remove(thing);
            }

        }


    }
}
