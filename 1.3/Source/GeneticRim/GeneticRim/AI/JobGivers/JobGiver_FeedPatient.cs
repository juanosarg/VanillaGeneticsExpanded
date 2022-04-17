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
			if (!FoodUtility.TryFindBestFoodSourceFor_NewTemp(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out Thing _, out ThingDef _, canRefillDispenser: false, canUseInventory: true, canUsePackAnimalInventory: true))
			{
				JobFailReason.Is("NoFood".Translate());
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
			if (FoodUtility.TryFindBestFoodSourceFor_NewTemp(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out Thing foodSource, out ThingDef foodDef, canRefillDispenser: false, canUseInventory: true, canUsePackAnimalInventory: true))
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
		}
	}

