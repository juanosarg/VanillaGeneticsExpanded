
using Verse;
using RimWorld;

namespace GeneticRim
{
    class CompRegisterMechHybridWithAntenna : ThingComp
    {
       
        public CompProperties_RegisterMechHybridWithAntenna Props
        {
            get
            {
                return (CompProperties_RegisterMechHybridWithAntenna)this.props;
            }
        }

        public override void CompTick()
        {
            base.CompTick();

            if (this.parent.IsHashIntervalTick(Props.timer))
            {
                CheckForAntenna();
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            CheckForAntenna();

        }

        public void CheckForAntenna()
        {
            Pawn pawn = this.parent as Pawn;

            if (pawn.Faction == Faction.OfPlayer)
            {
                bool alreadyRegistered = false;
                foreach (Building_MechahybridAntenna antenna in StaticCollectionsClass.mech_antennas)
                {
                    if (antenna.assignedMechs.Contains((Pawn)pawn))
                    {
                        alreadyRegistered = true;
                    }
                }

                if (!alreadyRegistered)
                {
                    foreach (Building_MechahybridAntenna antenna in StaticCollectionsClass.mech_antennas)
                    {
                        if (antenna.AddMechToList((Pawn)pawn))
                        {
                            break;
                        }
                    }
                }

            }

        }

        public override void PostDeSpawn(Map map)
        {
          
            foreach (Building_MechahybridAntenna antenna in StaticCollectionsClass.mech_antennas)
            {
               antenna.RemoveMechFromList((Pawn)this.parent);
               
            }

            
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            foreach (Building_MechahybridAntenna antenna in StaticCollectionsClass.mech_antennas)
            {
                antenna.RemoveMechFromList((Pawn)this.parent);

            }
        }


    }
}
