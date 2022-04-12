using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse;
using System.Linq;


namespace GeneticRim
{
    [StaticConstructorOnStartup]
    public class Command_SetParagonList : Command
    {

        public Map map;
        public Building_Mechahybridizer building;
       
        List<Pawn> listOfPawns;



        public Command_SetParagonList()
        {
            
            
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            List<Thing> listFacilities = building.TryGetComp<CompAffectedByFacilities>()?.LinkedFacilitiesListForReading;
            List<Building_Mechafuse> listFuses = new List<Building_Mechafuse>();
            foreach (Thing facility in listFacilities)
            {
                Building_Mechafuse fuse = facility as Building_Mechafuse;
                listFuses.Add(fuse);
            }
            Building_Mechafuse unSpentFuse;
            listFuses.Where(x => (x.active == true)).TryRandomElement(out unSpentFuse);
            if (unSpentFuse == null)
            {
                Messages.Message("GR_WarningNoFusesLeft".Translate(), building, MessageTypeDefOf.NeutralEvent);
            }



            listOfPawns = Find.CurrentMap.mapPawns.SpawnedColonyAnimals.Where(x => (x.kindDef.HasModExtension<DefExtension_Paragon>())).ToList();
           
            foreach (Pawn pawn in listOfPawns)
            {
                
                    list.Add(new FloatMenuOption(pawn.LabelCap, delegate
                    {
                        pawn.Map.GetComponent<ArchotechExtractableAnimals_MapComponent>().AddParagonToCarry(pawn,building);
                    }, MenuOptionPriority.Default, null, null, 29f, null, null));
                

            }




            if (list.Count > 0)
            {
                list = list.OrderBy(item => item.Label).ToList();

            }
            else
            {
                list.Add(new FloatMenuOption("GR_NoValidParagonsToConvert".Translate(), delegate
                {

                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

       




    }


}

