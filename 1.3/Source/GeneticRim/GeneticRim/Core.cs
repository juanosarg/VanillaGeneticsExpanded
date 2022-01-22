using System.Collections.Generic;
using System.Linq;
using Verse;

namespace GeneticRim
{
    [StaticConstructorOnStartup]
    public static class Core
    {
        public static List<ThingDef> genomes;
        public static List<ThingDef> boosters;
        public static List<ThingDef> genoframes;
        static Core()
        {
            genomes = DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterial) ?? false).ToList();
            boosters = DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories?.Contains(InternalDefOf.GR_Boosters) ?? false).ToList();
            genoframes = DefDatabase<ThingDef>.AllDefs.Where(x => x.thingCategories?.Contains(InternalDefOf.GR_Genoframes) ?? false).ToList();
        }
    }
}
