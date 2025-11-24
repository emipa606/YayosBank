using HarmonyLib;
using UnityEngine;
using Verse;

namespace rimstocks.HarmonyPatches;

[HarmonyPatch(typeof(Thing), nameof(Thing.TryAbsorbStack))]
internal class Thing_TryAbsorbStack
{
    public static void Prefix(Thing __instance, Thing other, bool respectStackLimit)
    {
        var cp = __instance.TryGetComp<CompLifespan>();
        if (cp == null)
        {
            return;
        }

        var cp_other = other.TryGetComp<CompLifespan>();
        if (cp_other == null)
        {
            return;
        }

        var num = ThingUtility.TryAbsorbStackNumToTake(__instance, other, respectStackLimit);
        cp.age = Mathf.CeilToInt(((cp.age * __instance.stackCount) + (cp_other.age * num)) /
                                 (float)(__instance.stackCount + num));

        return;
    }
}