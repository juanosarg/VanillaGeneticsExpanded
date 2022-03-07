
using Verse;
using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;




namespace GeneticRim
{
    class CompCreateThingDefAround : CompAbilityEffect
    {

        new public CompProperties_CreateThingDefAround Props
        {
            get
            {
                return (CompProperties_CreateThingDefAround)this.props;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            CreateAround();
        }

        public void CreateAround()
        {
            CellRect rect = GenAdj.OccupiedRect(parent.pawn.Position, parent.pawn.Rotation, IntVec2.One);
            rect = rect.ExpandedBy(Props.radius);

            foreach (IntVec3 current in rect.Cells)
            {
                if (current.InBounds(parent.pawn.Map) && Rand.Chance(Props.thingCreatedChance))
                {

                    if(Props.thingCreated.thingClass == typeof(Filth)) {
                        int filthNumber = 0;
                        List<Thing> list = parent.pawn.Map.thingGrid.ThingsListAt(current);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if ((list[i] is Filth) && list[i].def == Props.thingCreated)
                            {
                                filthNumber++;
                            }
                        }
                        if (filthNumber < 3)
                        {
                            Thing thing = ThingMaker.MakeThing(Props.thingCreated, null);
                            thing.Rotation = Rot4.North;
                            thing.Position = current;
                            thing.SpawnSetup(parent.pawn.Map, false);
                        }

                    }
                    else
                    {
                        Thing thing = ThingMaker.MakeThing(Props.thingCreated, null);
                        thing.Rotation = Rot4.North;
                        thing.Position = current;
                        thing.SpawnSetup(parent.pawn.Map, false);
                    }                    


                }

            }


        }




    }
}
