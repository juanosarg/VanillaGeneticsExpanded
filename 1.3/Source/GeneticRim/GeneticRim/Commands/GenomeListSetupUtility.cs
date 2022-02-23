using RimWorld;
using Verse;

using UnityEngine;


namespace GeneticRim
{
    public static class GenomeListSetupUtility
    {
        public static Command_SetGenomeList SetGenomeListCommand(Building_DNAStorageBank building, Map map)
        {
            return new Command_SetGenomeList()
            {
                defaultDesc = "GR_SelectGenomeToHarvestDesc".Translate(),
                defaultLabel = "GR_SelectGenomeToHarvest".Translate(),
                icon = ContentFinder<Texture2D>.Get("Things/Item/GR_geneticmaterialswhite", true),
                hotKey = KeyBindingDefOf.Misc1,
                map = map,
                building = building
               
            };
        }

      
    }
}
