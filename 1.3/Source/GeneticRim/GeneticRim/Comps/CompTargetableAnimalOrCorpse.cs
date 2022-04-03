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
                canTargetItems = true,
                canTargetBuildings = false,
                mapObjectTargetsMustBeAutoAttackable = false,
                validator = (TargetInfo x) =>  ((x.Thing is Corpse)||(x.Thing is Pawn && x.Thing.Faction == Faction.OfPlayer))
            };
        }

        public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
        {
            yield return targetChosenByPlayer;
        }
    }
}
