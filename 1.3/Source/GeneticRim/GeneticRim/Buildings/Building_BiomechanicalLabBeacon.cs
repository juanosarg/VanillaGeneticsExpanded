using System;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using RimWorld;
using System.Text;
using Verse.Sound;

namespace GeneticRim
{

 
    public class Building_BiomechanicalLabBeacon : Building
    {
        public int tickCounter=600;
        public int tickInterval = 60;
        public bool light = true;
        Graphic lightGraphic;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            lightGraphic = GraphicsCache.spark;
           
        }

        public override void ExposeData()
        {
            base.ExposeData();

           
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));
            Scribe_Values.Look(ref this.tickInterval, nameof(this.tickInterval));

            Scribe_Values.Look(ref this.light, nameof(this.light));



        }

        public override void Tick()
        {
            base.Tick();

            tickCounter--;

            if (tickCounter% tickInterval==0)
            {
                light = !light;
                InternalDefOf.GR_Beep.PlayOneShot(new TargetInfo(this.Position, this.Map, false));
            } 

            if(tickCounter == 400)
            {
                tickInterval = 30;
            }
            if (tickCounter == 250)
            {
                tickInterval = 15;
            }
            if (tickCounter == 150)
            {
                tickInterval = 10;
            }
            if (tickCounter == 75)
            {
                tickInterval = 5;
            }

            if (tickCounter <= 0)
            {
                GenExplosion.DoExplosion(this.Position, this.Map, 2.9f, DamageDefOf.Flame, this, -1, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
                Slate slate = new Slate();
                Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(InternalDefOf.GR_OpportunitySite_BiomechanicalLab, slate);

                QuestUtility.SendLetterQuestAvailable(quest);
                this.Destroy();
            }


        }

        public override void Draw()
        {
            base.Draw();



            if (light)
            {
                var vector2 = this.DrawPos + Altitudes.AltIncVect;
                vector2.y += 10;

                lightGraphic?.DrawFromDef(vector2, Rot4.North, null);
            }

            
        }
        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());
            
                sb.AppendLine("\n" + "GR_DecodingRadioSignals".Translate(tickCounter.ToStringTicksToPeriod(true, false, true, true)));



            return sb.ToString().Trim();
        }

    }
}