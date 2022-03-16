using System;
using Verse;
using System.Collections.Generic;

namespace GeneticRim
{
    public class HediffCompProperties_PeriodicWounds : HediffCompProperties
    {

        public float severitySecondStage = 0.6f;
        public float severityThirdStage = 0.85f;

        public float chanceCutSecondStage = 0.35f;
        public float chanceCutThirdStage = 0.65f;


        public int mtbWoundsSecondStage = 100;
        public int mtbWoundsThirdStage = 100;

        public HediffCompProperties_PeriodicWounds()
        {
            this.compClass = typeof(HediffComp_PeriodicWounds);
        }
    }
}
