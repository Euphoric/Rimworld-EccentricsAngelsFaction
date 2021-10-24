using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
// ReSharper disable InconsistentNaming

namespace Euphoric.EccentricTech.Faction
{
    [HarmonyPatch]
    internal static class BlockEccentricStockGeneratorPatch
    {
        private static bool IsEccentricItem(ThingDef thingDef)
        {
            var isEccentricApparel = thingDef.apparel?.tags != null && thingDef.apparel.tags.Contains("Eccentric");
            var isEccentricWeapon = thingDef.weaponTags != null && thingDef.weaponTags.Contains("Eccentric");
            var isEccentricTradeItem = thingDef.tradeTags != null && thingDef.tradeTags.Contains("Eccentric");

            return isEccentricApparel || isEccentricWeapon || isEccentricTradeItem;
        }

        private static bool ShouldTradeEccentricItem(StockGenerator stockGenerator, ThingDef thingDef)
        {
            bool isEccentricItem = IsEccentricItem(thingDef);
            bool isEccentricFaction = stockGenerator.trader.faction == EccentricsAngelsFactionDefOf.EccentricsAngels;

            return isEccentricFaction | !isEccentricItem;
        }

        [HarmonyPatch(typeof(StockGenerator_MiscItems), "HandlesThingDef")]
        [HarmonyPostfix]
        public static void StockGenerator_MiscItems_HandlesThingDef_Postfix(StockGenerator_MiscItems __instance, ThingDef thingDef, ref bool __result)
        {
            if (!__result)
                return;

            var shouldTradeItem = ShouldTradeEccentricItem(__instance, thingDef);
            __result &= shouldTradeItem;
        }

        [HarmonyPatch(typeof(StockGenerator_Category), "HandlesThingDef")]
        [HarmonyPostfix]
        public static void StockGenerator_Category_HandlesThingDef_Postfix(StockGenerator_Category __instance, ThingDef t, ref bool __result)
        {
            if (!__result)
                return;

            var shouldTradeItem = ShouldTradeEccentricItem(__instance, t);
            __result &= shouldTradeItem;
        }

        [HarmonyPatch(typeof(StockGenerator_Tag), "HandlesThingDef")]
        [HarmonyPostfix]
        public static void StockGenerator_Tag_HandlesThingDef_Postfix(StockGenerator_Tag __instance, ThingDef thingDef, ref bool __result)
        {
            if (!__result)
                return;

            var shouldTradeItem = ShouldTradeEccentricItem(__instance, thingDef);
            __result &= shouldTradeItem;
        }

        [HarmonyPatch(typeof(StockGenerator_BuyExpensiveSimple), "HandlesThingDef")]
        [HarmonyPostfix]
        public static void StockGenerator_BuyExpensiveSimple_HandlesThingDef_Postfix(StockGenerator_BuyExpensiveSimple __instance, ThingDef thingDef, ref bool __result)
        {
            if (!__result)
                return;

            var shouldTradeItem = ShouldTradeEccentricItem(__instance, thingDef);
            __result &= shouldTradeItem;
        }

        [HarmonyPatch(typeof(StockGenerator_BuyTradeTag), "HandlesThingDef")]
        [HarmonyPostfix]
        public static void StockGenerator_BuyTradeTag_HandlesThingDef_Postfix(StockGenerator_BuyTradeTag __instance, ThingDef thingDef, ref bool __result)
        {
            if (!__result)
                return;

            var shouldTradeItem = ShouldTradeEccentricItem(__instance, thingDef);
            __result &= shouldTradeItem;
        }

        [HarmonyPatch(typeof(StockGenerator_MultiDef), "HandlesThingDef")]
        [HarmonyPostfix]
        public static void StockGenerator_MultiDef_HandlesThingDef_Postfix(StockGenerator_MultiDef __instance, ThingDef thingDef, ref bool __result)
        {
            if (!__result)
                return;

            var shouldTradeItem = ShouldTradeEccentricItem(__instance, thingDef);
            __result &= shouldTradeItem;
        }
    }
}