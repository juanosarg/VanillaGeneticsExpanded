
using Verse;
using RimWorld;


namespace GeneticRim
{
    public class HediffComp_Exploder : HediffComp
    {


        public HediffCompProperties_Exploder Props
        {
            get
            {
                return (HediffCompProperties_Exploder)this.props;
            }
        }

        public QualityCategory quality = QualityCategory.Normal;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref quality, "quality", QualityCategory.Normal);
        }

        public override void CompPostMake()
        {
            base.CompPostMake();

            this.quality = this.parent.TryGetComp<HediffCompImplantQuality>()?.quality ?? QualityCategory.Awful;
        }

        public override void Notify_PawnDied()
        {
            float explosionForce = Props.explosionForce;
            switch (quality)
            {
                case QualityCategory.Awful:
                    explosionForce = (int)(explosionForce * 0.5);
                    break;
                case QualityCategory.Poor:
                    explosionForce = (int)(explosionForce * 0.75);
                    break;

                case QualityCategory.Good:
                    explosionForce = (int)(explosionForce * 1.25);
                    break;
                case QualityCategory.Excellent:
                    explosionForce = (int)(explosionForce * 1.5);
                    break;
                case QualityCategory.Masterwork:
                    explosionForce = (int)(explosionForce * 1.75);
                    break;
                case QualityCategory.Legendary:
                    explosionForce = (int)(explosionForce * 2);
                    break;

            }
            GenExplosion.DoExplosion(this.parent.pawn.Corpse.Position, this.parent.pawn.Corpse.Map, explosionForce, DamageDefOf.Flame, this.parent.pawn.Corpse, -1,-1,null, null, null, null,null, 0f, 1, false, null, 0f, 1);

            
        }




    }
}
