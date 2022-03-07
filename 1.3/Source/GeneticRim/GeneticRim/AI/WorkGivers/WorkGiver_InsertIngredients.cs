using System;
using Verse;
using Verse.AI;
using RimWorld;
using System.Collections.Generic;

namespace GeneticRim
{
    public class WorkGiver_InsertIngredients : WorkGiver_Scanner
    {
        private static string NoIngredientFound;

        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(InternalDefOf.GR_GenePod);


        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public static void ResetStaticData()
        {

            WorkGiver_InsertIngredients.NoIngredientFound = "GR_NoIngredientFound".Translate();
        }


        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            CompGenomorpher comp = t.TryGetComp<CompGenomorpher>();
            
            if (!comp.BringIngredients)
            {
                return false;
            }

            if (!t.IsForbidden(pawn))
            {
                LocalTargetInfo target = t;
                if (pawn.CanReserve(target, 1, 1, null, forced))
                {
                    if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
                    {
                        return false;
                    }
                    if (!pawn.CanReserve(comp.genomeDominant)|| !pawn.CanReserve(comp.genomeSecondary)|| !pawn.CanReserve(comp.frame) || (comp.booster!=null&&!pawn.CanReserve(comp.booster)))
                    {
                        comp.duration = -1;
                        comp.progress = -1f;
                        comp.growthCell = null;
                        return false;
                    }

                    return !t.IsBurning();
                }
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            CompGenomorpher comp = t.TryGetComp<CompGenomorpher>();

            if (!comp.BringIngredients)
                return JobMaker.MakeJob(JobDefOf.Vomit);

            List<Thing> chosenThings = new List<Thing> ();
            chosenThings.Add(comp.genomeDominant);
            chosenThings.Add(comp.genomeSecondary);
            chosenThings.Add(comp.frame);
            chosenThings.Add(comp.booster);

            Job job = JobMaker.MakeJob(InternalDefOf.GR_InsertIngredients, t);
            job.targetQueueB = new List<LocalTargetInfo>(chosenThings.Count);
            job.count = 1;
            for (int i = 0; i < chosenThings.Count; i++)
            {
                Thing chosenThing = chosenThings[i];
                if(chosenThing != null)
                    job.targetQueueB.Add(chosenThing);
            }
            job.haulMode = HaulMode.ToCellNonStorage;
           
            return job;

         
        }

       
    }
}



