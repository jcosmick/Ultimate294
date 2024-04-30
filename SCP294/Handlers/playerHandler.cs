using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Hazards;
using MapEditorReborn.API.Features.Objects;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp173;
using RelativePositioning;
using SCP294.Classes;
using SCP294.Types;
using SCP294.Types.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCP294.handlers
{
    public class playerHandler
    {
        public void Joined(JoinedEventArgs args)
        {

        }

        public void ChangingItem(ChangingItemEventArgs args) {
            if (args.Player == null) return;
            if (args.Item == null) return;
            if (args.Player.IsNPC) return;

            if (SCP294.Instance.CustomDrinkItems.TryGetValue(args.Item.Serial, out DrinkInfo drinkInfo))
            {
                args.Player.ShowHint($"You pulled out the Drink of {drinkInfo.DrinkName}", 3);
            }
        }

        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            if (!SCP294.Instance.Config.EnableUsageByCoinFlip) return;

            System.Random rand = new System.Random();
            // Player MUST Be Human
            Player player = ev.Player;
            // Only Continue if Player is close enough to use 294
            if (!SCP294Object.CanPlayerUse294(player))
            {
                Log.Info("Player: "+player.DisplayNickname+ " flipped a coin but is not near a SCP294 vending machine");
                return;
            }
            SchematicObject scp294 = SCP294Object.GetClosest294(player);

            if (SCP294.Instance.SpawnedSCP294s[scp294])
            {
                player.ShowHint("This SCP-294 is on cooldown!");
                return;
            }
            if (SCP294.Instance.SCP294UsesLeft[scp294] == 0)
            {
                player.ShowHint("This SCP-294 has been deactivated...");
                return;
            }
            ArraySegment<string> arguments = new ArraySegment<string>(SCP294.Instance.DrinkManager.LoadedDrinks.RandomItem<CustomDrink>().DrinkNames.RandomItem<string>().Split());
            // Other Drinks
            foreach (CustomDrink customDrink in SCP294.Instance.DrinkManager.LoadedDrinks)
            {
                foreach (string drinkName in customDrink.DrinkNames)
                {
                    if (drinkName.ToLower() == String.Join(" ", arguments.Where(s => !String.IsNullOrEmpty(s))).ToLower())
                    {
                        float failNumber = (float)rand.NextDouble();
                        if (customDrink.BackfireChance > failNumber)
                        {
                            // BACKFIRED!!!
                            if (customDrink.ExplodeOnBackfire)
                            {
                                List<DrinkEffect> stealEffects = new List<DrinkEffect>() {
                                new DrinkEffect ()
                                    {
                                        EffectType = EffectType.Ensnared,
                                        Time = SCP294.Instance.Config.DispenseDelay,
                                        EffectAmount = 1,
                                        ShouldAddIfPresent = true
                                    }
                                };
                                PlayerEffectsController controller = player.ReferenceHub.playerEffectsController;
                                foreach (DrinkEffect effect in stealEffects)
                                {
                                    if (player.TryGetEffect(effect.EffectType, out StatusEffectBase statusEffect))
                                    {
                                        byte newValue = (byte)Mathf.Min(255, statusEffect.Intensity + effect.EffectAmount);
                                        controller.ChangeState(statusEffect.GetType().Name, newValue, effect.Time, effect.ShouldAddIfPresent);
                                    }
                                }

                                player.RemoveItem(player.CurrentItem);
                                SCP294Object.PlayDispensingSound(player, DrinkSound.Unstable);
                                Timing.CallDelayed(SCP294.Instance.Config.DispenseDelay, () =>
                                {
                                    ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                                    grenade.FuseTime = 0.1f;
                                    grenade.SpawnActive(player.Position, player);
                                    player.Kill($"Ordered a backfired {drinkName} from SCP-294");
                                });

                                // Cooldown
                                SCP294.Instance.SpawnedSCP294s[scp294] = true;
                                Timing.CallDelayed(SCP294.Instance.Config.CooldownTime, () =>
                                {
                                    SCP294.Instance.SpawnedSCP294s[scp294] = false;
                                });

                                Log.Info($"SCP-294 Started Dispensing a Drink of {drinkName}. {(SCP294.Instance.Config.ForceRandom ? "(Server Forced Random Drink)" : "")}");
                                SCP294Object.SetSCP294Uses(scp294, SCP294.Instance.SCP294UsesLeft[scp294] - 1);
                            }
                            else
                            {
                                List<DrinkEffect> stealEffects = new List<DrinkEffect>() {
                                new DrinkEffect ()
                                    {
                                        EffectType = EffectType.CardiacArrest,
                                        Time = 8,
                                        EffectAmount = 1,
                                        ShouldAddIfPresent = true
                                    }
                                };
                                PlayerEffectsController controller = player.ReferenceHub.playerEffectsController;
                                foreach (DrinkEffect effect in stealEffects)
                                {
                                    if (player.TryGetEffect(effect.EffectType, out StatusEffectBase statusEffect))
                                    {
                                        byte newValue = (byte)Mathf.Min(255, statusEffect.Intensity + effect.EffectAmount);
                                        controller.ChangeState(statusEffect.GetType().Name, newValue, effect.Time, effect.ShouldAddIfPresent);
                                    }
                                }
                                player.ShowHint($"You feel queasy, as if you're missing some of your body's contents...\n<size=20>(SCP-294 Backfired, Dispensing a Cup of {player.Nickname})</size>", 5);

                                // Found Drink
                                player.RemoveItem(player.CurrentItem);
                                SCP294Object.PlayDispensingSound(player, DrinkSound.Normal);
                                Timing.CallDelayed(SCP294.Instance.Config.DispenseDelay, () =>
                                {
                                    Item drinkItem = Item.Create(ItemType.AntiSCP207);
                                    drinkItem.Scale = new Vector3(1f, 1f, 0.8f);
                                    if (SCP294.Instance.Config.SpawnInOutput)
                                    {
                                        Vector3 spawnPos = scp294.Position;
                                        spawnPos += scp294.Rotation * new Vector3(-0.375f, 1f, -0.425f);

                                        Pickup drinkPickup = SCP294Object.CreateDrinkPickup(drinkItem, spawnPos, Quaternion.Euler(-90, 0, 0));
                                    }
                                    else
                                    {
                                        player.AddItem(drinkItem);
                                    }

                                    SCP294.Instance.CustomDrinkItems.Add(drinkItem.Serial, new DrinkInfo()
                                    {
                                        ItemSerial = drinkItem.Serial,
                                        ItemObject = drinkItem,
                                        DrinkEffects = new List<DrinkEffect>() { },
                                        DrinkMessage = "The drink tastes like blood. It's still warm.",
                                        DrinkName = player.Nickname,
                                        KillPlayer = false,
                                        KillPlayerString = "",
                                        HealAmount = 0,
                                        HealStatusEffects = false,
                                        Tantrum = false
                                    });
                                });

                                // Cooldown
                                SCP294.Instance.SpawnedSCP294s[scp294] = true;
                                Timing.CallDelayed(SCP294.Instance.Config.CooldownTime, () =>
                                {
                                    SCP294.Instance.SpawnedSCP294s[scp294] = false;
                                });

                                Log.Info($"SCP-294 Backfired!!! It Started Dispensing a Drink of {player.Nickname}");
                                SCP294Object.SetSCP294Uses(scp294, SCP294.Instance.SCP294UsesLeft[scp294] - 1);
                            }
                        }
                        else
                        {
                            // Found Drink
                            player.RemoveItem(player.CurrentItem);
                            if (customDrink.Explode)
                            {
                                // BACKFIRED!!!
                                List<DrinkEffect> stealEffects = new List<DrinkEffect>() {
                                new DrinkEffect ()
                                    {
                                        EffectType = EffectType.Ensnared,
                                        Time = SCP294.Instance.Config.DispenseDelay,
                                        EffectAmount = 1,
                                        ShouldAddIfPresent = true
                                    }
                                };
                                PlayerEffectsController controller = player.ReferenceHub.playerEffectsController;
                                foreach (DrinkEffect effect in stealEffects)
                                {
                                    if (player.TryGetEffect(effect.EffectType, out StatusEffectBase statusEffect))
                                    {
                                        byte newValue = (byte)Mathf.Min(255, statusEffect.Intensity + effect.EffectAmount);
                                        controller.ChangeState(statusEffect.GetType().Name, newValue, effect.Time, effect.ShouldAddIfPresent);
                                    }
                                }
                            }
                            SCP294Object.PlayDispensingSound(player, customDrink.Explode ? DrinkSound.Unstable : DrinkSound.Normal);
                            Timing.CallDelayed(SCP294.Instance.Config.DispenseDelay, () =>
                            {
                                if (customDrink.Explode)
                                {
                                    ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                                    grenade.FuseTime = 0.1f;
                                    grenade.SpawnActive(player.Position, player);
                                    player.Kill($"Ordered a {drinkName} from SCP-294");
                                }
                                else
                                {
                                    Item drinkItem = Item.Create(customDrink.AntiColaModel ? ItemType.AntiSCP207 : ItemType.SCP207);
                                    drinkItem.Scale = new Vector3(1f, 1f, 0.8f);
                                    if (SCP294.Instance.Config.SpawnInOutput)
                                    {
                                        Vector3 spawnPos = scp294.Position;
                                        spawnPos += scp294.Rotation * new Vector3(-0.375f, 1f, -0.425f);

                                        Pickup drinkPickup = SCP294Object.CreateDrinkPickup(drinkItem, spawnPos, Quaternion.Euler(-90, 0, 0));
                                    }
                                    else
                                    {
                                        player.AddItem(drinkItem);
                                    }

                                    SCP294.Instance.CustomDrinkItems.Add(drinkItem.Serial, new DrinkInfo()
                                    {
                                        ItemSerial = drinkItem.Serial,
                                        ItemObject = drinkItem,
                                        DrinkEffects = customDrink.DrinkEffects,
                                        DrinkMessage = customDrink.DrinkMessage,
                                        DrinkName = drinkName,
                                        KillPlayer = customDrink.KillPlayer,
                                        KillPlayerString = customDrink.KillPlayerString,
                                        HealAmount = customDrink.HealAmount,
                                        HealStatusEffects = customDrink.HealStatusEffects,
                                        Tantrum = customDrink.Tantrum,
                                        DrinkCallback = customDrink.DrinkCallback
                                    });
                                }
                            });

                            // Cooldown
                            SCP294.Instance.SpawnedSCP294s[scp294] = true;
                            Timing.CallDelayed(SCP294.Instance.Config.CooldownTime, () =>
                            {
                                SCP294.Instance.SpawnedSCP294s[scp294] = false;
                            });

                            Log.Info($"SCP-294 Started Dispensing a Drink of {drinkName}. {(SCP294.Instance.Config.ForceRandom ? "(Server Forced Random Drink)" : "")}");
                            SCP294Object.SetSCP294Uses(scp294, SCP294.Instance.SCP294UsesLeft[scp294] - 1);
                        }
                    }
                }
            }
            player.ShowHint("SCP-294 couldn't determine your drink, and refunded you your coin.");
    }

        public void UsedItem(UsedItemEventArgs args)
        {
            if (args.Player == null) return;
            if (args.Item == null) return;
            if (args.Player.IsNPC) return;

            if (SCP294.Instance.CustomDrinkItems.TryGetValue(args.Item.Serial, out DrinkInfo drinkInfo))
            {
                if (args.Player.IsScp)
                {
                    Timing.CallDelayed(0.01f, () =>
                    {
                        PlayerEffectsController controller = args.Player.ReferenceHub.playerEffectsController;
                        if (args.Player.TryGetEffect(args.Item.Type == ItemType.AntiSCP207 ? EffectType.AntiScp207 : EffectType.Scp207, out StatusEffectBase statusEffect))
                        {
                            byte newValue = (byte)Mathf.Min(255, statusEffect.Intensity - 1);
                            controller.ChangeState(statusEffect.GetType().Name, newValue, 0, false);
                        }
                    });
                }

                // Run any Callbacks
                if (drinkInfo.DrinkCallback != null) drinkInfo.DrinkCallback(args.Player);

                // Show the Message for Drinking
                args.Player.ShowHint(drinkInfo.DrinkMessage, 5);

                args.Player.CurrentItem = null;

                if (drinkInfo.KillPlayer)
                {
                    DrinkCallbacks.DrinkKill(args.Player, drinkInfo);
                } else
                {
                    args.Player.Heal(drinkInfo.HealAmount);
                    // Give player effects from drink
                    PlayerEffectsController controller = args.Player.ReferenceHub.playerEffectsController;
                    if (drinkInfo.HealStatusEffects)
                    {
                        foreach (EffectType effect in Enum.GetValues(typeof(EffectType)))
                        {
                            if (args.Player.TryGetEffect(effect, out StatusEffectBase statusEffect))
                            {
                                byte newValue = (byte)Mathf.Min(255, 0);
                                controller.ChangeState(statusEffect.GetType().Name, newValue, 0, false);
                            }
                        }
                    }
                    foreach (DrinkEffect effect in drinkInfo.DrinkEffects)
                    {
                        if (args.Player.TryGetEffect(effect.EffectType, out StatusEffectBase statusEffect))
                        {
                            byte newValue = (byte)Mathf.Min(255, statusEffect.Intensity + effect.EffectAmount);
                            controller.ChangeState(statusEffect.GetType().Name, newValue, effect.Time, effect.ShouldAddIfPresent);
                        }
                    }
                }
                 
                // Spawn Tantrum when player Drink Funny
                if (drinkInfo.Tantrum)
                {
                    if (PlayerRoleLoader.TryGetRoleTemplate(RoleTypeId.Scp173, out PlayerRoleBase result))
                    {
                        Scp173TantrumAbility ability = ((Scp173Role)result).GetComponentInChildren<Scp173TantrumAbility>();

                        if (Physics.Raycast(args.Player.Position, Vector3.down, out RaycastHit hitInfo, 3f, ability._tantrumMask))
                        {
                            TantrumEnvironmentalHazard tantrumEnvironmentalHazard = UnityEngine.Object.Instantiate(ability._tantrumPrefab);
                            Vector3 targetPos = hitInfo.point + (Vector3.up * 1.25f);
                            tantrumEnvironmentalHazard.SynchronizedPosition = new RelativePosition(targetPos);
                            NetworkServer.Spawn(tantrumEnvironmentalHazard.gameObject);
                            foreach (TeslaGate teslaGate in TeslaGateController.Singleton.TeslaGates)
                            {
                                if (teslaGate.IsInIdleRange(args.Player.Position))
                                {
                                    teslaGate.TantrumsToBeDestroyed.Add(tantrumEnvironmentalHazard);
                                }
                            }
                        }
                    }
                }

                SCP294.Instance.CustomDrinkItems.Remove(args.Item.Serial);
            }
        }
    }
}
