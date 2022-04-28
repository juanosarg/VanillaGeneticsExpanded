
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace GeneticRim
{
    class CompFleshGrowth : ThingComp
    {


        public CompProperties_FleshGrowth Props
        {
            get
            {
                return (CompProperties_FleshGrowth)this.props;
            }
        }



        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            if (this.parent.Map != null)
            {

                CellRect rect = GenAdj.OccupiedRect(parent.Position, parent.Rotation, IntVec2.One);
                rect = rect.ExpandedBy(5);
                foreach (IntVec3 current in rect.Cells)
                {
                    if (current.InBounds(parent.Map))
                    {

                        List<Thing> list = parent.Map.thingGrid.ThingsListAt(current);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].TryGetComp<CompGlower>() != null)
                            {
                                list[i].Kill();
                            }
                        }


                    }

                }

            }
        }

        public override void CompTick()
        {
            base.CompTick();

            if (this.parent.IsHashIntervalTick(7500))
            {

                float num = this.parent.Map.glowGrid.GameGlowAt(this.parent.Position, false);

                if (num < 0.5)
                {
                    CellRect rect = GenAdj.OccupiedRect(parent.Position, parent.Rotation, IntVec2.One);
                    rect = rect.ExpandedBy(3);
                    IntVec3 current = rect.Cells.RandomElement();

                    while (current == this.parent.Position)
                    {
                        current = rect.Cells.RandomElement();
                    }
                    bool buildingFound = false;
                    List<Thing> list = parent.Map.thingGrid.ThingsListAt(current);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if ((list[i].def == InternalDefOf.GR_FleshGrowth_Building)|| list[i].def.IsDoor)
                        {
                            buildingFound = true;
                        }
                    }


                    if (!buildingFound) {
                        Room room = current.GetRoom(this.parent.Map);
                        if (current.InBounds(parent.Map) && room?.OutdoorsForWork == false)
                        {

                            Thing thing = ThingMaker.MakeThing(InternalDefOf.GR_FleshGrowth_Building, null);
                            thing.Rotation = Rot4.North;
                            thing.Position = current;

                            thing.SpawnSetup(parent.Map, false);


                        }
                    }
                    

                }


            }
            if (this.parent.IsHashIntervalTick(30000))
            {

                Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(InternalDefOf.GR_FleshFlies, null, fixedBiologicalAge: 1, fixedChronologicalAge: 1,
                                                                                           newborn: false, forceGenerateNewPawn: true));
                IntVec3 near = CellFinder.StandableCellNear(this.parent.Position, this.parent.Map, 1f);

                if (near.InBounds(parent.Map))
                {
                    GenSpawn.Spawn(pawn, near, this.parent.Map);

                    pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, true, false, null, false);
                    pawn.health.AddHediff(InternalDefOf.GR_GreaterScaria);
                }



            }

        }

        public override string CompInspectStringExtra()
        {

            return "GR_FleshGrowth".Translate();

        }
    }

}

