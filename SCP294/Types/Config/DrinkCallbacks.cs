using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using MEC;
using PlayerRoles;
using SCP294.CustomItems;
using SCP294.Utils;
using System.Collections.Generic;
using System.Linq;
using UncomplicatedCustomItems.API;
using UncomplicatedCustomItems.API.Features;
using UnityEngine;

namespace SCP294.Types.Config
{
    public class DrinkCallbacks
    {
        // DRINK CALLBACKS
        public static void DrinkKill(Player player, DrinkInfo drinkInfo)
        {
            if (player.IsScp)
            {
                player.Health -= 250;
                PlayerEffectsController controller = player.ReferenceHub.playerEffectsController;
                if (player.TryGetEffect(EffectType.CardiacArrest, out StatusEffectBase statusEffect))
                {
                    byte newValue = (byte)Mathf.Min(255, 1);
                    controller.ChangeState(statusEffect.GetType().Name, newValue, 30f, false);
                }
                if (player.Health < 1)
                {
                    player.Kill(drinkInfo.KillPlayerString);
                }
            }
            else
            {
                player.Kill(drinkInfo.KillPlayerString);
            }
        }
        public static readonly List<Color> Colors = new List<Color>
            {
                new Color(1, 0.2f, 0.2f),
                new Color(1, 0.5f, 0),
                Color.yellow,
                Color.green*1.1f,
                Color.cyan,
                new Color(0f, 0.65f, 1.3f),
                new Color(0.78f, 0.13f, 1.3f),
            };

        public static void BallSpam(Player player) // SCP-018
        {
            int numBallToSpawn = 5;
            for (int i = 0; i < numBallToSpawn; i++)
            {
                Projectile.CreateAndSpawn(ItemType.SCP018, player.Position, default, player).GameObject.GetComponent<Rigidbody>().AddForce(new Vector3(500, 500, 500));
            }
        }

        public static void Pirots(Player player) // maxwin 20 monete
        {
            var currentColor = Color.clear;
            SoundHandler.PlayAudio("pirots.ogg", 50, false, "pirots", player.Position, 12f, player);
            Timing.RunCoroutine(pirots());

            IEnumerator<float> pirots()
            {
                for (int i = 0; i < 20; i++)
                {
                    yield return Timing.WaitForSeconds(0.5f);
                    currentColor = Color.Lerp(Colors[(int)(i % Colors.Count)], Colors[(int)((i + 1) % Colors.Count)], i % 1);
                    player.CurrentRoom.Color = currentColor * 1.5f;
                    Pickup.CreateAndSpawn(ItemType.Coin, player.Position, default, player).GameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
                }
                Map.ResetLightsColor();
                yield break;
            }
        }

        public static void Shrink(Player player)
        {
            ChangeSizeX(player, -0.15f);
            ChangeSizeY(player, -0.15f);
            ChangeSizeZ(player, -0.15f);
            LimitSizeOnDrinks(player);
        }
        public static void Enderman(Player player)
        {
            if (Warhead.IsDetonated)
            {
                Timing.CallDelayed(0.1f, () =>
                {
                    player.ShowHint("La warhead è già esplosa, il drink non ha avuto nessun effetto", 5f);
                });
            }
            else if (Map.IsLczDecontaminated)
            {
                List<Room> AllRoomExceptLcz = Room.List.Where(room => !room.Name.StartsWith("LCZ")).ToList();
                player.Teleport(AllRoomExceptLcz.GetRandomValue());
                SoundHandler.PlayAudio("enderman.ogg", 50, false, "enderman", player.Position, 2f, player);
            }
            else
            {
                player.Teleport(Room.List.GetRandomValue());
                SoundHandler.PlayAudio("enderman.ogg", 50, false, "enderman", player.Position, 2f, player);
            }
        }
        public static void Grow(Player player)
        {
            ChangeSizeX(player, 0.15f);
            ChangeSizeY(player, 0.15f);
            ChangeSizeZ(player, 0.15f);
            LimitSizeOnDrinks(player);
        }

        public static void Wide(Player player)
        {
            ChangeSizeX(player, 0.15f);
            ChangeSizeZ(player, 0.15f);
            LimitSizeOnDrinks(player);
        }
        public static void Flash(Player player)
        {
            int numFlashToSpawn = 4;
            for (int i = 0; i < numFlashToSpawn; i++)
            {
                Projectile.CreateAndSpawn(ProjectileType.Flashbang, player.Position, default, true).GameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
            }
        }

        public static void Zombie(Player player)
        {
            player.RoleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.RemoteAdmin);
            player.Teleport(RoomType.Hcz049);
            Helium(player, 1200f);
            ChangeSizeX(player, -0.40f);
            ChangeSizeY(player, -0.40f);
            ChangeSizeZ(player, -0.40f);

        }
        public static void Tradimento(Player player)
        {
            if (player.Role.Team == Team.ClassD && (Warhead.IsDetonated || Map.IsLczDecontaminated))
            {
                player.RoleManager.ServerSetRole(RoleTypeId.NtfCaptain, RoleChangeReason.RemoteAdmin);
            }
            else if (player.Role.Team == Team.Scientists && (Warhead.IsDetonated || Map.IsLczDecontaminated))
            {
                player.RoleManager.ServerSetRole(RoleTypeId.ChaosRepressor, RoleChangeReason.RemoteAdmin);
            }
            else if (player.Role.Team == Team.ClassD)
            {
                player.RoleManager.ServerSetRole(RoleTypeId.Scientist, RoleChangeReason.RemoteAdmin);
            }
            else if (player.Role.Team == Team.Scientists)
            {
                player.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.RemoteAdmin);
            }
            else if (player.Role.Team == Team.ChaosInsurgency)
            {
                player.RoleManager.ServerSetRole(RoleTypeId.NtfCaptain, RoleChangeReason.RemoteAdmin);
            }
            else if (player.Role.Team == Team.FoundationForces)
            {
                player.RoleManager.ServerSetRole(RoleTypeId.ChaosRepressor, RoleChangeReason.RemoteAdmin);
            }

        }
        public static void uomonero(Player player)
        {
            var posizione = player.Position;
            player.Teleport(RoomType.Pocket);
            Timing.RunCoroutine(uscita());

            IEnumerator<float> uscita()
            {
                yield return Timing.WaitForSeconds(15f);
                player.Teleport(posizione);
                yield break;
            }


        }

        public static void Thin(Player player)
        {
            ChangeSizeX(player, -0.15f);
            ChangeSizeZ(player, -0.15f);
            LimitSizeOnDrinks(player);
        }

        public static void Boykisser(Player player)
        {
            Timing.CallDelayed(0.1f, () =>
            {
                player.ShowHint("<size=3><scale=0.5><line-height=3px><color=#FFFDFF>███████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FCFAFC>█</color><color=#EBEAEC>█</color><color=#EFEEF0>█</color><color=#FFFFFF>███</color><color=#FFFDFF>█████████████████████████████████████████████████</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>████</color><color=#FFFDFF>█████</color>\n<line-height=3px><color=#FFFDFF>██████████</color><color=#FFFFFF>██</color><color=#FCFBFC>█</color><color=#EAE9EA>█</color><color=#787778>█</color><color=#666567>█</color><color=#716F71>█</color><color=#BDBBBD>█</color><color=#F5F4F5>█</color><color=#FFFFFF>██</color><color=#FFFDFF>███████████████████</color><color=#FFFEFF>█████</color><color=#FFFDFF>████████████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█████</color><color=#EFEDEF>█</color><color=#C5C3C5>█</color><color=#BEBDBE>█</color><color=#E8E7E9>█</color><color=#FFFFFF>███</color><color=#FFFEFF>██</color>\n<line-height=3px><color=#FFFDFF>█████████</color><color=#FFFFFF>█</color><color=#ECEBEC>█</color><color=#5F5E60>█</color><color=#A4A3A4>█</color><color=#FFFFFF>█</color><color=#FDFCFE>█</color><color=#F6F5F7>█</color><color=#BDBBBD>█</color><color=#797779>█</color><color=#545254>█</color><color=#A6A5A6>█</color><color=#F9F7F9>█</color><color=#FFFFFF>██</color><color=#FFFDFF>███████</color><color=#FFFEFF>█████</color><color=#FFFDFF>████</color><color=#FFFEFF>█</color><color=#FFFFFF>████</color><color=#FFFEFF>█</color><color=#FFFDFF>██████████████████</color><color=#FFFFFF>███</color><color=#F4F3F4>█</color><color=#D4D3D5>█</color><color=#C0BFC1>█</color><color=#ACABAC>█</color><color=#7A797B>█</color><color=#5B5A5C>█</color><color=#868486>█</color><color=#AAA9AA>█</color><color=#DFDEDF>█</color><color=#FFFEFF>█</color><color=#BBBABC>█</color><color=#E9E8E9>█</color><color=#FFFFFF>██</color>\n<line-height=3px><color=#FFFDFF>████████</color><color=#FFFFFF>█</color><color=#F3F1F3>█</color><color=#454446>█</color><color=#717072>█</color><color=#F2F1F2>█</color><color=#FFFFFF>█</color><color=#FEFDFF>██</color><color=#FFFFFF>██</color><color=#D7D6D7>█</color><color=#6F6D6F>█</color><color=#545254>█</color><color=#ACABAC>█</color><color=#F7F6F7>█</color><color=#FFFFFF>██</color><color=#FFFEFF>██</color><color=#FFFFFF>████████████</color><color=#FDFBFD>█</color><color=#908F90>█</color><color=#6F6E6F>█</color><color=#ACAAAC>█</color><color=#DDDCDD>█</color><color=#FFFFFF>██</color><color=#FFFEFF>█</color><color=#FFFDFF>█████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>██</color><color=#F7F6F8>█</color><color=#BEBDBE>█</color><color=#6F6D6F>█</color><color=#656465>█</color><color=#7C7A7C>█</color><color=#7D7C7D>█</color><color=#3A393A>█</color><color=#989798>█</color><color=#EAE9EA>█</color><color=#FFFFFF>███</color><color=#CDCCCD>█</color><color=#262527>█</color><color=#D3D2D3>█</color><color=#FFFFFF>██</color>\n<line-height=3px><color=#FFFDFF>███████</color><color=#FFFFFF>██</color><color=#817F81>█</color><color=#434243>█</color><color=#F9F8F9>█</color><color=#FFFFFF>█</color><color=#FEFDFF>█</color><color=#FDFCFE>███</color><color=#FEFDFF>█</color><color=#FFFFFF>██</color><color=#D1D0D1>█</color><color=#797779>█</color><color=#5C5B5D>█</color><color=#9F9E9F>█</color><color=#FAF8FA>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█</color><color=#FCFAFC>█</color><color=#F5F3F5>█</color><color=#DAD9DA>█</color><color=#C2C1C3>██</color><color=#C2C0C2>█</color><color=#C1BFC1>█</color><color=#C0BFC0>█</color><color=#D9D8D9>█</color><color=#E1DFE1>█</color><color=#F1EFF1>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█</color><color=#3A393B>█</color><color=#969597>█</color><color=#A09FA1>█</color><color=#454446>█</color><color=#8A888A>█</color><color=#E2E0E2>█</color><color=#FFFFFF>██</color><color=#FFFDFF>██████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#D3D2D3>█</color><color=#9F9E9F>█</color><color=#585759>█</color><color=#7A787A>█</color><color=#BBBABC>█</color><color=#F5F4F6>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#F9F7F9>█</color><color=#FFFFFF>██</color><color=#FFFDFF>██</color><color=#FFFFFF>█</color><color=#B6B4B6>█</color><color=#3C3B3C>█</color><color=#FEFDFE>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color>\n<line-height=3px><color=#FFFDFF>██████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#A6A4A6>█</color><color=#2E2D2F>█</color><color=#D6D5D6>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FEFDFF>█████</color><color=#FDFCFE>█</color><color=#FFFFFF>██</color><color=#ACABAC>█</color><color=#3E3D3F>█</color><color=#3D3C3D>█</color><color=#7C7A7C>█</color><color=#676667>█</color><color=#3F3D3F>█</color><color=#646365>█</color><color=#8A898A>█</color><color=#929193>█</color><color=#AFADAF>█</color><color=#ADABAD>█</color><color=#ADACAE>█</color><color=#939293>█</color><color=#8B8A8C>█</color><color=#807F80>█</color><color=#6C6B6D>█</color><color=#6E6C6E>█</color><color=#A2A1A3>█</color><color=#383738>█</color><color=#D3D2D4>█</color><color=#FFFFFF>█</color><color=#E3E1E3>█</color><color=#989799>█</color><color=#504E50>█</color><color=#7E7D7F>█</color><color=#F8F6F8>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██████</color><color=#FFFEFF>█</color><color=#FFFFFF>██</color><color=#FEFCFE>█</color><color=#C3C2C4>█</color><color=#646364>█</color><color=#646264>█</color><color=#A7A6A8>█</color><color=#DCDBDD>█</color><color=#FFFFFF>██</color><color=#FFFEFF>███</color><color=#FFFFFF>█</color><color=#FFFEFF>██</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#B7B6B7>█</color><color=#3A393A>█</color><color=#FDFCFD>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color>\n<line-height=3px><color=#FFFDFF>██████</color><color=#FFFFFF>█</color><color=#F4F2F4>█</color><color=#383739>█</color><color=#A1A0A2>█</color><color=#FFFFFF>█</color><color=#FFFEFF>███████</color><color=#FFFDFF>█</color><color=#FDFDFF>█</color><color=#FFFEFF>█</color><color=#EEECEE>█</color><color=#706E70>█</color><color=#252325>█</color><color=#343334>█</color><color=#9C9B9C>█</color><color=#DCDBDC>█</color><color=#D5D4D5>█</color><color=#E7E5E7>█</color><color=#FFFFFF>████████</color><color=#F9F8F9>█</color><color=#D2D2D3>█</color><color=#797779>█</color><color=#121112>█</color><color=#B3B1B3>█</color><color=#FFFFFF>███</color><color=#EBE9EB>█</color><color=#696769>█</color><color=#4F4E4F>█</color><color=#DEDDDE>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFFFF>██</color><color=#F0EEF0>█</color><color=#9B9A9B>█</color><color=#616062>█</color><color=#696869>█</color><color=#BEBDBE>█</color><color=#FDFCFE>█</color><color=#FFFFFF>██</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>██████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#BAB8BA>█</color><color=#3C3B3C>█</color><color=#FDFCFD>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color>\n<line-height=3px><color=#FFFDFF>█████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#B2B1B3>█</color><color=#363537>█</color><color=#EEECEE>█</color><color=#FFFFFF>██</color><color=#FFFEFF>██</color><color=#FFFDFF>███</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FDFCFE>██</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#CAC9CA>█</color><color=#5D5B5D>█</color><color=#302F30>█</color><color=#504F50>█</color><color=#9D9B9D>█</color><color=#CFCED0>█</color><color=#E5E3E5>█</color><color=#F5F4F6>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███</color><color=#FFFEFF>█</color><color=#FFFFFF>███</color><color=#8C8B8C>█</color><color=#9E9D9F>█</color><color=#FFFFFF>█</color><color=#FDFCFE>██</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#999899>█</color><color=#323133>█</color><color=#BCBBBC>█</color><color=#FFFFFF>████</color><color=#E3E2E3>█</color><color=#7F7E7F>█</color><color=#5C5A5C>█</color><color=#7D7C7D>█</color><color=#DCDCDC>█</color><color=#FFFFFF>██</color><color=#FFFEFF>█</color><color=#FEFDFE>█</color><color=#FEFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#BAB8BA>█</color><color=#3D3C3D>█</color><color=#FCFBFC>█</color><color=#FFFFFF>██</color>\n<line-height=3px><color=#FFFDFF>█████</color><color=#FFFFFF>█</color><color=#FDFCFD>█</color><color=#545254>█</color><color=#868486>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FFFEFF>██</color><color=#FFFDFF>███</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FEFDFF>███</color><color=#FFFDFF>█</color><color=#FFFFFF>██</color><color=#EEECEE>█</color><color=#D7D6D7>█</color><color=#ABAAAC>█</color><color=#8A898A>█</color><color=#7E7D7E>█</color><color=#908E90>█</color><color=#F9F7F9>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██████</color><color=#FFFEFF>█</color><color=#FFFFFF>██</color><color=#FFFDFF>████</color><color=#FFFFFF>██</color><color=#BFBDBF>█</color><color=#454345>█</color><color=#7E7C7E>█</color><color=#FCFBFC>█</color><color=#FFFFFF>█</color><color=#A3A2A3>█</color><color=#3E3C3E>█</color><color=#949394>█</color><color=#F4F3F4>█</color><color=#FFFFFF>██</color><color=#FFFEFF>█████</color><color=#FFFDFF>█████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#BAB8BA>█</color><color=#3C3B3C>█</color><color=#FCFBFC>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color>\n<line-height=3px><color=#FFFDFF>█████</color><color=#FFFFFF>█</color><color=#E0DFE0>█</color><color=#363436>█</color><color=#D5D4D5>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FFFEFF>██</color><color=#FFFDFF>█████</color><color=#FFFEFF>████</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█████</color><color=#FEFDFE>█</color><color=#FFFDFF>█████████</color><color=#FFFEFF>██████</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#EBEAEB>█</color><color=#414042>█</color><color=#706E70>█</color><color=#6A686A>█</color><color=#525153>█</color><color=#D9D8DA>█</color><color=#FFFFFF>██</color><color=#FFFDFF>████████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#B9B8B9>█</color><color=#3B3A3B>█</color><color=#FCFBFC>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█</color>\n<line-height=3px><color=#FFFDFF>█████</color><color=#FFFFFF>█</color><color=#E0DFE0>█</color><color=#403F40>█</color><color=#E9E7E9>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FFFEFF>██</color><color=#FFFDFF>███████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█████████████████</color><color=#FFFFFF>█</color><color=#DEDDDE>█</color><color=#414042>█</color><color=#706F71>█</color><color=#FCFBFC>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██████████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#BCBBBC>█</color><color=#3A393A>█</color><color=#FCFBFC>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█</color>\n<line-height=3px><color=#FFFDFF>████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#E0DFE0>█</color><color=#3D3B3D>█</color><color=#E6E4E6>█</color><color=#FFFFFF>█</color><color=#FFFEFF>████</color><color=#FFFDFF>███████████████</color><color=#FFFEFF>███</color><color=#FFFDFF>██████████████████</color><color=#FFFFFF>█</color><color=#FDFCFD>█</color><color=#FDFCFE>█</color><color=#FFFFFF>█</color><color=#FFFDFF>████████████████████</color><color=#FFFFFF>█</color><color=#9A999A>█</color><color=#403E40>█</color><color=#FDFBFD>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color>\n<line-height=3px><color=#FFFDFF>████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#DFDEDF>█</color><color=#3B393B>█</color><color=#E6E4E6>█</color><color=#FFFFFF>█</color><color=#FFFDFF>████████████</color><color=#FFFEFF>████</color><color=#FFFDFF>██████████████████████</color><color=#FFFEFF>██</color><color=#FFFDFF>█</color><color=#FFFFFF>██</color><color=#FFFDFF>████████████████████</color><color=#FFFFFF>█</color><color=#F8F6F8>█</color><color=#424143>█</color><color=#A1A0A1>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color>\n<line-height=3px><color=#FFFDFF>████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#DFDEDF>█</color><color=#3B3A3B>█</color><color=#E8E7E8>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>██</color><color=#FFFDFF>████████</color><color=#FFFFFF>██</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFFFF>███</color><color=#FFFDFF>█████████████████</color><color=#FFFFFF>███████████</color><color=#FFFEFF>█</color><color=#FFFDFF>████████████████</color><color=#FFFFFF>█</color><color=#DEDDDE>█</color><color=#2F2D2F>█</color><color=#DDDCDD>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██</color>\n<line-height=3px><color=#FFFDFF>████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#E3E2E3>█</color><color=#353435>█</color><color=#CECDCE>█</color><color=#FFFFFF>█</color><color=#FDFBFD>█</color><color=#FEFDFE>█</color><color=#FEFDFF>█</color><color=#FFFDFF>██████</color><color=#FFFFFF>██</color><color=#CECDCF>█</color><color=#7B797B>█</color><color=#787778>█</color><color=#848385>█</color><color=#6D6C6E>█</color><color=#9E9D9E>█</color><color=#E4E2E4>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██</color><color=#FFFEFF>█</color><color=#FFFDFF>██████████</color><color=#FFFEFF>█</color><color=#FFFFFF>██</color><color=#FCFAFC>█</color><color=#D9D7D9>█</color><color=#A9A8A9>█</color><color=#979697>█</color><color=#89888A>█</color><color=#949394>██</color><color=#8F8E8F>██</color><color=#C2C1C3>█</color><color=#F5F4F5>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>██████████████</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#686668>█</color><color=#605F60>█</color><color=#FCFBFD>█</color><color=#FEFDFF>██</color><color=#FFFDFF>█</color>\n<line-height=3px><color=#FFFDFF>█████</color><color=#FFFEFF>██</color><color=#555455>█</color><color=#878587>█</color><color=#FFFFFF>█</color><color=#FCFBFD>█</color><color=#FDFCFE>█</color><color=#FEFDFF>█</color><color=#FFFDFF>█████</color><color=#FFFFFF>█</color><color=#EFEEEF>█</color><color=#878687>█</color><color=#444344>█</color><color=#A2A1A2>█</color><color=#F1F0F1>█</color><color=#F4F3F5>█</color><color=#DAD9DA>█</color><color=#676668>█</color><color=#706F71>█</color><color=#FAF8FA>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███████████</color><color=#FEFDFF>█</color><color=#F7F6F7>█</color><color=#B1B0B1>█</color><color=#5B595B>█</color><color=#676667>█</color><color=#9D9C9E>█</color><color=#CBCACB>█</color><color=#E0DFE1>█</color><color=#EBE9EB>█</color><color=#E9E7E9>█</color><color=#E2E1E2>█</color><color=#B5B4B5>█</color><color=#767476>█</color><color=#595759>█</color><color=#989798>█</color><color=#FFFFFF>██</color><color=#FFFDFF>█████████████</color><color=#FFFFFF>█</color><color=#B1B0B1>█</color><color=#302E30>█</color><color=#DEDDDF>█</color><color=#FFFFFF>█</color><color=#FDFCFE>█</color><color=#FEFDFF>█</color><color=#FFFDFF>█</color>\n<line-height=3px><color=#FFFDFF>█████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#B7B6B7>█</color><color=#333133>█</color><color=#EEEDEE>█</color><color=#FFFFFF>█</color><color=#FDFCFE>█</color><color=#FEFDFF>█</color><color=#FFFDFF>████</color><color=#FFFFFF>█</color><color=#F0EEF0>█</color><color=#464547>█</color><color=#605F61>█</color><color=#EFEEEF>█</color><color=#FFFFFF>██</color><color=#E1E0E1>█</color><color=#767476>█</color><color=#414041>█</color><color=#222022>█</color><color=#302F30>█</color><color=#9B9A9C>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#C0BFC1>█</color><color=#616062>█</color><color=#CBCACC>█</color><color=#FBFAFC>█</color><color=#FFFFFF>█</color><color=#F4F3F4>█</color><color=#C8C6C8>█</color><color=#C6C5C6>█</color><color=#D9D8DA>█</color><color=#EAE9EA>█</color><color=#FFFFFF>██</color><color=#D3D2D3>█</color><color=#4E4D4E>█</color><color=#6D6C6D>█</color><color=#EDEBED>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█████████</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#E8E7E8>█</color><color=#333233>█</color><color=#A09FA0>█</color><color=#FFFFFF>█</color><color=#FDFCFE>██</color><color=#FEFDFF>█</color><color=#FFFDFF>█</color>\n<line-height=3px><color=#FFFDFF>██████</color><color=#FFFFFF>█</color><color=#F9F8FA>█</color><color=#4B4A4B>█</color><color=#787678>█</color><color=#FDFCFE>█</color><color=#FFFFFF>██</color><color=#FFFEFF>█</color><color=#FFFDFF>██</color><color=#FFFFFF>█</color><color=#EDECED>█</color><color=#4D4C4D>█</color><color=#6C6B6C>█</color><color=#FCFBFC>█</color><color=#FFFFFF>██</color><color=#EBEAEC>█</color><color=#3D3C3E>█</color><color=#070507>█</color><color=#0D0C0E>█</color><color=#100F10>█</color><color=#0A090A>█</color><color=#383638>█</color><color=#FAF8FA>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██████████</color><color=#FFFEFF>█</color><color=#FEFDFF>█</color><color=#FFFFFF>███</color><color=#C2C1C3>█</color><color=#343334>█</color><color=#100F10>█</color><color=#121112>█</color><color=#1B1A1C>█</color><color=#242325>█</color><color=#626162>█</color><color=#F7F6F8>█</color><color=#FFFFFF>█</color><color=#F7F6F8>█</color><color=#848384>█</color><color=#393839>█</color><color=#D2D0D2>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#E5E3E5>█</color><color=#514F51>█</color><color=#6C6B6C>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>████</color>\n<line-height=3px><color=#FFFDFF>█████</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFFFF>█</color><color=#E6E4E6>█</color><color=#494749>█</color><color=#686668>█</color><color=#F9F8F9>█</color><color=#D5D4D5>█</color><color=#F8F6F8>█</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#727173>█</color><color=#4F4E50>█</color><color=#F8F6F8>█</color><color=#FFFFFF>█</color><color=#FDFCFE>█</color><color=#FFFFFF>█</color><color=#A09FA0>█</color><color=#0F0D0F>█</color><color=#151416>█</color><color=#161416>██</color><color=#100E10>█</color><color=#3D3B3D>█</color><color=#FAF8FA>█</color><color=#FFFFFF>█</color><color=#FFFDFF>███████████</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#E0DEE0>█</color><color=#2F2D2F>█</color><color=#0D0C0D>█</color><color=#131214>██</color><color=#121113>█</color><color=#100F11>█</color><color=#0D0C0E>█</color><color=#949394>█</color><color=#FFFFFF>███</color><color=#979697>█</color><color=#2D2C2E>█</color><color=#E3E2E3>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#F9F8F9>█</color><color=#848284>█</color><color=#504F51>█</color><color=#9E9D9F>█</color><color=#F6F5F6>█</color><color=#FFFFFF>███</color><color=#FFFEFF>██</color><color=#FFFDFF>█</color>\n<line-height=3px><color=#FFFDFF>███</color><color=#FFFEFF>█</color><color=#FFFFFF>█████</color><color=#ECEBEC>█</color><color=#6C6B6C>█</color><color=#5B5A5C>█</color><color=#D8D7D8>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#B3B2B3>█</color><color=#302F30>█</color><color=#D6D4D6>█</color><color=#FFFFFF>█</color><color=#FDFCFF>█</color><color=#FDFCFE>█</color><color=#FFFFFF>█</color><color=#8F8E90>█</color><color=#0D0B0D>█</color><color=#151416>█</color><color=#161416>██</color><color=#100E10>█</color><color=#3D3C3E>█</color><color=#FAF8FA>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█████████████</color><color=#FFFFFF>█</color><color=#CBC9CB>█</color><color=#171517>█</color><color=#121113>█</color><color=#141315>████</color><color=#100F11>█</color><color=#302F31>█</color><color=#EAE9EB>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FEFDFF>█</color><color=#5C5A5C>█</color><color=#7A787A>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FAF8FA>█</color><color=#D1D0D1>█</color><color=#7D7C7D>█</color><color=#C8C6C8>█</color><color=#EBEBEB>█</color><color=#E4E3E4>█</color><color=#E1E0E1>█</color><color=#E6E5E7>█</color><color=#F9F7F9>█</color><color=#FFFDFF>███</color>\n<line-height=3px><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FCFBFD>█</color><color=#C6C5C6>█</color><color=#B5B3B5>█</color><color=#B7B5B7>█</color><color=#B2B0B2>█</color><color=#B1B0B1>█</color><color=#B6B5B7>█</color><color=#D1CFD1>█</color><color=#B1B0B1>█</color><color=#E0DFE0>█</color><color=#EAE9EB>█</color><color=#FCFBFC>█</color><color=#FFFFFF>█</color><color=#777678>█</color><color=#727173>█</color><color=#FFFFFF>█</color><color=#FEFDFF>█</color><color=#FDFCFE>█</color><color=#FEFDFE>█</color><color=#FFFFFF>█</color><color=#8A898A>█</color><color=#080608>█</color><color=#161516>█</color><color=#161416>██</color><color=#100E10>█</color><color=#393739>█</color><color=#F9F7F9>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█████████████</color><color=#FFFFFF>█</color><color=#CAC8CA>█</color><color=#161516>█</color><color=#121113>█</color><color=#141315>████</color><color=#131214>█</color><color=#0A090B>█</color><color=#B8B7B9>█</color><color=#FFFFFF>█</color><color=#FEFDFE>█</color><color=#FFFFFF>█</color><color=#929092>█</color><color=#504E50>█</color><color=#FEFDFE>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██</color><color=#FFFFFF>█</color><color=#ECEAEC>█</color><color=#7A797B>█</color><color=#727172>█</color><color=#7A797A>█</color><color=#999899>█</color><color=#A0A0A1>█</color><color=#9F9EA0>█</color><color=#A4A2A4>█</color><color=#A3A2A4>█</color><color=#434243>█</color><color=#5C5A5C>█</color><color=#FCFBFC>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color>\n<line-height=3px><color=#FFFDFF>█</color><color=#FFFEFF>██</color><color=#FEFCFE>█</color><color=#7A797B>█</color><color=#1D1B1D>█</color><color=#949394>█</color><color=#C9C8CA>█</color><color=#C1C0C1>█</color><color=#A4A3A5>█</color><color=#A3A2A3>█</color><color=#7B7A7C>█</color><color=#383739>█</color><color=#8F8E8F>█</color><color=#F4F2F4>█</color><color=#FFFFFF>█</color><color=#777778>█</color><color=#747374>█</color><color=#FFFFFF>█</color><color=#FEFDFF>█</color><color=#FDFCFE>█</color><color=#FEFDFF>█</color><color=#FFFFFF>█</color><color=#C6C4C6>█</color><color=#1D1B1D>█</color><color=#0D0C0E>█</color><color=#151315>██</color><color=#111011>█</color><color=#787678>█</color><color=#FFFFFF>██</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>██████████</color><color=#FFFFFF>█</color><color=#E0DEE0>█</color><color=#282728>█</color><color=#09080A>█</color><color=#131214>█</color><color=#141315>██</color><color=#131214>█</color><color=#141315>█</color><color=#070608>█</color><color=#B6B5B7>█</color><color=#FFFFFF>█</color><color=#FDFCFF>█</color><color=#FFFFFF>█</color><color=#DEDDDF>█</color><color=#8C8B8D>█</color><color=#F5F4F5>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██</color><color=#FFFEFF>█</color><color=#F9F7F9>█</color><color=#D7D6D8>█</color><color=#F9F7F9>█</color><color=#FFFFFF>██</color><color=#FFFEFF>█</color><color=#D6D5D6>█</color><color=#ABAAAC>█</color><color=#7E7D7E>█</color><color=#555455>█</color><color=#BCBBBD>█</color><color=#FFFFFF>█</color><color=#FDFCFE>█</color><color=#FEFCFE>█</color>\n<line-height=3px><color=#FFFDFF>███</color><color=#FFFFFF>██</color><color=#AAA9AA>█</color><color=#4B4A4B>█</color><color=#8E8C8E>█</color><color=#FCFAFC>█</color><color=#FFFFFF>██</color><color=#FAF9FB>█</color><color=#B8B7B9>█</color><color=#F9F8F9>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#969597>█</color><color=#949294>█</color><color=#FFFFFF>█</color><color=#FEFDFF>█</color><color=#FDFCFF>█</color><color=#FDFDFF>█</color><color=#FEFDFF>█</color><color=#FFFFFF>█</color><color=#A09FA1>█</color><color=#3D3C3E>█</color><color=#131213>█</color><color=#0A090A>█</color><color=#4A484A>█</color><color=#B8B7B9>█</color><color=#CAC8CA>█</color><color=#CCCACC>█</color><color=#F9F7F9>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#CFCED0>█</color><color=#585658>█</color><color=#100F10>█</color><color=#0B0A0C>█</color><color=#0A090B>█</color><color=#0B0A0D>██</color><color=#4A484B>█</color><color=#E8E6E8>█</color><color=#FFFFFF>█</color><color=#FEFDFE>█</color><color=#FEFCFE>█</color><color=#FFFFFF>██</color><color=#FFFEFF>█</color><color=#FFFDFF>█████</color><color=#FDFBFD>█</color><color=#E7E5E7>█</color><color=#A5A4A5>█</color><color=#787778>█</color><color=#727172>█</color><color=#6E6D6E>█</color><color=#A2A1A3>█</color><color=#CFCDCF>█</color><color=#F9F8F9>█</color><color=#FFFFFF>█</color><color=#FEFDFE>██</color><color=#FEFDFF>█</color>\n<line-height=3px><color=#FFFDFF>████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#E6E5E7>█</color><color=#797879>█</color><color=#4E4D4E>█</color><color=#A09FA0>█</color><color=#F2F1F2>█</color><color=#FFFFFF>██</color><color=#FEFEFF>█</color><color=#FFFFFF>████</color><color=#FFFEFF>█</color><color=#FEFFFF>█</color><color=#FFFFFF>██</color><color=#FDFCFE>█</color><color=#FEFDFF>█</color><color=#FFFFFF>█</color><color=#EEEDEF>█</color><color=#605F60>█</color><color=#262526>█</color><color=#222122>█</color><color=#1E1D1E>█</color><color=#323132>█</color><color=#868587>█</color><color=#F6F5F7>█</color><color=#FEFDFF>█</color><color=#FDFCFE>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#F9F7F9>█</color><color=#C4C3C4>█</color><color=#8E8D8F>█</color><color=#7E7D7F>█</color><color=#7C7B7F>█</color><color=#97979D>█</color><color=#F1F4F9>█</color><color=#FFFFFF>█</color><color=#FAF2F3>█</color><color=#FDF9F9>█</color><color=#FFFDFF>██</color><color=#FFFFFF>███</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#B3B1B3>█</color><color=#343335>█</color><color=#8F8E8F>█</color><color=#D3D2D3>█</color><color=#F5F4F5>█</color><color=#FFFFFF>████</color><color=#FFFDFF>█</color><color=#FFFEFF>███</color>\n<line-height=3px><color=#FFFDFF>██████</color><color=#FFFFFF>██</color><color=#D7D6D7>█</color><color=#7A797B>█</color><color=#616062>█</color><color=#6E6D6E>█</color><color=#E4E3E3>█</color><color=#FFF6F4>█</color><color=#F1C4C6>█</color><color=#EBA7A4>█</color><color=#E09393>█</color><color=#E5BBBE>█</color><color=#FEFDFF>█</color><color=#FAE8EE>█</color><color=#EAB4B7>█</color><color=#F2D3D4>█</color><color=#FFFFFF>█</color><color=#FFFDFE>█</color><color=#FEFDFE>█</color><color=#FFFEFF>█</color><color=#FEFDFE>█</color><color=#E9E8E9>█</color><color=#DCDBDD>█</color><color=#DDDCDE>█</color><color=#F1F0F1>█</color><color=#FFFFFF>█</color><color=#FEFDFF>█</color><color=#FDFCFE>██</color><color=#FEFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█████████</color><color=#FFFEFF>██</color><color=#FFFFFF>█████</color><color=#FFF7F2>█</color><color=#ECB7B4>█</color><color=#DA726E>█</color><color=#DD5759>█</color><color=#F2B7B7>█</color><color=#FFFFFF>█</color><color=#F8F8F8>█</color><color=#F4E6E2>█</color><color=#EBCCC3>█</color><color=#FCF8F2>█</color><color=#FFFFFE>█</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#D2D1D3>█</color><color=#5B5A5B>█</color><color=#595759>█</color><color=#E8E7E9>█</color><color=#FFFFFF>█</color><color=#FDFCFD>█</color><color=#FDFCFE>█</color><color=#FEFDFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███</color>\n<line-height=3px><color=#FFFDFF>███████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█</color><color=#555455>█</color><color=#5D5C5D>█</color><color=#F9F8F9>█</color><color=#F7DEDB>█</color><color=#E39893>█</color><color=#EEABA7>█</color><color=#D14E4D>█</color><color=#D94644>█</color><color=#E77F82>█</color><color=#DC7378>█</color><color=#DE8287>█</color><color=#F5D3D6>█</color><color=#FFFFFF>█</color><color=#FFFEFE>█</color><color=#FFFEFF>█</color><color=#FEFDFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>████</color><color=#FEFDFF>████</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█████</color><color=#FFFEFF>█</color><color=#FFFDFF>███</color><color=#FFFEFF>█████</color><color=#FFFFFF>█</color><color=#FFF9F6>█</color><color=#D78A8D>█</color><color=#DC7C7E>█</color><color=#F5C3BE>█</color><color=#DA696C>█</color><color=#E06468>█</color><color=#E28B8C>█</color><color=#DA7A7E>█</color><color=#DE8184>█</color><color=#DB8D89>█</color><color=#FCF1EE>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFFFF>█</color><color=#FDFCFD>█</color><color=#8A898B>█</color><color=#3A383A>█</color><color=#BBB9BB>█</color><color=#FFFFFF>█</color><color=#FFFDFF>███████</color>\n<line-height=3px><color=#FFFDFF>███████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#69686A>█</color><color=#515051>█</color><color=#F7F6F8>█</color><color=#FFFFFF>███</color><color=#FFFEFE>█</color><color=#E9B8B9>█</color><color=#ECAAAB>█</color><color=#F7D6D6>█</color><color=#FDF8F4>█</color><color=#FFFFFF>██</color><color=#FFFEFD>█</color><color=#FFFFFE>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>███████</color><color=#FFFDFF>█</color><color=#FFFFFF>████</color><color=#FFFEFF>███</color><color=#FFFFFF>███</color><color=#FFFDFF>███████</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FBF5F9>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#ECCACD>█</color><color=#EAA8AE>█</color><color=#F5CCCD>█</color><color=#FDEDEF>█</color><color=#FFFCFF>█</color><color=#FFFFFF>█</color><color=#FFFEFF>███</color><color=#FFFFFF>█████</color><color=#BBBABB>█</color><color=#2F2E2F>█</color><color=#CFCDCF>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██████</color>\n<line-height=3px><color=#FFFDFF>██████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#B3B1B3>█</color><color=#2B292B>█</color><color=#E2E1E2>█</color><color=#FFFFFF>████████</color><color=#FEFFFF>██</color><color=#FEFEFE>█</color><color=#FFFEFF>███</color><color=#FFFDFF>████</color><color=#FFFEFF>█</color><color=#FFFFFF>████</color><color=#E3E2E4>█</color><color=#B1B0B1>█</color><color=#CDCCCD>█</color><color=#DBD9DB>█</color><color=#FAF9FA>█</color><color=#FFFFFF>██</color><color=#F8F7F9>█</color><color=#979698>█</color><color=#D2D1D3>█</color><color=#FFFFFF>█</color><color=#FFFDFF>████████</color><color=#FFFEFF>██</color><color=#FFFDFF>█</color><color=#FFFFFF>███████</color><color=#F5F3F5>█</color><color=#FDFCFD>█</color><color=#D5D3D5>█</color><color=#A4A3A5>█</color><color=#A8A7A9>█</color><color=#9A999B>█</color><color=#8F8E8F>█</color><color=#838284>█</color><color=#4E4D4F>█</color><color=#D6D5D7>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██████</color>\n<line-height=3px><color=#FFFDFF>██████</color><color=#FFFFFF>█</color><color=#F7F5F7>█</color><color=#383739>█</color><color=#6A686A>█</color><color=#DCDADC>█</color><color=#D2D0D2>█</color><color=#CECDCF>█</color><color=#B4B3B3>█</color><color=#B9B8B8>█</color><color=#B6B4B6>█</color><color=#D4D2D4>█</color><color=#FFFDFF>█</color><color=#FFFFFF>███</color><color=#FFFEFF>████</color><color=#FFFDFF>███</color><color=#FFFEFF>█</color><color=#F8F6F8>█</color><color=#D3D2D4>█</color><color=#D2D1D3>█</color><color=#D1D0D2>█</color><color=#9D9C9D>█</color><color=#464446>█</color><color=#8D8B8D>█</color><color=#B1B0B1>█</color><color=#6C6B6C>█</color><color=#6E6D6E>█</color><color=#8B8A8B>█</color><color=#89888A>█</color><color=#717072>█</color><color=#5F5E60>█</color><color=#D1D0D1>█</color><color=#FFFFFF>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█████</color><color=#FFFEFF>██</color><color=#FFFFFF>█████</color><color=#E8E7EA>█</color><color=#C5C4C7>█</color><color=#979597>█</color><color=#6E6D6E>█</color><color=#6F6E70>█</color><color=#C8C7C8>█</color><color=#E9E9E9>█</color><color=#C8C7C8>█</color><color=#C9C8C9>█</color><color=#C7C6C7>█</color><color=#D7D6D8>█</color><color=#E3E2E4>█</color><color=#F8F7F8>█</color><color=#FFFFFF>█</color><color=#FFFDFF>███████</color>\n<line-height=3px><color=#FFFDFF>██████</color><color=#FFFFFF>█</color><color=#F1F0F1>█</color><color=#646364>█</color><color=#6F6D6F>█</color><color=#A2A1A3>█</color><color=#A09FA0>█</color><color=#9D9C9E>█</color><color=#A4A2A4>█</color><color=#C1BFC1>█</color><color=#A6A5A6>█</color><color=#676668>█</color><color=#858485>█</color><color=#7E7D7F>█</color><color=#B2B0B3>█</color><color=#E2E0E2>█</color><color=#FCFAFC>█</color><color=#FFFFFF>████</color><color=#FFFDFF>██</color><color=#FFFFFF>█</color><color=#F0EEF0>█</color><color=#A7A6A7>█</color><color=#A4A3A5>█</color><color=#A09FA1>█</color><color=#9B999B>█</color><color=#D9D8DA>█</color><color=#FFFFFF>███</color><color=#F2F1F3>█</color><color=#EFEEEF>██</color><color=#F3F2F4>█</color><color=#FFFFFF>██</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█████</color><color=#FFFFFF>███</color><color=#F4F2F4>█</color><color=#E3E1E3>█</color><color=#D2D1D2>█</color><color=#A8A7A8>█</color><color=#7D7C7F>█</color><color=#565559>█</color><color=#878689>█</color><color=#B2B1B2>█</color><color=#DDDCDD>█</color><color=#FBFAFB>█</color><color=#FFFFFF>████████</color><color=#FFFEFF>█</color><color=#FFFDFF>███████</color>\n<line-height=3px><color=#FFFDFF>██████</color><color=#FFFEFF>█</color><color=#FFFFFF>█████████</color><color=#FFFEFF>█</color><color=#B3B2B3>█</color><color=#858485>█</color><color=#6F6E70>█</color><color=#686768>█</color><color=#817F81>█</color><color=#838284>█</color><color=#8D8C8D>█</color><color=#BCBBBC>█</color><color=#E8E6E8>█</color><color=#FFFFFF>█████████</color><color=#FEFDFF>█</color><color=#FDFCFE>█</color><color=#FEFDFF>█</color><color=#FFFEFF>████</color><color=#FEFDFF>██</color><color=#FFFDFF>██████</color><color=#FFFFFF>█</color><color=#F4F2F4>█</color><color=#BEBDBE>█</color><color=#797779>█</color><color=#717072>█</color><color=#777577>█</color><color=#888789>█</color><color=#9C9A9C>█</color><color=#C8C6C8>█</color><color=#F0EFF0>█</color><color=#FFFFFF>█████████</color><color=#FFFEFF>█</color><color=#FFFDFF>██████████</color>\n<line-height=3px><color=#FFFDFF>███████</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#FFFEFF>█████</color><color=#FFFFFF>██</color><color=#636264>█</color><color=#151415>█</color><color=#646365>█</color><color=#747374>█</color><color=#7E7C7E>█</color><color=#919092>█</color><color=#787678>█</color><color=#595859>█</color><color=#807E80>█</color><color=#B7B5B7>█</color><color=#B6B4B6>█</color><color=#CCCBCD>█</color><color=#E0DFE0>█</color><color=#F5F3F5>█</color><color=#FEFDFF>███</color><color=#FDFCFE>█</color><color=#FEFDFF>█</color><color=#FFFDFF>██████████████</color><color=#FFFFFF>█</color><color=#EAE9EA>█</color><color=#484748>█</color><color=#4A484A>█</color><color=#E5E4E6>█</color><color=#FFFFFF>█████</color><color=#FFFEFF>██████████</color><color=#FFFDFF>██████████</color>\n<line-height=3px><color=#FFFDFF>████████████████</color><color=#FFFFFF>█</color><color=#F9F8F9>█</color><color=#807F80>█</color><color=#4D4C4E>█</color><color=#ADACAD>█</color><color=#D0CFD0>█</color><color=#B7B6B7>█</color><color=#B8B7B9>█</color><color=#BBB9BB>█</color><color=#B6B5B7>█</color><color=#B2B1B2>█</color><color=#9F9E9F>█</color><color=#8B898B>█</color><color=#777577>█</color><color=#B8B7B9>█</color><color=#FFFFFF>█</color><color=#FEFCFE>█</color><color=#FDFCFF>█</color><color=#FDFCFE>█</color><color=#FFFDFF>█</color><color=#FFFEFF>████████</color><color=#FFFDFF>███████</color><color=#FFFFFF>█</color><color=#F1F0F1>█</color><color=#818082>█</color><color=#5F5D5F>█</color><color=#838183>█</color><color=#EAE9EA>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>█████████████████████</color>\n<line-height=3px><color=#FFFDFF>█████████████████</color><color=#FFFFFF>██</color><color=#C2C1C3>█</color><color=#484749>█</color><color=#7B7A7B>█</color><color=#EEEDEE>█</color><color=#FFFFFF>████████</color><color=#FEFDFF>█</color><color=#FDFCFE>█</color><color=#FEFCFE>█</color><color=#FDFCFE>█</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███████████████</color><color=#FFFFFF>██</color><color=#F3F2F3>█</color><color=#7A787A>█</color><color=#474647>█</color><color=#AFADAF>█</color><color=#FFFFFF>██</color><color=#FFFDFF>████████████████████</color>\n<line-height=3px><color=#FFFDFF>██████████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#F0EFF0>█</color><color=#888688>█</color><color=#5D5B5D>█</color><color=#838384>█</color><color=#CBCACB>█</color><color=#F6F5F6>█</color><color=#FFFFFF>█</color><color=#FFFEFF>████</color><color=#FFFDFF>█████</color><color=#FFFEFF>█</color><color=#FFFDFF>███████████████</color><color=#FFFEFF>██</color><color=#FFFFFF>██</color><color=#CFCECF>█</color><color=#333234>█</color><color=#8C8B8C>█</color><color=#FFFFFF>██</color><color=#FFFDFF>███████████████████</color>\n<line-height=3px><color=#FFFDFF>██████████████████</color><color=#FFFEFF>█</color><color=#FFFDFF>█</color><color=#FFFFFF>█</color><color=#FDFCFD>█</color><color=#838284>█</color><color=#212022>█</color><color=#666566>█</color><color=#E0DFE0>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███</color><color=#FFFEFF>████</color><color=#FFFDFF>████████████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#DBDADC>█</color><color=#4E4C4E>█</color><color=#646364>█</color><color=#F0EFF1>█</color><color=#FFFFFF>█</color><color=#FFFDFF>██████████████████</color>\n<line-height=3px><color=#FFFDFF>███████████████████</color><color=#FFFFFF>█</color><color=#EDECEE>█</color><color=#4F4E4F>█</color><color=#716F71>█</color><color=#E3E2E3>█</color><color=#FFFFFF>█████</color><color=#FFFDFF>████████████████████████████</color><color=#FFFFFF>█</color><color=#F6F5F6>█</color><color=#4D4C4E>█</color><color=#676668>█</color><color=#FFFFFF>██</color><color=#FFFDFF>█████████████████</color>\n<line-height=3px><color=#FFFDFF>█████████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#CAC9CA>█</color><color=#2E2D2F>█</color><color=#4F4E4F>█</color><color=#C3C2C4>█</color><color=#CDCBCD>█</color><color=#C6C5C6>█</color><color=#ACABAC>█</color><color=#A8A7A8>█</color><color=#AFAEB0>█</color><color=#F2F0F2>█</color><color=#FFFFFF>█</color><color=#FFFDFF>████████████████████████████</color><color=#FFFFFF>█</color><color=#E4E3E4>█</color><color=#484748>█</color><color=#7D7C7E>█</color><color=#FFFDFF>█</color><color=#FFFFFF>█</color><color=#FFFDFF>████████████████</color>\n<line-height=3px><color=#FFFDFF>█████████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#737274>█</color><color=#515052>█</color><color=#A7A6A8>█</color><color=#B4B3B4>█</color><color=#B0AFB1>█</color><color=#B0AFB0>█</color><color=#AEACAE>█</color><color=#CECCCE>█</color><color=#717072>█</color><color=#A9A7A9>█</color><color=#FFFFFF>█</color><color=#FFFDFF>███████████████████████████</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#EBEAEB>█</color><color=#3C3B3C>█</color><color=#868586>█</color><color=#FFFFFF>█</color><color=#FFFEFF>██</color><color=#FFFDFF>██████████████</color>\n<line-height=3px><color=#FFFDFF>█████████████████</color><color=#FFFEFF>█</color><color=#FEFDFE>█</color><color=#FAF8FA>█</color><color=#FFFFFF>███████</color><color=#797779>█</color><color=#848385>█</color><color=#FFFFFF>█</color><color=#FFFDFF>███████████████████████████</color><color=#FFFEFF>███</color><color=#FFFFFF>█</color><color=#CCCBCC>█</color><color=#1E1C1E>█</color><color=#CBCACB>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>██████████████</color>\n<line-height=3px><color=#FFFDFF>██████████████████</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>████</color><color=#FFFFFF>█</color><color=#FEFCFE>█</color><color=#4D4C4D>█</color><color=#979698>█</color><color=#FFFFFF>█</color><color=#FFFDFF>███████████████████████████</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>█</color><color=#727172>█</color><color=#A7A5A7>█</color><color=#FFFFFF>█</color><color=#FFFDFF>███████████████</color>\n<line-height=3px><color=#FFFDFF>█████████████████████████</color><color=#FFFEFF>█</color><color=#FCFAFC>█</color><color=#CCCACC>█</color><color=#F5F3F5>█</color><color=#FFFFFF>█</color><color=#FFFDFF>███████████████████████████</color><color=#FFFEFF>██</color><color=#FFFFFF>█</color><color=#FFFEFF>█</color><color=#FFFFFF>██</color><color=#FFFDFF>█</color><color=#FFFEFF>█</color><color=#FFFDFF>███████████████</color></size></scale>\n<line-height=24px><size=24><voffset=-32px>Non è cambiato niente UwU</size>", 5f);
            });
        }

        // SIZE MANAGING FOR CALLBACKS
        public static void ChangeSizeX(Player player, float amount)
        {
            player.Scale = new Vector3(player.Scale.x + amount, player.Scale.y, player.Scale.z);
        }
        public static void ChangeSizeY(Player player, float amount)
        {
            player.Scale = new Vector3(player.Scale.x, player.Scale.y + amount, player.Scale.z);
        }
        public static void ChangeSizeZ(Player player, float amount)
        {
            player.Scale = new Vector3(player.Scale.x, player.Scale.y, player.Scale.z + amount);
        }
        public static void LimitSizeOnDrinks(Player player)
        {
            // MAX
            if (player.Scale.x > SCP294.Instance.Config.MaxSizeFromDrink.x)
            {
                player.Scale = new Vector3(SCP294.Instance.Config.MaxSizeFromDrink.x, player.Scale.y, player.Scale.z);
            }
            if (player.Scale.y > SCP294.Instance.Config.MaxSizeFromDrink.y)
            {
                player.Scale = new Vector3(player.Scale.x, SCP294.Instance.Config.MaxSizeFromDrink.y, player.Scale.z);
            }
            if (player.Scale.z > SCP294.Instance.Config.MaxSizeFromDrink.z)
            {
                player.Scale = new Vector3(player.Scale.x, player.Scale.y, SCP294.Instance.Config.MaxSizeFromDrink.z);
            }

            // MIN
            if (player.Scale.x < SCP294.Instance.Config.MinSizeFromDrink.x)
            {
                player.Scale = new Vector3(SCP294.Instance.Config.MinSizeFromDrink.x, player.Scale.y, player.Scale.z);
            }
            if (player.Scale.y < SCP294.Instance.Config.MinSizeFromDrink.y)
            {
                player.Scale = new Vector3(player.Scale.x, SCP294.Instance.Config.MinSizeFromDrink.y, player.Scale.z);
            }
            if (player.Scale.z < SCP294.Instance.Config.MinSizeFromDrink.z)
            {
                player.Scale = new Vector3(player.Scale.x, player.Scale.y, SCP294.Instance.Config.MinSizeFromDrink.z);
            }

            // VOICE
            SCP294.Instance.PlayerVoicePitch[player.UserId] = Mathf.Clamp((1 - player.Scale.y) + 1f, 0.1f, 2f);
        }

        public static void Helium(Player player, float delay = 90f)
        {
            SCP294.Instance.PlayerVoicePitch[player.UserId] = 1.5f;
            Timing.CallDelayed(delay, () =>
            {
                // VOICE
                SCP294.Instance.PlayerVoicePitch[player.UserId] = Mathf.Clamp((1 - player.Scale.y) + 1f, 0.1f, 2f);
            });
        }
        public static void Sulfur(Player player)
        {
            SCP294.Instance.PlayerVoicePitch[player.UserId] = 0.5f;
            Timing.CallDelayed(90f, () =>
            {
                // VOICE
                SCP294.Instance.PlayerVoicePitch[player.UserId] = Mathf.Clamp((1 - player.Scale.y) + 1f, 0.1f, 2f);
            });
        }
        public static void DeadEye(Player player)
        {
            SummonedCustomItem revolver = SummonedCustomItem.Summon(new Revolver(), player);
            Timing.CallDelayed(0.3f, () =>
            {
                player.CurrentItem = Item.Get(revolver.Serial);
            });

            List<Player> players = DistanceUtils.getAllPlayersInRange(player.Position, 16f);
            players.ForEach(p =>
            {
                if (p.DisplayNickname != player.DisplayNickname)
                {
                    Log.Debug(p.DisplayNickname);
                    p.ShowHint("I guess we got to pay for our sins");
                    p.EnableEffect(EffectType.SinkHole, 5f);
                }
            });
            SoundHandler.PlayAudio("deadeye.ogg", 89, false, "deadeye", player.Position, 5f, player);
            Timing.CallDelayed(5f, () =>
            {
                if (Utilities.TryGetSummonedCustomItem(revolver.Serial, out SummonedCustomItem Summoned))
                {
                    Summoned.Destroy();
                }
            });
        }
        public static void Juggernog(Player player)
        {
            SoundHandler.PlayAudio("juggernog.ogg", 50, false, "juggernog", player.Position, 5f, player);
            player.MaxHealth = (player.MaxHealth + 25);
            player.Heal(150);
            if (player.MaxHealth > 150)
            {
                player.MaxHealth = 150;
            }
        }
        public static void Maxammo(Player player)
        {
            SoundHandler.PlayAudio("maxammo.ogg", 50, false, "maxammo", player.Position, 5f, player);
            player.AddAmmo(AmmoType.Nato556, (ushort)player.GetAmmoLimit(AmmoType.Nato556));
            player.AddAmmo(AmmoType.Nato762, (ushort)player.GetAmmoLimit(AmmoType.Nato762));
            player.AddAmmo(AmmoType.Nato9, (ushort)player.GetAmmoLimit(AmmoType.Nato9));
            player.AddAmmo(AmmoType.Ammo44Cal, (ushort)player.GetAmmoLimit(AmmoType.Ammo44Cal));
            player.AddAmmo(AmmoType.Ammo12Gauge, (ushort)player.GetAmmoLimit(AmmoType.Ammo12Gauge));
        }
    }
}