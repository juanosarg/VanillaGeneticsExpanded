using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse;
using System.Linq;


namespace GeneticRim
{
    [StaticConstructorOnStartup]
    public class Command_SetGenomeList : Command
    {

        public Map map;
        public Building_DNAStorageBank building;



        public Command_SetGenomeList()
        {
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            HashSet<ThingDef> listOfAllEndgameGenomes = new HashSet<ThingDef>();
            HashSet <EndgameGenomesDef> allLists = DefDatabase<EndgameGenomesDef>.AllDefsListForReading.ToHashSet();
            foreach (EndgameGenomesDef element in allLists)
            {
                listOfAllEndgameGenomes.AddRange(element.genomes);

            }

           


            foreach (ThingDef thing in listOfAllEndgameGenomes)
            {
                
                    list.Add(new FloatMenuOption(thing.LabelCap, delegate
                    {
                        bool duplicatedFlag = false;
                        foreach (Thing item in building.Map.listerBuldingOfDefInProximity.GetForCell(building.Position, 30, InternalDefOf.GR_DNAStorageBank))
                        {
                            Building_DNAStorageBank otherBuilding = item as Building_DNAStorageBank;

                            if(otherBuilding.selectedGenome== thing)
                            {
                                duplicatedFlag = true;
                                break;
                            }

                        }

                        if (duplicatedFlag)
                        {
                            Messages.Message("GR_DuplicatedGenomeSelection".Translate(), building, MessageTypeDefOf.NeutralEvent);
                        }
                        else {
                            if (building.selectedGenome != thing)
                            {
                                building.progress = 0f;
                            }
                            building.selectedGenome = thing;
                            building.hasAsked = false;
                        }
                        
                        
                        
                    }, MenuOptionPriority.Default, null, null, 29f, null, null));
                

            }




            if (list.Count > 0)
            {
                list = list.OrderBy(item => item.Label).ToList();

            }
            else
            {
                list.Add(new FloatMenuOption("GR_NoEndgameGenomes".Translate(), delegate
                {

                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

       




    }


}

