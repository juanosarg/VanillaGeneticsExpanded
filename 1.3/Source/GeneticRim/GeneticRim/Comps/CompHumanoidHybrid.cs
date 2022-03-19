
using Verse;
using RimWorld;

namespace GeneticRim
{
    class CompHumanoidHybrid : ThingComp
    {


        public CompProperties_HumanoidHybrid Props
        {
            get
            {
                return (CompProperties_HumanoidHybrid)this.props;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if(this.parent.Faction == Faction.OfPlayer) {
                StaticCollectionsClass.AddHumanoidHybridToList(this.parent);
            }

        }

        public override void PostDeSpawn(Map map)
        {
            StaticCollectionsClass.RemoveHumanoidHybridFromList(this.parent);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            StaticCollectionsClass.RemoveHumanoidHybridFromList(this.parent);
        }


    }
}
