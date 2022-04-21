
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
namespace GeneticRim
{
    public class JobDriver_Talk : JobDriver
    {
        private const int TalkDuration = 2000;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnNotCasualInterruptible(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
            Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).socialMode = RandomSocialMode.Off;
            Toils_General.WaitWith(TargetIndex.A, TalkDuration, useProgressBar: false, maintainPosture: true).socialMode = RandomSocialMode.Off;
            yield return Toils_General.Do(delegate
            {
                Pawn recipient = (Pawn)pawn.CurJob.targetA.Thing;

                if (pawn.def == InternalDefOf.GR_Mancat)
                {
                    pawn.interactions.TryInteractWith(recipient, InternalDefOf.GR_UwUTalkingToHumans);
                    
                    if (Rand.Chance(0.1f))
                    {
                        if (recipient.needs?.mood?.thoughts?.memories?.GetFirstMemoryOfDef(InternalDefOf.GR_Uwuchat) == null)
                        {
                            recipient.needs?.mood?.thoughts?.memories?.TryGainMemory(InternalDefOf.GR_DisgustingUwuchat, pawn);
                        }
                    }
                    else
                    {
                        if (recipient.needs?.mood?.thoughts?.memories?.GetFirstMemoryOfDef(InternalDefOf.GR_DisgustingUwuchat) == null)
                        {
                            recipient.needs?.mood?.thoughts?.memories?.TryGainMemory(InternalDefOf.GR_Uwuchat);
                        }
                        if (!recipient.Dead && recipient.needs.joy != null)
                        {
                            JoyKindDef joyKind = JoyKindDefOf.Social;
                            recipient.needs.joy.GainJoy(0.15f, joyKind);
                        }

                    }

                }
                else
                {
                    pawn.interactions.TryInteractWith(recipient, InternalDefOf.GR_TalkingToHumans);
                    if (!recipient.Dead && recipient.needs.joy != null)
                    {
                        JoyKindDef joyKind = JoyKindDefOf.Social;
                        recipient.needs.joy.GainJoy(0.1f, joyKind);
                    }

                }


            });
        }
    }
}
