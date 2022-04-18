
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
namespace GeneticRim
{
	public class JobDriver_Recruit : JobDriver
	{
		private const int TalkDuration = 2000;

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		private static readonly SimpleCurve ResistanceImpactFactorCurve_Mood = new SimpleCurve
	{
		new CurvePoint(0f, 0.2f),
		new CurvePoint(0.5f, 1f),
		new CurvePoint(1f, 1.5f)
	};

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
			Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).socialMode = RandomSocialMode.Off;
			Toils_General.WaitWith(TargetIndex.A, TalkDuration, useProgressBar: false, maintainPosture: true).socialMode = RandomSocialMode.Off;
			yield return Toils_General.Do(delegate
			{
				Pawn recipient = (Pawn)pawn.CurJob.targetA.Thing;
				pawn.interactions.TryInteractWith(recipient, InternalDefOf.GR_TalkingToHumans);
				if (!recipient.interactions.InteractedTooRecentlyToInteract())
				{
					float num2 = 1f;
					float resistanceReduce = 0f;
					num2 *= ResistanceImpactFactorCurve_Mood.Evaluate((recipient.needs.mood == null) ? 1f : recipient.needs.mood.CurInstantLevelPercentage);

					num2 = Mathf.Min(num2, recipient.guest.resistance);
					float resistance = recipient.guest.resistance;
					recipient.guest.resistance = Mathf.Max(0f, recipient.guest.resistance - num2);
					resistanceReduce = resistance - recipient.guest.resistance;
					string text = "TextMote_ResistanceReduced".Translate(resistance.ToString("F1"), recipient.guest.resistance.ToString("F1"));
					if (recipient.needs.mood != null && recipient.needs.mood.CurLevelPercentage < 0.4f)
					{
						text += "\n(" + "lowMood".Translate() + ")";
					}

					MoteMaker.ThrowText((pawn.DrawPos + recipient.DrawPos) / 2f, pawn.Map, text, 8f);
					if (recipient.guest.resistance == 0f)
					{
						TaggedString taggedString2 = "MessagePrisonerResistanceBroken".Translate(recipient.LabelShort, pawn.LabelShort, pawn.Named("WARDEN"), recipient.Named("PRISONER"));
						if (recipient.guest.interactionMode == PrisonerInteractionModeDefOf.AttemptRecruit)
						{
							taggedString2 += " " + "MessagePrisonerResistanceBroken_RecruitAttempsWillBegin".Translate();
						}
						Messages.Message(taggedString2, recipient, MessageTypeDefOf.PositiveEvent);
					}
					recipient.mindState.interactionsToday += 1;
				}
				


			});
		}
	}
}
