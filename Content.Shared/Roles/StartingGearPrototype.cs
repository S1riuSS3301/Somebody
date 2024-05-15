using Content.Shared.Preferences;
using Robust.Shared.Prototypes;
using Content.Shared.Humanoid; // AlphaCentauri-Underwear

namespace Content.Shared.Roles
{
    [Prototype("startingGear")]
    public sealed partial class StartingGearPrototype : IPrototype
    {
        [DataField]
        public Dictionary<string, EntProtoId> Equipment = new();

        [DataField]
        public List<EntProtoId> Inhand = new(0);

        /// <summary>
        /// Inserts entities into the specified slot's storage (if it does have storage).
        /// </summary>
        [DataField]
        public Dictionary<string, List<EntProtoId>> Storage = new();

        // AlphaCentauri-Underwear-Start
        [DataField("underweart")]
        private string _underweart = string.Empty;

        [DataField("underwearb")]
        private string _underwearb = string.Empty;
        // AlphaCentauri-Underwear-End

        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = string.Empty;

        public string GetGear(string slot)
        {
            /*if (profile != null)
            {
                // AlphaCentauri-Underwear-Start
                if (slot == "underweart" && profile.Sex == Sex.Female && !string.IsNullOrEmpty(_underweart))
                    return _underweart;
                if (slot == "underwearb" && profile.Sex == Sex.Female && !string.IsNullOrEmpty(_underwearb))
                    return _underwearb;
                // AlphaCentauri-Underwear-End
            }*/

            return Equipment.TryGetValue(slot, out var equipment) ? equipment : string.Empty;
        }
    }
}
