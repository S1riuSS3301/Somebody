using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed partial class MentagAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MentagAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, MentagAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        message = Regex.Replace(message, "r+", "rrr");
        message = Regex.Replace(message, "R+", "RRR");
        message = Regex.Replace(message, "h+", "hhh");
        message = Regex.Replace(message, "H+", "HHH");


        // RU-Localization-Start
        // р => ррр
        message = Regex.Replace(
            message,
            "р+",
            _random.Pick(new List<string>() { "рр", "ррр" })
        );
        // Р => РРР
        message = Regex.Replace(
            message,
            "Р+",
            _random.Pick(new List<string>() { "РР", "РРР" })
        );
        // х => ххх
        message = Regex.Replace(
            message,
            "х+",
            _random.Pick(new List<string>() { "хх", "ххх" })
        );
        // Х => ХХХ
        message = Regex.Replace(
            message,
            "Х+",
            _random.Pick(new List<string>() { "ХХ", "ХХХ" })
        );
       
        args.Message = message;
    }
}
