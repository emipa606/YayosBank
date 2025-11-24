using Verse;

namespace rimstocks;

public class RimstocksModSettings : ModSettings
{
    public float DelistingPrice = 100f;
    public float dividendPer = 0.08f;
    public int limitDate = 0;

    public int loanDate = 60;
    public float loanPer = 0.05f;
    public int loanScale = 2000;
    public float maxReward = 20000f;

    public int militaryAid_cost = 5;
    public float militaryAid_multiply = 1f;
    public float priceEvent_multiply = 1f;

    public bool rimwarLink = true;
    public float rimwarPriceFactor = 0.33f;

    public float sellPrice = 0.92f;
    // default values chosen to match your previous defaults
    public bool useEnemyFaction = false;
    public bool useVanillaEnemyFaction = false;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref useEnemyFaction, "useEnemyFaction", useEnemyFaction);
        Scribe_Values.Look(ref useVanillaEnemyFaction, "useVanillaEnemyFaction", useVanillaEnemyFaction);

        Scribe_Values.Look(ref rimwarLink, "rimwarLink", rimwarLink);
        Scribe_Values.Look(ref rimwarPriceFactor, "rimwarPriceFactor", rimwarPriceFactor);

        Scribe_Values.Look(ref sellPrice, "sellPrice", sellPrice);
        Scribe_Values.Look(ref dividendPer, "dividendPer", dividendPer);
        Scribe_Values.Look(ref maxReward, "maxReward", maxReward);
        Scribe_Values.Look(ref DelistingPrice, "DelistingPrice", DelistingPrice);
        Scribe_Values.Look(ref limitDate, "limitDate", limitDate);

        Scribe_Values.Look(ref militaryAid_cost, "militaryAid_cost", militaryAid_cost);
        Scribe_Values.Look(ref militaryAid_multiply, "militaryAid_multiply", militaryAid_multiply);
        Scribe_Values.Look(ref priceEvent_multiply, "priceEvent_multiply", priceEvent_multiply);

        Scribe_Values.Look(ref loanDate, "loanDate", loanDate);
        Scribe_Values.Look(ref loanPer, "loanPer", loanPer);
        Scribe_Values.Look(ref loanScale, "loanScale", loanScale);
    }
}
