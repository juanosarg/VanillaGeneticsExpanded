
using Verse;
using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;




namespace GeneticRim
{
    class CompDetonate : ThingComp, AnimalBehaviours.PawnGizmoProvider
    {


       

        public CompProperties_Detonate Props
        {
            get
            {
                return (CompProperties_Detonate)this.props;
            }
        }

        public IEnumerable<Gizmo> GetGizmos()
        {
            if (this.parent.Faction == Faction.OfPlayer) {
                yield return new Command_Action
                {
                    defaultLabel = Props.gizmoLabel.Translate(),
                    defaultDesc = Props.gizmoDesc.Translate(),
                    icon = ContentFinder<Texture2D>.Get(Props.gizmoImage, true),
                    action = delegate
                    {
                        Detonate();
                    }
                };
            }
            
                
            
        }

        public void Detonate()
        {
            List<Thing> ignoredThings = new List<Thing>();
            if (Props.damageUser) {
                ignoredThings.Add(this.parent);
            }
                
            GenExplosion.DoExplosion(parent.Position, parent.Map, Props.radius, Props.damageType, parent, Props.damageAmount, Props.damagePenetration, Props.soundCreated, null, null, null, 
                Props.thingCreated, Props.thingCreatedChance, 1, false, null, 0f, 1,Props.chanceToStartFire,false, null, ignoredThings);

            

        }




    }
}
