using System;
using System.Collections.Generic;
using System.Linq;

using Verse;

namespace GeneticRim
{
    class GameComponent_HyperlinkInitializer : GameComponent
    {


        public GameComponent_HyperlinkInitializer(Game game)
        {
        }

        public override void FinalizeInit()
        {

            HashSet<ThingDef> excavators = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.thingCategories?.Contains(InternalDefOf.GR_GenomeExcavators) == true).ToHashSet();
            HashSet<ThingDef> genomes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => ((x.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierOne) == true)|| (x.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierTwoOrThree) == true))).ToHashSet();


            foreach (ThingDef excavator in excavators)
            {

                AddExcavatorHyperlinks(excavator);
            }

            foreach (ThingDef genome in genomes)
            {

                AddGenomeHyperlinks(genome);
            }
        }

        private void AddExcavatorHyperlinks(ThingDef excavator)
        {
            if (excavator.descriptionHyperlinks == null)
            {
                excavator.descriptionHyperlinks = new List<DefHyperlink>();
            }

            CompProperties_TargetEffect_Extract comp = excavator.GetCompProperties<CompProperties_TargetEffect_Extract>();

            List<string> tier = comp.tier;

            HashSet<ThingDef> allExcavatorsLinks = new HashSet<ThingDef>();
            HashSet<ExtractableAnimalsList> allLists = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
            foreach (ExtractableAnimalsList individualList in allLists)
            {
                if (tier.Contains(individualList.tier))
                {
                    allExcavatorsLinks.Add(individualList.itemProduced);
                }
            }
            foreach (ThingDef thing in allExcavatorsLinks)
            {
                excavator.descriptionHyperlinks.Add(thing);
               
            }
        }

        private void AddGenomeHyperlinks(ThingDef genome)
        {
            if (genome.descriptionHyperlinks == null)
            {
                genome.descriptionHyperlinks = new List<DefHyperlink>();
            }

           

            HashSet<ThingDef> allGenomesLinks = new HashSet<ThingDef>();
            HashSet<ExtractableAnimalsList> allLists = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
            foreach (ExtractableAnimalsList individualList in allLists)
            {
                if (individualList.itemProduced==genome)
                {
                    if (!individualList.needsHumanLike)
                    {
                        allGenomesLinks.AddRange(individualList.extractableAnimals);
                    }
                   
                }
            }
            foreach (ThingDef thing in allGenomesLinks)
            {
                genome.descriptionHyperlinks.Add(thing);

            }
        }

    }
}
