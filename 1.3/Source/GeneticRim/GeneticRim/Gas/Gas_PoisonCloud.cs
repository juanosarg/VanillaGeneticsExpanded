﻿using RimWorld;
using Verse;
using System;
using System.Collections.Generic;

namespace GeneticRim
{
    public class Gas_PoisonCloud : Gas
    {
        private int tickerInterval = 18;




        public override void Tick()
        {
           /* base.Tick();
            try
            {
                if (tickerInterval >= 18)
                {

                    HashSet<Thing> hashSet = new HashSet<Thing>(this.Position.GetThingList(this.Map));
                    if (hashSet != null)
                    {
                        foreach (Thing current in hashSet)
                        {
                            Pawn pawn = current as Pawn;
                            bool flag = (pawn != null);
                            if (flag)
                            {
                                if (DraftingList.draftable_animals.ContainsKey(pawn))
                                {
                                    if (!DraftingList.draftable_animals[pawn][7])
                                    {
                                        pawn.health.AddHediff(HediffDefOf.ToxicBuildup);
                                        this.Destroy();
                                    }
                                }
                                else {
                                    pawn.health.AddHediff(HediffDefOf.ToxicBuildup);
                                    this.Destroy();
                                }



                            }
                        }

                    }
                    tickerInterval = 0;



                }
                tickerInterval++;
            }
            catch (NullReferenceException e)
            {
                //A weird error is produced sometimes when GetThingList returns a NullReferenceException. I did a try-catch which is inellegant, but it works
            }*/

        }


    }
}
