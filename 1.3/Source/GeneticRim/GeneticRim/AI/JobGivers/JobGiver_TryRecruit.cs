
using System.Linq;
using RimWorld;
using Verse;
using System;
using Verse.AI;
using RimWorld.Planet;
using System.Collections.Generic;

namespace GeneticRim
{
	public class JobGiver_TryRecruit : ThinkNode_JobGiver
	{
		public ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

		public  PathEndMode PathEndMode => PathEndMode.OnCell;

		public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawned;
		}

		public bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawnedCount == 0;
		}

		protected bool ShouldTakeCareOfPrisoner(Pawn warden, Thing prisoner, bool forced = false)
		{
			Pawn pawn = prisoner as Pawn;
			if (pawn == null || !pawn.IsPrisonerOfColony || !pawn.guest.PrisonerIsSecure || !pawn.Spawned || pawn.InAggroMentalState || prisoner.IsForbidden(warden) || pawn.IsFormingCaravan() || !warden.CanReserveAndReach(pawn, PathEndMode.OnCell, warden.NormalMaxDanger(), 1, -1, null, forced))
			{
				return false;
			}
			return true;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{

			if (ShouldSkip(pawn))
				return null;

			Predicate<Thing> predicate = (Thing x) => pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawned.Contains(x);
			Thing t = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn),
				PathEndMode, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn), 100f, predicate, PotentialWorkThingsGlobal(pawn));
			if (t is null)
			{
				return null;
			}

			if (!ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}

			

			Pawn pawn2 = (Pawn)t;
            
			PrisonerInteractionModeDef interactionMode = pawn2.guest.interactionMode;
			if ((interactionMode == PrisonerInteractionModeDefOf.AttemptRecruit || interactionMode == PrisonerInteractionModeDefOf.ReduceResistance) && pawn2.guest.ScheduledForInteraction && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && (!pawn2.Downed || pawn2.InBed()) && pawn.CanReserve(t) && pawn2.Awake())
			{
				if (interactionMode == PrisonerInteractionModeDefOf.ReduceResistance && pawn2.guest.Resistance <= 0f)
				{
					return null;
				}
				return JobMaker.MakeJob(InternalDefOf.GR_HumanoidHybridRecruit, t);
			}
			return null;

		}
	}
}
