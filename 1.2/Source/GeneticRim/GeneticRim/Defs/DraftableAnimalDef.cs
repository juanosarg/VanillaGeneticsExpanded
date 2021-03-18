using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace GeneticRim
{
    public class DraftableAnimalDef : Def
    {

        //DraftableAnimalDef is a simple custom def that allows you to input a list of defNames for use
        //in Genetic Rim's custom animal drafting

        //A list of pawnkind defNames
        public List<string> draftablePawnDefs;
    }
}