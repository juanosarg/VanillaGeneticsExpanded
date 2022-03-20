
using RimWorld;
using Verse;
namespace GeneticRim
{
	public class ThoughtWorker_Precept_Failures : ThoughtWorker_Precept
	{
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return StaticCollectionsClass.failures_in_map.Count>0;
		}
	}
}
