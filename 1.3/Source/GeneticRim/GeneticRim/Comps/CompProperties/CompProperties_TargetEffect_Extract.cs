
using RimWorld;
using Verse;
namespace GeneticRim
{
	public class CompProperties_TargetEffect_Extract : CompProperties
	{
		public string tier;
		public int numberOfUses;

		public CompProperties_TargetEffect_Extract()
		{
			compClass = typeof(CompTargetEffect_Extract);
		}
	}
}
