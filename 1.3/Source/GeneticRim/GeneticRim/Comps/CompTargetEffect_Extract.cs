
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

		
		public ThingDef thingToSpawn;

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
						if (individualList.tier == Props.tier)
						{

							if (target is Corpse)
							{
								Corpse pawn = target as Corpse;
								if (individualList.extractableAnimals.Contains(pawn.InnerPawn.def))
								{
									thingToSpawn = individualList.itemProduced;
								}
							}
							else
							{
								Pawn pawn = target as Pawn;
								if (individualList.extractableAnimals.Contains(pawn.def))
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
						
						if (target is Corpse)
						{

							Corpse pawn = target as Corpse;
							pawn.InnerPawn.health.AddHediff(InternalDefOf.GR_ExtractedBrain);
						}
						else
						{
							Pawn pawn = target as Pawn;
							pawn.health.AddHediff(InternalDefOf.GR_ExtractedBrain);
							pawn.Kill(null);
						}

						for (int i = 0; i < 20; i++)
						{
							IntVec3 c;
							CellFinder.TryFindRandomReachableCellNear(target.Position, target.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
							FilthMaker.TryMakeFilth(c, target.Map, ThingDefOf.Filth_Blood);

						}
						InternalDefOf.GR_Pop.PlayOneShot(new TargetInfo(target.Position, target.Map, false));
						this.numberOfUses--;
                        if (numberOfUses <= 0)
                        {
							user.carryTracker.DestroyCarriedThing();
						}
					}
					else
					{
						Messages.Message("GR_NoValidExtraction".Translate(), MessageTypeDefOf.RejectInput, true);
					}
				}


				

				




			}
		}
	}
}
