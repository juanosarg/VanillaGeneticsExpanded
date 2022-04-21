
using System.Linq;
using RimWorld;
using Verse;
using System;
using Verse.AI;
using RimWorld.Planet;
using System.Collections.Generic;

namespace GeneticRim
{
	public class JobGiver_ClearSnow : ThinkNode_JobGiver
	{
		
		public PathEndMode PathEndMode => PathEndMode.Touch;

		public  IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowClear.ActiveCells;
		}

		public bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.SnowClear.TrueCount == 0;
		}

		public bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (pawn.Map.snowGrid.GetDepth(c) < 0.2f)
			{
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				return false;
			}
			if (!pawn.CanReserve(c, 1, -1, null, forced))
			{
				return false;
			}
			return true;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{

			if (ShouldSkip(pawn))
				return null;

			IntVec3 cell;
			PotentialWorkCellsGlobal(pawn).TryRandomElement(out cell);

			if (!HasJobOnCell(pawn,cell))
			{
				return null;
			}

			return JobMaker.MakeJob(JobDefOf.ClearSnow, cell);

		}
	}
}
