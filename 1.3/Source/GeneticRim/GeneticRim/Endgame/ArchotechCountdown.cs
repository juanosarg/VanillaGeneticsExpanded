using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace GeneticRim
{
    public static class ArchotechCountdown
    {
        private static float timeLeft = -1000f;

        private static Building_ArchoWomb womb;

        private const float InitialTime = 7.2f;

        public static bool CountingDown
        {
            get
            {
                return ArchotechCountdown.timeLeft >= 0f;
            }
        }

        public static void InitiateCountdown(Building_ArchoWomb womb)
        {
            SoundDefOf.ShipTakeoff.PlayOneShotOnCamera(null);
            ArchotechCountdown.womb = womb;
            ArchotechCountdown.timeLeft = 7.2f;
            ScreenFader.StartFade(Color.white, 7.2f);
        }

        public static void ShipCountdownUpdate()
        {
            if (ArchotechCountdown.timeLeft > 0f)
            {
                ArchotechCountdown.timeLeft -= Time.deltaTime;
                if (ArchotechCountdown.timeLeft <= 0f)
                {
                    ArchotechCountdown.CountdownEnded();
                }
            }
        }

        public static void CancelCountdown()
        {
            ArchotechCountdown.timeLeft = -1000f;
        }

        private static void CountdownEnded()
        {
           
          

            StringBuilder stringBuilder = new StringBuilder();
            Pawn theFutureTamer = null;
            foreach (Pawn current in PawnsFinder.AllMaps_FreeColonistsSpawned)
            {
                stringBuilder.AppendLine("   " + current.LabelCap);
                theFutureTamer = current;
            }
            
          

           PawnGenerationRequest request = new PawnGenerationRequest(InternalDefOf.GR_ArchotechCentipede, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false);
           Pawn pawn = PawnGenerator.GeneratePawn(request);
           pawn.training.Train(TrainableDefOf.Obedience, theFutureTamer, true);
           pawn.training.Train(TrainableDefOf.Release, theFutureTamer, true);
           PawnUtility.TrySpawnHatchedOrBornPawn(pawn, womb);

           
            string victoryText = "GR_GameOverArchotech".Translate(stringBuilder.ToString());
            GameVictoryUtility.ShowCredits(victoryText);
            womb.StopHibernatingWomb();
        }
    }
}

