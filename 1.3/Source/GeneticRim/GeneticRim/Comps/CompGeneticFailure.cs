
using Verse;
using RimWorld;

namespace GeneticRim
{
    class CompGeneticFailure : ThingComp
    {


        public CompProperties_GeneticFailure Props
        {
            get
            {
                return (CompProperties_GeneticFailure)this.props;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
           
            StaticCollectionsClass.AddFailuresToList(this.parent);
          

        }

        public override void PostDeSpawn(Map map)
        {
            StaticCollectionsClass.RemoveFailuresFromList(this.parent);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            StaticCollectionsClass.RemoveFailuresFromList(this.parent);
        }


    }
}
