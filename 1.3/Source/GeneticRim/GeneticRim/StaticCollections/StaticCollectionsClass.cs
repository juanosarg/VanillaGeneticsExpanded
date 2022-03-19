
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

       


    }
}
