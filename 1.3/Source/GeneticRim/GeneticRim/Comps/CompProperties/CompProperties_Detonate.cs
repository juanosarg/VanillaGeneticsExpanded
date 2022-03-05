using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GeneticRim
{


    public class CompProperties_Detonate : CompProperties
    {
        public string gizmoImage;
        public string gizmoLabel;
        public string gizmoDesc;
        public float radius;
        public DamageDef damageType;
        public int damageAmount = -1;
        public float damagePenetration = -1;
        public SoundDef soundCreated = null;
        public ThingDef thingCreated = null;
        public float thingCreatedChance = 0;
        public float chanceToStartFire = 0;


        public bool damageUser = true;
       

        public CompProperties_Detonate()
        {
            this.compClass = typeof(CompDetonate);
        }
    }
}