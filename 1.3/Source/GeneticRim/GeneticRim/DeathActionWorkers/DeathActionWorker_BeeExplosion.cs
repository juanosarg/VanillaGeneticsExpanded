
using RimWorld;
using Verse;


namespace GeneticRim
{
    public class DeathActionWorker_BeeExplosion : DeathActionWorker
    {



        public override void PawnDied(Corpse corpse)
        {
            float radius;
            if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
            {
                radius = 1.9f;
            }
            else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
            {
                radius = 2.9f;
            }
            else
            {
                radius = 3.9f;
            }



            GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, Named("BR_BeeExplosion"), corpse.InnerPawn, 1, -1, SoundDef.Named("Explosion_Bomb"), null, null, null, ThingDef.Named("GR_InsectCloudMotes"), 1f, 1, false, null, 0f, 1);
        }

        public static DamageDef Named(string defName)
        {
            return DefDatabase<DamageDef>.GetNamed(defName, true);
        }


    }
}