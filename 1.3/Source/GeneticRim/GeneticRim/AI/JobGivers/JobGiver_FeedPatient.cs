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
    public class JobGiver_FeedPatient : ThinkNode_JobGiver
    {
		private static HashSet<Thing> filtered = new HashSet<Thing>();
		public PathEndMode PathEndMode => PathEndMode.ClosestTouch;
        public ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

        public Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }

        public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.SpawnedHungryPawns;
        }

        public bool HasJobOnThing(Pawn pawn, Thing t)
        {
            Pawn pawn2 = t as Pawn;
            if (pawn2 == null || pawn2 == pawn)
            {
                return false;
            }

            if (!FeedPatientUtility.IsHungry(pawn2))
            {
                return false;
            }
            if (!FeedPatientUtility.ShouldBeFed(pawn2))
            {
                return false;
            }
            if (!pawn.CanReserve(t, 1, -1, null))
            {
                return false;
            }
            
            return true;
        }
        protected override Job TryGiveJob(Pawn pawn)
        {

            Predicate<Thing> predicate = (Thing x) => x.def.category == ThingCategory.Pawn && HasJobOnThing(pawn, x);
            Thing t = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn),
                PathEndMode, TraverseParms.For(pawn, MaxPathDanger(pawn), TraverseMode.ByPawn), 100f, predicate, PotentialWorkThingsGlobal(pawn));
            if (t is null)
            {
                return null;
            }
            Pawn pawn2 = (Pawn)t;
            if (TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out Thing foodSource, out ThingDef foodDef, canRefillDispenser: false, canUseInventory: true, canUsePackAnimalInventory: true))
            {
                float nutrition = FoodUtility.GetNutrition(foodSource, foodDef);
                Job job = JobMaker.MakeJob(JobDefOf.FeedPatient);
                job.targetA = foodSource;
                job.targetB = pawn2;
                job.count = FoodUtility.WillIngestStackCountOf(pawn2, foodDef, nutrition);
                return job;
            }
            return null;
        }



        public bool TryFindBestFoodSourceFor(Pawn getter, Pawn eater, bool desperate, out Thing foodSource, out ThingDef foodDef, bool canRefillDispenser = true, bool canUseInventory = true, bool canUsePackAnimalInventory = false, bool allowForbidden = false, bool allowCorpse = true, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false, bool ignoreReservations = false, bool calculateWantedStackCount = false, FoodPreferability minPrefOverride = FoodPreferability.Undefined)
        {
            
            bool allowDrug = !eater.IsTeetotaler();
            Thing thing = null;
            Pawn packAnimal = null;           
            Pawn getter2 = getter;
            Pawn eater2 = eater;
            bool allowPlant = getter == eater;
            bool allowForbidden2 = allowForbidden;
            ThingDef foodDef2;
            Thing thing2 = BestFoodSourceOnMap(getter2, eater2, desperate, out foodDef2, FoodPreferability.MealLavish, allowPlant, allowDrug, allowCorpse, allowDispenserFull: true, canRefillDispenser, allowForbidden2, allowSociallyImproper, allowHarvest, forceScanWholeMap, ignoreReservations, calculateWantedStackCount, minPrefOverride);
            if (thing2 == null && thing == null)
            {
                thing = FirstFoodInClosestPackAnimalInventory((minPrefOverride == FoodPreferability.Undefined) ? FoodPreferability.MealAwful : minPrefOverride);
            }
            if (thing != null || thing2 != null)
            {
                if (thing == null && thing2 != null)
                {
                    foodSource = thing2;
                    foodDef = foodDef2;
                    return true;
                }
                ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing);
                if (thing2 == null)
                {
                    foodSource = thing;
                    foodDef = finalIngestibleDef;
                    return true;
                }
                float num = FoodUtility.FoodOptimality(eater, thing2, foodDef2, (getter.Position - thing2.Position).LengthManhattan);
                float num2 = FoodUtility.FoodOptimality(eater, thing, finalIngestibleDef, (packAnimal != null) ? (getter.Position - packAnimal.Position).LengthManhattan : 0);
                num2 -= 32f;
                if (num > num2)
                {
                    foodSource = thing2;
                    foodDef = foodDef2;
                    return true;
                }
                foodSource = thing;
                foodDef = FoodUtility.GetFinalIngestibleDef(foodSource);
                return true;
            }
            
            
            foodSource = null;
            foodDef = null;
            return false;
            Thing FirstFoodInClosestPackAnimalInventory(FoodPreferability foodPref)
            {
                Thing result = null;
                if ((canUseInventory & canUsePackAnimalInventory) && eater.IsColonist && getter.IsColonist && getter.Map != null)
                {
                    Thing thing3 = null;
                    {
                        foreach (Pawn spawnedColonyAnimal in getter.Map.mapPawns.SpawnedColonyAnimals)
                        {
                            thing3 = FoodUtility.BestFoodInInventory(spawnedColonyAnimal, eater, foodPref);
                            if (thing3 != null && (packAnimal == null || (getter.Position - packAnimal.Position).LengthManhattan > (getter.Position - spawnedColonyAnimal.Position).LengthManhattan) && !spawnedColonyAnimal.IsForbidden(getter) && getter.CanReach(spawnedColonyAnimal, PathEndMode.OnCell, Danger.Some))
                            {
                                packAnimal = spawnedColonyAnimal;
                                result = thing3;
                            }
                        }
                        return result;
                    }
                }
                return result;
            }
        }

		public Thing BestFoodSourceOnMap(Pawn getter, Pawn eater, bool desperate, out ThingDef foodDef, FoodPreferability maxPref = FoodPreferability.MealLavish, bool allowPlant = true, bool allowDrug = true, bool allowCorpse = true, bool allowDispenserFull = true, bool allowDispenserEmpty = true, bool allowForbidden = false, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false, bool ignoreReservations = false, bool calculateWantedStackCount = false, FoodPreferability minPrefOverride = FoodPreferability.Undefined, float? minNutrition = null)
		{
			foodDef = null;
			
			FoodPreferability minPref;
			if (minPrefOverride == FoodPreferability.Undefined)
			{
				if (eater.NonHumanlikeOrWildMan())
				{
					minPref = FoodPreferability.NeverForNutrition;
				}
				else if (desperate)
				{
					minPref = FoodPreferability.DesperateOnly;
				}
				else
				{
					minPref = (((int)eater.needs.food.CurCategory >= 2) ? FoodPreferability.RawBad : FoodPreferability.MealAwful);
				}
			}
			else
			{
				minPref = minPrefOverride;
			}
			Predicate<Thing> foodValidator = delegate (Thing t)
			{
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = t as Building_NutrientPasteDispenser;
				if (building_NutrientPasteDispenser != null)
				{
					if (!allowDispenserFull || (int)ThingDefOf.MealNutrientPaste.ingestible.preferability < (int)minPref || (int)ThingDefOf.MealNutrientPaste.ingestible.preferability > (int)maxPref || !eater.WillEat(ThingDefOf.MealNutrientPaste, getter) || (t.Faction != getter.Faction && t.Faction != getter.HostFaction) || (!allowForbidden && t.IsForbidden(getter)) || !building_NutrientPasteDispenser.powerComp.PowerOn || (!allowDispenserEmpty && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers()) || !t.InteractionCell.Standable(t.Map) || !IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) || !getter.Map.reachability.CanReachNonLocal(getter.Position, new TargetInfo(t.InteractionCell, t.Map), PathEndMode.OnCell, TraverseParms.For(getter, Danger.Some)))
					{
						return false;
					}
				}
				else
				{
					int stackCount = 1;
					float statValue = t.GetStatValue(StatDefOf.Nutrition);
					if (minNutrition.HasValue)
					{
						stackCount = FoodUtility.StackCountForNutrition(minNutrition.Value, statValue);
					}
					else if (calculateWantedStackCount)
					{
						stackCount = FoodUtility.WillIngestStackCountOf(eater, t.def, statValue);
					}
					if ((int)t.def.ingestible.preferability < (int)minPref || (int)t.def.ingestible.preferability > (int)maxPref || !eater.WillEat(t, getter) || !t.def.IsNutritionGivingIngestible || !t.IngestibleNow || (!allowCorpse && t is Corpse) || (!allowDrug && t.def.IsDrug) || (!allowForbidden && t.IsForbidden(getter)) || (!desperate && t.IsNotFresh()) || t.IsDessicated() || !IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) || (!getter.AnimalAwareOf(t) && !forceScanWholeMap) || (!ignoreReservations && !getter.CanReserve(t, 10, stackCount)))
					{
						return false;
					}
				}
				return true;
			};
			ThingRequest thingRequest = ((eater.RaceProps.foodType & (FoodTypeFlags.Plant | FoodTypeFlags.Tree)) == 0 || !allowPlant) ? ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree) : ThingRequest.ForGroup(ThingRequestGroup.FoodSource);
			Thing bestThing;
			if (getter.RaceProps.Humanlike)
			{
				bestThing = SpawnedFoodSearchInnerScan(eater, getter.Position, getter.Map.listerThings.ThingsMatching(thingRequest), PathEndMode.ClosestTouch, TraverseParms.For(getter), 9999f, foodValidator);
				if (allowHarvest)
				{
					Thing thing = GenClosest.ClosestThingReachable(searchRegionsMax: (!forceScanWholeMap || bestThing != null) ? 30 : (-1), root: getter.Position, map: getter.Map, thingReq: ThingRequest.ForGroup(ThingRequestGroup.HarvestablePlant), peMode: PathEndMode.Touch, traverseParams: TraverseParms.For(getter), maxDistance: 9999f, validator: delegate (Thing x)
					{
						Plant plant = (Plant)x;
						if (!plant.HarvestableNow)
						{
							return false;
						}
						ThingDef harvestedThingDef = plant.def.plant.harvestedThingDef;
						if (!harvestedThingDef.IsNutritionGivingIngestible)
						{
							return false;
						}
						if (!eater.WillEat(harvestedThingDef, getter))
						{
							return false;
						}
						if (!getter.CanReserve(plant))
						{
							return false;
						}
						if (!allowForbidden && plant.IsForbidden(getter))
						{
							return false;
						}
						return (bestThing == null || (int)FoodUtility.GetFinalIngestibleDef(bestThing).ingestible.preferability < (int)harvestedThingDef.ingestible.preferability) ? true : false;
					});
					if (thing != null)
					{
						bestThing = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(thing, harvest: true);
					}
				}
				if (foodDef == null && bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing);
				}
			}
			else
			{
				int maxRegionsToScan = GetMaxRegionsToScan(getter, forceScanWholeMap);
				filtered.Clear();
				foreach (Thing item in GenRadial.RadialDistinctThingsAround(getter.Position, getter.Map, 2f, useCenter: true))
				{
					Pawn pawn = item as Pawn;
					if (pawn != null && pawn != getter && pawn.RaceProps.Animal && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Ingest && pawn.CurJob.GetTarget(TargetIndex.A).HasThing)
					{
						filtered.Add(pawn.CurJob.GetTarget(TargetIndex.A).Thing);
					}
				}
				bool ignoreEntirelyForbiddenRegions = !allowForbidden && ForbidUtility.CaresAboutForbidden(getter, cellTarget: true) && getter.playerSettings != null && getter.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap != null;
				Predicate<Thing> validator = delegate (Thing t)
				{
					if (!foodValidator(t))
					{
						return false;
					}
					if (filtered.Contains(t))
					{
						return false;
					}
					if (!(t is Building_NutrientPasteDispenser) && (int)t.def.ingestible.preferability <= 2)
					{
						return false;
					}
					return (!t.IsNotFresh()) ? true : false;
				};
				bestThing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingRequest, PathEndMode.ClosestTouch, TraverseParms.For(getter), 9999f, validator, null, 0, maxRegionsToScan, forceAllowGlobalSearch: false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
				filtered.Clear();
				if (bestThing == null)
				{
					desperate = true;
					bestThing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingRequest, PathEndMode.ClosestTouch, TraverseParms.For(getter), 9999f, foodValidator, null, 0, maxRegionsToScan, forceAllowGlobalSearch: false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
				}
				if (bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing);
				}
			}
			return bestThing;
		}
		private bool IsFoodSourceOnMapSociallyProper(Thing t, Pawn getter, Pawn eater, bool allowSociallyImproper)
		{
			if (!allowSociallyImproper)
			{
				bool animalsCare = !getter.RaceProps.Animal;
				if (!t.IsSociallyProper(getter) && !t.IsSociallyProper(eater, eater.IsPrisonerOfColony, animalsCare))
				{
					return false;
				}
			}
			return true;
		}
		private Thing SpawnedFoodSearchInnerScan(Pawn eater, IntVec3 root, List<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null)
		{
			if (searchSet == null)
			{
				return null;
			}
			Pawn pawn = traverseParams.pawn ?? eater;
			int num = 0;
			int num2 = 0;
			Thing result = null;
			float num3 = 0f;
			float num4 = float.MinValue;
			for (int i = 0; i < searchSet.Count; i++)
			{
				Thing thing = searchSet[i];
				num2++;
				float num5 = (root - thing.Position).LengthManhattan;
				if (!(num5 > maxDistance))
				{
					num3 = FoodUtility.FoodOptimality(eater, thing, FoodUtility.GetFinalIngestibleDef(thing), num5);
					if (!(num3 < num4) && pawn.Map.reachability.CanReach(root, thing, peMode, traverseParams) && thing.Spawned && (validator == null || validator(thing)))
					{
						result = thing;
						num4 = num3;
						num++;
					}
				}
			}
			return result;
		}
		private int GetMaxRegionsToScan(Pawn getter, bool forceScanWholeMap)
		{
			if (getter.RaceProps.Humanlike)
			{
				return -1;
			}
			if (forceScanWholeMap)
			{
				return -1;
			}
			if (getter.Faction == Faction.OfPlayer)
			{
				return 100;
			}
			return 30;
		}
	}

	
}

