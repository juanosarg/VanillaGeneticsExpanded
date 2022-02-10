using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticRim
{
    using RimWorld;
    using UnityEngine;
    using Verse;

    public class CompElectroWomb : ThingComp
    {
        public CompProperties_ElectroWomb Props => (CompProperties_ElectroWomb)this.props;

        public bool Free => this.growingResult == null;

        public PawnKindDef growingResult;
        public ThingDef booster;
        public float       progress;

        public override void CompTick()
        {
            base.CompTick();

            if (this.growingResult != null)
            {
                this.progress += 1f / GenDate.TicksPerHour; // (GenDate.TicksPerDay * 7f);

                if (this.progress > 1)
                {
                    Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.growingResult, Faction.OfPlayer, fixedBiologicalAge: 0, fixedChronologicalAge: 0, 
                                                                                     newborn: true, forceGenerateNewPawn: true));

                    IntVec3 near = CellFinder.StandableCellNear(this.parent.Position, this.parent.Map, 5f);

                    if(this.booster != InternalDefOf.GR_BoosterFertility)
                    {
                        pawn.health.AddHediff(HediffDefOf.Sterilized);
                    }

                    if (this.booster == InternalDefOf.GR_BoosterController)
                    {
                        pawn.health.AddHediff(InternalDefOf.GR_AnimalControlImplant);
                    }

                    GenSpawn.Spawn(pawn, near, this.parent.Map);

                    this.progress      = 0;
                    this.growingResult = null;
                    
                    //finished
                }
            }
        }

        public override string CompInspectStringExtra()
        {
            return "GR_ElectroWomb_Progress".Translate(this.progress.ToStringPercent());
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Defs.Look(ref this.growingResult, nameof(this.growingResult));
            Scribe_Values.Look(ref this.progress, nameof(this.progress));
        }

        public override void PostDraw()
        {
            base.PostDraw();

            Vector3 pos = this.parent.DrawPos;
            pos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
            Graphic graphic         = this.growingResult?.lifeStages.Last().bodyGraphicData.Graphic.GetCopy(this.parent.def.graphicData.drawSize * this.progress, null);
            graphic?.DrawFromDef(pos, Rot4.South, null);
        }


        public void InitProcess(Thing growthCell)
        {
            CompGrowthCell growthComp = growthCell.TryGetComp<CompGrowthCell>();

            PawnKindDef result = Core.GetHybrid(growthComp.genomeDominant, growthComp.genomeSecondary, growthComp.genoframe,       growthComp.booster,
                                              out float swapChance,      out float failureChance,    out PawnKindDef swapResult, out PawnKindDef failureResult);

            this.booster = growthComp.booster;

            growthCell.Destroy();

            bool swap = Rand.Chance(swapChance);
            result = swap ? result : swapResult;

            bool failure = Rand.Chance(failureChance);

            if (!failure && result != null && result.RaceProps.baseBodySize < this.Props.maxBodySize)
            {
                this.growingResult = result;
                this.progress      = 0;
                return;
            }

            Log.Message("FAILURE");
            //todo: failures here
        }
    }
}
