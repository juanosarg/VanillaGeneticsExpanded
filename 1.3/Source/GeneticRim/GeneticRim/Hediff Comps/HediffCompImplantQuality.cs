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

       

        public override string CompLabelInBracketsExtra => quality.GetLabel();

    }
}
