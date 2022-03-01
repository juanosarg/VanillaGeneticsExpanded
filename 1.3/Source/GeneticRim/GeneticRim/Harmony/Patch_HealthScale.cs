namespace GeneticRim
{
    using HarmonyLib;
    using Verse;

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.HealthScale), MethodType.Getter)]
    public static class Patch_HealthScale
    {
        [HarmonyPostfix]
        public static void Postfix(ref float __result, Pawn __instance) =>
            __result *= __instance?.TryGetComp<CompHybrid>()?.GetHealthScaleFactor() ?? 1f;
    }
}