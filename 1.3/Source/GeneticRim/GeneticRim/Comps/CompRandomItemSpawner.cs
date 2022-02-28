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



    public class CompRandomItemSpawner : ThingComp
    {
        public CompProperties_RandomItemSpawner Props => (CompProperties_RandomItemSpawner)this.props;

       
        public override void CompTick()
        {
            base.CompTick();
            if (this.parent.IsHashIntervalTick(500))
            {

                if (Find.CurrentMap.mapPawns.FreeColonistsSpawnedCount > 0)
                {
                    SpawnItemAndDelete();
                }

            }
        }

        public void SpawnItemAndDelete()
        {
            GenSpawn.Spawn(Props.items.RandomElement(), this.parent.Position, this.parent.Map);
            this.parent.Destroy();

        }

    }
}
