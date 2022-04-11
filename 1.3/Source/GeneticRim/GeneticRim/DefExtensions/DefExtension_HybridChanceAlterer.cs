using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GeneticRim
{
    

    public class DefExtension_HybridChanceAlterer : DefModExtension
    {
        // Deducted from genome swap chance.       
        public float stability;

        // Deducted from failure outcome chance.
        public float safety;

        // Hybrid process time multiplier.
        public float timeMultiplier;

        // Hybrid draftability toggle.
        public bool isController = false;

        // Hybrid fertility toggle.
        public bool isFertilityUnblocker = false;

        // Add initial hediffs to hybrid.
        public List<HediffToBodyparts> addedHediffs;



        public bool isBooster;
        public bool isGenome;

    }
}
