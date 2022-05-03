using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Text;
using Verse.Sound;

namespace GeneticRim
{


    public class Building_MechahybridAntenna : Building
    {
        public HashSet<Pawn> assignedMechs = new HashSet<Pawn>();
        public int maxMechs = 5;
        public const int checkingPeriod = 600;


        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref assignedMechs, "assignedMechs", LookMode.Reference);

        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(checkingPeriod))
            {
                maxMechs = GeneticRim_Mod.settings.GR_HybridsPerAntenna;
                foreach (Pawn pawn in assignedMechs)
                {

                    if (pawn.Dead || pawn.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.GR_GreaterScaria)!=null)
                    {
                        RemoveMechFromList(pawn);
                    }

                    CompDieUnlessReset comp = pawn.TryGetComp<CompDieUnlessReset>();
                    if (comp != null)
                    {
                        comp.ResetTimer();
                    }

                }


            }
        }

        public bool AddMechToList(Pawn pawn)
        {



            if (!assignedMechs.Contains(pawn) && assignedMechs.Count < maxMechs)
            {
                assignedMechs.Add(pawn);
                return true;
            }
            else return false;
        }

        public void RemoveMechFromList(Pawn pawn)
        {
            if (assignedMechs.Contains(pawn))
            {
                assignedMechs.Remove(pawn);
            }

        }

        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());

            sb.AppendLine("GR_MechsInThisAntenna".Translate());


            int i = 0;
            foreach (Pawn pawn in assignedMechs)
            {
                i++;
                sb.Append(pawn.LabelCap);
                if (i < assignedMechs.Count) { sb.Append(" - "); }

            }







            return sb.ToString().Trim();
        }


    }
}