namespace GeneticRim
{
    using System.Linq;
    using HarmonyLib;
    using Verse;

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.BodySize), MethodType.Getter)]
    public static class Patch_BodySize
    {
        [HarmonyPostfix]
        public static void Postfix(ref float __result, Pawn __instance) =>
            __result *= Core.hybridPawnKinds.Contains(__instance.kindDef) ? __instance.TryGetComp<CompHybrid>().GetBodySizeFactor() : 1f;
    }
}