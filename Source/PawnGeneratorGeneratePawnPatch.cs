using System.Linq;
using HarmonyLib;
using Verse;

namespace Euphoric.EccentricTech.Faction
{
    /// <summary>
    /// Warn if trying to generate pawn for Eccentric's Angels that isn't it's PawnKind.
    /// </summary>
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", typeof(PawnGenerationRequest))]
    internal static class PawnGeneratorGeneratePawnPatch
    {
        [HarmonyPostfix]
        public static void Prefix(PawnGenerationRequest request)
        {
            var factionPawnDefs = DefDatabase<PawnKindDef>.AllDefs.Where(def =>
                def.defaultFactionType == EccentricsAngelsFactionDefOf.EccentricsAngels);

            if (request.Faction?.def == EccentricsAngelsFactionDefOf.EccentricsAngels && !factionPawnDefs.Contains(request.KindDef))
            {
                Log.Warning("Tried to generate pawn for Eccentric's Angels faction which doesn't belong there.");
            }
        }
    }
}