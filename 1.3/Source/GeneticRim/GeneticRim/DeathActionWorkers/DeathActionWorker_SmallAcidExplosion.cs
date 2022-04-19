
using RimWorld;
using Verse;

namespace GeneticRim
{
    public class DeathActionWorker_SmallAcidExplosion : DeathActionWorker
    {



        public override void PawnDied(Corpse corpse)
        {
           



            GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, InternalDefOf.GR_Acid, corpse.InnerPawn, 10, -1, InternalDefOf.GR_Pop, null, null, null, ThingDef.Named("Filth_Fuel"), 0.3f, 1, false, null, 0f, 1);
        }


    }
}
