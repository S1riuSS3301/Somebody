using Content.Shared.Actions.ActionTypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Ratvar
{
    [RegisterComponent]
    public sealed partial class RatvarComponent : Component
    {
        /// <summary>
        ///     The action for the Punish with Lightning
        /// </summary>
        [DataField("actionPunishLightning", required: false)]
        public InstantAction ActionPunishLightning = new();
    }
}
