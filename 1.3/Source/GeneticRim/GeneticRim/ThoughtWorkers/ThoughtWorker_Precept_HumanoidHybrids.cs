
using RimWorld;
using Verse;
namespace GeneticRim
{
	public class ThoughtWorker_Precept_HumanoidHybrids : ThoughtWorker_Precept
	{
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return StaticCollectionsClass.humanoid_hybrids.Count>0;
		}
	}
}
