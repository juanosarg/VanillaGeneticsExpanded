
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace GeneticRim
{
	public class ThinkNode_Dogman : ThinkNode_Conditional
	{


		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.def == InternalDefOf.GR_Dogman)
			{
				return true;
			}
			return false;
		}
	}
}
