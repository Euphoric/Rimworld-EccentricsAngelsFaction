using System.Reflection;
using HarmonyLib;
using Verse;

namespace Euphoric.EccentricTech.Faction
{
    [StaticConstructorOnStartup]
    // ReSharper disable once UnusedType.Global
    public static class EccentricsAngelsHarmonyPatch
    {
        static EccentricsAngelsHarmonyPatch()
        {
            //Harmony.DEBUG = true;
            
            var harmonyInstance = new Harmony("Euphoric.EccentricTech.EccentricsAngelsFaction");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            
            Log.Message("Eccentric's Angels Faction patched.");
        }
    }
}