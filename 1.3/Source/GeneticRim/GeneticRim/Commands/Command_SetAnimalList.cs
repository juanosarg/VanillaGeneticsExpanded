using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse;
using System.Linq;


namespace GeneticRim
{
    [StaticConstructorOnStartup]
    public class Command_SetAnimalList : Command
    {

        public Map map;
        public Building_DNAStorageBank building;
        public ThingDef selectedGenome;
        List<Pawn> listOfPawns;



        public Command_SetAnimalList()
        {
            
            
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();
           
            listOfPawns = Find.CurrentMap.mapPawns.SpawnedColonyAnimals.Where(x => (x.kindDef.GetModExtension<DefExtension_Hybrid>()?.dominantGenome == selectedGenome)
            || (x.kindDef.GetModExtension<DefExtension_Hybrid>()?.secondaryGenome == selectedGenome)).ToList();
           
            foreach (Pawn pawn in listOfPawns)
            {
                
                    list.Add(new FloatMenuOption(pawn.LabelCap, delegate
                    {
                        pawn.Map.GetComponent<ArchotechExtractableAnimals_MapComponent>().AddAnimalToCarry(pawn,building);
                    }, MenuOptionPriority.Default, null, null, 29f, null, null));
                

            }




            if (list.Count > 0)
            {
                list = list.OrderBy(item => item.Label).ToList();

            }
            else
            {
                list.Add(new FloatMenuOption("GR_NoValidAnimalsToExtract".Translate(), delegate
                {

                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

       




    }


}

