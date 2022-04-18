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
    public class JobGiver_Harvest : ThinkNode_JobGiver
    {
        public PathEndMode PathEndMode => PathEndMode.Touch;

        public Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }

        public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            List<Designation> desList = pawn.Map.designationManager.allDesignations;
            for (int i = 0; i < desList.Count; i++)
            {
                Designation designation = desList[i];
                if (designation.def == DesignationDefOf.CutPlant || designation.def == DesignationDefOf.HarvestPlant)
                {
                    yield return designation.target.Thing;
                }
            }
        }

        public bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            if (!pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.CutPlant))
            {
                return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.HarvestPlant);
            }
            return false;
        }

      

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (ShouldSkip(pawn))
                return null;

            Predicate<Thing> predicate = (Thing x) => pawn.Map.designationManager.DesignationOn(x, DesignationDefOf.CutPlant)!=null||
            pawn.Map.designationManager.DesignationOn(x, DesignationDefOf.HarvestPlant) != null;
            Thing t = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Plant),
                PathEndMode, TraverseParms.For(pawn, MaxPathDanger(pawn), TraverseMode.ByPawn), 100f, predicate, PotentialWorkThingsGlobal(pawn));
            if (t is null)
            {
                return null;
            }
            
            if (!pawn.CanReserve(t, 1, -1, null))
            {
                return null;
            }
            if (t.IsForbidden(pawn))
            {
                return null;
            }
            if (t.IsBurning())
            {
                return null;
            }
            

            return JobMaker.MakeJob(InternalDefOf.GR_AnimalHarvestJob, t);
             
        }
    }
}

