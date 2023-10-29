using Content.Server.Virology;
//using Content.Server.Virology.Components;
using Content.Server.Popups;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Swab;
using Content.Shared.Mobs.Components;
using Robust.Server.GameObjects;

namespace Content.Server.Virology.Systems;

public sealed class VirologySwabSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<VirologySwabComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<VirologySwabComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<VirologySwabComponent, BotanySwabDoAfterEvent>(OnDoAfter);
    }

    private void OnExamined(EntityUid uid, VirologySwabComponent swab, ExaminedEvent args)
    {
        /*if (args.IsInDetailsRange)
        {
            if (swab.SmearData != null)
                args.PushMarkup(Loc.GetString("swab-used"));
            else
                args.PushMarkup(Loc.GetString("swab-unused"));
        }*/
    }

    private void OnAfterInteract(EntityUid uid, VirologySwabComponent swab, AfterInteractEvent args)
    {

        if (args.Target == null || !args.CanReach || !HasComp<MobStateComponent>(args.Target))
            return;

        //TryComp<VirusComponent>(args.Target, out var actor);
        
        /*_doAfterSystem.TryStartDoAfter(new DoAfterArgs(args.User, swab.SwabDelay, new BotanySwabDoAfterEvent(), uid, target: args.Target, used: uid)
        {
            Broadcast = true,
            BreakOnTargetMove = true,
            BreakOnUserMove = true,
            NeedHand = true
        });*/
    }

    private void OnDoAfter(EntityUid uid, VirologySwabComponent swab, DoAfterEvent args)
    {
        /*if (args.Cancelled || args.Handled || !TryComp<PlantHolderComponent>(args.Args.Target, out var plant))
            return;

        if (swab.SmearData == null)
        {
            // Pick up pollen
            swab.SmearData = plant.Seed;
            _popupSystem.PopupEntity(Loc.GetString("botany-swab-from"), args.Args.Target.Value, args.Args.User);
        }
        else
        {
            var old = plant.Seed;
            if (old == null)
                return;
            //plant.Seed = _mutationSystem.Cross(swab.SmearData, old); // Cross-pollenate
            swab.SmearData = old; // Transfer old plant pollen to swab
            _popupSystem.PopupEntity(Loc.GetString("botany-swab-to"), args.Args.Target.Value, args.Args.User);
        }

        args.Handled = true;*/
    }

}

