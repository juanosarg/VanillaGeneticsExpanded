using RimWorld;
using UnityEngine;
using Verse;


namespace GeneticRim
{




    public class GeneticRim_Mod : Mod
    {
        public static GeneticRim_Settings settings;

        public GeneticRim_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<GeneticRim_Settings>();
        }
        public override string SettingsCategory() => "Vanilla Genetics Expanded";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }





    }
}

