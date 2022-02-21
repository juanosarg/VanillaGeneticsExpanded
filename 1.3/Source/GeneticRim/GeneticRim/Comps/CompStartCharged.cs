using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;
namespace GeneticRim
{
   


    public class CompStartCharged : ThingComp
    {
        public CompProperties_StartCharged Props => (CompProperties_StartCharged)this.props;

       

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            CompPowerBattery comp = this.parent.GetComp<CompPowerBattery>();

            comp.SetStoredEnergyPct(1f);

        }

    }
}
