using HarmonyLib;
using RimWorld;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using Mlie;
using Verse;

namespace rimstocks;

public class RimstocksMod : Mod
{
    private static string currentVersion;

    private static readonly bool exist_rimWar;
    private static bool rimwarLink;
    public static float DelistingPrice;
    public static float dividendPer;
    public static int ExtraHistoryTabIndex = 3;
    public static int limitDate;
    public static int loanDate;
    public static float loanPer;
    public static int loanScale;
    public static float maxReward;
    public static int militaryAid_cost;
    public static float militaryAid_multiply;
    public static float priceEvent_multiply;
    public static float rimwarPriceFactor;
    public static float sellPrice;
    public static RimstocksModSettings SettingsInstance;

    // Keep your public static fields so other code referencing them remains identical.
    public static bool useEnemyFaction;
    public static bool useVanillaEnemyFaction;

    static RimstocksMod()
    {
        if (ModsConfig.ActiveModsInLoadOrder.Any(mod => mod.PackageId.ToLower().Contains("Torann.RimWar".ToLower())))
        {
            exist_rimWar = true;
        }

        if (ModsConfig.IsActive("WealthList.ui.tmpfix"))
        {
            ExtraHistoryTabIndex = 4;
        }
    }

    public RimstocksMod(ModContentPack content) : base(content)
    {
        SettingsInstance = GetSettings<RimstocksModSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);

        // Import HugsLib settings
        importOldHugsLibSettings();

        // Initialize static values from settings immediately
        ApplySettingsToStatics();

        new Harmony("yayo.rimstocks.1").PatchAll();
    }

    private void ApplySettingsToStatics()
    {
        // copy settings into static fields so other code that depends on them continues to work
        if (SettingsInstance == null)
            return;

        useEnemyFaction = SettingsInstance.useEnemyFaction;
        useVanillaEnemyFaction = SettingsInstance.useVanillaEnemyFaction;

        rimwarLink = SettingsInstance.rimwarLink;
        rimwarPriceFactor = SettingsInstance.rimwarPriceFactor;

        sellPrice = Mathf.Clamp(SettingsInstance.sellPrice, 0.01f, 1f);
        dividendPer = Mathf.Clamp(SettingsInstance.dividendPer, 0f, 5f);
        maxReward = Mathf.Clamp(SettingsInstance.maxReward, 0f, 500000f);
        DelistingPrice = Mathf.Clamp(SettingsInstance.DelistingPrice, 1f, 1000f);
        limitDate = SettingsInstance.limitDate;
        militaryAid_cost = SettingsInstance.militaryAid_cost;
        militaryAid_multiply = SettingsInstance.militaryAid_multiply;
        priceEvent_multiply = SettingsInstance.priceEvent_multiply;

        loanDate = SettingsInstance.loanDate;
        loanPer = SettingsInstance.loanPer;
        loanScale = SettingsInstance.loanScale;
    }


    private static void importOldHugsLibSettings()
    {
        var hugsLibConfig = Path.Combine(GenFilePaths.SaveDataFolderPath, "HugsLib", "ModSettings.xml");
        if (!new FileInfo(hugsLibConfig).Exists)
        {
            return;
        }

        var xml = XDocument.Load(hugsLibConfig);
        var modNodeName = "yayo.rimstocks";

        var modSettings = xml.Root?.Element(modNodeName);
        if (modSettings == null)
        {
            return;
        }

        foreach (var modSetting in modSettings.Elements())
        {
            if (modSetting.Name == "useEnemyFaction")
            {
                SettingsInstance.useEnemyFaction = bool.Parse(modSetting.Value);
            }
            if (modSetting.Name == "ExtraHistoryTabIndex")
            {
                RimstocksMod.ExtraHistoryTabIndex = int.Parse(modSetting.Value);
            }
            if (modSetting.Name == "useVanillaEnemyFaction")
            {
                SettingsInstance.useVanillaEnemyFaction = bool.Parse(modSetting.Value);
            }
            if (modSetting.Name == "rimwarLink")
            {
                SettingsInstance.rimwarLink = bool.Parse(modSetting.Value);
            }
            if (modSetting.Name == "rimwarPriceFactor")
            {
                SettingsInstance.rimwarPriceFactor = float.Parse(modSetting.Value);
            }
            if (modSetting.Name == "sellPrice")
            {
                SettingsInstance.sellPrice = float.Parse(modSetting.Value);
            }
            if (modSetting.Name == "dividendPer")
            {
                SettingsInstance.dividendPer = float.Parse(modSetting.Value);
            }
            if (modSetting.Name == "maxReward")
            {
                SettingsInstance.maxReward = float.Parse(modSetting.Value);
            }
            if (modSetting.Name == "DelistingPrice")
            {
                SettingsInstance.DelistingPrice = float.Parse(modSetting.Value);
            }
            if (modSetting.Name == "limitDate")
            {
                SettingsInstance.limitDate = int.Parse(modSetting.Value);
            }
            if (modSetting.Name == "militaryAid_cost")
            {
                SettingsInstance.militaryAid_cost = int.Parse(modSetting.Value);
            }
            if (modSetting.Name == "militaryAid_multiply")
            {
                SettingsInstance.militaryAid_multiply = float.Parse(modSetting.Value);
            }
            if (modSetting.Name == "priceEvent_multiply")
            {
                SettingsInstance.priceEvent_multiply = float.Parse(modSetting.Value);
            }
            if (modSetting.Name == "loanDate")
            {
                SettingsInstance.loanDate = int.Parse(modSetting.Value);
            }
            if (modSetting.Name == "loanPer")
            {
                SettingsInstance.loanPer = float.Parse(modSetting.Value);
            }
            if (modSetting.Name == "loanScale")
            {
                SettingsInstance.loanScale = int.Parse(modSetting.Value);
            }
        }

        SettingsInstance.Write();
        xml.Root.Element(modNodeName)?.Remove();
        xml.Save(hugsLibConfig);
        //Statics applied right after this in constructor
        Log.Message($"[{modNodeName}]: Imported old HugLib-settings");
    }

    public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
    {
        Listing_Standard listing = new Listing_Standard();
        listing.Begin(inRect);
        listing.ColumnWidth = inRect.width / 2 - 16f;


        // Section 1: simple booleans / restart-required items
        listing.CheckboxLabeled(
            "YayosBank_useEnemyFaction".Translate(),
            ref SettingsInstance.useEnemyFaction,
            "YayosBank_RequiresRestart".Translate());
        listing.CheckboxLabeled(
            "YayosBank_useVanillaEnemyFaction".Translate(),
            ref SettingsInstance.useVanillaEnemyFaction,
            "YayosBank_RequiresRestart".Translate());

        // Integers / floats (limitDate)
        listing.Label("limitDate.t".Translate(), tooltip: "limitDate.d".Translate());
        SettingsInstance.limitDate = (int)Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.limitDate,
            1f,
            3650f,
            true,
            SettingsInstance.limitDate.ToString(),
            null,
            null,
            1f);

        listing.Gap(12f);

        // RimWar link and factor
        listing.CheckboxLabeled("rimwarLink.t".Translate(), ref SettingsInstance.rimwarLink, "rimwarLink.d".Translate());
        listing.Label(
            "rimwarPriceFactor.t".Translate() + ": " + SettingsInstance.rimwarPriceFactor.ToString("0.##"),
            tooltip: "rimwarPriceFactor.t".Translate());
        SettingsInstance.rimwarPriceFactor = Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.rimwarPriceFactor,
            0f,
            2f,
            true,
            SettingsInstance.rimwarPriceFactor.ToString("0.##"),
            null,
            null);

        listing.Gap(12f);

        // Price & dividend settings
        listing.Label("sellPrice.t".Translate() + ": " + SettingsInstance.sellPrice.ToString("0.00"));
        SettingsInstance.sellPrice = Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.sellPrice,
            0.01f,
            1f,
            true,
            SettingsInstance.sellPrice.ToString("0.00"),
            null,
            null);

        listing.Gap(12f);
        listing.Label(
            "dividendPer.t".Translate() + ": " + SettingsInstance.dividendPer.ToString("0.00"),
            tooltip: "dividendPer.t".Translate());
        SettingsInstance.dividendPer = Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.dividendPer,
            0f,
            5f,
            true,
            SettingsInstance.dividendPer.ToString("0.00"),
            null,
            null);

        listing.Gap(12f);
        listing.Label(
            "maxReward.t".Translate() + ": " + SettingsInstance.maxReward.ToString("0"),
            tooltip: "maxReward.d".Translate());
        SettingsInstance.maxReward = Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.maxReward,
            0f,
            500000f,
            true,
            SettingsInstance.maxReward.ToString("0"),
            null,
            null);

        listing.Gap(12f);
        listing.Label(
            "DelistingPrice.t".Translate() + ": " + SettingsInstance.DelistingPrice.ToString("0"),
            tooltip: "DelistingPrice.d".Translate());
        SettingsInstance.DelistingPrice = Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.DelistingPrice,
            1f,
            1000f,
            true,
            SettingsInstance.DelistingPrice.ToString("0"),
            null,
            null);

        listing.NewColumn();

        // military aid & price event
        listing.Label(
            "militaryAid_cost.t".Translate() + ": " + SettingsInstance.militaryAid_cost,
            tooltip: "militaryAid_cost.d".Translate());
        SettingsInstance.militaryAid_cost = (int)Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.militaryAid_cost,
            0f,
            100f,
            true,
            SettingsInstance.militaryAid_cost.ToString(),
            null,
            null);

        listing.Gap(12f);
        listing.Label(
            "militaryAid_multiply.t".Translate() + ": " + SettingsInstance.militaryAid_multiply.ToString("0.##"),
            tooltip: "militaryAid_multiply.d".Translate());
        SettingsInstance.militaryAid_multiply = Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.militaryAid_multiply,
            0f,
            10f,
            true,
            SettingsInstance.militaryAid_multiply.ToString("0.##"),
            null,
            null);

        listing.Gap(12f);
        listing.Label(
            "priceEvent_multiply.t".Translate() + ": " + SettingsInstance.priceEvent_multiply.ToString("0.##"),
            tooltip: "priceEvent_multiply.d".Translate());
        SettingsInstance.priceEvent_multiply = Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.priceEvent_multiply,
            0f,
            10f,
            true,
            SettingsInstance.priceEvent_multiply.ToString("0.##"),
            null,
            null);

        listing.Gap(12f);

        // Loans
        listing.Label("loanDate.t".Translate() + ": " + SettingsInstance.loanDate);
        SettingsInstance.loanDate = (int)Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.loanDate,
            1f,
            3650f,
            true,
            SettingsInstance.loanDate.ToString(),
            null,
            null);

        listing.Gap(12f);
        listing.Label("loanPer.t".Translate() + ": " + SettingsInstance.loanPer.ToString("0.00"));
        SettingsInstance.loanPer = Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.loanPer,
            0f,
            1f,
            true,
            SettingsInstance.loanPer.ToString("0.00"),
            null,
            null);

        listing.Gap(12f);
        listing.Label("loanScale.t".Translate() + ": " + SettingsInstance.loanScale, tooltip: "loanScale.d".Translate());
        SettingsInstance.loanScale = (int)Widgets.HorizontalSlider(
            listing.GetRect(22f),
            SettingsInstance.loanScale,
            0f,
            1000000f,
            true,
            SettingsInstance.loanScale.ToString(),
            null,
            null);

        listing.Gap(12f);
        if (listing.ButtonText("YayosBank_ResetYayosBank_ResetToDefault".Translate()))
        {
            SettingsInstance.useEnemyFaction = false;
            SettingsInstance.useVanillaEnemyFaction = false;
            SettingsInstance.rimwarLink = true;
            SettingsInstance.rimwarPriceFactor = 0.33f;
            SettingsInstance.sellPrice = 0.92f;
            SettingsInstance.dividendPer = 0.08f;
            SettingsInstance.maxReward = 20000f;
            SettingsInstance.DelistingPrice = 100f;
            SettingsInstance.limitDate = 0;
            SettingsInstance.militaryAid_cost = 5;
            SettingsInstance.militaryAid_multiply = 1f;
            SettingsInstance.priceEvent_multiply = 1f;
            SettingsInstance.loanDate = 60;
            SettingsInstance.loanPer = 0.05f;
            SettingsInstance.loanScale = 2000;
            // Apply immediately
            ApplySettingsToStatics();
        }

        if (currentVersion != null)
        {
            listing.Gap();
            GUI.contentColor = Color.gray;
            listing.Label("YayosBank_CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing.End();
    }

    public override string SettingsCategory() { return "YayosBank_ModName".Translate(); }

    public override void WriteSettings()
    {
        base.WriteSettings();

        // Update static fields and call the same post-change logic from SettingsChanged()
        ApplySettingsToStatics();

        // Call Core.patchIncident() to replicate old behaviour when settings changed
        Core.patchIncident();
    }

    // 'use_rimwar' property
    public static bool use_rimwar => exist_rimWar && rimwarLink;
}
