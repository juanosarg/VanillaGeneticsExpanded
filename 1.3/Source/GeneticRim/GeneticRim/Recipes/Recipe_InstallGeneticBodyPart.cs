
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;


namespace GeneticRim
{

	public class Recipe_InstallGeneticBodyPart : Recipe_Surgery
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, delegate (BodyPartRecord record)
			{
				IEnumerable<Hediff> source = pawn.health.hediffSet.hediffs.Where((Hediff x) => x.Part == record);
				if (source.Count() == 1 && source.First().def == recipe.addsHediff)
				{
					return false;
				}
				if (record.parent != null && !pawn.health.hediffSet.GetNotMissingParts().Contains(record.parent))
				{
					return false;
				}
				return (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record)) ? true : false;
			});
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = !PawnGenerator.IsBeingGenerated(pawn) && IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			if (billDoer != null)
			{
				if (CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
				
				if (flag && flag2 && part.def.spawnThingOnRemoved != null)
				{
					ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn, billDoer);
				}
				if (flag2)
				{
					ReportViolation(pawn, billDoer, pawn.HomeFaction, -70);
				}
				if (ModsConfig.IdeologyActive)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InstalledProsthetic, billDoer.Named(HistoryEventArgsNames.Doer)));
				}
			}
			
			else
			{
				pawn.health.RestorePart(part);
			}
			pawn.health.AddHediff(recipe.addsHediff, part);

			if (recipe?.addsHediff != null && ingredients != null)
			{
				var hediff = pawn.health?.hediffSet?.hediffs?.FindLast(x => x.def == recipe.addsHediff);

				if (hediff != null)
				{
					var comp = hediff.TryGetComp<HediffCompImplantQuality>();
					if (comp != null)
					{
						foreach (var ingredient in ingredients)
						{
							if (ingredient != null && hediff.def.spawnThingOnRemoved == ingredient.def && ingredient.TryGetQuality(out var qualityCategory))
							{
								comp.quality = qualityCategory;
								comp.SelectSeverity(qualityCategory);
								break;
							}
						}
					}
				}
			}

		}

		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			if ((pawn.Faction == billDoerFaction || pawn.Faction == null) && !pawn.IsQuestLodger())
			{
				return false;
			}
			if (recipe.addsHediff.addedPartProps != null && recipe.addsHediff.addedPartProps.betterThanNatural)
			{
				return false;
			}
			return HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}
	}
}
