using Content.Shared.AlphaCentauri.ERPStatus;

namespace Content.Server.DetailExaminable
{
    [RegisterComponent]
    public sealed partial class DetailExaminableComponent : Component
    {
        [DataField("content", required: true)] [ViewVariables(VVAccess.ReadWrite)]
        public string Content = "";

        [DataField("ERPStatus", required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public EnumStatus ERPStatus = EnumStatus.NO;

        public string GetERPStatusName()
        {
            switch (ERPStatus)
            {
                case EnumStatus.HALF:
                    return Loc.GetString("character-erp-status-half");
                case EnumStatus.FULL:
                    return Loc.GetString("character-erp-status-full");
                default:
                    return Loc.GetString("character-erp-status-no");
            }
        }
    }
}
