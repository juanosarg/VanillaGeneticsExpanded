using RimWorld;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GeneticRim
{
    public class GeneticRim_Settings : ModSettings

    {


      
        public const float GR_GenomorpherSpeedMultiplierBase = 1;
        public float GR_GenomorpherSpeedMultiplier = GR_GenomorpherSpeedMultiplierBase;

        public const float GR_WombSpeedMultiplierBase = 1;
        public float GR_WombSpeedMultiplier = GR_WombSpeedMultiplierBase;

        public const float GR_FailureRateBase = 10;
        public float GR_FailureRate = GR_FailureRateBase;

        public const float GR_QuestRateBase = 1;
        public float GR_QuestRate = GR_QuestRateBase;

        public bool GR_DisableOldAgeDiseases = false;
        public bool GR_DisableHybridRaids = false;







        public override void ExposeData()
        {
            base.ExposeData();


            Scribe_Values.Look(ref GR_WombSpeedMultiplier, "GR_WombSpeedMultiplier", GR_WombSpeedMultiplierBase);
            Scribe_Values.Look(ref GR_GenomorpherSpeedMultiplier, "GR_GenomorpherSpeedMultiplier", GR_GenomorpherSpeedMultiplierBase);
            Scribe_Values.Look(ref GR_FailureRate, "GR_FailureRate", GR_FailureRateBase);

            Scribe_Values.Look(ref GR_DisableOldAgeDiseases, "GR_DisableOldAgeDiseases",false);
            Scribe_Values.Look(ref GR_DisableHybridRaids, "GR_DisableHybridRaids", false);






        }
        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();


            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("GR_DisableOldAgeDiseases".Translate(), ref GR_DisableOldAgeDiseases, "GR_DisableOldAgeDiseasesTooltip".Translate());
            listingStandard.CheckboxLabeled("GR_DisableHybridRaids".Translate(), ref GR_DisableHybridRaids, "GR_DisableHybridRaidsTooltip".Translate());

            var GenomorpherSpeedMultiplierLabel = listingStandard.LabelPlusButton("GR_GenomorpherSpeedMultiplier".Translate() + ": " + GR_GenomorpherSpeedMultiplier, "GR_GenomorpherSpeedMultiplierTooltip".Translate());
            GR_GenomorpherSpeedMultiplier = (float)Math.Round(listingStandard.Slider(GR_GenomorpherSpeedMultiplier, 0.1f, 2f), 2);
            if (listingStandard.Settings_Button("GR_Reset".Translate(), new Rect(0f, GenomorpherSpeedMultiplierLabel.position.y + 35, 180f, 29f)))
            {
                GR_GenomorpherSpeedMultiplier = GR_GenomorpherSpeedMultiplierBase;
            }

            var WombSpeedMultiplierLabel = listingStandard.LabelPlusButton("GR_WombSpeedMultiplier".Translate() + ": " + GR_WombSpeedMultiplier, "GR_WombMultiplierTooltip".Translate());
            GR_WombSpeedMultiplier = (float)Math.Round(listingStandard.Slider(GR_WombSpeedMultiplier, 0.1f, 2f), 2);
            if (listingStandard.Settings_Button("GR_Reset".Translate(), new Rect(0f, WombSpeedMultiplierLabel.position.y + 35, 180f, 29f)))
            {
                GR_WombSpeedMultiplier = GR_WombSpeedMultiplierBase;
            }

            var FailureRateLabel = listingStandard.LabelPlusButton("GR_FailureRate".Translate() + ": " + GR_FailureRate +"%", "GR_FailureRateTooltip".Translate());
            GR_FailureRate = (float)Math.Round(listingStandard.Slider(GR_FailureRate, -50f, 100f), 1);
            if (listingStandard.Settings_Button("GR_Reset".Translate(), new Rect(0f, FailureRateLabel.position.y + 35, 180f, 29f)))
            {
                GR_FailureRate = GR_FailureRateBase;
            }

            var QuestRateLabel = listingStandard.LabelPlusButton("GR_QuestRate".Translate() + ": " + GR_QuestRate, "GR_QuestRateTooltip".Translate());
            GR_QuestRate = (float)Math.Round(listingStandard.Slider(GR_QuestRate, 0.1f, 5f), 1);
            if (listingStandard.Settings_Button("GR_Reset".Translate(), new Rect(0f, QuestRateLabel.position.y + 35, 180f, 29f)))
            {
                GR_QuestRate = GR_QuestRateBase;
            }


            listingStandard.End();

           
            base.Write();

        }




    }










}
