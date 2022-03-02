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


    public class Building_ArchocentipedeFormer : Building
    {

        public HashSet<ThingDef> listOfAllEndgameGenomes;
        public Dictionary<ThingDef, float> FacilitiesAndProgress = new Dictionary<ThingDef, float>();
        List<ThingDef> list2;
        List<float> list3;
        public int totalGenomeAmount =0;
        public float totalGenomeProgress =0;
        public float growthCellProgress = -1;
        public const int growthCellDuration = 24; //in hours


        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref FacilitiesAndProgress, "FacilitiesAndProgress", LookMode.Def, LookMode.Value, ref list2, ref list3);
            Scribe_Values.Look(ref this.totalGenomeAmount, nameof(this.totalGenomeAmount));
            Scribe_Values.Look(ref this.totalGenomeProgress, nameof(this.totalGenomeProgress));
            Scribe_Values.Look(ref this.growthCellProgress, nameof(this.growthCellProgress));



        }

        public Building_ArchocentipedeFormer()
        {

            listOfAllEndgameGenomes = new HashSet<ThingDef>();
            HashSet<EndgameGenomesDef> allLists = DefDatabase<EndgameGenomesDef>.AllDefsListForReading.ToHashSet();
            foreach (EndgameGenomesDef element in allLists)
            {
                listOfAllEndgameGenomes.AddRange(element.genomes);

            }
            totalGenomeAmount = listOfAllEndgameGenomes.Count;

        }

        public bool gizmoDisabled()
        {

            

            if (totalGenomeProgress < totalGenomeAmount)
            {
                return true;
            }
            else return false;
            


        }

        public override void Tick()
        {
            base.Tick();

            if (this.IsHashIntervalTick(500))
            {
                List<Thing> listBanks = this.TryGetComp<CompAffectedByFacilities>()?.LinkedFacilitiesListForReading;
                foreach (Thing thing in listBanks)
                {
                    Building_DNAStorageBank bank = thing as Building_DNAStorageBank;
                    if (bank.selectedGenome != null)
                    {
                        if (!FacilitiesAndProgress.ContainsKey(bank.selectedGenome))
                        {
                            FacilitiesAndProgress.Add(bank.selectedGenome, bank.progress);
                        }
                        else
                        {
                            FacilitiesAndProgress[bank.selectedGenome] = bank.progress;
                        }
                    }

                }

                totalGenomeProgress = 0;
                foreach (KeyValuePair<ThingDef, float> entry in FacilitiesAndProgress)
                {
                   
                    totalGenomeProgress += entry.Value;
                }

            }

            if (growthCellProgress!=-1) {

                this.growthCellProgress += 1f / (GenDate.TicksPerHour * growthCellDuration);
                if (this.growthCellProgress > 1)
                {

                    GenSpawn.Spawn(InternalDefOf.GR_ArchoGrowthcell, this.InteractionCell, this.Map);
                    growthCellProgress = -1;
                }

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

                command_Action.defaultDesc = "GR_BeginArchotechGrowthCellDesc".Translate();
                command_Action.defaultLabel = "GR_BeginArchotechGrowthCell".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("Things/Item/GR_ArchoGrowthcell", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    growthCellProgress = 0;
                    List<Thing> listBanks = this.TryGetComp<CompAffectedByFacilities>()?.LinkedFacilitiesListForReading;
                    foreach (Thing thing in listBanks)
                    {
                        Building_DNAStorageBank bank = thing as Building_DNAStorageBank;
                        bank.progress = 0;

                    }

                };
                if (gizmoDisabled())
                {
                    command_Action.Disable("GR_ArchoFormerDisabledReason".Translate());

                }
                yield return command_Action;
                if (Prefs.DevMode && this.growthCellProgress != -1)
                {
                    Command_Action command_Action2 = new Command_Action();
                    command_Action2.defaultLabel = "DEBUG: Finish former work";
                    command_Action2.action = delegate
                    {

                        this.growthCellProgress = 1;

                    };
                    yield return command_Action;

                }
            }

        }



        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());
            if (this.growthCellProgress != -1)
            {
                sb.AppendLine("GR_ArchoFormerCellProgress".Translate(this.growthCellProgress.ToStringPercent()));
               
            }
            else {
                foreach (ThingDef thing in listOfAllEndgameGenomes)
                {
                    if (FacilitiesAndProgress.ContainsKey(thing))
                    {
                        sb.AppendLine("GR_ArchoFormerGenomeProgress".Translate(thing.LabelCap, FacilitiesAndProgress[thing].ToStringPercent()));
                    }
                    else
                    {
                        sb.AppendLine("GR_ArchoFormerGenomeProgress".Translate(thing.LabelCap, "0%"));
                    }

                }
            }
            



            return sb.ToString().Trim();
        }




    }
}