using HarmonyLib;
using RimWorld;

namespace rimstocks;

[HarmonyPatch(typeof(Tradeable), "InitPriceDataIfNeeded")]
internal class Patch_Tradeable_InitPriceDataIfNeeded
{
    [HarmonyPostfix]
    private static void Postfix(Tradeable __instance)
    {
        if (__instance.ThingDef.tradeTags == null || !__instance.ThingDef.tradeTags.Contains("warbond"))
        {
            return;
        }

        Traverse.Create(__instance).Field("pricePlayerBuy").SetValue(__instance.ThingDef.BaseMarketValue);
        Traverse.Create(__instance).Field("pricePlayerSell").SetValue(__instance.ThingDef.BaseMarketValue *
                                                                      modBase.sellPrice *
                                                                      (__instance.AnyThing.HitPoints /
                                                                       (float)__instance.AnyThing.MaxHitPoints));
    }
}