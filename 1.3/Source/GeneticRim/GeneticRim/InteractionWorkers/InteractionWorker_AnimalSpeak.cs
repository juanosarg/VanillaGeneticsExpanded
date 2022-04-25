
using RimWorld;
using Verse;

namespace GeneticRim
{
    public class InteractionWorker_AnimalSpeak : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (StaticCollectionsClass.IsHumanoidHybrid(initiator) && recipient.RaceProps.Humanlike)
            {
                return 1f;
            }
            else return 0;


        }
    }
}
