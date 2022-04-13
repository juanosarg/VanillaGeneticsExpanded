namespace GeneticRim
{
    using RimWorld;
    using System.Collections.Generic;
    using System.Linq;
    using Verse;

    [StaticConstructorOnStartup]
    public static class Core
    {
        public static List<ThingDef>                                          genomes;
        public static List<ThingDef>                                          boosters;
        public static List<ThingDef>                                          genoframes;
        public static Dictionary<ThingDef, Dictionary<ThingDef, PawnKindDef>> hybrids;
        public static List<PawnKindDef>                                       failures;

        static Core()
        {
            genomes    = DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterial) ?? false).ToList();
            boosters   = DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories?.Contains(InternalDefOf.GR_Boosters)        ?? false).ToList();
            genoframes = DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories?.Contains(InternalDefOf.GR_Genoframes)      ?? false).ToList();
            failures   = DefDatabase<PawnKindDef>.AllDefs.Where(x => x.HasModExtension<DefExtension_HybridFailure>()).ToList();

            { // Hybrids
                hybrids = new Dictionary<ThingDef, Dictionary<ThingDef, PawnKindDef>>();

                foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefsListForReading)
                {
                    DefExtension_Hybrid hybridExt = pawnKindDef.GetModExtension<DefExtension_Hybrid>();

                    if (hybridExt != null)
                    {
                        if (!hybrids.ContainsKey(hybridExt.dominantGenome))
                            hybrids.Add(hybridExt.dominantGenome, new Dictionary<ThingDef, PawnKindDef>());
                        hybrids[hybridExt.dominantGenome].Add(hybridExt.secondaryGenome, pawnKindDef);

                        if(!pawnKindDef.race.HasComp(typeof(CompHybrid)))
                            pawnKindDef.race.comps.Add(new CompProperties(typeof(CompHybrid)));

                        if (!pawnKindDef.race.HasComp(typeof(CompApplyAgeDiseases)))
                            pawnKindDef.race.comps.Add(new CompProperties(typeof(CompApplyAgeDiseases)));
                    }

                    if (pawnKindDef.race.tradeTags?.Contains("AnimalGeneticMechanoid") == true) {
                        if (!pawnKindDef.race.HasComp(typeof(CompHybrid)))
                            pawnKindDef.race.comps.Add(new CompProperties(typeof(CompHybrid)));
                    }
                }
            }
        }

        public static PawnKindDef GetHybrid(ThingDef  genomeDominant, ThingDef  genomeSecondary, ThingDef        genoframe,  ThingDef        booster,
                                            out float swapChance,     out float failureChance,   out PawnKindDef swapResult, out PawnKindDef failureResult)
        {
            
            if (genomeDominant == null || genomeSecondary == null)
            {
                swapChance    = 0;
                failureChance = 1;
                swapResult    = null;
                failureResult = null;
                return null;
            }

            
            DefExtension_HybridChanceAlterer frameExtension   = genoframe?.GetModExtension<DefExtension_HybridChanceAlterer>();
            DefExtension_HybridChanceAlterer boosterExtension = booster?.GetModExtension<DefExtension_HybridChanceAlterer>();
            if ((genomeDominant.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierTwoOrThree) == true)|| genomeDominant== genomeSecondary)
            {
                swapChance = 0f;
            }
            else { swapChance = (10f - (frameExtension?.stability ?? 0f) - (boosterExtension?.stability ?? 0f)) / 100f; }

            if (swapChance < 0f)
            {
                swapChance = 0f;
            }

            float paragonFailureFactor = 0f;

            if (genomeDominant==genomeSecondary) {
                paragonFailureFactor = 0.15f;
            }

            float failure = (GeneticRim_Mod.settings.GR_FailureRate - (frameExtension?.safety ?? 0f) - (boosterExtension?.safety ?? 0f));
            failureChance = (failure / 100f)+ paragonFailureFactor;
            if (failureChance < 0f)
            {
                failureChance = 0f;
                failure = 0f;
            }
            

            if (!hybrids.TryGetValue(genomeSecondary, out Dictionary<ThingDef, PawnKindDef> secondaryChain) || !secondaryChain.TryGetValue(genomeDominant, out swapResult))
                swapResult = null;
            failureResult = failures.FirstOrDefault(td => td.GetModExtension<DefExtension_HybridFailure>().InRange(failure +paragonFailureFactor*100)) ?? failures.RandomElement();
            
            if (hybrids.TryGetValue(genomeDominant, out secondaryChain))
                if (secondaryChain.TryGetValue(genomeSecondary, out PawnKindDef result))
                    return result;
            
            return null;
        }

        public static QualityCategory? GetQualityFromGenoframe(ThingDef genoframe)
        {
            var extension = genoframe.GetModExtension<DefExtension_Quality>();
            if (extension != null)
            {
                return extension.quality;
            }
            return null;
        }
    }
}
