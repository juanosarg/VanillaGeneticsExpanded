using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse.AI.Group;
using Verse;

namespace GeneticRim
{
	public static class SpawnPawnWithQuality
	{
		

		[DebugAction("Vanilla Genetics Expanded", "Spawn pawn with quality", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnPawnQuality()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (PawnKindDef item in DefDatabase<PawnKindDef>.AllDefs.Where(x => x.GetModExtension<DefExtension_Hybrid>()!=null).OrderBy((PawnKindDef kd) => kd.defName))
			{
				PawnKindDef localKindDef = item;
				list.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Tool, delegate
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					Pawn newPawn = PawnGenerator.GeneratePawn(localKindDef, faction);
					GenSpawn.Spawn(newPawn, UI.MouseCell(), Find.CurrentMap);
					CompHybrid compHybrid = newPawn.TryGetComp<CompHybrid>();
					if (compHybrid != null)
					{
						compHybrid.quality = QualityUtility.GenerateQualityRandomEqualChance(); 

					}
					if (faction != null && faction != Faction.OfPlayer)
					{
						Lord lord = null;
						if (newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Pawn p) => p != newPawn))
						{
							lord = ((Pawn)GenClosest.ClosestThing_Global(newPawn.Position, newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, (Thing p) => p != newPawn && ((Pawn)p).GetLord() != null)).GetLord();
						}
						if (lord == null)
						{
							LordJob_DefendPoint lordJob = new LordJob_DefendPoint(newPawn.Position);
							lord = LordMaker.MakeNewLord(faction, lordJob, Find.CurrentMap);
						}
						lord.AddPawn(newPawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		
		

	}
}

