using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Disease;

[Serializable, NetSerializable]
public sealed partial class DiseaseVaccineDoAfterEvent : SimpleDoAfterEvent
{
}
