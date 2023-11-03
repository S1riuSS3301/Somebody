using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Verbs;
using Content.Shared.AlphaCentauri.ERPStatus;
using Robust.Shared.Utility;

namespace Content.Server.DetailExaminable
{
    public sealed class DetailExaminableSystem : EntitySystem
    {
        [Dependency] private readonly ExamineSystemShared _examineSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<DetailExaminableComponent, GetVerbsEvent<ExamineVerb>>(OnGetExamineVerbs);
        }

        private void OnGetExamineVerbs(EntityUid uid, DetailExaminableComponent component, GetVerbsEvent<ExamineVerb> args)
        {
            if (Identity.Name(args.Target, EntityManager) != MetaData(args.Target).EntityName)
                return;

            var detailsRange = _examineSystem.IsInDetailsRange(args.User, uid);

            var verb = new ExamineVerb()
            {
                Act = () =>
                {
                    var markup = new FormattedMessage();

                    // AlphaCentauri-ERPStatus-Start
                    if (component.ERPStatus == EnumStatus.FULL)
                        markup.PushColor(Color.Green);
                    else if (component.ERPStatus == EnumStatus.HALF)
                        markup.PushColor(Color.Yellow);
                    else
                        markup.PushColor(Color.Red);
                    markup.AddMarkup(component.GetERPStatusName() + "\n\n");
                    markup.PushColor(Color.White);
                    // AlphaCentauri-ERPStatus-End

                    markup.AddMarkup(component.Content);

                    _examineSystem.SendExamineTooltip(args.User, uid, markup, false, false);
                },
                Text = Loc.GetString("detail-examinable-verb-text"),
                Category = VerbCategory.Examine,
                Disabled = !detailsRange,
                Message = detailsRange ? null : Loc.GetString("detail-examinable-verb-disabled"),
                Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/examine.svg.192dpi.png"))
            };

            args.Verbs.Add(verb);
        }
    }
}
