using RimWorld;
using Verse;
using System.Collections.Generic;

namespace GeneticRim
{
	public class CompProperties_TargetEffect_Extract : CompProperties
	{
		public List<string> tier;
		public int numberOfUses;

		public CompProperties_TargetEffect_Extract()
		{
			compClass = typeof(CompTargetEffect_Extract);
		}
	}
}
