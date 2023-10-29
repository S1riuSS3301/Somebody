using Content.Server.Botany.Components;
using Content.Server.Botany.Systems;
using Content.Shared.Atmos;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;
using Robust.Shared.Utility;

namespace Content.Server.Virology;

[Prototype("smear")]
public sealed class SmearPrototype : SmearData, IPrototype
{
    [IdDataField] public string ID { get; private init; } = default!;
}

public enum HarvestType : byte
{
    NoRepeat,
    Repeat,
    SelfHarvest
}

[DataDefinition]
public partial struct SmearChemQuantity
{
    [DataField("Min")] public int Min;
    [DataField("Max")] public int Max;
    [DataField("PotencyDivisor")] public int PotencyDivisor;
}

// TODO reduce the number of friends to a reasonable level. Requires ECS-ing things like plant holder component.
[Virtual, DataDefinition]
[Access(typeof(BotanySystem), typeof(PlantHolderSystem), typeof(SeedExtractorSystem), typeof(PlantHolderComponent), typeof(ReagentEffect))]
public partial class SmearData
{
    #region Tracking

    /// <summary>
    ///     The name of this seed. Determines the name of seed packets.
    /// </summary>
    [DataField("name")]
    public string Name { get; private set; } = "";

    /// <summary>
    ///     The noun for this type of seeds. E.g. for fungi this should probably be "spores" instead of "seeds". Also
    ///     used to determine the name of seed packets.
    /// </summary>
    [DataField("noun")]
    public string Noun { get; private set; } = "";

    /// <summary>
    ///     Name displayed when examining the hydroponics tray. Describes the actual plant, not the seed itself.
    /// </summary>
    [DataField("displayName")]
    public string DisplayName { get; private set; } = "";

    [DataField("mysterious")] public bool Mysterious;

    /// <summary>
    ///     If true, the properties of this seed cannot be modified.
    /// </summary>
    [DataField("immutable")] public bool Immutable;

    /// <summary>
    ///     If true, there is only a single reference to this seed and it's properties can be directly modified without
    ///     needing to clone the seed.
    /// </summary>
    [ViewVariables]
    public bool Unique = false; // seed-prototypes or yaml-defined seeds for entity prototypes will not generally be unique.
    #endregion

    #region Output
    /// <summary>
    ///     The entity prototype that is spawned when this type of seed is extracted from produce using a seed extractor.
    /// </summary>
    [DataField("packetPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string PacketPrototype = "SeedBase";

    /// <summary>
    ///     The entity prototype this seed spawns when it gets harvested.
    /// </summary>
    [DataField("productPrototypes", customTypeSerializer: typeof(PrototypeIdListSerializer<EntityPrototype>))]
    public List<string> ProductPrototypes = new();

    [DataField("chemicals")] public Dictionary<string, SmearChemQuantity> Chemicals = new();

    [DataField("consumeGasses")] public Dictionary<Gas, float> ConsumeGasses = new();

    [DataField("exudeGasses")] public Dictionary<Gas, float> ExudeGasses = new();

    #endregion


    public SmearData Clone()
    {
        DebugTools.Assert(!Immutable, "There should be no need to clone an immutable seed.");

        var newSmear = new SmearData
        {
            Name = Name,
            Noun = Noun,
            DisplayName = DisplayName,
            Mysterious = Mysterious,

            PacketPrototype = PacketPrototype,
            ProductPrototypes = new List<string>(ProductPrototypes),
            ConsumeGasses = new Dictionary<Gas, float>(ConsumeGasses),
            ExudeGasses = new Dictionary<Gas, float>(ExudeGasses),

            // Newly cloned seed is unique. No need to unnecessarily clone if repeatedly modified.
            Unique = true,
        };

        return newSmear;
    }
}
