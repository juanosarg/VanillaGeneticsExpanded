namespace GeneticRim
{
    using HarmonyLib;
    using RimWorld;
    using Verse;

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.LabelNoCount), MethodType.Getter)]
    public static class Patch_PawnLabel
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance, ref string __result)
        {
            CompHybrid comp = __instance?.TryGetComp<CompHybrid>();
            if (comp != null) {
                __result += $" ({comp.quality.GetLabel().CapitalizeFirst()})";
            }
            
        }
    }
}
