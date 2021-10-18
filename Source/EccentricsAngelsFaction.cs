using System.Reflection;
using HarmonyLib;
using Verse;

namespace Euphoric.EccentricTech.Faction
{
    // ReSharper disable once UnusedType.Global
    public class EccentricsAngelsFaction : Mod
    {
        public EccentricsAngelsFaction(ModContentPack content)
            : base(content)
        {
            var harmonyInstance = new Harmony("Euphoric.EccentricTech.EccentricsAngelsFaction");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            Log.Message("Eccentric's Angels Faction patched.");
        }
    }
}