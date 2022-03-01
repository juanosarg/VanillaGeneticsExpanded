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
    public class Building_DNAStorageBank : Building
    {
        private static readonly Material barFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f));
        private static readonly Material barUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f));

        public ThingDef selectedGenome;
        public float progress;
        public bool hasAsked = false;

        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            if (this.Faction == Faction.OfPlayer)
            {

                if (this.selectedGenome == null) { 
                    yield return GenomeListSetupUtility.SetGenomeListCommand(this, this.Map); 
                }else
                {
                    if (hasAsked || progress==0f) {
                        yield return GenomeListSetupUtility.SetGenomeListCommand(this, this.Map);

                    }
                    else
                    {
                        Command_Action command_Action = new Command_Action();

                        command_Action.defaultDesc = "GR_SelectGenomeToHarvestDesc".Translate();
                        command_Action.defaultLabel = "GR_SelectGenomeToHarvest".Translate();
                        command_Action.icon = ContentFinder<Texture2D>.Get("Things/Item/GR_geneticmaterialswhite", true);
                        command_Action.hotKey = KeyBindingDefOf.Misc1;
                        command_Action.action = delegate
                        {
                            Messages.Message("GR_AreYouSureDNA".Translate(), this, MessageTypeDefOf.NeutralEvent);
                            hasAsked = true;

                        };
                        yield return command_Action;
                    }
                }



                if (this.selectedGenome != null)
                {


                    yield return GenomeListSetupUtility.SetAnimalListCommand(this, this.Map, this.selectedGenome);
                    if (Prefs.DevMode)
                    {
                        Command_Action command_Action = new Command_Action();
                        command_Action.defaultLabel = "DEBUG: Finish DNA progress";
                        command_Action.action = delegate
                        {
                            this.progress = 1;

                        };
                        yield return command_Action;

                    }

                }

                
            }

        }

        public override void Draw()
        {
            base.Draw();

            GenDraw.FillableBarRequest fillableBarRequest = default(GenDraw.FillableBarRequest);

            fillableBarRequest.center = this.DrawPos + Vector3.up;
            fillableBarRequest.size = new Vector2(1.6f, 0.2f);
            fillableBarRequest.fillPercent = this.progress;
            fillableBarRequest.filledMat = barFilledMat;
            fillableBarRequest.unfilledMat = barUnfilledMat;
            fillableBarRequest.margin = 0.15f;
            fillableBarRequest.rotation = this.Rotation.Rotated(RotationDirection.Clockwise);

           
           

            GenDraw.DrawFillableBar(fillableBarRequest);

        }

        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());

            if (selectedGenome != null)
            {
                sb.AppendLine("GR_SelectedGenome".Translate(selectedGenome.LabelCap));
                sb.AppendLine("GR_DNABankProgress".Translate(this.progress.ToStringPercent()));
            }


            return sb.ToString().Trim();
        }

        public override void ExposeData()
        {
            base.ExposeData();


            Scribe_Defs.Look(ref this.selectedGenome, nameof(this.selectedGenome));
            Scribe_Values.Look(ref this.progress, nameof(this.progress));
            Scribe_Values.Look(ref this.hasAsked, nameof(this.hasAsked));


        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            
            NotifyDestructionToFormer();
            base.DeSpawn(mode);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            
            NotifyDestructionToFormer();
            base.Destroy(mode);
        }

        public void NotifyDestructionToFormer()
        {
            foreach (Thing thing in this.GetComp<CompFacility>().LinkedBuildings)
            {
                
                Building_ArchocentipedeFormer building = thing as Building_ArchocentipedeFormer;
                if (building != null)
                {
                    if (building.FacilitiesAndProgress.ContainsKey(this.selectedGenome))
                    {
                        building.FacilitiesAndProgress.Remove(this.selectedGenome);
                    }
                }
            }

        }


    }
}