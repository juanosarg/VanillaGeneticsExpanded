using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;
using RimWorld;

namespace GeneticRim
{
    public class JobDriver_InsertIngredients : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            
            Thing thing = job.GetTarget(TargetIndex.A).Thing;
            if (!pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed))
            {
                return false;
            }
            
            pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
            return true;

        }

       

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
           
            CompGenomorpher comp = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompGenomorpher>();
           

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);


            foreach (LocalTargetInfo target in job.GetTargetQueue(TargetIndex.B)) {
              
                yield return Toils_General.DoAtomic(delegate
                {
                    job.SetTarget(TargetIndex.B, target);
                });
                yield return Toils_Reserve.Reserve(TargetIndex.B);
                yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
                yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
              
                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
                yield return Toils_General.Wait(200, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
                yield return Toils_General.DoAtomic(delegate
                {
                    job.GetTarget(TargetIndex.B).Thing.Destroy();
                });
                
            }
            yield return Toils_General.DoAtomic(delegate
            {
                comp.bringIngredients = false;
                comp.progress = 0;
                comp.duration = comp.durationTicks;
            });

           
     
            yield break;





        }
    }
}