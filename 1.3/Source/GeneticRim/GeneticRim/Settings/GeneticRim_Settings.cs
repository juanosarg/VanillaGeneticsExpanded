﻿using RimWorld;
using UnityEngine;
using Verse;


namespace GeneticRim
{
    public class GeneticRim_Settings : ModSettings

    {

        /*
      
        public static float failureRate = 10;
     


        public override void ExposeData()
        {
            base.ExposeData();
           
            Scribe_Values.Look(ref failureRate, "failureRate", 10);




        }


    }
    public class GeneticRim_Mod : Mod
    {
        public static GeneticRim_Settings settings;
        public GeneticRim_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<GeneticRim_Settings>();
        }
        public override string SettingsCategory() => "Vanilla Genetic Rim Expanded";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(inRect);
            ls.Gap(12f);
            
            var label = "GR_IncubatorFailureRate".Translate();
            GeneticRim_Settings.failureRate= Widgets.HorizontalSlider(inRect.TopHalf().TopHalf().TopHalf(), GeneticRim_Settings.failureRate, 0f, 100f, false, label, "0%", "100%", -1);
            ls.Gap(90f);
           
           

            settings.Write();
            ls.End();
           


        }*/
    }
}
