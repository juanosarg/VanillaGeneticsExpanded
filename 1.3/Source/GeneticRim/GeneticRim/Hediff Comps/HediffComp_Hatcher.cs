using System;
using Verse;
using RimWorld;


namespace GeneticRim
{
    public class HediffComp_Hatcher : HediffComp
    {

        private int HatchingTicker = 0;

        public HediffCompProperties_Hatcher Props
        {
            get
            {
                return (HediffCompProperties_Hatcher)this.props;
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

        public override void CompPostTick(ref float severityAdjustment)
        {
            Hatch();
        }

        public void Hatch()
        {
            if (HatchingTicker < (this.Props.hatcherDaystoHatch * 60000))
            {
                HatchingTicker += 1;
            }
            else
            {
                int amountToSpawn = Props.baseAmount;
                switch (quality)
                {
                    case QualityCategory.Awful:
                        amountToSpawn = (int)(amountToSpawn * 0.5);
                        break;
                    case QualityCategory.Poor:
                        amountToSpawn = (int)(amountToSpawn * 0.75);
                        break;

                    case QualityCategory.Good:
                        amountToSpawn = (int)(amountToSpawn * 1.25);
                        break;
                    case QualityCategory.Excellent:
                        amountToSpawn = (int)(amountToSpawn * 1.5);
                        break;
                    case QualityCategory.Masterwork:
                        amountToSpawn = (int)(amountToSpawn * 1.75);
                        break;
                    case QualityCategory.Legendary:
                        amountToSpawn = (int)(amountToSpawn * 2);
                        break;

                }

                if ((this.parent.pawn.Map != null) && ((this.parent.pawn.Faction == Faction.OfPlayer) || ((this.parent.pawn.IsPrisoner) && (this.parent.pawn.Map.IsPlayerHome))))
                {
                    Thing thing = GenSpawn.Spawn(Props.thingToHatch, this.parent.pawn.Position, this.parent.pawn.Map);
                    thing.stackCount = amountToSpawn;
                }
                HatchingTicker = 0;

            }

        }


    }
}
