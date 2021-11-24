
using Verse;

namespace GeneticRim
{
    class CompAnimalControlHub : ThingComp
    {
       

        public CompProperties_AnimalControlHub Props
        {
            get
            {
                return (CompProperties_AnimalControlHub)this.props;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {

            DraftingList.AddControlHubBuilt();

        }

        public override void PostDeSpawn(Map map)
        {
            DraftingList.RemoveControlHubBuilt();
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            DraftingList.RemoveControlHubBuilt();
        }

       
    }
}
