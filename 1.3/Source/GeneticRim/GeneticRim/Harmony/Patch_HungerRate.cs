namespace GeneticRim
{
    using HarmonyLib;
    using RimWorld;
    using Verse;

    [HarmonyPatch(typeof(Need_Food), "HungerRate", MethodType.Getter)]
    public static class Patch_HungerRate
    {
        [HarmonyPostfix]
        public static void Postfix(ref float __result, Pawn ___pawn) =>
            __result *= ___pawn?.TryGetComp<CompHybrid>()?.GetHungerRateFactor() ?? 1f;
    }
}