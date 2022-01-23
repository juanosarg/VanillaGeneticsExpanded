using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace GeneticRim
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HotSwappableAttribute : Attribute
    {
    }

    [HotSwappableAttribute]
    public class Window_DesignateGrowthCell : Window
    {
        public const int BottomButtonWidth = 190;
        public const int SelectionButtonWidth = 190;

        public CompGenomorpher comp;
        public ThingDef genomeDominant;
        public ThingDef genomeSecondary;
        public ThingDef genoframe;
        public ThingDef booster;

        private static readonly Color PawnOutcomeBackground = new Color(0.13f, 0.13f, 0.13f);
        public override Vector2 InitialSize => new Vector2(1000, 800);
        public Window_DesignateGrowthCell(CompGenomorpher comp)
        {
            this.comp = comp;
            this.doCloseButton = false;
            this.forcePause = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            PawnKindDef mainResult = Core.GetHybrid(this.genomeDominant, this.genomeSecondary, this.genoframe, this.booster, 
                                                                 out float swapChance, out float failureChance, out PawnKindDef swapResult, out PawnKindDef failureResult);

            
            float fullWeight = 1 + failureChance;

            Log.ErrorOnce(swapChance + " | " + failureChance, Mathf.RoundToInt(swapChance*100f + failureChance*100f));

            var xPos = inRect.x + 15;
            var explanationLabelBox = new Rect(xPos, inRect.y + 5, 300, 30);
            DrawExplanation(explanationLabelBox, "GR_GenomorpherExplanation".Translate());

            Text.Font = GameFont.Medium;
            var precictedOutComesBox = new Rect(xPos, explanationLabelBox.yMax + 5, 250, 40);
            Widgets.Label(precictedOutComesBox, "GR_PredictedOutcomes".Translate());

            var failureOutComeBox = new Rect(precictedOutComesBox.xMax + 520, precictedOutComesBox.y, 250, 40);
            Widgets.Label(failureOutComeBox, "GR_FailureOutcome".Translate());
            Text.Font = GameFont.Small;

            var firstOutcomeBox = new Rect(xPos, precictedOutComesBox.yMax + 10, 165, 165);
            DrawPawnOutcome(firstOutcomeBox, mainResult, (1f-swapChance) / fullWeight);

            var secondOutcomeBox = new Rect(xPos, firstOutcomeBox.yMax + 30, 95, 95);
            DrawPawnOutcome(secondOutcomeBox, swapResult, swapChance / fullWeight);

            var failureOutcomeBox = new Rect(failureOutComeBox.x, failureOutComeBox.yMax + 10, 165, 165);
            DrawPawnOutcome(failureOutcomeBox, failureResult, failureChance / fullWeight, false);

            explanationLabelBox = new Rect(inRect.x + 300, inRect.y + 120, 330, 30);
            DrawExplanation(explanationLabelBox, "GR_GenomorpherExplanationPartTwo".Translate(), TextAnchor.MiddleCenter);

            var selectDominantGenomeRect = new Rect(inRect.x + 550, failureOutcomeBox.yMax - 12, SelectionButtonWidth, 24);
            DrawButton(selectDominantGenomeRect, "GR_SelectDominantGenome".Translate(), delegate
            {
                var floatOptions = new List<FloatMenuOption>();
                foreach (var genome in Core.genomes.Except(genomeSecondary).ToList())
                {
                    floatOptions.Add(new FloatMenuOption(genome.label, delegate
                    {
                        genomeDominant = genome;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(floatOptions));
            }, "GR_SelectDominantGenomeExplanation".Translate());

            var selectSecondaryGenomeRect = new Rect(selectDominantGenomeRect.x + 160, selectDominantGenomeRect.yMax + 50, SelectionButtonWidth, 24);
            DrawButton(selectSecondaryGenomeRect, "GR_SelectSecondaryGenome".Translate(), delegate
            {
                var floatOptions = new List<FloatMenuOption>();
                foreach (var genome in Core.genomes.Except(genomeDominant).ToList())
                {
                    floatOptions.Add(new FloatMenuOption(genome.label, delegate
                    {
                        genomeSecondary = genome;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(floatOptions));
            }, "GR_SelectSecondaryGenomeExplanation".Translate());

            var selectGenoframeRect = new Rect(selectSecondaryGenomeRect.x - 140, selectSecondaryGenomeRect.yMax + 140, SelectionButtonWidth, 24);
            DrawButton(selectGenoframeRect, "GR_SelectGenoframe".Translate(), delegate
            {
                var floatOptions = new List<FloatMenuOption>();
                foreach (var genoframe in Core.genoframes.ToList())
                {
                    floatOptions.Add(new FloatMenuOption(genoframe.label, delegate
                    {
                        this.genoframe = genoframe;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(floatOptions));
            }, "GR_SelectGenoframeExplanation".Translate());

            var selectBoosterRect = new Rect(selectGenoframeRect.x - 40, selectGenoframeRect.yMax + 60, SelectionButtonWidth, 24);
            DrawButton(selectBoosterRect, "GR_SelectBooster".Translate(), delegate
            {
                var floatOptions = new List<FloatMenuOption>();
                foreach (var booster in Core.boosters.ToList())
                {
                    floatOptions.Add(new FloatMenuOption(booster.label, delegate
                    {
                        this.booster = booster;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(floatOptions));
            }, "GR_SelectBoosterExplanation".Translate());

            var synthesisExplanationRect = new Rect(selectBoosterRect.x - 10, selectBoosterRect.yMax + 50, 430, 100);
            GUI.color = Color.grey;
            Text.Anchor = TextAnchor.UpperRight;
            Widgets.Label(synthesisExplanationRect, "GR_InitiateSynthesisExplanation".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;

            var genomeDominantRect = new Rect(350, 200, 75, 75);
            if (genomeDominant != null)
            {
                DrawThing(genomeDominant, genomeDominantRect);
            }

            var genomeSecondaryRect = new Rect(genomeDominantRect.xMax + 50, genomeDominantRect.yMax + 30, genomeDominantRect.width, genomeDominantRect.height);
            if (genomeSecondary != null)
            {
                DrawThing(genomeSecondary, genomeSecondaryRect);
            }
            var genoframeRect = new Rect(genomeDominantRect.x, genomeDominantRect.yMax + 100, genomeDominantRect.width, genomeDominantRect.height);
            if (genoframe != null)
            {
                DrawThing(genoframe, genoframeRect);
            }

            var boosterRect = new Rect(genoframeRect.x, genoframeRect.yMax + 100, genomeDominantRect.width, genomeDominantRect.height);
            if (booster != null)
            {
                DrawThing(booster, boosterRect);
            }

            if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 32, BottomButtonWidth, 32), "CloseButton".Translate()))
            {
                Close();
            }

            if (Widgets.ButtonText(new Rect((inRect.width / 2) - (BottomButtonWidth / 2), inRect.yMax - 32, BottomButtonWidth, 32), "GR_RandomizeAll".Translate()))
            {
                RandomizeAll();
            }

            if (Widgets.ButtonText(new Rect(inRect.xMax - BottomButtonWidth, inRect.yMax - 32, BottomButtonWidth, 32), "CloseButton".Translate()))
            {
                InitiateSynthesis();
            }

            GUI.color = Color.white;
            Text.Font = GameFont.Small;
        }

        private void DrawThing(ThingDef thingDef, Rect rect)
        {
            Widgets.DefIcon(rect, thingDef);
            var label = thingDef.label.CapitalizeFirst();
            var size = Text.CalcSize(label);
            var labelRect = new Rect(rect.x - 10, rect.yMax + 10, size.x, 24);
            Widgets.Label(labelRect, label);
        }

        private void RandomizeAll()
        {
            this.genomeDominant = Core.genomes.RandomElement();
            this.genomeSecondary = Core.genomes.Except(genomeSecondary).RandomElement();
            this.genoframe = Core.genoframes.RandomElement();
            this.booster = Core.boosters.RandomElement();
        }

        private void InitiateSynthesis()
        {
            // for erdelf
        }

        private void DrawButton(Rect rect, string label, Action action, string explanation)
        {
            if (Widgets.ButtonText(rect, label))
            {
                action();
            }
            var explanationRect = new Rect(rect.x - 30, rect.yMax + 5, rect.width + 60, 30);
            DrawExplanation(explanationRect, explanation, TextAnchor.MiddleCenter);
        }

        private void DrawPawnOutcome(Rect outcomeBox, PawnKindDef pawnKindDef, float chance, bool percentageInfoToLeft = true)
        {
            Widgets.DrawBoxSolid(outcomeBox, PawnOutcomeBackground);
            if (pawnKindDef != null)
            {
                GUI.DrawTexture(outcomeBox, pawnKindDef.race.uiIcon);
                Text.Font = GameFont.Medium;
                var percentString     = chance.ToString("P");
                var percentStringSize = Text.CalcSize(percentString);
                var percentLabelRect = percentageInfoToLeft
                                           ? new Rect(outcomeBox.x + outcomeBox.width - percentStringSize.x, outcomeBox.yMax - 30, percentStringSize.x, 30)
                                           : new Rect(outcomeBox.x,                                          outcomeBox.yMax - 30, percentStringSize.x, 30);
                Widgets.Label(percentLabelRect, percentString);
            }

            Text.Font = GameFont.Small;
            var infoBox = new Rect(outcomeBox.x, outcomeBox.yMax + 5, 18, 18);
            Widgets.InfoCardButton(infoBox, pawnKindDef?.race);

            var pawnName = pawnKindDef?.label.CapitalizeFirst();
            var ppawnLabelSize = Text.CalcSize(pawnName);
            var pawnLabel = new Rect(infoBox.xMax + 5, infoBox.y, ppawnLabelSize.x, 24);
            Widgets.Label(pawnLabel, pawnName);
        }

        private void DrawExplanation(Rect explanationRect, string explanationString, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            var oldAnchor = Text.Anchor;
            Text.Anchor = anchor;
            Text.Font = GameFont.Tiny;
            GUI.color = Color.grey;
            Widgets.Label(explanationRect, explanationString);
            GUI.color = Color.white;
            Text.Font = GameFont.Small;
            Text.Anchor = oldAnchor;
        }
    }
}
