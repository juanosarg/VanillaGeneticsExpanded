// RimWorld.JobDriver_UseItem
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
namespace GeneticRim
{
	public class JobDriver_ArchotechDNAExtraction : JobDriver
	{
		
	

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			job.count = 1;
			this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B);


			Toil toil = Toils_General.Wait(100);
			toil.WithProgressBarToilDelay(TargetIndex.B);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			toil.FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			if (job.targetB.IsValid)
			{
				toil.FailOnDespawnedOrNull(TargetIndex.B);

			}
			yield return toil;
			Toil use = new Toil();

			use.initAction = delegate
			{
				Pawn pawn = job.targetA.Pawn;
				this.Map.GetComponent<ArchotechExtractableAnimals_MapComponent>().RemoveAnimalToCarry(pawn);

				float ParagonOrHybridFactor = 0.5f;

				if (pawn.kindDef.GetModExtension<DefExtension_Hybrid>()?.dominantGenome == pawn.kindDef.GetModExtension<DefExtension_Hybrid>()?.secondaryGenome)
				{
					ParagonOrHybridFactor = 1f;
				}
				

				float DNAExtractionFactor = pawn.TryGetComp<CompHybrid>()?.GetDNAExtractionFactor() ?? 0f;

				Building_DNAStorageBank building = (Building_DNAStorageBank)job.targetB.Thing;

				float totalProgress = building.progress + (DNAExtractionFactor * ParagonOrHybridFactor);

                if (totalProgress >= 1)
                {
					building.progress = 1;

                }
                else { 
					building.progress += DNAExtractionFactor * ParagonOrHybridFactor; 
				}
				

				pawn.Destroy();

			};
			use.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return use;
			yield break;
		}
	}
}
