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

    [StaticConstructorOnStartup]
    public class Building_ArchoWomb : Building
    {

        public bool readyToStart = false;
        public float wombProgress = -1;
        public const int wombDuration = 355; //in hours, almost 15 days. A bit less so the spaceship letter won't pop up
        Graphic usedGraphic;

        public override void ExposeData()
        {
            base.ExposeData();

          
            Scribe_Values.Look(ref this.readyToStart, nameof(this.readyToStart));
            Scribe_Values.Look(ref this.wombProgress, nameof(this.wombProgress));




        }

       

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            usedGraphic = GraphicsCache.graphicTopArcho;
            usedGraphic.drawSize = this.def.graphicData.drawSize;
        }

        public override void Draw()
        {
            base.Draw();
            if (wombProgress != -1) { 
            var vector = this.DrawPos + Altitudes.AltIncVect;
            vector.y += 5;

            Vector2 drawingSize = this.def.graphicData.drawSize / 2 * this.wombProgress;

            Graphic graphic = InternalDefOf.GR_ArchotechCentipede.lifeStages.Last().bodyGraphicData.Graphic.GetCopy(drawingSize, null);
            graphic?.DrawFromDef(vector, Rot4.South, null);}

            var vector2 = this.DrawPos + Altitudes.AltIncVect;
            vector2.y += 6;

            usedGraphic?.DrawFromDef(vector2, Rot4.North, null);
        }

        public override void Tick()
        {
            base.Tick();

            if (wombProgress != -1)
            {

                this.wombProgress += 1f / (GenDate.TicksPerHour * wombDuration);
                if (this.wombProgress >= 1)
                {
                   
                    ArchotechCountdown.InitiateCountdown(this);
                    wombProgress = -1;
                }

            }
        }

        public void InitProcess()
        {
            readyToStart = true;

        }

        public void StartupHibernatingWomb()
        {
            
                CompHibernatable compHibernatable = this.TryGetComp<CompHibernatable>();
                if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
                {
                    compHibernatable.Startup();
                }
            
        }

        public void StopHibernatingWomb()
        {

            CompHibernatable compHibernatable = this.TryGetComp<CompHibernatable>();
            if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting)
            {
                compHibernatable.State= HibernatableStateDefOf.Hibernating;
            }

        }





        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }


            if (this.Faction == Faction.OfPlayer)
            {


                Command_Action command_Action = new Command_Action();

                command_Action.defaultDesc = "GR_BeginArchotechCentipedeDesc".Translate();
                command_Action.defaultLabel = "GR_BeginArchotechCentipede".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("ui/commands/GR_AwakenArchotech", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {


                    DiaNode diaNode = new DiaNode("GR_BeginArchotechCentipedeText".Translate());
                    DiaOption diaOption = new DiaOption("Confirm".Translate());
                    diaOption.action = delegate
                    {
                        wombProgress = 0;
                        StartupHibernatingWomb();
                        readyToStart = false;
                    };
                    diaOption.resolveTree = true;
                    diaNode.options.Add(diaOption);
                    DiaOption diaOption2 = new DiaOption("GoBack".Translate());
                    diaOption2.resolveTree = true;
                    diaNode.options.Add(diaOption2);
                    Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));

                    readyToStart = false;

                };
                if (!readyToStart)
                {
                    command_Action.Disable("GR_ArchotechGrowthCellNotInserted".Translate());

                }
                yield return command_Action;
                if (Prefs.DevMode && this.wombProgress != -1)
                {
                    Command_Action command_Action2 = new Command_Action();
                    command_Action2.defaultLabel = "DEBUG: Finish womb work";
                    command_Action2.action = delegate
                    {
                        
                         this.wombProgress = 1; 

                    };
                    yield return command_Action2;

                }
            }

        }

        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());
            if (this.wombProgress != -1)
            {
                sb.AppendLine("\n"+"GR_ArchoWombProgress".Translate(this.wombProgress.ToStringPercent()));

            }
      

            return sb.ToString().Trim();
        }


    }
}