
using RimWorld;
using System.Collections.Generic;
using Verse;
using System.Linq;

namespace GeneticRim
{
    public class IncidentWorker_Monstrosities : IncidentWorker
    {
       
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }
            Map map = (Map)parms.target;           
            return RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 result, map, CellFinder.EdgeRoadChance_Animal);        
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;

            float totalPoints = parms.points/2;

            HashSet<PawnKindDef> hybridsList;
            if (StaticCollectionsClass.AnyMechAntennas()) {
                hybridsList = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => (x.race?.tradeTags?.Contains("AnimalGenetic") == true) && (x.race?.tradeTags?.Contains("AnimalGeneticFailure") == false) && (x.race?.tradeTags?.Contains("AnimalGeneticCentipede") == false)).ToHashSet();

            }
            else
            {
                hybridsList = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => (x.race?.tradeTags?.Contains("AnimalGenetic") == true) && (x.race?.tradeTags?.Contains("AnimalGeneticFailure") == false) && (x.race?.tradeTags?.Contains("AnimalGeneticMechanoid") == false)).ToHashSet();

            }

            List<Pawn> list = new List<Pawn>();

            PawnKindDef firstPawn;
            hybridsList.TryRandomElementByWeight((PawnKindDef a) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(a, parms.points), out firstPawn);
            IntVec3 result = parms.spawnCenter;
            if (firstPawn != null)
            {
                
                if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal))
                {
                    return false;
                }
                Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(firstPawn, null, PawnGenerationContext.NonPlayer, map.Tile));
                list.Add(item);
                totalPoints -= item.kindDef.combatPower;
                while (totalPoints>0)
                {
                    PawnKindDef nextPawn;
                    hybridsList.TryRandomElementByWeight((PawnKindDef a) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(a, totalPoints), out nextPawn);
                    Pawn nextitem = PawnGenerator.GeneratePawn(new PawnGenerationRequest(nextPawn, null, PawnGenerationContext.NonPlayer, map.Tile));
                    list.Add(nextitem);
                    totalPoints -= nextitem.kindDef.combatPower;
                }

            }


            Rot4 rot = Rot4.FromAngleFlat((map.Center - result).AngleFlat);
            for (int i = 0; i < list.Count; i++)
            {
                Pawn pawn = list[i];
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, map, 10);
                QuestUtility.AddQuestTag(GenSpawn.Spawn(pawn, loc, map, rot), parms.questTag);
                pawn.health.AddHediff(InternalDefOf.GR_GreaterScaria);
                pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent);
                CompHybrid compHybrid = pawn.TryGetComp<CompHybrid>();

                if (compHybrid != null)
                {
                    compHybrid.quality = QualityUtility.GenerateQualityRandomEqualChance();
                }

            }
            SendStandardLetter("GR_LetterLabelMonstrositiesArrived".Translate(), "GR_LetterMonstrositiesArrived".Translate(), LetterDefOf.ThreatBig, parms, list[0]);
            Find.TickManager.slower.SignalForceNormalSpeedShort();
           
            return true;
        }
    }
}