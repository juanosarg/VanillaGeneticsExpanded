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

        public static Command_SetAnimalList SetAnimalListCommand(Building_DNAStorageBank building, Map map,ThingDef selectedGenome)
        {
            return new Command_SetAnimalList()
            {
                defaultDesc = "GR_SelectAnimalToHarvestDesc".Translate(),
                defaultLabel = "GR_SelectAnimalToHarvest".Translate(),
                icon = ContentFinder<Texture2D>.Get("ui/commands/GR_BringAnimal", true),
                hotKey = KeyBindingDefOf.Misc1,
                map = map,
                building = building,
                selectedGenome= selectedGenome

            };
        }

        public static Command_SetParagonList SetParagonListCommand(Building_Mechahybridizer building, Map map)
        {
            return new Command_SetParagonList()
            {
                defaultDesc = "GR_SelectParagonToConvertDesc".Translate(),
                defaultLabel = "GR_SelectParagonToConvert".Translate(),
                icon = ContentFinder<Texture2D>.Get("ui/commands/MechahybridizeParagon", true),
                hotKey = KeyBindingDefOf.Misc1,
                map = map,
                building = building

            };
        }


    }
}
