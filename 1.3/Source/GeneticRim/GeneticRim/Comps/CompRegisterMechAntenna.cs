
using Verse;
using RimWorld;

namespace GeneticRim
{
    class CompRegisterMechAntenna : ThingComp
    {


        public CompProperties_RegisterMechAntenna Props
        {
            get
            {
                return (CompProperties_RegisterMechAntenna)this.props;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
           
                StaticCollectionsClass.AddMechAntennaToList((Building_MechahybridAntenna)this.parent);
           

        }

        public override void PostDeSpawn(Map map)
        {
            StaticCollectionsClass.RemoveMechAntennaFromList((Building_MechahybridAntenna)this.parent);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            StaticCollectionsClass.RemoveMechAntennaFromList((Building_MechahybridAntenna)this.parent);
        }


    }
}
