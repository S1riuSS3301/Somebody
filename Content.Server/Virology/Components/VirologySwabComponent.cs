using System.Threading;

namespace Content.Server.Virology
{
    [RegisterComponent]
    public sealed partial class VirologySwabComponent : Component
    {
        [DataField("swabDelay")]
        public float SwabDelay = 2f;

        public SmearData? SmearData;
    }
}
