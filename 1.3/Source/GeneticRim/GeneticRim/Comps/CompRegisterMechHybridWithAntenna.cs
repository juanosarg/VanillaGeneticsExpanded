
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
                    //Log.Message("checking antenna "+antenna.LabelCap);
                    if (antenna.assignedMechs.Contains((Pawn)pawn))
                    {
                        //Log.Message(antenna.LabelCap+" contains this mech");
                        alreadyRegistered = true;
                    }
                }

                if (!alreadyRegistered)
                {
                    foreach (Building_MechahybridAntenna antenna in StaticCollectionsClass.mech_antennas)
                    {
                        //Log.Message("Trying to add mech to "+antenna.LabelCap);
                        if (antenna.AddMechToList((Pawn)pawn))
                        {
                            //Log.Message("Mech added");
                            break;
                        }
                    }
                }

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
