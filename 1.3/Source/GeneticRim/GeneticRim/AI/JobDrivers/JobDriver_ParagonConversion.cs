
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
namespace GeneticRim
{
	public class JobDriver_ParagonConversion : JobDriver
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
				this.Map.GetComponent<ArchotechExtractableAnimals_MapComponent>().RemoveParagonToCarry(pawn);

				Building_Mechahybridizer building = (Building_Mechahybridizer)job.targetB.Thing;
				building.TryAcceptThing(pawn);
				building.progress = 0;
				building.Map.mapDrawer.MapMeshDirty(building.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
				

			};
			use.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return use;
			yield break;
		}
	}
}
