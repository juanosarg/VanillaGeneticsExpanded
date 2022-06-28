
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

        // A list of mech antennas in the world
        public static HashSet<Building_MechahybridAntenna> mech_antennas = new HashSet<Building_MechahybridAntenna>();

        // A dictionary on Manffalos and their experience
        public static Dictionary<Pawn, float> manffalo_and_experience = new Dictionary<Pawn, float>();


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

        public static bool IsHumanoidHybrid(Thing thing)
        {
            return humanoid_hybrids.Contains(thing);
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

        public static void AddMechAntennaToList(Building_MechahybridAntenna thing)
        {

            if (!mech_antennas.Contains(thing))
            {
                mech_antennas.Add(thing);
            }
        }

        public static void RemoveMechAntennaFromList(Building_MechahybridAntenna thing)
        {
            if (mech_antennas.Contains(thing))
            {
                mech_antennas.Remove(thing);
            }

        }

        public static bool AnyMechAntennas()
        {

            return mech_antennas.Count > 0;
        }

        public static void AddManffaloAndExperience(Pawn pawn)
        {
            if (!manffalo_and_experience.ContainsKey(pawn))
            {
                manffalo_and_experience.Add(pawn, 1);
            }
            
        }

        public static void SetManffaloExperience(Pawn pawn, float xp)
        {
            if (!manffalo_and_experience.ContainsKey(pawn))
            {
                manffalo_and_experience[pawn] = xp;
            }

        }

        public static void RemoveManffaloAndExperience(Pawn pawn)
        {
            if (manffalo_and_experience.ContainsKey(pawn))
            {
                manffalo_and_experience.Remove(pawn);
            }

        }
        //Adding this as a way for AlphaMemes Rituals to get genome without too much annoying harmony/Digging
        public static ThingDef GenomeForPawn(Pawn pawn)
        {   
            ThingDef thingDef = null;
            HashSet<ExtractableAnimalsList> allLists = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
            foreach (ExtractableAnimalsList individualList in allLists)
            {
                if ((individualList.needsHumanLike && pawn.def.race.Humanlike) || (individualList.extractableAnimals?.Contains(pawn.def) == true))
                {
                    thingDef = individualList.itemProduced;
                }
            }
            return thingDef;
        }
    }
}
