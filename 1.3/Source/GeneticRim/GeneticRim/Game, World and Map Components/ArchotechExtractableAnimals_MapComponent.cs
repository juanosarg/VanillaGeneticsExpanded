using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using RimWorld.Planet;

namespace GeneticRim
{
    public class ArchotechExtractableAnimals_MapComponent : MapComponent
    {

       

        public Dictionary<Pawn, Thing> animalsToCarry = new Dictionary<Pawn, Thing>();
        List<Pawn> list2;
        List<Thing> list3;

        public Dictionary<Pawn, Thing> paragonsToCarry = new Dictionary<Pawn, Thing>();
        List<Pawn> list4;
        List<Thing> list5;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref animalsToCarry, "animalsToCarry", LookMode.Reference, LookMode.Reference, ref list2, ref list3);
            Scribe_Collections.Look(ref paragonsToCarry, "paragonsToCarry", LookMode.Reference, LookMode.Reference, ref list4, ref list5);

        }


        public ArchotechExtractableAnimals_MapComponent(Map map) : base(map)
        {

        }

        public override void FinalizeInit()
        {
            foreach (KeyValuePair<Pawn,Thing> entry in animalsToCarry)
            {
                if (entry.Key.Dead)
                {
                    RemoveAnimalToCarry(entry.Key);
                }

            }

            foreach (KeyValuePair<Pawn, Thing> entry in paragonsToCarry)
            {
                if (entry.Key.Dead)
                {
                    RemoveParagonToCarry(entry.Key);
                }

            }


            base.FinalizeInit();

        }

        public void AddAnimalToCarry(Pawn pawn, Thing building)
        {
            if (!animalsToCarry.ContainsKey(pawn))
            {
                animalsToCarry.Add(pawn, building);
            }

        }
        public void RemoveAnimalToCarry(Pawn pawn)
        {
            if (animalsToCarry.ContainsKey(pawn))
            {
                animalsToCarry.Remove(pawn);
            }

        }

        public void AddParagonToCarry(Pawn pawn, Thing building)
        {
            if (!paragonsToCarry.ContainsKey(pawn))
            {
                paragonsToCarry.Add(pawn, building);
            }

        }
        public void RemoveParagonToCarry(Pawn pawn)
        {
            if (paragonsToCarry.ContainsKey(pawn))
            {
                paragonsToCarry.Remove(pawn);
            }

        }




    }


}
