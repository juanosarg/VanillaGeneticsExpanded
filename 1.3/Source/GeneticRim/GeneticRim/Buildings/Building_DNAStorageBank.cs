using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using System.Text;

namespace GeneticRim
{
    public class Building_DNAStorageBank : Building
    {


        public ThingDef selectedGenome; 

        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            if (this.Faction == Faction.OfPlayer) {


                yield return GenomeListSetupUtility.SetGenomeListCommand(this, this.Map);


            }

        }

        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());

            if (selectedGenome!=null) { 
                sb.AppendLine("GR_SelectedGenome".Translate(selectedGenome.LabelCap)); 
            }


            return sb.ToString().Trim();
        }

        public override void ExposeData()
        {
            base.ExposeData();
  

            Scribe_Defs.Look(ref this.selectedGenome, nameof(this.selectedGenome));
           
        }


    }
}