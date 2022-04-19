
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
            if (Props.createSingleThing) {

                if (this.parent.pawn.Map != null)
                {
                    bool thingFound = false;
                    List<Thing> list = parent.pawn.Map.thingGrid.ThingsListAt(parent.pawn.Position);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if ((list[i].def == Props.thingCreated))
                        {
                            thingFound = true;
                        }
                    }
                    if (!thingFound)
                    {
                     
                        Thing newbuilding = GenSpawn.Spawn(Props.thingCreated, parent.pawn.Position, parent.pawn.Map, WipeMode.Vanish);
                        newbuilding.SetFaction(parent.pawn.Faction);
                    }


                    


                }


            }
            else
            {
                CellRect rect = GenAdj.OccupiedRect(parent.pawn.Position, parent.pawn.Rotation, IntVec2.One);
                rect = rect.ExpandedBy(Props.radius);
                int totalCreated = 0;

                foreach (IntVec3 current in rect.Cells)
                {
                    if (current.InBounds(parent.pawn.Map) && Rand.Chance(Props.thingCreatedChance))
                    {

                        if (Props.thingCreated.thingClass == typeof(Filth))
                        {
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
                                if (Props.count == 0 || (Props.count != 0 && totalCreated < Props.count)) { thing.SpawnSetup(parent.pawn.Map, false); }
                                if (Props.count != 0) { totalCreated++; }
                            }

                        }
                        else
                        {
                            Thing thing = ThingMaker.MakeThing(Props.thingCreated, null);
                            thing.Rotation = Rot4.North;
                            thing.Position = current;
                            if (Props.count == 0 || (Props.count != 0 && totalCreated < Props.count)) { thing.SpawnSetup(parent.pawn.Map, false); }
                            if (Props.count != 0) { totalCreated++; }
                        }


                    }

                }
            }


        }




    }
}
