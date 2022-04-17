
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace GeneticRim
{
    public class JobDriver_AnimalDeconstruct : JobDriver
    {
        private const float MaxDeconstructWork = 3000f;

        private const float MinDeconstructWork = 20f;

        private float workLeft;

        private float totalNeededWork;

        protected Thing Target => job.targetA.Thing;

        protected Building Building => (Building)Target.GetInnerIfMinified();

        protected DesignationDef Designation => DesignationDefOf.Deconstruct;

        protected float TotalNeededWork => Mathf.Clamp(Building.GetStatValue(StatDefOf.WorkToBuild), 20f, 3000f);

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workLeft, "workLeft", 0f);
            Scribe_Values.Look(ref totalNeededWork, "totalNeededWork", 0f);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOn(() => Building == null || !Building.DeconstructibleBy(pawn.Faction));
			this.FailOnThingMissingDesignation(TargetIndex.A, Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, (Target is Building_Trap) ? PathEndMode.OnCell : PathEndMode.Touch);
			Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = delegate
			{
				totalNeededWork = TotalNeededWork;
				workLeft = totalNeededWork;
			};
			doWork.tickAction = delegate
			{
				workLeft -= 1.7f;
				
				if (workLeft <= 0f)
				{
					doWork.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.WithProgressBar(TargetIndex.A, () => 1f - workLeft / totalNeededWork);
			
			yield return doWork;
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				if (Target.Faction != null)
				{
					Target.Faction.Notify_BuildingRemoved(Building, pawn);
				}
				FinishedRemoving();
				base.Map.designationManager.RemoveAllDesignationsOn(Target);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
		}

        protected void FinishedRemoving()
        {
            Target.Destroy(DestroyMode.Deconstruct);
            
        }

      
    }
}
