using System;
using RimWorld;
using System.Reflection;
using Verse;


namespace GeneticRim
{
	public class StatPart_WorkTableImplants : StatPart
	{
		public float implantsWorkTableWorkSpeedFactor = 1.25f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && Applies(req.Thing))
			{
				val *= implantsWorkTableWorkSpeedFactor;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && Applies(req.Thing))
			{
				
				return "GR_GeneticTableImplantsSpeed".Translate() + ": x" + implantsWorkTableWorkSpeedFactor.ToStringPercent();
			}
			return null;
		}

		public static bool Applies(Thing th)
		{

			bool isImplantsTable = (th.def == InternalDefOf.GR_TissueGrowingVat);
			return isImplantsTable && Current.Game.World.factionManager.OfPlayer.ideos?.GetPrecept(InternalDefOf.GR_WorktableSpeeds_Implants) != null;
		}
	}
}
