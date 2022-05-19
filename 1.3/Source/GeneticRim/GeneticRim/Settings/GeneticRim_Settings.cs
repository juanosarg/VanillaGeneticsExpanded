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

        public const float GR_RaidsRateBase = 1;
        public float GR_RaidsRate = GR_RaidsRateBase;

        public const int GR_HybridsPerAntennaBase = 5;
        public int GR_HybridsPerAntenna = GR_HybridsPerAntennaBase;

        public const int GR_HybridSpawnerRadiusBase = 12;
        public int GR_HybridSpawnerRadius = GR_HybridSpawnerRadiusBase;

        public bool GR_DisableOldAgeDiseases = false;
        public bool GR_DisableHybridRaids = false;
        public bool GR_MakeAllHybridsFertile = false;
        public bool GR_MakeAllHybridsControllable = false;
        public bool GR_DisableGrowthCellAlerts = false;
        public bool GR_DisableWombAlerts = false;
        public bool GR_DisableMechanoidIFF = false;
        public bool GR_DisableGreaterScariaRotting = false;


        private static Vector2 scrollPosition = Vector2.zero;





        public override void ExposeData()
        {
            base.ExposeData();


            Scribe_Values.Look(ref GR_WombSpeedMultiplier, "GR_WombSpeedMultiplier", GR_WombSpeedMultiplierBase);
            Scribe_Values.Look(ref GR_GenomorpherSpeedMultiplier, "GR_GenomorpherSpeedMultiplier", GR_GenomorpherSpeedMultiplierBase);
            Scribe_Values.Look(ref GR_FailureRate, "GR_FailureRate", GR_FailureRateBase);
            Scribe_Values.Look(ref GR_QuestRate, "GR_QuestRate", GR_QuestRateBase);
            Scribe_Values.Look(ref GR_RaidsRate, "GR_RaidsRate", GR_RaidsRateBase);
            Scribe_Values.Look(ref GR_HybridsPerAntenna, "GR_HybridsPerAntenna", GR_HybridsPerAntennaBase);
            Scribe_Values.Look(ref GR_HybridSpawnerRadius, "GR_HybridSpawnerRadius", GR_HybridSpawnerRadiusBase);



            Scribe_Values.Look(ref GR_DisableOldAgeDiseases, "GR_DisableOldAgeDiseases",false);
            Scribe_Values.Look(ref GR_DisableHybridRaids, "GR_DisableHybridRaids", false);
            Scribe_Values.Look(ref GR_MakeAllHybridsFertile, "GR_MakeAllHybridsFertile", false);
            Scribe_Values.Look(ref GR_MakeAllHybridsControllable, "GR_MakeAllHybridsControllable", false);
            Scribe_Values.Look(ref GR_DisableGrowthCellAlerts, "GR_DisableGrowthCellAlerts", false);
            Scribe_Values.Look(ref GR_DisableWombAlerts, "GR_DisableWombAlerts", false);
            Scribe_Values.Look(ref GR_DisableMechanoidIFF, "GR_DisableMechanoidIFF", false);
            Scribe_Values.Look(ref GR_DisableGreaterScariaRotting, "GR_DisableGreaterScariaRotting", false);






        }
        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            var scrollContainer = inRect.ContractedBy(10);
            scrollContainer.height -= listingStandard.CurHeight;
            scrollContainer.y += listingStandard.CurHeight;
            Widgets.DrawBoxSolid(scrollContainer, Color.grey);
            var innerContainer = scrollContainer.ContractedBy(1);
            Widgets.DrawBoxSolid(innerContainer, new ColorInt(42, 43, 44).ToColor);
            var frameRect = innerContainer.ContractedBy(5);
            frameRect.y += 15;
            frameRect.height -= 15;
            var contentRect = frameRect;
            contentRect.x = 0;
            contentRect.y = 0;
            contentRect.width -= 20;


            

            contentRect.height = 950f;

           
            Widgets.BeginScrollView(frameRect, ref scrollPosition, contentRect, true);
            listingStandard.Begin(contentRect.AtZero());


            //listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("GR_DisableOldAgeDiseases".Translate(), ref GR_DisableOldAgeDiseases, "GR_DisableOldAgeDiseasesTooltip".Translate());
            listingStandard.CheckboxLabeled("GR_DisableHybridRaids".Translate(), ref GR_DisableHybridRaids, "GR_DisableHybridRaidsTooltip".Translate());
            listingStandard.CheckboxLabeled("GR_MakeAllHybridsFertile".Translate(), ref GR_MakeAllHybridsFertile, "GR_MakeAllHybridsFertileTooltip".Translate());
            listingStandard.CheckboxLabeled("GR_MakeAllHybridsControllable".Translate(), ref GR_MakeAllHybridsControllable, "GR_MakeAllHybridsControllableTooltip".Translate());
            listingStandard.CheckboxLabeled("GR_DisableGrowthCellAlerts".Translate(), ref GR_DisableGrowthCellAlerts, "GR_DisableGrowthCellAlertsTooltip".Translate());
            listingStandard.CheckboxLabeled("GR_DisableWombAlerts".Translate(), ref GR_DisableWombAlerts, "GR_DisableWombAlertsTooltip".Translate());
            listingStandard.CheckboxLabeled("GR_DisableMechanoidIFF".Translate(), ref GR_DisableMechanoidIFF, "GR_DisableMechanoidIFFTooltip".Translate());
            listingStandard.CheckboxLabeled("GR_DisableGreaterScariaRotting".Translate(), ref GR_DisableGreaterScariaRotting, "GR_DisableGreaterScariaRottingTooltip".Translate());


            listingStandard.GapLine();
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

            var RaidsRateLabel = listingStandard.LabelPlusButton("GR_RaidsRate".Translate() + ": " + GR_RaidsRate, "GR_RaidsRateTooltip".Translate());
            GR_RaidsRate = (float)Math.Round(listingStandard.Slider(GR_RaidsRate, 0.1f, 5f), 1);
            if (listingStandard.Settings_Button("GR_Reset".Translate(), new Rect(0f, RaidsRateLabel.position.y + 35, 180f, 29f)))
            {
                GR_RaidsRate = GR_RaidsRateBase;
            }

            var HybridsPerAntennaLabel = listingStandard.LabelPlusButton("GR_HybridsPerAntenna".Translate() + ": " + GR_HybridsPerAntenna, "GR_HybridsPerAntennaTooltip".Translate());
            GR_HybridsPerAntenna = (int)(listingStandard.Slider(GR_HybridsPerAntenna, 1f, 50f));
            if (listingStandard.Settings_Button("GR_Reset".Translate(), new Rect(0f, HybridsPerAntennaLabel.position.y + 35, 180f, 29f)))
            {
                GR_HybridsPerAntenna = GR_HybridsPerAntennaBase;
            }

            var HybridSpawnerRadiusLabel = listingStandard.LabelPlusButton("GR_HybridSpawnerRadius".Translate() + ": " + GR_HybridSpawnerRadius, "GR_HybridSpawnerRadiusTooltip".Translate());
            GR_HybridSpawnerRadius = (int)(listingStandard.Slider(GR_HybridSpawnerRadius, 5f, 40f));
            if (listingStandard.Settings_Button("GR_Reset".Translate(), new Rect(0f, HybridSpawnerRadiusLabel.position.y + 35, 180f, 29f)))
            {
                GR_HybridSpawnerRadius = GR_HybridSpawnerRadiusBase;
            }


            listingStandard.End();
            Widgets.EndScrollView();

            base.Write();

        }




    }










}
