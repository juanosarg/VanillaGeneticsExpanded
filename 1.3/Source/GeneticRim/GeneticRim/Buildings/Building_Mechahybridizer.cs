using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;
using RimWorld;
using System.Linq;
using System.Text;


namespace GeneticRim
{


    public class Building_Mechahybridizer : Building, IThingHolder
    {

        public ThingOwner innerContainer = null;
        protected bool contentsKnown;
        public float duration = 30000; // 2500 ticks per hour * 12 hours
        public float progress = -1;
        public bool oneRaidPerProgress = false;
        public int mechRaidProgress = 0;
        public const int mechRaidInterval = 15000;
        Graphic usedGraphic;
        Building_Mechafuse unSpentFuse;


        public Building_Mechahybridizer()
        {
            //Constructor initializes the container 
            this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
           

        }

        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            yield return GenomeListSetupUtility.SetParagonListCommand(this, this.Map);
            if (Prefs.DevMode && this.progress != -1)
            {
                Command_Action command_Action2 = new Command_Action();
                command_Action2.defaultLabel = "DEBUG: Finish work";
                command_Action2.action = delegate
                {

                    this.progress = 1;

                };
                yield return command_Action2;

            }

        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            usedGraphic = GraphicsCache.workingMechahybridizer;

        }

        public override void Draw()
        {
            base.Draw();
            if (progress != -1)
            {
                GenDraw.FillableBarRequest fillableBarRequest = default(GenDraw.FillableBarRequest);
                fillableBarRequest.center = this.DrawPos + Vector3.forward * 0.8f + Vector3.right * 0.025f;
                fillableBarRequest.size = new Vector2(2.3f, 0.2f);
                fillableBarRequest.fillPercent = this.progress;
                fillableBarRequest.filledMat = GraphicsCache.barFilledMat;
                fillableBarRequest.unfilledMat = GraphicsCache.barUnfilledMat;
                fillableBarRequest.margin = 0.15f;
                fillableBarRequest.rotation = this.Rotation.Rotated(RotationDirection.Clockwise);
                GenDraw.DrawFillableBar(fillableBarRequest);
            }


        }

        public override Graphic Graphic
        {
            get
            {
                if (progress != -1)
                {
                    return usedGraphic;
                }
                else return this.DefaultGraphic;
            }
        }


        public void Setup()
        {
            unSpentFuse = null;
            List<Thing> listFacilities = this.TryGetComp<CompAffectedByFacilities>()?.LinkedFacilitiesListForReading;
            List<Building_Mechafuse> listFuses = new List<Building_Mechafuse>();
            foreach (Thing facility in listFacilities)
            {
                Building_Mechafuse fuse = facility as Building_Mechafuse;
                listFuses.Add(fuse);
            }
            listFuses.Where(x => (x.active == true)).TryRandomElement(out unSpentFuse);
            progress = 0;
            this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
            oneRaidPerProgress = false;
        }


        public override void Tick()
        {
            base.Tick();

            if (progress >= 0)
            {

                progress += 1f / this.duration;

                if(unSpentFuse == null && !oneRaidPerProgress)
                {
                    mechRaidProgress++;

                    if (mechRaidProgress > mechRaidInterval) {
                        if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.Mechanoid) != null)
                        {
                            IncidentParms parms = new IncidentParms();
                            parms.target = this.Map;
                            parms.faction = Faction.OfMechanoids;
                            parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target) * 2.5f;
                            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
                            parms.raidArrivalMode = PawnsArrivalModeDefOf.CenterDrop;
                            IncidentDef def = IncidentDefOf.RaidEnemy;
                            def.Worker.TryExecute(parms);
                            oneRaidPerProgress = true;
                        }
                        mechRaidProgress = 0;
                    }
                    
                    

                }

                if (progress >= 1)
                {

                    progress = -1f;
                   
                    this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);

                    Pawn innerPawn = this.innerContainer?.FirstOrFallback() as Pawn;
                    PawnKindDef kindDef = innerPawn?.kindDef.GetModExtension<DefExtension_Paragon>()?.mechToConvertTo;
                    Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, Faction.OfPlayer, fixedBiologicalAge: 1, fixedChronologicalAge: 1,
                                                                                         newborn: false, forceGenerateNewPawn: true));
                    IntVec3 near = CellFinder.StandableCellNear(this.Position, this.Map, 5f);
                    GenSpawn.Spawn(pawn, near, this.Map);
                    pawn.health.AddHediff(InternalDefOf.GR_RecentlyHatched);
                    pawn.health.AddHediff(InternalDefOf.GR_AnimalControlHediff);

                    if (innerPawn.Name != null && !innerPawn.Name.ToString().UncapitalizeFirst().Contains(innerPawn.def.label))
                    {
                        pawn.Name = innerPawn.Name;
                    }
                    if (innerPawn.training != null)
                    {
                        pawn.training = innerPawn.training;
                    }
                    if (innerPawn.relations != null)
                    {
                        pawn.relations = innerPawn.relations;
                    }
                    if (innerPawn.playerSettings != null && innerPawn.playerSettings.AreaRestriction != null)
                    {
                        pawn.playerSettings.AreaRestriction = innerPawn.playerSettings.AreaRestriction;
                    }
                    QualityCategory quality = innerPawn.TryGetComp<CompHybrid>()?.quality ?? QualityCategory.Normal;

                    CompHybrid comp = pawn.TryGetComp<CompHybrid>();
                    if (comp != null)
                    {
                        comp.quality = quality;
                    }


                    if (unSpentFuse != null) { 
                        unSpentFuse.active = false;
                        GenExplosion.DoExplosion(unSpentFuse.Position, this.Map, 2.9f, DamageDefOf.Flame, this, -1, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
                    }

                    this.DestroyContents();

                }



            }
        }

        public override string GetInspectString()
        {
            StringBuilder sb = new StringBuilder(base.GetInspectString());
            if (this.progress != -1)
            {
                sb.AppendLine("GR_MechahybridizerProgress".Translate(this.progress.ToStringPercent()));

            }
          




            return sb.ToString().Trim();
        }


        public override void ExposeData()
        {
            //Save all the key variables so they work on game save / load
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[] { this });
            Scribe_Deep.Look<Building_Mechafuse>(ref this.unSpentFuse, "unSpentFuse", new object[] { this });
            Scribe_Values.Look(ref this.mechRaidProgress, nameof(this.mechRaidProgress));
            Scribe_Values.Look(ref this.progress, nameof(this.progress));
            Scribe_Values.Look(ref this.oneRaidPerProgress, nameof(this.oneRaidPerProgress));

        }

        public ThingOwner GetDirectlyHeldThings()
        {
            //Not used, included just in case something external calls it           
            return this.innerContainer;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            //Not used, included just in case something external calls it
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
        }

        public virtual void EjectContents()
        {
            //Remove ingredients from the container. 
            if (this.Map != null)
            {
                this.innerContainer.TryDropAll(this.Position, base.Map, ThingPlaceMode.Near, null, null);
            }
        }

        public void DestroyContents()
        {
            //Empties all containers and destroys contents

            if (this.innerContainer != null && this.innerContainer.Any)
            {
                this.innerContainer.ClearAndDestroyContents();
            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            EjectContents();
            base.Destroy(mode);
        }

        public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            EjectContents();
            base.Kill(dinfo, exactCulprit);
        }

        public virtual bool Accepts(Thing thing)
        {
            return this.innerContainer.CanAcceptAnyOf(thing, true);
        }

        public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (!this.Accepts(thing))
            {
                return false;
            }
            bool flag;
            if (thing.holdingOwner != null)
            {
                thing.holdingOwner.Remove(thing);
                this.innerContainer.TryAdd(thing, thing.stackCount, false);
                flag = true;
            }
            else
            {
                flag = this.innerContainer.TryAdd(thing, true);
            }
            if (flag)
            {
                if (thing.Faction != null && thing.Faction.IsPlayer)
                {
                    this.contentsKnown = true;
                }
                return true;
            }
            return false;
        }
    }
}