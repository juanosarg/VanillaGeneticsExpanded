using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GeneticRim
{
    using RimWorld;
    using UnityEngine;

    public class CompGenomorpher : ThingComp
    {
        public float progress;
        public int duration;
        public int durationTicks;
        private Thing growthCell;
        public bool bringIngredients = false;
        public Thing genomeDominant;
        public Thing genomeSecondary;
        public Thing frame;
        public Thing booster;


        public void Initialize(Thing genomeDominant, Thing genomeSecondary, Thing frame, Thing booster, int durationTicks)
        {
            this.growthCell = ThingMaker.MakeThing(InternalDefOf.GR_GrowthCell);
            CompGrowthCell cell = this.growthCell.TryGetComp<CompGrowthCell>();
            cell.genomeDominant  = genomeDominant.def;
            cell.genomeSecondary = genomeSecondary.def;
            cell.genoframe       = frame.def;
            cell.booster         = booster?.def;

            this.genomeDominant = genomeDominant;
            this.genomeSecondary = genomeSecondary;
            this.frame = frame;
            this.booster = booster;
            this.durationTicks = durationTicks;

            this.bringIngredients = true;
            
        }

        public override void CompTick()
        {
            base.CompTick();

            if (this.duration > 0)
            {
                this.progress += 1f / this.duration;

                if (this.progress >= 1)
                {
                    GenSpawn.Spawn(this.growthCell, this.parent.InteractionCell, this.parent.Map);
                    this.duration   = -1;
                    this.growthCell = null;
                }
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_Action
            {
                defaultLabel = "GR_DesignateGrowthCell".Translate(),
                defaultDesc = "GR_DesignateGrowthCellDesc".Translate(),
                action = delegate
                {
                    Find.WindowStack.Add(new Window_DesignateGrowthCell(this));
                }
            };
        }

        private static readonly Material barFilledMat   = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f,  0.475f, 0.1f));
        private static readonly Material barUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f,  0.15f));

        public override void PostDraw()
        {
            base.PostDraw();
            GenDraw.FillableBarRequest fillableBarRequest = default(GenDraw.FillableBarRequest);
            fillableBarRequest.center      = parent.DrawPos + Vector3.up * 0.1f + Vector3.left * 0.25f;
            fillableBarRequest.size        = new Vector2(1f, 0.14f);
            fillableBarRequest.fillPercent = this.progress;
            fillableBarRequest.filledMat   = barFilledMat;
            fillableBarRequest.unfilledMat = barUnfilledMat;
            fillableBarRequest.margin      = 0.15f;
            fillableBarRequest.rotation    = this.parent.Rotation.Rotated(RotationDirection.Clockwise);
            GenDraw.DrawFillableBar(fillableBarRequest);
            fillableBarRequest.center = parent.DrawPos + Vector3.up * 0.1f + Vector3.right * 0.1f;
            GenDraw.DrawFillableBar(fillableBarRequest);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref this.progress, nameof(this.progress));
            Scribe_Values.Look(ref this.duration, nameof(this.duration));
            Scribe_Values.Look(ref this.bringIngredients, nameof(this.bringIngredients));
            Scribe_Values.Look(ref this.durationTicks, nameof(this.durationTicks));

            Scribe_Deep.Look(ref this.growthCell, nameof(this.growthCell));
            Scribe_Deep.Look(ref this.genomeDominant, nameof(this.genomeDominant));
            Scribe_Deep.Look(ref this.genomeSecondary, nameof(this.genomeSecondary));
            Scribe_Deep.Look(ref this.frame, nameof(this.frame));
            Scribe_Deep.Look(ref this.booster, nameof(this.booster));
        }

       
    }
}
