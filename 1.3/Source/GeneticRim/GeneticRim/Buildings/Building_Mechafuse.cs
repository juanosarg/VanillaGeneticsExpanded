using System;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using RimWorld;
using System.Text;
using Verse.Sound;

namespace GeneticRim
{


    public class Building_Mechafuse : Building
    {
        public bool active = true;
        Graphic usedGraphic;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            usedGraphic = GraphicsCache.spentfuse;

        }
        public override Graphic Graphic
        {
            get
            {
                if (active)
                {
                    return this.DefaultGraphic;
                }
                else return usedGraphic;
            }
        }

        public override void ExposeData()
        {
            //Save all the key variables so they work on game save / load
            base.ExposeData();

            Scribe_Values.Look(ref this.active, nameof(this.active));
        }

        public override string Label
        {
            get {

                if (active)
                {
                    return base.Label;
                } else return base.Label+ " (Spent)";


            }


        }

    }
}