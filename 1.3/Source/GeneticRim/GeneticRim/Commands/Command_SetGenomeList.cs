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
        public Thing drone;



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
                        building.selectedGenome = thing;
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

