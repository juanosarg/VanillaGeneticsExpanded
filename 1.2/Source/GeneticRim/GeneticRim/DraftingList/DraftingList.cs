
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace GeneticRim
{
    
    public static class DraftingList
    {
        
        public static IDictionary<Thing, bool[]> draftable_animals = new Dictionary<Thing, bool[]>();

        public static int numberOfAnimalControlHubsBuilt = 0;


        public static void AddAnimalToList(Thing thing, bool[] abilityArray)
        {
           
            if (!draftable_animals.ContainsKey(thing))
            {
                draftable_animals.Add(thing, abilityArray);
            }
        }

        public static void RemoveAnimalFromList(Thing thing)
        {
            if (draftable_animals.ContainsKey(thing))
            {
                draftable_animals.Remove(thing);
            }

        }

        public static void AddControlHubBuilt()
        {
            numberOfAnimalControlHubsBuilt++;
        }

        public static void RemoveControlHubBuilt()
        {
            if (numberOfAnimalControlHubsBuilt>0)
            {
                numberOfAnimalControlHubsBuilt--;
            }

        }




    }
}
