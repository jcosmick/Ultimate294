using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Pickups;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using MEC;
using SCP294.Types;
using SCP294.Types.Config;
using SCP294.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCP294.Classes
{
    public class SCP294Object
    {

        public static void dispenseDrink(ArraySegment<string> arguments, System.Random rand, SchematicObject scp294, Player player)
        {
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
        }
        /// <summary>
        /// Coroutine that handles the hint upon approaching SCP-294
        /// </summary>
        /// <returns></returns>
        public static IEnumerator<float> Handle294Hint()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(0.1f);

                foreach (Player player in Player.List)
                {
                    if (player == null) continue;
                    if (player.IsNPC) continue;
                    if (CanPlayerUse294(player))
                    {
                        if (!SCP294.Instance.PlayersNear294.Contains(player.UserId))
                        {
                            SchematicObject scp294 = GetClosest294(player);
                            if (SCP294.Instance.SCP294UsesLeft.Keys.Contains(scp294) && SCP294.Instance.SCP294UsesLeft[scp294] == 0)
                            {
                                player.ShowHint("<size=300>\n</size>\n<size=35>You Approach SCP-294.</size>\n<size=30>It seems to have lost all power, rendering it unusable for now...</size>", 3);
                            }
                            else
                            {
                                player.ShowHint("<size=300>\n</size>\n<size=35>You Approach SCP-294.</size>\n<size=30>If you had a coin, you could buy a drink...</size>\n<size=20>(Hold a Coin, Open Console, Use the command '.scp294 <drink>' to dispense your drink of choice)</size>", 3);
                            }
                            SCP294.Instance.PlayersNear294.Add(player.UserId);
                        }
                    }
                    else
                    {
                        if (SCP294.Instance.PlayersNear294.Contains(player.UserId))
                        {
                            SCP294.Instance.PlayersNear294.Remove(player.UserId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Naturally Spawns Instances of SCP-294 based on Config Values
        /// </summary>
        public static void SpawnSCP294()
        {
            SCP294.Instance.SpawnedSCP294s = new Dictionary<SchematicObject, bool>();

            if (SCP294.Instance.Config.ForceScp294SpawningLocations)
            {
                foreach (var roomType in SCP294.Instance.Config.SpawningLocations.SpawnRooms.Keys.ToList())
                {
                    Room SpawnRoom = Room.Get(roomType);
                    SpawnTransform relativeOffsetTransform = SCP294.Instance.Config.SpawningLocations.SpawnRooms[SpawnRoom.Type].RandomItem();

                    CreateSCP294(SpawnRoom.Position + (SpawnRoom.Rotation * relativeOffsetTransform.Position), Quaternion.Euler(SpawnRoom.Rotation.eulerAngles + Quaternion.Euler(relativeOffsetTransform.Rotation.x, relativeOffsetTransform.Rotation.y, relativeOffsetTransform.Rotation.z).eulerAngles), relativeOffsetTransform.Scale);
                }
            }
            else
            {
                // Get Room and Visual
                for (int i = 0; i < SCP294.Instance.Config.SpawningLocations.SpawnAmount; i++)
                {
                    Room SpawnRoom = RoomHandler.GetRandomRoom(SCP294.Instance.Config.SpawningLocations.SpawnRooms.Keys.ToList());
                    if (!SpawnRoom) continue;
                    SpawnTransform relativeOffsetTransform = SCP294.Instance.Config.SpawningLocations.SpawnRooms[SpawnRoom.Type].RandomItem();

                    CreateSCP294(SpawnRoom.Position + (SpawnRoom.Rotation * relativeOffsetTransform.Position), Quaternion.Euler(SpawnRoom.Rotation.eulerAngles + Quaternion.Euler(relativeOffsetTransform.Rotation.x, relativeOffsetTransform.Rotation.y, relativeOffsetTransform.Rotation.z).eulerAngles), relativeOffsetTransform.Scale);
                }
            }
        }

        /// <summary>
        /// Spawns an instance of SCP-294 at the given location
        /// </summary>
        public static void CreateSCP294(Vector3 Position, Quaternion Rotation, Vector3 Scale)
        {
            SchematicObject scp294 = ObjectSpawner.SpawnSchematic("scp294", Vector3.zero, Quaternion.identity, Vector3.one, null, false);
            scp294.Position = Position;
            scp294.Rotation = Rotation;
            scp294.Scale = Scale;

            // Add Illumination to Front
            Vector3 lightPos = scp294.Position;
            lightPos += scp294.Rotation * new Vector3(0f, 1.25f, -1.25f);
            SCP294.Instance.SCP294LightSources.Add(scp294, ObjectSpawner.SpawnLightSource(new LightSourceSerializable()
            {
                Color = "#FFF",
                Intensity = 0.25f,
                Shadows = true,
                Range = 1
            }, lightPos));

            // Add to 294 List
            SCP294.Instance.SpawnedSCP294s.Add(scp294, false);
            SCP294.Instance.SCP294UsesLeft.Add(scp294, SCP294.Instance.Config.MaxUsesPerMachine);
        }

        /// <summary>
        /// Destroys the nearest SCP-294 to the player
        /// </summary>
        /// <param name="scp294">Instance of SCP-294 to change</param>
        public static void RemoveSCP294(SchematicObject scp294)
        {
            if (scp294 != null && SCP294.Instance.SCP294UsesLeft.Keys.Contains(scp294))
            {
                SCP294.Instance.SpawnedSCP294s.Remove(scp294);
                SCP294.Instance.SCP294UsesLeft.Remove(scp294);
                try
                {
                    if (SCP294.Instance.SCP294LightSources.Keys.Contains(scp294))
                    {
                        SCP294.Instance.SCP294LightSources[scp294].Destroy();
                        SCP294.Instance.SCP294LightSources.Remove(scp294);
                    }
                }
                catch (Exception) { }
                scp294.Destroy();
            }
        }

        /// <summary>
        /// Set the uses the SCP-294 instance has left
        /// </summary>
        /// <param name="scp294">Instance of SCP-294 to change</param>
        /// <param name="useCount">Amount of Uses</param>
        public static void SetSCP294Uses(SchematicObject scp294, int useCount)
        {
            if (scp294 != null && SCP294.Instance.SCP294UsesLeft.Keys.Contains(scp294))
            {
                SCP294.Instance.SCP294UsesLeft[scp294] = useCount;
                // Disable and Enable
                SCP294.Instance.SCP294LightSources[scp294].Light.Range = useCount == 0 ? 0 : 1;
                SCP294.Instance.SCP294LightSources[scp294].Light.Intensity = useCount == 0 ? 0 : 0.25f;
            }
        }

        /// <summary>
        /// Plays the dispensing sound effect at the SCP-294 nearest to the player
        /// </summary>
        /// <param name="player">The Player who Triggered the Sound Effect. Used to get the SCP-294 closest to them</param>
        /// <param name="soundType">The Sound Type to play, either normal or unstable</param>
        public static void PlayDispensingSound(Player player, DrinkSound soundType)
        {
            SchematicObject scp294 = GetClosest294(player);

            SoundHandler.PlayAudio(new DrinkSoundFiles().List[(int)soundType], 50, false, "SCP-294", scp294.Position + new Vector3(0, 1, 0), 6f);
        }

        /// <summary>
        /// Check if the player is nearby a SCP-294 Instance within using distance
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool CanPlayerUse294(Player player)
        {
            foreach (SchematicObject scp294 in SCP294.Instance.SpawnedSCP294s.Keys)
            {
                if (!scp294) continue;
                if (Vector3.Distance(player.Position, scp294.Position) < SCP294.Instance.Config.UseDistance) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the closest SCP-294 to the player
        /// </summary>
        /// <param name="player"></param>
        /// <returns>SchematicObject reference of the SCP-294 closest to the player</returns>
        public static SchematicObject GetClosest294(Player player)
        {
            float closestDistance = 99999f;
            SchematicObject closestObject = null;
            foreach (SchematicObject scp294 in SCP294.Instance.SpawnedSCP294s.Keys)
            {
                if (!scp294) continue;
                if (Vector3.Distance(player.Position, scp294.Position) < closestDistance)
                {
                    closestDistance = Vector3.Distance(player.Position, scp294.Position);
                    closestObject = scp294;
                };
            }
            return closestObject;
        }

        /// <summary>
        /// A Patched version that spawns a new Drink Pickup using the created Item's Serial ID instead of giving it a new one. Important for drinks to function
        /// </summary>
        /// <param name="item"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="spawn"></param>
        /// <returns></returns>
        public static Pickup CreateDrinkPickup(Item item, Vector3 position, Quaternion rotation = default(Quaternion), bool spawn = true)
        {
            ItemPickupBase itemPickupBase = UnityEngine.Object.Instantiate(item.Base.PickupDropModel, position, rotation);
            itemPickupBase.Info = new PickupSyncInfo(item.Type, item.Weight, item.Serial);
            itemPickupBase.gameObject.transform.localScale = item.Scale;
            Pickup pickup = Pickup.Get(itemPickupBase);
            if (spawn)
            {
                pickup.Spawn();
            }

            return pickup;
        }
    }
}
