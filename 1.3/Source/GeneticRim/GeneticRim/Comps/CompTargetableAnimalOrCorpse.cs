using System.Collections.Generic;
using RimWorld;
using Verse;
using System.Linq;

namespace GeneticRim
{
    public class CompTargetableAnimalOrCorpse : CompTargetable
    {

        public new CompProperties_TargetableAnimalOrCorpse Props
        {
            get
            {
                return (CompProperties_TargetableAnimalOrCorpse)this.props;
            }
        }

        protected override bool PlayerChoosesTarget => true;

     
        protected override TargetingParameters GetTargetingParameters()
        {

            

            return new TargetingParameters
            {
                canTargetPawns = true,
                canTargetBuildings = false,
                canTargetItems = true,
                validator = (TargetInfo x) => BaseTargetValidator(x.Thing) && ((x.Thing is Corpse)||(x.Thing is Pawn))
            };
        }

        public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
        {
            yield return targetChosenByPlayer;
        }
    }
}
