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


            foreach (ThingDef excavator in excavators)
            {
                
                AddHyperlinks(excavator);
            }
        }

        private void AddHyperlinks(ThingDef excavator)
        {
            if (excavator.descriptionHyperlinks == null)
            {
                excavator.descriptionHyperlinks = new List<DefHyperlink>();
            }

            CompProperties_TargetEffect_Extract comp = excavator.GetCompProperties<CompProperties_TargetEffect_Extract>();

            string tier = comp.tier;

            HashSet<ThingDef> allLinks = new HashSet<ThingDef>();
            HashSet<ExtractableAnimalsList> allLists = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
            foreach (ExtractableAnimalsList individualList in allLists)
            {
                if (individualList.tier == tier)
                {
                    allLinks.AddRange(individualList.extractableAnimals);
                }
            }

            foreach (ThingDef thing in allLinks)
            {
                excavator.descriptionHyperlinks.Add(thing);

               
            }



        }

    }
}
