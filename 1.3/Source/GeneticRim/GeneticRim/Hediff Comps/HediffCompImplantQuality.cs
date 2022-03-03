using System;
using Verse;
using RimWorld;


namespace GeneticRim
{
    public class HediffCompImplantQuality : HediffComp
    {

      

        public HediffCompProperties_ImplantQuality Props
        {
            get
            {
                return (HediffCompProperties_ImplantQuality)this.props;
            }
        }

        public QualityCategory quality = QualityCategory.Normal;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref quality, "quality", QualityCategory.Normal);
        }

        public void SelectSeverity(QualityCategory quality)
        {
            
            switch (quality)
            {
                case QualityCategory.Awful:
                    this.parent.Severity = 0f;
                    break;
                case QualityCategory.Poor:
                    this.parent.Severity = 0.1f;
                    break;
                case QualityCategory.Normal:
                    this.parent.Severity = 0.2f;
                    break;
                case QualityCategory.Good:
                    this.parent.Severity = 0.3f;
                    break;
                case QualityCategory.Excellent:
                    this.parent.Severity = 0.4f;
                    break;
                case QualityCategory.Masterwork:
                    this.parent.Severity = 0.5f;
                    break;
                case QualityCategory.Legendary:
                    this.parent.Severity = 0.6f;
                    break;

            }

        }



        public override string CompLabelInBracketsExtra => quality.GetLabel();

    }
}
