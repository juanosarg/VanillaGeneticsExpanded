
using RimWorld;
using Verse;

namespace GeneticRim
{
    public class InteractionWorker_UWUSpeak : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (initiator.def==InternalDefOf.GR_Mancat && recipient.RaceProps.Humanlike)
            {
                return 1f;
            }
            else return 0;


        }
    }
}
