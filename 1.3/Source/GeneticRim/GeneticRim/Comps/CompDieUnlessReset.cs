﻿using RimWorld;
using Verse;
using Verse.Sound;

namespace GeneticRim

{
    public class CompDieUnlessReset : ThingComp
    {
        public int tickCounter = 0;

        public CompProperties_DieUnlessReset Props
        {
            get
            {
                return (CompProperties_DieUnlessReset)this.props;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));


        }

        public override void CompTick()
        {
            base.CompTick();
            tickCounter++;

            if (tickCounter >= Props.timeToDieInTicks)
            {
                Pawn pawn = this.parent as Pawn;

                if (pawn != null && pawn.Map != null)
                {

                    if (Props.effect)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            IntVec3 c;
                            CellFinder.TryFindRandomReachableCellNear(this.parent.Position, this.parent.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                            FilthMaker.TryMakeFilth(c, this.parent.Map, Props.effectFilth);
                        }
                        SoundDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
                    }
                    pawn.Destroy();
                }
                tickCounter = 0;
            }
        }

        public void ResetTimer()
        {
            tickCounter = 0;
        }

        public override string CompInspectStringExtra()
        {


            string text = base.CompInspectStringExtra();
            string timeToLive = Props.message.Translate((Props.timeToDieInTicks - tickCounter).ToStringTicksToPeriod(true, false, true, true));



            return text + timeToLive;
        }

    }
}
