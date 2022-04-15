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
    public class JobGiver_Slaughter : ThinkNode_JobGiver
    {

        public PathEndMode PathEndMode => PathEndMode.OnCell;

        public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Slaughter))
            {
                yield return item.target.Thing;
            }
            foreach (Pawn item2 in pawn.Map.autoSlaughterManager.AnimalsToSlaughter)
            {
                yield return item2;
            }
        }
        public bool ShouldSkip(Pawn pawn)
        {
            if (!pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Slaughter))
            {
                return pawn.Map.autoSlaughterManager.AnimalsToSlaughter.Count == 0;
            }
            return false;
        }
        public bool HasJobOnThing(Pawn pawn, Thing t)
        {
            Pawn pawn2 = t as Pawn;
            if (pawn2 == null || !pawn2.RaceProps.Animal)
            {
                return false;
            }
            if (!pawn2.ShouldBeSlaughtered())
            {
                return false;
            }
            if (pawn.Faction != t.Faction)
            {
                return false;
            }
            if (pawn2.InAggroMentalState)
            {
                return false;
            }
            if (!pawn.CanReserve(t, 1, -1, null))
            {
                return false;
            }
            if (pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                JobFailReason.Is("IsIncapableOfViolenceShort".Translate(pawn));
                return false;
            }
            if (ModsConfig.IdeologyActive && !new HistoryEvent(HistoryEventDefOf.SlaughteredAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
            {
                return false;
            }
            if (HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) && !new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
            {
                return false;
            }
            if (pawn.Ideo != null && pawn.Ideo.IsVeneratedAnimal(pawn2) && !new HistoryEvent(HistoryEventDefOf.SlaughteredVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
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
                PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn), 100f, predicate, PotentialWorkThingsGlobal(pawn));
            if (t is null)
            {
                return null;
            }
            return JobMaker.MakeJob(JobDefOf.Slaughter, t);
        }
    }
}
