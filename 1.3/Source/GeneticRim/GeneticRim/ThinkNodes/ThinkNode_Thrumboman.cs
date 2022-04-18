
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace GeneticRim
{
	public class ThinkNode_Thrumboman : ThinkNode_Conditional
	{


		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.def == InternalDefOf.GR_Thrumboman)
			{
				return true;
			}
			return false;
		}
	}
}
