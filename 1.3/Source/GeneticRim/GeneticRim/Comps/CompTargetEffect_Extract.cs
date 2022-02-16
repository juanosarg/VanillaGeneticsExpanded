
using RimWorld;
using Verse;
using Verse.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeneticRim
{
	public class CompTargetEffect_Extract : CompTargetEffect
	{

		
		public ThingDef thingToSpawn;

		public CompProperties_TargetEffect_Extract Props
		{
			get
			{
				return (CompProperties_TargetEffect_Extract)this.props;
			}
		}

		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (user.IsColonistPlayerControlled && user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly))
			{

				HashSet<ExtractableAnimalsList> allLists = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
				foreach (ExtractableAnimalsList individualList in allLists)
				{
					if (individualList.tier == Props.tier)
					{
                        if (individualList.extractableAnimals.Contains(target.def)) {
							thingToSpawn = individualList.itemProduced;
						}

						
						
					}
				}
                if (thingToSpawn != null)
				{
					Thing newThing = GenSpawn.Spawn(thingToSpawn, target.Position, target.Map, WipeMode.Vanish);
				}

				target.Destroy();




			}
		}
	}
}
