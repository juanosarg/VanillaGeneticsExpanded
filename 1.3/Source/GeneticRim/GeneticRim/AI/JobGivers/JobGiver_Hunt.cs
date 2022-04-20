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
	public class JobGiver_Hunt : ThinkNode_JobGiver
	{
		public PathEndMode PathEndMode => PathEndMode.OnCell;
		public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Hunt))
			{
				yield return item.target.Thing;
			}
		}

		public Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			
			if (!pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Hunt))
			{
				return true;
			}
			return false;
		}

		public bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.AnimalOrWildMan())
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Hunt) == null)
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
			Thing t = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn),
				PathEndMode, TraverseParms.For(pawn, MaxPathDanger(pawn), TraverseMode.ByPawn), 100f, predicate, PotentialWorkThingsGlobal(pawn));
			if (t is null)
			{
				return null;
			}
			
			return JobMaker.MakeJob(InternalDefOf.GR_AnimalHuntJob, t);
		}
	}
}
