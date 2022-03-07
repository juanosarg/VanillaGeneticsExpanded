using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace GeneticRim
{


    [StaticConstructorOnStartup]
    public class CompElectroWomb : ThingComp
    {
        public CompProperties_ElectroWomb Props => (CompProperties_ElectroWomb)this.props;

        private static readonly Material barFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f));
        private static readonly Material barUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f));

        public bool Free => this.growingResult == null;

        public PawnKindDef growingResult;
        public PawnKindDef failureResult;

        public ThingDef genomeDominant;
        public ThingDef genomeSecondary;
        public ThingDef genoframe;
        public ThingDef booster;

        public float progress;

        bool failure;

        public int hoursProcess =1;

        Graphic usedGraphic;

        public CompPowerTrader compPowerTrader;

      

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            this.compPowerTrader = this.parent.GetComp<CompPowerTrader>();
           
            if (Props.isLarge)
            {
                usedGraphic = GraphicsCache.graphicTopLarge;
                
            }
            else { usedGraphic = GraphicsCache.graphicTopSmall; }
            usedGraphic.drawSize = this.parent.def.graphicData.drawSize;

        }

        public override void CompTick()
        {
            base.CompTick();

            if (this.growingResult != null)
            {
                if (compPowerTrader?.PowerOn == true)
                {

                    this.progress += 1f / hoursProcess; // (GenDate.TicksPerDay * 7f);

                    if (this.progress > 1)
                    {
                        Pawn pawn = null;
                        if (failure) {

                            pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.failureResult, null, fixedBiologicalAge: 1, fixedChronologicalAge: 0,
                                                                                         newborn: false, forceGenerateNewPawn: true));
                        }
                        else {
                            pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.growingResult, Faction.OfPlayer, fixedBiologicalAge: 1, fixedChronologicalAge: 0,
                                                                                      newborn: false, forceGenerateNewPawn: true));
                        }
                        
                        IntVec3 near = CellFinder.StandableCellNear(this.parent.Position, this.parent.Map, 5f);
                        GenSpawn.Spawn(pawn, near, this.parent.Map);

                        pawn.drafter = new Pawn_DraftController(pawn);
                        pawn.equipment = new Pawn_EquipmentTracker(pawn);

                        if (!failure) {
                            DefExtension_HybridChanceAlterer extension = this.booster?.GetModExtension<DefExtension_HybridChanceAlterer>();

                            if (extension?.isFertilityUnblocker != true)
                            {
                                pawn.health.AddHediff(HediffDefOf.Sterilized);
                            }

                            if (extension?.isController == true)
                            {
                                pawn.health.AddHediff(InternalDefOf.GR_AnimalControlHediff);
                            }

                            if (extension?.addedHediffs != null)
                            {
                                AddInitialHybridHediffs(extension, pawn);
                            }

                        }
                       

                        CompHybrid compHybrid = pawn.TryGetComp<CompHybrid>();
                        if (compHybrid != null) {
                            compHybrid.quality = this.genoframe.GetModExtension<DefExtension_Quality>().quality;

                        }

                        this.progress = 0;
                        this.growingResult = null;

                       
                    }

                }

            }
        }

        public void AddInitialHybridHediffs(DefExtension_HybridChanceAlterer extension,Pawn pawn)
        {
            foreach (HediffToBodyparts hediffToBodyParts in extension.addedHediffs)
            {
                foreach(BodyPartDef part in hediffToBodyParts.bodyparts)
                {
                    if (!pawn.RaceProps.body.GetPartsWithDef(part).EnumerableNullOrEmpty())
                    {
                        pawn.health.AddHediff(hediffToBodyParts.hediff, pawn.RaceProps.body.GetPartsWithDef(part).RandomElement());
                    }

                }

            }

        }

        public override string CompInspectStringExtra()
        {
            if (this.growingResult != null)
            {
                return "GR_ElectroWomb_Progress".Translate(this.progress.ToStringPercent());
            }
            else return null;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Defs.Look(ref this.growingResult, nameof(this.growingResult));
            Scribe_Defs.Look(ref this.failureResult, nameof(this.failureResult));
            Scribe_Values.Look(ref this.progress, nameof(this.progress));
            Scribe_Values.Look(ref this.hoursProcess, nameof(this.hoursProcess));
            Scribe_Values.Look(ref this.failure, nameof(this.failure));


        }

        public override void PostDraw()
        {
            base.PostDraw();

            var vector = this.parent.DrawPos + Altitudes.AltIncVect;
            vector.y += 5;

            Vector2 drawingSize = this.parent.def.graphicData.drawSize/2 * this.progress;

            Graphic graphic = this.growingResult?.lifeStages.Last().bodyGraphicData.Graphic.GetCopy(drawingSize, null);
            graphic?.DrawFromDef(vector, Rot4.South, null);

            vector = this.parent.DrawPos + Altitudes.AltIncVect;
            vector.y += 6;

            usedGraphic?.DrawFromDef(vector, Rot4.North, null);

            GenDraw.FillableBarRequest fillableBarRequest = default(GenDraw.FillableBarRequest);
            if (Props.isLarge)
            {
                fillableBarRequest.center = this.parent.DrawPos - Vector3.forward * 0.9f;
                fillableBarRequest.size = new Vector2(0.85f, 0.07f);
            }
            else
            {
                fillableBarRequest.center = this.parent.DrawPos - Vector3.forward * 0.375f;
                fillableBarRequest.size = new Vector2(0.65f, 0.07f);
            }
            
            
            fillableBarRequest.fillPercent = this.progress;
            fillableBarRequest.filledMat = barFilledMat;
            fillableBarRequest.unfilledMat = barUnfilledMat;
            fillableBarRequest.margin = 0.1f;
          
            GenDraw.DrawFillableBar(fillableBarRequest);

        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            
            if (Prefs.DevMode)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "DEBUG: Finish womb work";
                command_Action.action = delegate
                {
                    if (this.progress > 0)
                    { this.progress = 1; }

                };
                yield return command_Action;

            }
        }

        public void InitProcess(Thing growthCell)
        {
            CompGrowthCell growthComp = growthCell.TryGetComp<CompGrowthCell>();

            PawnKindDef result = Core.GetHybrid(growthComp.genomeDominant, growthComp.genomeSecondary, growthComp.genoframe, growthComp.booster,
                                              out float swapChance, out float failureChance, out PawnKindDef swapResult, out PawnKindDef failureResult);

            this.booster         = growthComp.booster;
            this.genoframe       = growthComp.genoframe;
            this.genomeDominant  = growthComp.genomeDominant;
            this.genomeSecondary = growthComp.genomeSecondary;

            hoursProcess = Props.hoursProcess * GenDate.TicksPerHour;
            float? timeMultiplier = booster?.GetModExtension<DefExtension_HybridChanceAlterer>()?.timeMultiplier;

            if (timeMultiplier != null && timeMultiplier != 0)
            {
                hoursProcess = (int)(hoursProcess * timeMultiplier);
            }

            growthCell.Destroy();

           
            if (swapChance != 0) {
                bool swap = Rand.Chance(swapChance);
                result = swap ? swapResult : result;
            }

            if (failureChance != 0)
            {
                failure = Rand.Chance(failureChance);
                this.failureResult = failureResult;
            }
            
            this.growingResult = result;
            this.progress = 0;

            if (result == null || result.RaceProps.baseBodySize > this.Props.maxBodySize)
            {
                failure = true;
            }
          

        }
    }
}
