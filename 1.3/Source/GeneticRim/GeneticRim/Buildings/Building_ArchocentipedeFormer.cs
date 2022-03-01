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


        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref FacilitiesAndProgress, "FacilitiesAndProgress", LookMode.Def, LookMode.Value, ref list2, ref list3);
            Scribe_Values.Look(ref this.totalGenomeAmount, nameof(this.totalGenomeAmount));
            Scribe_Values.Look(ref this.totalGenomeProgress, nameof(this.totalGenomeProgress));


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
                    Messages.Message("working", this, MessageTypeDefOf.NeutralEvent);


                };
                if (gizmoDisabled())
                {
                    command_Action.Disable("GR_ArchoFormerDisabledReason".Translate());

                }
                yield return command_Action;

            }

        }



        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());

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



            return sb.ToString().Trim();
        }




    }
}