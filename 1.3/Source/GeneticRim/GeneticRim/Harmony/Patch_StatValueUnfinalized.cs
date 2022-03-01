namespace GeneticRim
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using HarmonyLib;
    using RimWorld;
    using Verse;

    [HarmonyPatch(typeof(StatWorker), nameof(StatWorker.GetValueUnfinalized))]
    public static class Patch_StatValueUnfinalized
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes                    = instructions.ToList();

            bool   found                    = false;
            for (int i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (!found && codes[i].opcode == OpCodes.Brfalse && codes[i - 1].opcode == OpCodes.Ldloc_1)
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld,    AccessTools.Field(typeof(StatWorker), "stat"));
                    yield return new CodeInstruction(OpCodes.Call,     AccessTools.Method(typeof(Patch_StatValueUnfinalized), nameof(GetValueFromComp)));
                    yield return new CodeInstruction(OpCodes.Mul);
                    yield return new CodeInstruction(OpCodes.Stloc_0);
                }
            }
        }

        public static float GetValueFromComp(Pawn pawn, StatDef stat) => 
            pawn.TryGetComp<CompHybrid>()?.GetStatFactor(stat) ?? 1f;
    }
}