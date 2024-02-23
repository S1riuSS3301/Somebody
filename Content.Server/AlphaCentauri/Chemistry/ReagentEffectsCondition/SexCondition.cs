using Content.Shared.Chemistry.Reagent;
using Content.Shared.Humanoid;
using Robust.Shared.Prototypes;

namespace Content.Server.AlphaCentauri.Chemistry.ReagentEffectsCondition;

public sealed partial class SexCondition : ReagentEffectCondition
{
    [DataField("sex")]
    public Sex Sex = default!;

    [DataField("shouldHave")]
    public bool ShouldHave = true;

    public override bool Condition(ReagentEffectArgs args)
    {
        if (args.EntityManager.GetComponent<HumanoidAppearanceComponent>(args.SolutionEntity).Sex == Sex)
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
        return Loc.GetString("reagent-effect-condition-guidebook-sex", ("sex", Sex), ("shouldHave", ShouldHave));
    }
}
