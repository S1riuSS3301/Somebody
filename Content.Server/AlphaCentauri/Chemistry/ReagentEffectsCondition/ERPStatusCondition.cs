using Content.Server.DetailExaminable;
using Content.Shared.EntityEffects;
using Content.Shared.AlphaCentauri.ERPStatus;
using Robust.Shared.Prototypes;

namespace Content.Server.AlphaCentauri.Chemistry.ReagentEffectsCondition;

public sealed partial class ERPStatusCondition : EntityEffectCondition
{
    [DataField("erp")]
    public EnumStatus ERP = default!;

    [DataField("shouldHave")]
    public bool ShouldHave = true;

    public override bool Condition(EntityEffectBaseArgs args)
    {
        if (!(args.EntityManager.HasComponent<DetailExaminableComponent>(args.TargetEntity)))
            return false;

        if (args.EntityManager.GetComponent<DetailExaminableComponent>(args.TargetEntity).ERPStatus == ERP)
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
