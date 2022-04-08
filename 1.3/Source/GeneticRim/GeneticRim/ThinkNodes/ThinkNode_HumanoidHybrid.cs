
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace GeneticRim
{
	public class ThinkNode_HumanoidHybrid : ThinkNode_Conditional
	{
		

		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.def?.tradeTags?.Contains("AnimalHumanoidHybrid")==true)
			{
				return true;
			}
			return false;
		}
	}
}
