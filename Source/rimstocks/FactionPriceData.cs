﻿using System.Collections.Generic;
using rimstocks.HarmonyPatches;
using RimWorld;
using UnityEngine;
using Verse;

namespace rimstocks;

public class FactionPriceData : IExposable
{
    public Color color;
    public string defname;
    public bool graphEnabled = true;
    public string label;
    public Dictionary<int, float> timeToPriceData = new();

    private Dictionary<int, float> timeToTrendData = new();

    //public float loan = 0f;
    //public int loan_day = 0;
    public void ExposeData()
    {
        Scribe_Values.Look(ref graphEnabled, "graphEnabled", true);
        Scribe_Values.Look(ref defname, "defname", "defname");
        Scribe_Values.Look(ref label, "label", "FACTIONNAME");
        Scribe_Values.Look(ref color, "color");
        Scribe_Collections.Look(ref timeToPriceData, "timeToPriceData", LookMode.Value, LookMode.Value);
        Scribe_Collections.Look(ref timeToTrendData, "timeToTrendData", LookMode.Value, LookMode.Value);
    }

    public void savePrice(float tick, float price)
    {
        var unitTime = Mathf.FloorToInt(tick / Harmony_SomeNamespace.modularTicksUnit);
        timeToPriceData.Remove(unitTime);

        timeToPriceData.Add(unitTime, price);
    }

    public float loadPrice(float tick)
    {
        var unitTime = Mathf.FloorToInt(tick / Harmony_SomeNamespace.modularTicksUnit);
        if (timeToPriceData.TryGetValue(unitTime, out var price))
        {
            return price;
        }

        if (modBase.use_rimwar && FactionDef.Named(defname) != null)
        {
            return Core.getRimwarPriceByDef(FactionDef.Named(defname));
        }

        return FactionDef.Named(defname) != null
            ? Core.getDefaultPrice(FactionDef.Named(defname))
            : Rand.Range(200f, 6000f);
    }

    public void saveTrend(float tick, float trend)
    {
        var unitTime = Mathf.FloorToInt(tick / Harmony_SomeNamespace.modularTicksUnit);
        timeToTrendData.Remove(unitTime);

        timeToTrendData.Add(unitTime, trend);
    }

    public float loadTrend(float tick)
    {
        var unitTime = Mathf.FloorToInt(tick / Harmony_SomeNamespace.modularTicksUnit);
        if (timeToTrendData.TryGetValue(unitTime, out var trend))
        {
            return trend;
        }

        if (modBase.use_rimwar && FactionDef.Named(defname) != null)
        {
            return Core.getRimwarPriceByDef(FactionDef.Named(defname));
        }

        return FactionDef.Named(defname) != null
            ? Core.getDefaultPrice(FactionDef.Named(defname))
            : Rand.Range(200f, 6000f);
    }
}