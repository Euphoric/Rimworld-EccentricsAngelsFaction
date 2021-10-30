using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Euphoric.EccentricTech.Faction
{

    /// <summary>
    /// Rimworld can pick random faction when generating a parent for a pawn, while keeping the kind of the child pawn.
    /// This replaces the random selection method with one that doesn't include Eccentric's Angels.
    /// </summary>
    [HarmonyPatch]
    internal static class BlockEccentricsAngelsFactionFromParentsPawnGeneration
    {
        [HarmonyPatch(typeof(PawnRelationWorker_Sibling), "GenerateParent")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var originalMethod = AccessTools.Method(typeof(FactionManager), "TryGetRandomNonColonyHumanlikeFaction");
            var newMethod = AccessTools.Method(typeof(FactionHelper), "TryGetRandomNonColonyHumanlikeFactionExceptEccentricsAngel");
            foreach (var instruction in instructions)
            {
                if (instruction.Is(OpCodes.Callvirt, originalMethod))
                    yield return new CodeInstruction(OpCodes.Callvirt, newMethod);
                else
                    yield return instruction;
            }
        }
    }
}