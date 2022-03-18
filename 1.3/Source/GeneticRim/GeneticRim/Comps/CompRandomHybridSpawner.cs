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



    public class CompRandomHybridSpawner : ThingComp
    {
        public CompProperties_RandomHybridSpawner Props => (CompProperties_RandomHybridSpawner)this.props;

        public HashSet<PawnKindDef> hybridsList = new HashSet<PawnKindDef>();

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
           hybridsList = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => (x.race.tradeTags?.Contains("AnimalGenetic") == true)&&x.defName!= "GR_ArchotechCentipede").ToHashSet();

        }

        public override void CompTick()
        {
            base.CompTick();
            if (this.parent.IsHashIntervalTick(500))
            {

                int num = GenRadial.NumCellsInRadius(12);
                for (int i = 0; i < num; i++)
                {
                    IntVec3 current = this.parent.Position + GenRadial.RadialPattern[i];
                    if (current.InBounds(this.parent.Map))
                    {
                        Pawn pawn = current.GetFirstPawn(this.parent.Map);
                        if ((pawn != null) && (pawn.Faction==Faction.OfPlayer))
                        {
                            SpawnHostileHYbrid();
                            break;
                        }
                    }
                }


            }
        }

        public void SpawnHostileHYbrid()
        {
            Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(hybridsList.RandomElement(), null, fixedBiologicalAge: 3, fixedChronologicalAge: 3,
                                                                                       newborn: false, forceGenerateNewPawn: true));
            IntVec3 near = CellFinder.StandableCellNear(this.parent.Position, this.parent.Map, 2f);

            GenSpawn.Spawn(pawn, near, this.parent.Map);

            CompHybrid compHybrid = pawn.TryGetComp<CompHybrid>();

            if (compHybrid != null)
            {
                compHybrid.quality = QualityUtility.GenerateQualityRandomEqualChance();
            }

            pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, true, false, null, false);
            pawn.health.AddHediff(InternalDefOf.GR_GreaterScaria);

            this.parent.Destroy();
            
        }

    }
}
