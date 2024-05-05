
using Exiled.API.Features;
using System.Collections.Generic;
using UnityEngine;

namespace SCP294.Utils
{
    internal class DistanceUtils
    {
        public static bool isInRange(Vector3 pos1, Vector3 pos2, float range)
        {
            return Vector3.Distance(pos1, pos2) < range;
        }

        public static List<Player> getAllPlayersInRange(Vector3 center, float range)
        {
            List<Player> playersInRange = new List<Player>();
            foreach (var player in PluginAPI.Core.Player.GetPlayers())
            {
                if (player.IsAlive && player.Role != PlayerRoles.RoleTypeId.Tutorial && player.Role != PlayerRoles.RoleTypeId.Spectator)
                {
                    if (isInRange(center, player.Position, range))
                    {
                        playersInRange.Add(player);
                    }

                }
            }
            Log.Debug(playersInRange.ToArray().ToString());
            return playersInRange;
        }
    }
}
