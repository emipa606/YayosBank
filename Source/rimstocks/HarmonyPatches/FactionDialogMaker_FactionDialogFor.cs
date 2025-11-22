using HarmonyLib;
using RimWorld;
using Verse;

namespace rimstocks.HarmonyPatches;

[HarmonyPatch(typeof(FactionDialogMaker), nameof(FactionDialogMaker.FactionDialogFor))]
internal class FactionDialogMaker_FactionDialogFor
{
    private static void Postfix(ref DiaNode __result, Pawn negotiator, Faction faction)
    {
        DiaOption opt;
        // 대출
        if ((int)faction.def.techLevel >= 4)
        {
            opt = Util.RequestLoan(negotiator.Map, faction, negotiator);
            if (negotiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
            {
                opt.Disable("WorkTypeDisablesOption".Translate(SkillDefOf.Social.label));
            }

            __result.options.Insert(__result.options.Count - 1, opt);

            opt = Util.RequestLoanGiveUp(negotiator.Map, faction, negotiator);
            if (negotiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
            {
                opt.Disable("WorkTypeDisablesOption".Translate(SkillDefOf.Social.label));
            }

            __result.options.Insert(__result.options.Count - 1, opt);
        }


        // 채권 군사요청
        if (!Core.isWarbondFaction(faction.def))
        {
            return;
        }

        opt = Util.RequestMilitaryAidOptionWarbond(negotiator.Map, faction, negotiator);
        if (negotiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
        {
            opt.Disable("WorkTypeDisablesOption".Translate(SkillDefOf.Social.label));
        }

        __result.options.Insert(__result.options.Count - 1, opt);
    }
}