
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace GeneticRim
{
	public class ThinkNode_Manbear : ThinkNode_Conditional
	{


		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.def == InternalDefOf.GR_Manbear)
			{
				return true;
			}
			return false;
		}
	}
}
