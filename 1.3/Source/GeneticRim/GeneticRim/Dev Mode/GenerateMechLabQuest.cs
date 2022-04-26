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
	public static class GenerateMechLabQuest
	{


		[DebugAction("Vanilla Genetics Expanded", "Generate Mech lab quest", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GenerateMechLabQuestNow()
		{

			Slate slate = new Slate();
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(InternalDefOf.GR_OpportunitySite_BiomechanicalLab, slate);

			QuestUtility.SendLetterQuestAvailable(quest);
		}




	}
}

