using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticRim
{
    using Verse;

    public class DefExtension_HybridChanceAlterer : DefModExtension
    {
        /**
         * Deducted from genome swap chance.
         */
        public int stability;

        /**
         * Deducted from failure outcome chance.
         */
        public int safety;

        /**
         * Hybrid process time multiplier.
         */
        public float timeMultiplier;

        /**
         * Hybrid draftability toggle.
         */
        public bool isController = false;

        /**
         * Hybrid fertility toggle.
         */
        public bool isFertilityUnblocker = false;



        public bool isBooster;
        public bool isGenome;

    }
}
