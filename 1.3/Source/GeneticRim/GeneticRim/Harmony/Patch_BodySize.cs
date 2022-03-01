namespace GeneticRim
{
    using HarmonyLib;
    using Verse;

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.BodySize), MethodType.Getter)]
    public static class Patch_BodySize
    {
        [HarmonyPostfix]
        public static void Postfix(ref float __result, Pawn __instance) =>
            __result *= __instance?.TryGetComp<CompHybrid>()?.GetBodySizeFactor() ?? 1f;
    }
}