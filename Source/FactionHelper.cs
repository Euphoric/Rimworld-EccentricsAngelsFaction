using System.Linq;
using RimWorld;
using Verse;

namespace Euphoric.EccentricTech.Faction
{
    internal class FactionHelper
    {
        public bool TryGetRandomNonColonyHumanlikeFactionExceptEccentricsAngel(
            out RimWorld.Faction faction,
            bool tryMedievalOrBetter,
            bool allowDefeated = false,
            TechLevel minTechLevel = TechLevel.Undefined,
            bool allowTemporary = false)
        {
            Log.Message("Faction random request");

            return Find.FactionManager.AllFactions.Where(x =>
            {
                if (x.IsPlayer || x.Hidden || !x.def.humanlikeFaction || !allowDefeated && x.defeated || !allowTemporary && x.temporary)
                    return false;
                return minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel;
            }).Where(f=>f.def != EccentricsAngelsFactionDefOf.EccentricsAngels).TryRandomElementByWeight(x => tryMedievalOrBetter && x.def.techLevel < TechLevel.Medieval ? 0.1f : 1f, out faction);
        }
    }
}