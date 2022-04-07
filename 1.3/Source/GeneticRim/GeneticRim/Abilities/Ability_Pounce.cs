
using RimWorld;
using Verse;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace GeneticRim
{
    public class Ability_Pounce : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);

            LongEventHandler.QueueLongEvent(() =>
            {
                IntVec3 destination = target.Cell + ((this.pawn.Position - target.Cell).ToVector3().normalized * 2).ToIntVec3();
                Map map = this.pawn.Map;

                AbilityPawnFlyer flyer = (AbilityPawnFlyer)PawnFlyer.MakeFlyer(InternalDefOf.GR_SlowFlyer, this.pawn, destination);
                flyer.ability = this;
                flyer.target = destination.ToVector3();
                GenSpawn.Spawn(flyer, target.Cell, map);
                //target.Thing.TakeDamage(new DamageInfo(DamageDefOf.Blunt, this.GetPowerForPawn(), float.MaxValue, instigator: this.pawn));

            }, "chargeAbility", false, null);
        }

        public override bool ShowGizmoOnPawn() =>
           this.pawn != null && this.pawn.Faction == Faction.OfPlayer;

        
    }
}