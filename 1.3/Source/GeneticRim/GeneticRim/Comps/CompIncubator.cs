﻿using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using System;



namespace GeneticRim



{
    public class CompIncubator : ThingComp
    {
        private float gestateProgress;

        public Pawn hatcheeParent;

        public Pawn otherParent;

        public Faction hatcheeFaction;
        private PawnGenerationRequest request;
        System.Random randFailure = new System.Random();
        System.Random randSpawn = new System.Random();

        public bool WasMutant = false;

        public CompProperties_Incubator Props
        {
            get
            {
                return (CompProperties_Incubator)this.props;
            }
        }

        private CompTemperatureRuinable FreezerComp
        {
            get
            {
                return this.parent.GetComp<CompTemperatureRuinable>();
            }
        }

        protected bool TemperatureDamaged
        {
            get
            {
                CompTemperatureRuinable freezerComp = this.FreezerComp;
                return freezerComp != null && this.FreezerComp.Ruined;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<float>(ref this.gestateProgress, "gestateProgress", 0f, false);
            Scribe_References.Look<Pawn>(ref this.hatcheeParent, "hatcheeParent", false);
            Scribe_References.Look<Pawn>(ref this.otherParent, "otherParent", false);
            Scribe_References.Look<Faction>(ref this.hatcheeFaction, "hatcheeFaction", false);
        }

        public override void CompTick()
        {

            if (!this.TemperatureDamaged)
            {
                float num = 1f / (this.Props.hatcherDaystoHatch * 60000f);
                this.gestateProgress += num;
                if (this.gestateProgress > 1f)
                {
                    this.Hatch();
                    this.gestateProgress = 0;
                }
            }
        }

        public void Hatch()
        {

            if (this.parent.Map!=null)
            {

                FilthMaker.TryMakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_AmnioticFluid, 1);


                for (int i = 0; i < this.parent.stackCount; i++)
                {
                    
                    if (randFailure.NextDouble() < (1 - (GeneticRim_Settings.failureRate / 100)))
                    {
                        if (randSpawn.NextDouble() > 0.5 || Props.hatcherPawnSecondary == null)
                        {
                            request = new PawnGenerationRequest(this.Props.hatcherPawn, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false);
                        }
                        else
                        {
                            request = new PawnGenerationRequest(this.Props.hatcherPawnSecondary, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false);
                        }
                    }
                    else
                    {
                        request = new PawnGenerationRequest(PawnKindDef.Named("GR_AberrantFleshbeast"), null, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false);
                        WasMutant = true;
                    }
                    Pawn pawn = PawnGenerator.GeneratePawn(request);
                    if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, this.parent))
                    {
                        if (pawn != null)
                        {
                            if (pawn.Faction == Faction.OfPlayer)
                            {
                                pawn.health.AddHediff(HediffDef.Named("GR_RecentlyHatched"));
                            }

                            if (this.hatcheeParent != null)
                            {
                                if (pawn.playerSettings != null && this.hatcheeParent.playerSettings != null && this.hatcheeParent.Faction == this.hatcheeFaction)
                                {
                                    pawn.playerSettings.AreaRestriction = this.hatcheeParent.playerSettings.AreaRestriction;
                                }
                                if (pawn.RaceProps.IsFlesh)
                                {
                                    pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.hatcheeParent);
                                }
                            }
                            if (this.otherParent != null && (this.hatcheeParent == null || this.hatcheeParent.gender != this.otherParent.gender) && pawn.RaceProps.IsFlesh)
                            {
                                pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.otherParent);
                            }
                            if (WasMutant)
                            {
                                Messages.Message("GR_ANewCreatureWasBornMutant".Translate(), pawn, MessageTypeDefOf.NegativeEvent);
                            }
                            else
                            {
                                Messages.Message("GR_ANewCreatureWasBorn".Translate(), pawn, MessageTypeDefOf.PositiveEvent);

                            }
                        }
                        if (this.parent.Spawned)
                        {
                            FilthMaker.TryMakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_AmnioticFluid, 1);
                        }
                    }
                    else
                    {
                        Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
                    }
                }
                this.parent.Destroy(DestroyMode.Vanish);
            }
        }


        public override void PreAbsorbStack(Thing otherStack, int count)
        {
            float t = (float)count / (float)(this.parent.stackCount + count);
            CompIncubator comp = ((ThingWithComps)otherStack).GetComp<CompIncubator>();
            float b = comp.gestateProgress;
            this.gestateProgress = Mathf.Lerp(this.gestateProgress, b, t);
        }

        public override void PostSplitOff(Thing piece)
        {
            CompIncubator comp = ((ThingWithComps)piece).GetComp<CompIncubator>();
            comp.gestateProgress = this.gestateProgress;
            comp.hatcheeParent = this.hatcheeParent;
            comp.otherParent = this.otherParent;
            comp.hatcheeFaction = this.hatcheeFaction;
        }

        public override void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
        {
            base.PrePreTraded(action, playerNegotiator, trader);
            if (action == TradeAction.PlayerBuys)
            {
                this.hatcheeFaction = Faction.OfPlayer;
            }
            else if (action == TradeAction.PlayerSells)
            {
                this.hatcheeFaction = trader.Faction;
            }
        }

        public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
        {
            base.PostPostGeneratedForTrader(trader, forTile, forFaction);
            this.hatcheeFaction = forFaction;
        }

        public override string CompInspectStringExtra()
        {
            if (!this.TemperatureDamaged)
            {
                return Translator.Translate("IncubatorProgress") + ": " + this.gestateProgress.ToStringPercent();
            }
            return null;
        }
    }
}
