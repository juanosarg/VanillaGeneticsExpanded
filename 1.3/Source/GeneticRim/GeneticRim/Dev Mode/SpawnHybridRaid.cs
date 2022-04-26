using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse.AI.Group;
using Verse;
using RimWorld.QuestGen;

namespace GeneticRim
{
	public static class SpawnHybridRaid
	{


		[DebugAction("Vanilla Genetics Expanded", "Spawn hybrids raid", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnHybridRaidNow()
		{

			IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, Find.World);
			parms.target = Find.AnyPlayerHomeMap;
			IncidentDef def = InternalDefOf.GR_ManhunterMonstrosities;
			def.Worker.TryExecute(parms);
		}




	}
}

