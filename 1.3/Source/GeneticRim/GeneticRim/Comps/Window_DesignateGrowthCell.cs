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
    [StaticConstructorOnStartup]
    public class Window_DesignateGrowthCell : Window
    {
        public const int BottomButtonWidth = 190;
        public const int SelectionButtonWidth = 190;

        public static readonly Texture2D Hexagon = ContentFinder<Texture2D>.Get("ui/GeneticsUI_Hexagon");
        public static readonly Texture2D Arrow = ContentFinder<Texture2D>.Get("ui/GeneticsUI_Arrow");
        public static readonly Texture2D ArrowTwoWays = ContentFinder<Texture2D>.Get("ui/GeneticsUI_ArrowTwoWays");

        public CompGenomorpher comp;
        public Thing genomeDominant;
        public Thing genomeSecondary;
        public Thing genoframe;
        public Thing booster;

        public int durationTicks;

        private static readonly Color PawnOutcomeBackground = new Color(0.13f, 0.13f, 0.13f);
        public override Vector2 InitialSize => new Vector2(1000, 800);
        public Window_DesignateGrowthCell(CompGenomorpher comp)
        {
            this.comp = comp;
            this.doCloseButton = false;
            this.forcePause = true;
        }

        public List<Thing> genomes;
        public List<Thing> genomesCanBeSecondary;
        public List<Thing> genoframes;
        public List<Thing> boosters;

        public override void PreOpen()
        {
            base.PreOpen();


            this.genomes    = this.comp.parent.Map.listerThings.AllThings.Where(x => (x.def.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierOne)==true || x.def.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierTwoOrThree)==true)&&!x.IsForbidden(Faction.OfPlayer)).ToList();
            this.genomesCanBeSecondary = this.comp.parent.Map.listerThings.AllThings.Where(x => (x.def.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierOne) ?? false)&&!x.IsForbidden(Faction.OfPlayer)).ToList();

            this.boosters   = this.comp.parent.Map.listerThings.AllThings.Where(x => (x.def.thingCategories?.Contains(InternalDefOf.GR_Boosters)        ?? false)&&!x.IsForbidden(Faction.OfPlayer)).ToList();
            this.genoframes = this.comp.parent.Map.listerThings.AllThings.Where(x => (x.def.thingCategories?.Contains(InternalDefOf.GR_Genoframes)      ?? false)&& !x.IsForbidden(Faction.OfPlayer)).ToList();
        }

        public override void DoWindowContents(Rect inRect)
        {
            PawnKindDef mainResult = Core.GetHybrid(this.genomeDominant?.def, this.genomeSecondary?.def, this.genoframe?.def, this.booster?.def, 
                                                                 out float swapChance, out float failureChance, out PawnKindDef swapResult, out PawnKindDef failureResult);

            
            float fullWeight = 1 + failureChance;

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
            if(swapResult!= mainResult) {
                DrawPawnOutcome(secondOutcomeBox, swapResult, swapChance / fullWeight);

            }

            var genoframeQuality = genoframe != null ? Core.GetQualityFromGenoframe(genoframe.def) : null;
            var qualityInfoRect = new Rect(xPos, secondOutcomeBox.yMax + 100, 200, 30);
            Text.Font = GameFont.Medium;
            Widgets.Label(qualityInfoRect, "GR_Quality".Translate(genoframeQuality != null ? genoframeQuality.Value.GetLabel() : "None"));
            var qualityInfoExplanationRect = new Rect(xPos, qualityInfoRect.yMax + 5, 240, 30);
            DrawExplanation(qualityInfoExplanationRect, "GR_QualityExplanation".Translate());

            durationTicks = this.comp.Props.hoursProcess * GenDate.TicksPerHour;
            float? timeMultiplier = this.booster?.def?.GetModExtension<DefExtension_HybridChanceAlterer>()?.timeMultiplier;

            if (timeMultiplier != null && timeMultiplier!=0)
            {
                durationTicks = (int)(durationTicks*timeMultiplier);
            }
            
            var durationRect = new Rect(xPos, qualityInfoExplanationRect.yMax + 50, 300, 30);
            Text.Font = GameFont.Medium;
            Widgets.Label(durationRect, "GR_ProcessWillTake".Translate(durationTicks.ToStringTicksToDays()));
            var durationExplanationRect = new Rect(xPos, durationRect.yMax + 5, 240, 30);
            DrawExplanation(durationExplanationRect, "GR_ProcessDurationExplanation".Translate());

            var failureOutcomeBox = new Rect(failureOutComeBox.x, failureOutComeBox.yMax + 10, 165, 165);
            DrawPawnOutcome(failureOutcomeBox, failureResult, failureChance / fullWeight, false);

            explanationLabelBox = new Rect(inRect.x + 300, inRect.y + 80, 330, 30);
            DrawExplanation(explanationLabelBox, "GR_GenomorpherExplanationPartTwo".Translate(), TextAnchor.MiddleCenter);

            var selectDominantGenomeRect = new Rect(inRect.x + 550, failureOutcomeBox.yMax - 25, SelectionButtonWidth, 24);
            DrawButton(selectDominantGenomeRect, "GR_SelectDominantGenome".Translate(), delegate
            {
                var floatOptions = new List<FloatMenuOption>();
                foreach (var genome in this.genomes.ToList())
                {
                    floatOptions.Add(new FloatMenuOption(genome.def.LabelCap, delegate
                    {
                        genomeDominant = genome;
                        genomeSecondary = null;
                    }));
                }
                if (this.genomes.NullOrEmpty())
                {
                    floatOptions.Add(new FloatMenuOption("GR_NoGenomesInMap".Translate(), delegate
                    {
                        
                    }));

                }
                Find.WindowStack.Add(new FloatMenu(floatOptions));
            }, "GR_SelectDominantGenomeExplanation".Translate());

            var selectSecondaryGenomeRect = new Rect(selectDominantGenomeRect.x + 160, selectDominantGenomeRect.yMax + 62, SelectionButtonWidth, 24);
            DrawButton(selectSecondaryGenomeRect, "GR_SelectSecondaryGenome".Translate(), delegate
            {
                var floatOptions = new List<FloatMenuOption>();

                if (this.genomeDominant == null) {

                    floatOptions.Add(new FloatMenuOption("GR_ChooseDominantGenomeFirst".Translate(), delegate
                    {

                    }));

                } else {
                    foreach (var genome in this.genomesCanBeSecondary.ToList())
                    {
                        floatOptions.Add(new FloatMenuOption(genome.def.LabelCap, delegate
                        {
                            genomeSecondary = genome;
                        }));
                    }

                    if (this.genomeDominant.def.thingCategories?.Contains(InternalDefOf.GR_GeneticMaterialTierTwoOrThree) == true)
                    {
                        foreach (var genome in this.genomes.ToList())
                        {
                            if(genome!= genomeDominant) {
                                floatOptions.Add(new FloatMenuOption(genome.def.LabelCap, delegate
                                {
                                    genomeSecondary = genome;
                                }));
                            }
                            
                        }

                    }



                    if (this.genomesCanBeSecondary.NullOrEmpty())
                    {
                        floatOptions.Add(new FloatMenuOption("GR_NoGenomesInMap".Translate(), delegate
                        {

                        }));

                    }


                }

                
                Find.WindowStack.Add(new FloatMenu(floatOptions));
            }, "GR_SelectSecondaryGenomeExplanation".Translate());

            var selectGenoframeRect = new Rect(selectSecondaryGenomeRect.x - 140, selectSecondaryGenomeRect.yMax + 140, SelectionButtonWidth, 24);
            DrawButton(selectGenoframeRect, "GR_SelectGenoframe".Translate(), delegate
            {
                var floatOptions = new List<FloatMenuOption>();
                foreach (var genoframe in this.genoframes.ToList())
                {
                    floatOptions.Add(new FloatMenuOption(genoframe.def.LabelCap, delegate
                    {
                        this.genoframe = genoframe;
                    }));
                }
                if (this.genoframes.NullOrEmpty())
                {
                    floatOptions.Add(new FloatMenuOption("GR_NoGenoframesInMap".Translate(), delegate
                    {

                    }));

                }
                Find.WindowStack.Add(new FloatMenu(floatOptions));
            }, "GR_SelectGenoframeExplanation".Translate());

            var selectBoosterRect = new Rect(selectGenoframeRect.x - 40, selectGenoframeRect.yMax + 60, SelectionButtonWidth, 24);
            DrawButton(selectBoosterRect, "GR_SelectBooster".Translate(), delegate
            {
                var floatOptions = new List<FloatMenuOption>();
                foreach (var booster in this.boosters.ToList())
                {
                    floatOptions.Add(new FloatMenuOption(booster.def.LabelCap, delegate
                    {
                        this.booster = booster;

                        durationTicks = this.comp.Props.hoursProcess * GenDate.TicksPerHour;
                        float? timeMultiplierRefresh = this.booster?.def?.GetModExtension<DefExtension_HybridChanceAlterer>()?.timeMultiplier;

                        if (timeMultiplierRefresh != null)
                        {
                            durationTicks = (int)(durationTicks * timeMultiplierRefresh);
                        }

                    }));
                }
                if (this.boosters.NullOrEmpty())
                {
                    floatOptions.Add(new FloatMenuOption("GR_NoBoostersInMap".Translate(), delegate
                    {

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

            var arrowTwoWidth = ArrowTwoWays.width * arrowTwoScale;
            var arrowTwoHeight = ArrowTwoWays.height * arrowTwoScale;
            var arrowTwoRect = new Rect(test1 - 140, test2 - 150, arrowTwoWidth, arrowTwoHeight);
            GUI.DrawTexture(arrowTwoRect, ArrowTwoWays);

            var genomeDominantRect = new Rect(test1, test2, 75, 75);
            DrawThing(genomeDominant, genomeDominantRect);

            var arrowWidth = Arrow.width * arrowScale;
            var arrowHeight = Arrow.height * arrowScale;
            var arrowToLeftRect = new Rect(test1 - arrowWidth - 70, genomeDominantRect.center.y + 5, arrowWidth, arrowHeight);
            GUI.DrawTexture(arrowToLeftRect, Arrow);

            var genomeSecondaryRect = new Rect(genomeDominantRect.xMax + test3, genomeDominantRect.yMax + test4, genomeDominantRect.width, genomeDominantRect.height);
            DrawThing(genomeSecondary, genomeSecondaryRect);

            var genoframeRect = new Rect(genomeDominantRect.x, genomeDominantRect.yMax + test5, genomeDominantRect.width, genomeDominantRect.height);
            DrawThing(genoframe, genoframeRect);

            var boosterRect = new Rect(genoframeRect.x, genoframeRect.yMax + test6, genomeDominantRect.width, genomeDominantRect.height);
            DrawThing(booster, boosterRect);


            if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 32, BottomButtonWidth, 32), "CloseButton".Translate()))
            {
                Close();
            }

            if (Widgets.ButtonText(new Rect((inRect.width / 2) - (BottomButtonWidth / 2), inRect.yMax - 32, BottomButtonWidth, 32), "GR_RandomizeAll".Translate()))
            {
                RandomizeAll();
            }

            if (Widgets.ButtonText(new Rect(inRect.xMax - BottomButtonWidth, inRect.yMax - 32, BottomButtonWidth, 32), "GR_InitiateSynthesis".Translate(),
                                   active: this.genomeDominant != null && this.genomeSecondary != null && this.genoframe != null &&mainResult!=null))
            {
                InitiateSynthesis();
            }

            GUI.color = Color.white;
            Text.Font = GameFont.Small;
        }

        [TweakValue("000", 0, 1)] public static float arrowScale = 0.56f;
        [TweakValue("000", 0, 1)] public static float arrowTwoScale = 0.53f;
        [TweakValue("000", 0, 1)] public static float hexagonScale = 0.45f;
        [TweakValue("000", 0, 1000)] public static float test1 = 384f;
        [TweakValue("000", 0, 1000)] public static float test2 = 261.5f;
        [TweakValue("000", 0, 1000)] public static float test3 = 75f;
        [TweakValue("000", 0, 1000)] public static float test4 = 10f;
        [TweakValue("000", 0, 1000)] public static float test5 = 96.78f;
        [TweakValue("000", 0, 1000)] public static float test6 = 96.78f;
        private void DrawThing(Thing thing, Rect rect)
        {
            var hexWidth = Hexagon.width * hexagonScale;
            var hexHeight = Hexagon.height * hexagonScale;
            var hexRect = new Rect(rect.center.x - (hexWidth / 2), rect.center.y - (hexHeight / 2), hexWidth, hexHeight);
            GUI.DrawTexture(hexRect, Hexagon);
            if (thing != null)
            {
                Widgets.DefIcon(rect, thing.def);
                var label = thing.def.label.CapitalizeFirst();
                var size = Text.CalcSize(label);
                var labelRect = new Rect((rect.x + (rect.width / 2)) - (size.x / 2), rect.yMax + 10, size.x, 24);
                Widgets.Label(labelRect, label);
            }
        }

        private void RandomizeAll()
        {
            if (!this.genomes.NullOrEmpty())
            {
                this.genomeDominant = this.genomes.RandomElement();
            }
            if (!this.genomesCanBeSecondary.NullOrEmpty())
            {
                this.genomeSecondary = this.genomesCanBeSecondary.RandomElement();
            }
            if (!this.genoframes.NullOrEmpty())
            {
                this.genoframe = this.genoframes.RandomElement();
            }
            if (!this.boosters.NullOrEmpty())
            {
                this.booster = this.boosters.RandomElement();
            }

           
           
           
        }

        private void InitiateSynthesis()
        {
            float? timeMultiplier = this.booster?.def?.GetModExtension<DefExtension_HybridChanceAlterer>()?.timeMultiplier;

            if (timeMultiplier != null&& timeMultiplier != 0)
            {
                this.comp.Initialize(this.genomeDominant, this.genomeSecondary, this.genoframe, this.booster, (int)(GenDate.TicksPerHour * this.comp.Props.hoursProcess * timeMultiplier));
            }
            else { this.comp.Initialize(this.genomeDominant, this.genomeSecondary, this.genoframe, this.booster, GenDate.TicksPerHour * this.comp.Props.hoursProcess); }
            
            this.Close();
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
            if (pawnKindDef!=null) { Widgets.InfoCardButton(infoBox, pawnKindDef?.race);}
            

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
