using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld.Planet;
using Verse.AI;

namespace GeneticRim
{
    public class JobGiver_Execute : ThinkNode_JobGiver
    {

        public PathEndMode PathEndMode => PathEndMode.OnCell;

        public  IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawned;
        }
        public bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawnedCount == 0;
        }

        protected bool ShouldTakeCareOfPrisoner(Thing prisoner)
        {
            Pawn pawn = prisoner as Pawn;
            if (pawn == null || !pawn.IsPrisonerOfColony || !pawn.guest.PrisonerIsSecure || !pawn.Spawned || pawn.InAggroMentalState || pawn.IsFormingCaravan())
            {
               
                return false;
            }
            return true;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (ShouldSkip(pawn))
                return null;

            Predicate<Thing> predicate = (Thing x) => ShouldTakeCareOfPrisoner(x);
            Thing t = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn),
                PathEndMode, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn), 100f, predicate, PotentialWorkThingsGlobal(pawn));
            if (t is null)
            {
                return null;
            }
            
            Pawn pawn2 = (Pawn)t;
            if (pawn2.guest.interactionMode != PrisonerInteractionModeDefOf.Execution || !pawn.CanReserve(t))
            {
                return null;
            }
           
            return JobMaker.MakeJob(JobDefOf.PrisonerExecution, t);
        }
    }
}
