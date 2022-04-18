
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace GeneticRim
{
    public class JobDriver_PlantCut : JobDriver
    {
        private float workDone;



        protected const TargetIndex PlantInd = TargetIndex.A;

        protected Plant Plant => (Plant)job.targetA.Thing;



        public static float WorkDonePerTick(Pawn actor, Plant plant)
        {
            return Mathf.Lerp(3.3f, 1f, plant.Growth);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            LocalTargetInfo target = job.GetTarget(TargetIndex.A);
            if (target.IsValid && !pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
            {
                return false;
            }
            pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.A), job);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {


            Toil toil = Toils_Goto.GotoThing(PlantInd, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(PlantInd);

            yield return toil;
            Toil cut = new Toil();
            cut.tickAction = delegate
            {
                Pawn actor = cut.actor;

                Plant plant = Plant;
                workDone += WorkDonePerTick(actor, Plant);
                if (workDone >= plant.def.plant.harvestWork)
                {
                    if (plant.def.plant.harvestedThingDef != null)
                    {
                        StatDef stat = plant.def.plant.harvestedThingDef.IsDrug ? StatDefOf.DrugHarvestYield : StatDefOf.PlantHarvestYield;
                        float statValue = actor.GetStatValue(stat);

                        int num = plant.YieldNow();
                        if (statValue > 1f)
                        {
                            num = GenMath.RoundRandom((float)num * statValue);
                        }
                        if (num > 0)
                        {
                            Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef);
                            thing.stackCount = num;
                           
                            Find.QuestManager.Notify_PlantHarvested(actor, thing);
                            GenPlace.TryPlaceThing(thing, actor.Position, base.Map, ThingPlaceMode.Near);
                           
                        }

                    }
                    plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
                    plant.PlantCollected(pawn);
                    workDone = 0f;
                    ReadyForNextToil();
                }
            };
            cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);           
            cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            cut.defaultCompleteMode = ToilCompleteMode.Never;
            cut.WithEffect((Plant?.def.plant.IsTree ?? false) ? EffecterDefOf.Harvest_Tree : EffecterDefOf.Harvest_Plant, TargetIndex.A);
            cut.WithProgressBar(TargetIndex.A, () => workDone / Plant.def.plant.harvestWork, interpolateBetweenActorAndTarget: true);
            cut.PlaySustainerOrSound(() => Plant.def.plant.soundHarvesting);
            cut.activeSkill = (() => SkillDefOf.Plants);
            yield return cut;
           
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workDone, "workDone", 0f);
        }



       
    }
}

