namespace GeneticRim
{
    using System;
    using RimWorld;
    using Verse;

    public class CompHybrid : ThingComp
    {
        public QualityCategory quality;

        public float GetStatFactor(StatDef stat)
        {
            if (stat == StatDefOf.MoveSpeed)
            {
                return this.quality switch
                {
                    QualityCategory.Awful => 0.8f,
                    QualityCategory.Poor => 0.9f,
                    QualityCategory.Normal => 1f,
                    QualityCategory.Good => 1.1f,
                    QualityCategory.Excellent => 1.2f,
                    QualityCategory.Masterwork => 1.45f,
                    QualityCategory.Legendary => 1.65f,
                    _ => throw new ArgumentOutOfRangeException()
                };
            } else if (stat == StatDefOf.MarketValue)
            {
                return this.quality switch
                {
                    QualityCategory.Awful => 0.5f,
                    QualityCategory.Poor => 0.75f,
                    QualityCategory.Normal => 1f,
                    QualityCategory.Good => 1.25f,
                    QualityCategory.Excellent => 1.5f,
                    QualityCategory.Masterwork => 2f,
                    QualityCategory.Legendary => 3f,
                    _ => throw new ArgumentOutOfRangeException()
                };
            } else if (stat == StatDefOf.FilthRate)
            {
                return this.quality switch
                {
                    QualityCategory.Awful => 2f,
                    QualityCategory.Poor => 1.5f,
                    QualityCategory.Normal => 1f,
                    QualityCategory.Good => 0.8f,
                    QualityCategory.Excellent => 0.5f,
                    QualityCategory.Masterwork => 0.25f,
                    QualityCategory.Legendary => 0.05f,
                    _ => throw new ArgumentOutOfRangeException()
                };
            } else if (stat == StatDefOf.LeatherAmount)
            {
                return this.quality switch
                {
                    QualityCategory.Awful => 0.8f,
                    QualityCategory.Poor => 0.9f,
                    QualityCategory.Normal => 1f,
                    QualityCategory.Good => 1.1f,
                    QualityCategory.Excellent => 1.2f,
                    QualityCategory.Masterwork => 1.45f,
                    QualityCategory.Legendary => 1.65f,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return 1f;
        }

        public float GetToolPowerFactor() =>
            this.quality switch
            {
                QualityCategory.Awful => 0.8f,
                QualityCategory.Poor => 0.9f,
                QualityCategory.Normal => 1f,
                QualityCategory.Good => 1.1f,
                QualityCategory.Excellent => 1.2f,
                QualityCategory.Masterwork => 1.45f,
                QualityCategory.Legendary => 1.65f,
                _ => throw new ArgumentOutOfRangeException()
            };

        public float GetHungerRateFactor() =>
            this.quality switch
            {
                QualityCategory.Awful => 2f,
                QualityCategory.Poor => 1.5f,
                QualityCategory.Normal => 1f,
                QualityCategory.Good => 0.8f,
                QualityCategory.Excellent => 0.5f,
                QualityCategory.Masterwork => 0.25f,
                QualityCategory.Legendary => 0.05f,
                _ => throw new ArgumentOutOfRangeException()
            };

        public float GetBodySizeFactor() =>
            this.quality switch
            {
                QualityCategory.Awful => 0.85f,
                QualityCategory.Poor => 0.9f,
                QualityCategory.Normal => 1f,
                QualityCategory.Good => 1.05f,
                QualityCategory.Excellent => 1.1f,
                QualityCategory.Masterwork => 1.2f,
                QualityCategory.Legendary => 1.25f,
                _ => throw new ArgumentOutOfRangeException()
            };

        public float GetHealthScaleFactor() =>
            this.quality switch
            {
                QualityCategory.Awful => 0.8f,
                QualityCategory.Poor => 0.9f,
                QualityCategory.Normal => 1f,
                QualityCategory.Good => 1.1f,
                QualityCategory.Excellent => 1.2f,
                QualityCategory.Masterwork => 1.45f,
                QualityCategory.Legendary => 1.65f,
                _ => throw new ArgumentOutOfRangeException()
            };

        public float GetLifeExpectancyFactor() =>
            this.quality switch
            {
                QualityCategory.Awful => 0.5f,
                QualityCategory.Poor => 0.75f,
                QualityCategory.Normal => 1f,
                QualityCategory.Good => 1.25f,
                QualityCategory.Excellent => 1.5f,
                QualityCategory.Masterwork => 2f,
                QualityCategory.Legendary => 3f,
                _ => throw new ArgumentOutOfRangeException()
            };

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref this.quality, "quality");
        }
    }
}