using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticRim
{
    using RimWorld;
    using Verse;

    public class CompElectroWomb : ThingComp
    {
        public CompProperties_ElectroWomb Props => (CompProperties_ElectroWomb)this.props;

        public PawnKindDef growingResult;
        public float       progress;

        public override void CompTick()
        {
            base.CompTick();

            if (this.growingResult != null)
            {
                this.progress += 1f / (GenDate.TicksPerDay * 7f);

                if (this.progress > 1)
                {
                    Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.growingResult, Faction.OfPlayer, fixedBiologicalAge: 0, fixedChronologicalAge: 0, 
                                                                                     newborn: true, forceGenerateNewPawn: true));

                    IntVec3 near = CellFinder.StandableCellNear(this.parent.Position, this.parent.Map, 5f);

                    GenSpawn.Spawn(pawn, near, this.parent.Map);

                    this.progress      = 0;
                    this.growingResult = null;
                    
                    //finished
                }
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Defs.Look(ref this.growingResult, nameof(this.growingResult));
            Scribe_Values.Look(ref this.progress, nameof(this.progress));
        }


        public void InitProcess(Thing growthCell)
        {
            CompGrowthCell growthComp = growthCell.TryGetComp<CompGrowthCell>();

            PawnKindDef result = Core.GetHybrid(growthComp.genomeDominant, growthComp.genomeSecondary, growthComp.genoframe,       growthComp.booster,
                                              out float swapChance,      out float failureChance,    out PawnKindDef swapResult, out PawnKindDef failureResult);

            bool swap = Rand.Chance(swapChance);
            result = swap ? result : swapResult;

            bool failure = Rand.Chance(failureChance);

            if (!failure && result != null && result.RaceProps.baseBodySize < this.Props.maxBodySize)
            {
                this.growingResult = result;
                this.progress      = 0;
                return;
            }

            //todo: failures here
        }
    }
}
