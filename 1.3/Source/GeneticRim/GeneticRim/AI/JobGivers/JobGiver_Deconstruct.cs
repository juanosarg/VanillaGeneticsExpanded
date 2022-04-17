using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
namespace GeneticRim
{
	public class JobGiver_Deconstruct : ThinkNode_JobGiver
	{

		public DesignationDef Designation => DesignationDefOf.Deconstruct;

		public JobDef RemoveBuildingJob => InternalDefOf.GR_AnimalDeconstructJob;

		public PathEndMode PathEndMode => PathEndMode.Touch;

		public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(Designation))
			{
				yield return item.target.Thing;
			}
		}

		public bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(Designation);
		}

		public bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, Designation) == null)
			{
				return false;
			}
			return true;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{

			if (ShouldSkip(pawn))
				return null;

			Predicate<Thing> predicate = (Thing x) => HasJobOnThing(pawn, x);
			Thing t = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial),
				PathEndMode, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn), 100f, predicate, PotentialWorkThingsGlobal(pawn));
			if (t is null)
			{
				return null;
			}
			return JobMaker.MakeJob(RemoveBuildingJob, t);

		}
	}
}
