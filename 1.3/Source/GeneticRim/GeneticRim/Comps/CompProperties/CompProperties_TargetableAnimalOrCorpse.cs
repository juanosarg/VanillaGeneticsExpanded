
using RimWorld;
using Verse;
namespace GeneticRim
{
	public class CompProperties_TargetableAnimalOrCorpse : CompProperties_Targetable
	{
		public string tier;

		public CompProperties_TargetableAnimalOrCorpse()
		{
			compClass = typeof(CompTargetableAnimalOrCorpse);
		}
	}
}
