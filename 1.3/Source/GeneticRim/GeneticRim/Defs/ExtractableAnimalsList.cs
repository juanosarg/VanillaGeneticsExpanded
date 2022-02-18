using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace GeneticRim
{
    public class ExtractableAnimalsList : Def
    {
        //A list of pawnkind defNames, tier rank for the excavator and the item they should produce

        public List<ThingDef> extractableAnimals;
        public bool needsHumanLike = false;
        public string tier;
        public ThingDef itemProduced;
    }
}