using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
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
using System.Text.RegularExpressions;
using UnityEngine;

namespace SCP294.handlers
{
    public class playerHandler
    {
        public void Joined(JoinedEventArgs args)
        {

        }

        static string ExtractColor(string text)
        {
            // Define a regular expression pattern to match the color
            string pattern = @"<color=(#[A-Fa-f0-9]{6})>";

            // Search for the color pattern in the text
            Match match = Regex.Match(text, pattern);

            // If a match is found, return the color
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        public void ChangingItem(ChangingItemEventArgs args)
        {
            if (args.Player == null) return;
            if (args.Item == null) return;
            if (args.Player.IsNPC) return;

            if (SCP294.Instance.CustomDrinkItems.TryGetValue(args.Item.Serial, out DrinkInfo drinkInfo))
            {
                string hint = $"You pulled out the Drink of ";
                if (SCP294.Instance.Config.EnableRarity)
                {
                    Rarity drinkRarity = SCP294.Instance.RarityManager.GetRarityFromDrink(drinkInfo.DrinkName);
                    if(drinkRarity != null) //Could be null if it's a drink of another player or drink is not in a rarity for test reason
                    {
                        if (ExtractColor(drinkRarity.Name) != null)
                            hint += $"<color={ExtractColor(drinkRarity.Name)}><b>{drinkInfo.DrinkName}</b></color>";
                        else hint += $"<b>{drinkInfo.DrinkName}</b>";
                        hint += $", that is {SCP294.Instance.RarityManager.GetRarityFromDrink(drinkInfo.DrinkName).Name}";
                    }
                    
                }
                else
                {
                    hint += $"{drinkInfo.DrinkName}";
                }
                args.Player.ShowHint(hint, 3);
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
                Log.Info("Player: " + player.DisplayNickname + " flipped a coin but is not near a SCP294 vending machine");
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
            ArraySegment<string> arguments = new ArraySegment<string>();
            if (!SCP294.Instance.Config.EnableRarity)
                arguments = new ArraySegment<string>(SCP294.Instance.DrinkManager.LoadedDrinks.RandomItem<CustomDrink>().DrinkNames.RandomItem<string>().Split());
            else
            {
                System.Random random = new System.Random();
                float randomNumber = random.Next(0, 101); //1-100
                Rarity rarity = SCP294.Instance.RarityManager.GetRandomRarity(randomNumber);
                arguments = new ArraySegment<string>(rarity.Drinks.GetRandomValue().Split());
            }

            SCP294Object.dispenseDrink(arguments, rand, scp294, player);
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
                }
                else
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
