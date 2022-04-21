
using System.Collections.Generic;
using RimWorld;
using Verse;
using RimWorld.Planet;

namespace GeneticRim
{

    public class CompExperienceFromCaravan : ThingComp
    {

        public CompProperties_ExperienceFromCaravan Props => (CompProperties_ExperienceFromCaravan)props;


        public int interval = 2500;

        public float xp = 1;

        public override void PostExposeData()
        {
            base.PostExposeData();

          
            Scribe_Values.Look<float>(ref xp, "xp", 1, true);

        }

        public override void CompTick()
        {
            if (parent.IsHashIntervalTick(interval))
            {


                Pawn pawn = parent as Pawn;

                if (CaravanUtility.IsCaravanMember(pawn))
                {
                    StaticCollectionsClass.AddManffaloAndExperience((Pawn)parent);

                    if (xp <= 2)
                    {
                        xp += 0.01f;
                        StaticCollectionsClass.SetManffaloExperience((Pawn)parent,xp);
                    }


                }


            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {

            StaticCollectionsClass.RemoveManffaloAndExperience((Pawn)parent);
        }









    }
}