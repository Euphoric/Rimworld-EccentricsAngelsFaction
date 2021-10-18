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
    /// This code blocks attempts of generating parent for "Eccentric's Angels" and replaces it with "Ancients" faction.
    /// This should prevent random pawns being generated as part of the faction.
    /// </summary>
    [HarmonyPatch(typeof(PawnRelationWorker_Sibling), "GenerateParent")]
    internal static class BlockEccentricsAngelsFactionFromParentsPawnGeneration
    {
        static RimWorld.Faction BlockFaction(RimWorld.Faction faction)
        {
            if (faction.def == EccentricsAngelsFactionDefOf.EccentricsAngels)
            {
                return RimWorld.Faction.OfAncients;
            }

            return faction;
        }

        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = instructions.ToList();

            // === get instruction that contains variable "faction"
            var thingGetFaction = AccessTools.PropertyGetter(typeof(Thing), "Faction");
            var thingGetFactionIndex = list.FirstIndexOf(i => i.Is(OpCodes.Callvirt, thingGetFaction));
            var setFactionInstruction = list[thingGetFactionIndex + 1];
            //Log.Message($"Set faction instruction {setFactionInstruction};{setFactionInstruction.operand}.");

            // === find instruction which loads "faction" variable for "PawnGenerationRequest.ctor"
            ConstructorInfo constructor = typeof(PawnGenerationRequest).GetConstructors().Single();
            var loadFactionInstruction = list
                .TakeWhile(i => !i.Is(OpCodes.Newobj, constructor)) // end at constructor call
                .Reverse() // search in reverse from construcor
                .First(i => i.IsLdloc((LocalBuilder)setFactionInstruction.operand)); // find "faction" load instruction

            // === inserts call to "BlockFaction" method after the load instruction
            var factionLoadIndex = list.IndexOf(loadFactionInstruction);
            //Log.Message($"Instruction at {factionLoadIndex}:{loadFactionInstruction}.");
            MethodInfo blockFactionMethod = SymbolExtensions.GetMethodInfo<RimWorld.Faction, RimWorld.Faction>(f => BlockFaction(f));
            //Log.Message($"{blockFactionMethod}");
            var callInstruction = new CodeInstruction(OpCodes.Call, blockFactionMethod);
            list.Insert(factionLoadIndex + 1, callInstruction);

            return list;
        }
    }
}