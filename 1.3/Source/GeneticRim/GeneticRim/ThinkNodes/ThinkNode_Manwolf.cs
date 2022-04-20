
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace GeneticRim
{
	public class ThinkNode_Manwolf : ThinkNode_Conditional
	{


		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.def == InternalDefOf.GR_Manwolf)
			{
				return true;
			}
			return false;
		}
	}
}
