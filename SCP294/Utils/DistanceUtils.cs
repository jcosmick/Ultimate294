using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            foreach (var player in Player.GetPlayers())
            {
                if (player.IsAlive)
                {
                    playersInRange.Add(player);
                }
            }
            Log.Debug(playersInRange.ToArray().ToString());
            return playersInRange;
        }
    }
}
