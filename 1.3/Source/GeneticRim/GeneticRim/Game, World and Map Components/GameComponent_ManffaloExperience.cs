using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace GeneticRim
{
    public class GameComponent_ManffaloExperience : GameComponent
    {



        public int tickCounter = 0;
        public int tickInterval = 10000;
        public Dictionary<Pawn, float> manffalo_and_experience_backup = new Dictionary<Pawn, float>();
        List<Pawn> list2;
        List<float> list3;


        public GameComponent_ManffaloExperience(Game game) : base()
        {

        }

        public override void FinalizeInit()
        {
            StaticCollectionsClass.manffalo_and_experience = manffalo_and_experience_backup;

            base.FinalizeInit();

        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref manffalo_and_experience_backup, "manffalo_and_experience_backup", LookMode.Reference, LookMode.Value, ref list2, ref list3);

            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterManffaloXP", 0, true);

        }

        public override void GameComponentTick()
        {


            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                manffalo_and_experience_backup = StaticCollectionsClass.manffalo_and_experience;


                tickCounter = 0;
            }



        }


    }


}
