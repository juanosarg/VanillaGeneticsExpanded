namespace GeneticRim
{
    using HarmonyLib;
    using Verse;

    [HarmonyPatch(typeof(VerbProperties), nameof(VerbProperties.GetDamageFactorFor), typeof(Tool), typeof(Pawn), typeof(HediffComp_VerbGiver))]
    public static class Patch_GetDamageFactor
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn attacker, ref float __result) => 
            __result *= attacker?.TryGetComp<CompHybrid>()?.GetToolPowerFactor() ?? 1f;
    }
}