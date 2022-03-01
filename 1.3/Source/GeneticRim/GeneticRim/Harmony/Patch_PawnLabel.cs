namespace GeneticRim
{
    using HarmonyLib;
    using RimWorld;
    using Verse;

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.LabelNoCount), MethodType.Getter)]
    public static class Patch_PawnLabel
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance, ref string __result) => 
            __result += $" ({__instance?.TryGetComp<CompHybrid>()?.quality.GetLabel().CapitalizeFirst()})";
    }
}
