using System;
using Verse;
using System.Collections.Generic;

namespace GeneticRim
{
    public class HediffCompProperties_Vomiting : HediffCompProperties
    {

        public int mtbVomitingBlood;
        public int woundDamage;


        public HediffCompProperties_Vomiting()
        {
            this.compClass = typeof(HediffComp_Vomiting);
        }
    }
}
