
using Verse;
using RimWorld;

namespace GeneticRim
{
    class CompHorseHybrid : ThingComp
    {


        public CompProperties_HorseHybrid Props
        {
            get
            {
                return (CompProperties_HorseHybrid)this.props;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
           
            StaticCollectionsClass.AddHorseHybridToList((Pawn)this.parent);

          

        }

       

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            StaticCollectionsClass.RemoveHorseHybridFromList((Pawn)this.parent);
        }


    }
}
