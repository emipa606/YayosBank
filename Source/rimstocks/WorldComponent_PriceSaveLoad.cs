using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace rimstocks;

public class WorldComponent_PriceSaveLoad : WorldComponent
{
    public static WorldComponent_PriceSaveLoad staticInstance;
    private Dictionary<string, FactionData> ar_factionData = new();
    public Dictionary<string, FactionPriceData> factionToPriceData = new();
    private bool initialized;

    public WorldComponent_PriceSaveLoad(World world) : base(world)
    {
        staticInstance = this;
    }

    public static void savePrice(FactionDef faction, float tick, float price)
    {
        staticInstance.getFactionPriceDataFrom(faction).savePrice(tick, price);
    }

    public static float loadPrice(FactionDef faction, float tick)
    {
        return staticInstance.getFactionPriceDataFrom(faction).loadPrice(tick);
    }

    public static void saveTrend(FactionDef faction, float tick, float price)
    {
        staticInstance.getFactionPriceDataFrom(faction).saveTrend(tick, price);
    }

    public static float loadTrend(FactionDef faction, float tick)
    {
        return staticInstance.getFactionPriceDataFrom(faction).loadTrend(tick);
    }

    private FactionPriceData getFactionPriceDataFrom(FactionDef f)
    {
        var Key = Util.factionDefNameToKey(f.defName);
        if (factionToPriceData.TryGetValue(Key, out var from))
        {
            return from;
        }

        var fpdn = new FactionPriceData
        {
            defname = f.defName,
            label = f.label,
            color = f.colorSpectrum is { Count: > 0 } ? f.colorSpectrum[0] : Color.white
        };

        factionToPriceData.Add(Key, fpdn);

        /*
            if(t.IsPlayer)
            {//update for player since it could be renamed
                factionToPriceData[Key].label = t.GetCallLabel();
            }
            */
        return factionToPriceData[Key];
    }

    public FactionPriceData func_289013(string Key)
    {
        return factionToPriceData.GetValueOrDefault(Key);
    }

    public override void FinalizeInit(bool fromLoad)
    {
        if (!initialized)
        {
            initialized = true;

            //HugsLib per map stuff
            DoMapLoaded();


            float ticksNow = Core.AbsTickGame;
            /*
                foreach(Faction f in Find.FactionManager.AllFactions)
                {
                    savePrice(f, ticksNow, 0);
                }
                */
            foreach (var f in from f in DefDatabase<FactionDef>.AllDefs
                     where
                         Core.isWarbondFaction(f)
                     select f)
            {
                if (RimstocksMod.use_rimwar)
                {
                    savePrice(f, ticksNow, Core.getRimwarPriceByDef(f));
                }
                else if (f != null)
                {
                    savePrice(f, ticksNow, Core.getDefaultPrice(f));
                }
                else
                {
                    savePrice(null, ticksNow, Rand.Range(200f, 6000f));
                }
            }
        }
        else
        {
            foreach (var f in Core.ar_faction)
            {
                var key = Util.factionDefNameToKey(f.defName);
                if (!staticInstance.factionToPriceData.Keys.Contains(key))
                {
                    continue;
                }

                var rs = staticInstance.func_289013(key);
                rs.defname = Util.keyToFactionDefName(key);
            }
        }
    }

    //From HugsLib conversion
    private void DoMapLoaded()
    {
        // replicate MapLoaded logic
        // "마지막 채권가격 불러오기, 아이템 가격에 적용"
        if (Core.ar_warbondDef == null) return;
        for (var i = 0; i < Core.ar_warbondDef.Count; i++)
        {
            var f = Core.ar_faction[i];
            var lastPrice = WorldComponent_PriceSaveLoad.loadPrice(f, Core.AbsTickGame);
            Core.ar_warbondDef[i].SetStatBaseValue(StatDefOf.MarketValue, lastPrice);
        }
    }
    public override void ExposeData()
    {
        Scribe_Values.Look(ref initialized, "initialized");
        Scribe_Collections.Look(ref factionToPriceData, "yayo_FactionPriceData", LookMode.Value, LookMode.Deep);
        Scribe_Collections.Look(ref ar_factionData, "yayo_FactionData", LookMode.Value, LookMode.Deep);
        if (ar_factionData != null)
        {
            return;
        }

        foreach (var f in Find.FactionManager.AllFactions)
        {
            var data = new FactionData();
            ar_factionData?.Add(f.GetUniqueLoadID(), data);
        }
    }


    public static FactionData getFactionData(Faction f)
    {
        return staticInstance.getFactionData_p(f);
    }

    private FactionData getFactionData_p(Faction f)
    {
        var Key = f.GetUniqueLoadID();
        if (ar_factionData.TryGetValue(Key, out var p))
        {
            return p;
        }

        var data = new FactionData
        {
            loan = 0,
            loan_targetTick = 0,
            loan_totalTick = 0,
            loan_per = 0f,
            loan_raidMulti = 0f
        };
        ar_factionData.Add(Key, data);

        return ar_factionData[Key];
    }
}