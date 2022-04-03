
using RimWorld;
using Verse;
using Verse.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.Sound;


namespace GeneticRim
{
	public class CompTargetEffect_Extract : CompTargetEffect
	{

		
		public ThingDef thingToSpawn = null;

		public int numberOfUses=-1;

		public CompProperties_TargetEffect_Extract Props
		{
			get
			{
				return (CompProperties_TargetEffect_Extract)this.props;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
            if (numberOfUses == -1)
            {
				numberOfUses = Props.numberOfUses;

			}

		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref numberOfUses, "numberOfUses", 0);
		}


		public override string CompInspectStringExtra()
		{
			StringBuilder sb = new StringBuilder(base.CompInspectStringExtra());

			sb.AppendLine("GR_ExcavatorUsesLeft".Translate(this.numberOfUses));


			return sb.ToString().Trim();
		}

		public override void DoEffectOn(Pawn user, Thing target)
		{

			
			if (user.IsColonistPlayerControlled && user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly))
			{

				bool flagAlreadyExtracted = false;

				if (target is Corpse)
				{
					Corpse pawn = target as Corpse;
					Hediff hediff = pawn.InnerPawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.GR_ExtractedBrain);
                    if (hediff != null)
                    {
						flagAlreadyExtracted = true;
					}
				}
				else
				{
					Pawn pawn = target as Pawn;
					Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.GR_ExtractedBrain);
					if (hediff != null)
					{
						flagAlreadyExtracted = true;
					}
				}


                if (flagAlreadyExtracted) {
					Messages.Message("GR_AlreadyExtracted".Translate(), MessageTypeDefOf.RejectInput, true);
				} else {
					HashSet<ExtractableAnimalsList> allLists = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
					foreach (ExtractableAnimalsList individualList in allLists)
					{
						if (Props.tier.Contains(individualList.tier))
						{
							
							if (target is Corpse)
							{
								Corpse pawn = target as Corpse;
                                if ((individualList.needsHumanLike && pawn.InnerPawn.def.race.Humanlike)|| (individualList.extractableAnimals?.Contains(pawn.InnerPawn.def)==true)) { 							
									thingToSpawn = individualList.itemProduced;
								}
							}
							else
							{
								Pawn pawn = target as Pawn;
								if ((individualList.needsHumanLike && pawn.def.race.Humanlike) || (individualList.extractableAnimals?.Contains(pawn.def) == true)) 
								{
										thingToSpawn = individualList.itemProduced;
								}
							}

						}
					}

					if (thingToSpawn != null)
					{
						Thing newThing = ThingMaker.MakeThing(thingToSpawn);
						GenPlace.TryPlaceThing(newThing, target.Position, target.Map, ThingPlaceMode.Near);
						
						InternalDefOf.GR_Pop.PlayOneShot(new TargetInfo(target.Position, target.Map, false));
						this.numberOfUses--;
						if (numberOfUses <= 0)
						{
							user.carryTracker.DestroyCarriedThing();
						}
						if (target is Corpse)
						{

							Corpse pawn = target as Corpse;
							popBlood(null, pawn);
							pawn.InnerPawn.health.AddHediff(InternalDefOf.GR_ExtractedBrain);
						}
						else
						{
							Pawn pawn = target as Pawn;
							popBlood(pawn, null);
							pawn.health.AddHediff(InternalDefOf.GR_ExtractedBrain);
							pawn.Kill(null);
						}

						
					}
					else
					{
						Messages.Message("GR_NoValidExtraction".Translate(), MessageTypeDefOf.RejectInput, true);
					}
				}


				

				




			}else
            {
				Messages.Message("GR_ExtractionFailed".Translate(), MessageTypeDefOf.RejectInput, true);
			}
		}

		public void popBlood(Pawn pawn, Corpse corpse)
        {
			IntVec3 pos= IntVec3.Zero;
			Map map=null;
			ThingDef blood = ThingDefOf.Filth_Blood;

            if (pawn != null)
            {
				pos = pawn.Position;
				map = pawn.Map;
                if (pawn.def.race.Insect)
                {
					blood = ThingDef.Named("Filth_BloodInsect");
				}
            }
			if (corpse != null)
			{
				pos = corpse.InnerPawn.Position;
				map = corpse.InnerPawn.Map;
				if (corpse.InnerPawn.def.race.Insect)
				{
					blood = ThingDef.Named("Filth_BloodInsect");
				}
			}


			for (int i = 0; i < 20; i++)
			{
				IntVec3 c;
				CellFinder.TryFindRandomReachableCellNear(pos, map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
				FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Blood);

			}
		}

		

	}
}
