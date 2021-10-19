using System.Linq;
using HarmonyLib;
using Verse;

namespace Euphoric.EccentricTech.Faction
{
    /// <summary>
    /// Warn if trying to generate pawn for Eccentric's Angels that isn't it's PawnKind.
    /// </summary>
    [HarmonyPatch(typeof(Pawn), "SetFaction")]
    internal static class PawnSetFactionWarnPatch
    {
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        public static void Prefix(Pawn __instance, RimWorld.Faction newFaction)
        {
            var factionPawnDefs = DefDatabase<PawnKindDef>.AllDefs.Where(def =>
                def.defaultFactionType == EccentricsAngelsFactionDefOf.EccentricsAngels);

            if (newFaction?.def == EccentricsAngelsFactionDefOf.EccentricsAngels && !factionPawnDefs.Contains(__instance.kindDef))
            {
                Log.Warning("Set Eccentric's Angels faction to pawn which doesn't belong there.");
            }
        }
    }
}