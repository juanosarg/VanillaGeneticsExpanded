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
	public static class GenerateAbandonedLabQuest
	{


		[DebugAction("Vanilla Genetics Expanded", "Generate Abandoned lab quest", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GenerateLabQuest()
		{

			Slate slate = new Slate();
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(InternalDefOf.GR_OpportunitySite_AbandonedLab, slate);

			QuestUtility.SendLetterQuestAvailable(quest);
		}




	}
}

