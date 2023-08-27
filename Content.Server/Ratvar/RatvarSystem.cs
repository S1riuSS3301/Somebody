using Content.Server.Actions;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Nutrition.Components;
using Content.Server.Popups;
using Content.Server.Electrocution;
using Content.Shared.Actions;
using Content.Shared.Atmos;
using Content.Shared.Database;
using Content.Shared.Inventory;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Verbs;
using Content.Shared.Damage;
using Content.Shared.Electrocution;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.Ratvar
{
    public sealed partial class RatvarSystem : EntitySystem
    {
        [Dependency] private readonly ActionsSystem _action = default!;
        [Dependency] private readonly AtmosphereSystem _atmos = default!;
        [Dependency] private readonly HungerSystem _hunger = default!;
        [Dependency] private readonly PopupSystem _popup = default!;
        [Dependency] private readonly TransformSystem _xform = default!;
        [Dependency] private readonly MobThresholdSystem _mobThresholdSystem = default!;
        [Dependency] private readonly InventorySystem _inventorySystem = default!;
        [Dependency] private readonly ElectrocutionSystem _electrocutionSystem = default!;
        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<RatvarComponent, ComponentStartup>(OnStartup);

            //SubscribeLocalEvent<RatvarComponent, RatvarPunishLightningActionEvent>(OnPunishLightning);
        }

        private void OnStartup(EntityUid uid, RatvarComponent component, ComponentStartup args)
        {
            //_action.AddAction(uid, component.ActionPunishLightning, null);
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void OnPunishLightning(EntityUid uid, RatvarComponent component, RatvarPunishLightningActionEvent args)
        {
            if (args.Handled)
                return;

            if (!TryComp<HungerComponent>(uid, out var hunger))
                return;

            //hardElectrocute(GetVerbsEvent<Verb> trgt);
        }

        private void hardElectrocute(GetVerbsEvent<Verb> trgt)
        {
            if (TryComp<DamageableComponent>(trgt.Target, out var damageable) &&
            HasComp<MobStateComponent>(trgt.Target))
            {

                Verb hardElectrocute = new()
                {
                    Text = "Electrocute",
                    Category = VerbCategory.Smite,
                    Icon = new SpriteSpecifier.Rsi(new("/Textures/Clothing/Hands/Gloves/Color/yellow.rsi"), "icon"),
                    Act = () =>
                    {
                        int damageToDeal;
                        if (!_mobThresholdSystem.TryGetThresholdForState(trgt.Target, MobState.Critical, out var criticalThreshold))
                        {
                            // We can't crit them so try killing them.
                            if (!_mobThresholdSystem.TryGetThresholdForState(trgt.Target, MobState.Dead,
                                    out var deadThreshold))
                                return;// whelp.
                            damageToDeal = deadThreshold.Value.Int() - (int) damageable.TotalDamage;
                        }
                        else
                        {
                            damageToDeal = criticalThreshold.Value.Int() - (int) damageable.TotalDamage;
                        }

                        if (damageToDeal <= 0)
                            damageToDeal = 100; // murder time.

                        if (_inventorySystem.TryGetSlots(trgt.Target, out var slotDefinitions))
                        {
                            foreach (var slot in slotDefinitions)
                            {
                                if (!_inventorySystem.TryGetSlotEntity(trgt.Target, slot.Name, out var slotEnt))
                                    continue;

                                RemComp<InsulatedComponent>(slotEnt.Value); // Fry the gloves.
                            }
                        }

                        _electrocutionSystem.TryDoElectrocution(trgt.Target, null, damageToDeal,
                            TimeSpan.FromSeconds(30), refresh: true, ignoreInsulation: true);
                    },
                    Impact = LogImpact.Extreme,
                    Message = Loc.GetString("admin-smite-electrocute-description")
                };
                trgt.Verbs.Add(hardElectrocute);
            }
        }

    }

    public sealed partial class RatvarPunishLightningActionEvent : InstantActionEvent
    {

    }
}
