namespace GeneticRim
{
    using System.Text;
    using Verse;

    public class CompGrowthCell : ThingComp
    {
        public ThingDef genomeDominant;
        public ThingDef genomeSecondary;
        public ThingDef genoframe;
        public ThingDef booster;

        public override string CompInspectStringExtra()
        {
            StringBuilder sb = new StringBuilder(base.CompInspectStringExtra());

            sb.AppendLine("GR_GrowthCell_InspectDominant".Translate(this.genomeDominant.LabelCap));
            sb.AppendLine("GR_GrowthCell_InspectSecondary".Translate(this.genomeSecondary.LabelCap));
            sb.AppendLine("GR_GrowthCell_InspectGenoframe".Translate(this.genoframe.LabelCap));
            if(this.booster != null)
                sb.AppendLine("GR_GrowthCell_InspectBooster".Translate(this.booster.LabelCap));

            return sb.ToString().Trim();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Defs.Look(ref this.genomeDominant, nameof(this.genomeDominant));
            Scribe_Defs.Look(ref this.genomeSecondary, nameof(this.genomeSecondary));
            Scribe_Defs.Look(ref this.genoframe, nameof(this.genoframe));
            Scribe_Defs.Look(ref this.booster, nameof(this.booster));
        }
    }
}
