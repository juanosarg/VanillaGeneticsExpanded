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
            HashSet<ThingDef> genomes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => ((x.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierOne) == true) || (x.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierTwoOrThree) == true))).ToHashSet();


            foreach (ThingDef excavator in excavators)
            {

                AddExcavatorHyperlinks(excavator);
            }

            foreach (ThingDef genome in genomes)
            {

                AddGenomeHyperlinks(genome);
            }

            AddArchotechProjectHyperlinks();

            AddAnimalsToGenomeHyperlinks();
        }

        private void AddExcavatorHyperlinks(ThingDef excavator)
        {
            if (excavator.descriptionHyperlinks == null)
            {
                excavator.descriptionHyperlinks = new List<DefHyperlink>();
            }

            CompProperties_TargetEffect_Extract comp = excavator.GetCompProperties<CompProperties_TargetEffect_Extract>();

            if (comp != null)
            {
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

                    bool linkFound = false;

                    foreach (DefHyperlink link in excavator.descriptionHyperlinks)
                    {
                        if (link.def == thing)
                        {
                            linkFound = true;
                        }

                    }
                    if (!linkFound)
                    {
                        excavator.descriptionHyperlinks.Add(thing);
                    }

                   

                }

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
                if (individualList.itemProduced == genome)
                {
                    if (!individualList.needsHumanLike)
                    {
                        allGenomesLinks.AddRange(individualList.extractableAnimals);
                    }

                }
            }
            foreach (ThingDef thing in allGenomesLinks)
            {
                bool linkFound = false;

                foreach (DefHyperlink link in genome.descriptionHyperlinks)
                {
                    if (link.def == thing)
                    {
                        linkFound = true;
                    }

                }
                if (!linkFound)
                {
                    genome.descriptionHyperlinks.Add(thing);
                }
                
            }
        }

        private void AddArchotechProjectHyperlinks()
        {
            ThingDef former = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x== InternalDefOf.GR_ArchocentipedeFormer).FirstOrDefault();

            if (former.descriptionHyperlinks == null)
            {
                former.descriptionHyperlinks = new List<DefHyperlink>();
            }
            HashSet<ThingDef> allGenomesLinks = new HashSet<ThingDef>();
            HashSet<EndgameGenomesDef> allLists = DefDatabase<EndgameGenomesDef>.AllDefsListForReading.ToHashSet();
            foreach (EndgameGenomesDef individualList in allLists)
            {
                allGenomesLinks.AddRange(individualList.genomes);
            }
            
            foreach (ThingDef thing in allGenomesLinks)
            {
                bool linkFound = false;

                foreach (DefHyperlink link in former.descriptionHyperlinks)
                {
                    if (link.def == thing)
                    {
                        linkFound = true;
                    }

                }
                if (!linkFound)
                {
                    former.descriptionHyperlinks.Add(thing);
                }
               
            }
        }

        private void AddAnimalsToGenomeHyperlinks()
        {
            HashSet<ExtractableAnimalsList> extractableAnimalsLists = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();

            foreach (ExtractableAnimalsList extractableAnimalsList in extractableAnimalsLists)
            {
                if (extractableAnimalsList.extractableAnimals != null)
                {
                    foreach (ThingDef animal in extractableAnimalsList.extractableAnimals)
                    {
                        if (animal.descriptionHyperlinks == null)
                        {
                            animal.descriptionHyperlinks = new List<DefHyperlink>();
                        }
                        if (extractableAnimalsList.itemProduced != null) {
                            bool linkFound = false;

                            foreach (DefHyperlink link in animal.descriptionHyperlinks)
                            {
                                if(link.def== extractableAnimalsList.itemProduced)
                                {
                                    linkFound = true;
                                }
                            
                            }
                            if (!linkFound)
                            {
                                animal.descriptionHyperlinks.Add(extractableAnimalsList.itemProduced);
                            }

                             
                            
                        }
                        
                    }
                }
               

            }

           
        }

    }
}
