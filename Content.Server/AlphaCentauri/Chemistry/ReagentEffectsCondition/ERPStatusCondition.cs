using Content.Server.DetailExaminable;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.AlphaCentauri.ERPStatus;
using Robust.Shared.Prototypes;

namespace Content.Server.AlphaCentauri.Chemistry.ReagentEffectsCondition;

public sealed partial class ERPStatusCondition : ReagentEffectCondition
{
    [DataField("erp")]
    public EnumStatus ERP = default!;

    [DataField("shouldHave")]
    public bool ShouldHave = true;

    public override bool Condition(ReagentEffectArgs args)
    {
        if (!(args.EntityManager.HasComponent<DetailExaminableComponent>(args.SolutionEntity)))
            return false;

        if (args.EntityManager.GetComponent<DetailExaminableComponent>(args.SolutionEntity).ERPStatus == ERP)
        {
            return ShouldHave;
        }
        else
        {
            return !ShouldHave;
        }
    }
    public override string GuidebookExplanation(IPrototypeManager prototype)
    {
        return Loc.GetString("reagent-effect-condition-guidebook-erpstatus", ("erp", ERP), ("shouldHave", ShouldHave));
    }

}
