using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticRim
{
    using Comps;
    using RimWorld;
    using Verse;

    public class CompElectroWomb : ThingComp
    {
        private static Dictionary<ThingDef, Dictionary<ThingDef, PawnKindDef>> hybrids;

        public static Dictionary<ThingDef, Dictionary<ThingDef, PawnKindDef>> Hybrids
        {
            get
            {
                if (hybrids != null)
                    return hybrids;

                hybrids = new Dictionary<ThingDef, Dictionary<ThingDef, PawnKindDef>>();

                foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefsListForReading)
                {
                    DefExtension_Hybrid hybridExt = pawnKindDef.GetModExtension<DefExtension_Hybrid>();
                    
                    if (hybridExt != null)
                    {
                        if(!hybrids.ContainsKey(hybridExt.dominantGenome))
                            hybrids.Add(hybridExt.dominantGenome, new Dictionary<ThingDef, PawnKindDef>());
                        hybrids[hybridExt.dominantGenome].Add(hybridExt.secondaryGenome, pawnKindDef);
                    }
                }

                return hybrids;
            }
        }


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

            DefExtension_HybridChanceAlterer frameExtension = growthComp.genoframe.GetModExtension<DefExtension_HybridChanceAlterer>();
            DefExtension_HybridChanceAlterer boosterExtension = growthComp.booster.GetModExtension<DefExtension_HybridChanceAlterer>();
            
            bool swap     = Rand.Chance((10f - frameExtension.stability - boosterExtension.stability) / 100f);
            
            if(hybrids.TryGetValue(!swap ? growthComp.genomeDominant : growthComp.genomeSecondary, out Dictionary<ThingDef, PawnKindDef> secondaryChain))
            {
                if (secondaryChain.TryGetValue(!swap ? growthComp.genomeSecondary : growthComp.genomeDominant, out PawnKindDef pkd))
                {
                    if (!Rand.Chance((10f - frameExtension.safety - boosterExtension.safety) / 100f))
                    {
                        if (pkd.RaceProps.baseBodySize < this.Props.maxBodySize)
                        {
                            this.growingResult = pkd;
                            this.progress      = 0;
                            return;
                        }
                    }
                }
            }

            //todo: failures here
        }
    }
}
