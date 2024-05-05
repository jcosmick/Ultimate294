using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UncomplicatedCustomItems.Elements.SpecificData;
using UncomplicatedCustomItems.Elements;
using UncomplicatedCustomItems.Interfaces.SpecificData;
using UncomplicatedCustomItems.Interfaces;
using UnityEngine;
using Exiled.Events.EventArgs.Player;
using UncomplicatedCustomItems.API.Features;
using UncomplicatedCustomItems.API;
using Exiled.Events.EventArgs.Map;
using Exiled.CustomItems.API.Features;
using SCP294.handlers;
using Exiled.API.Enums;
using Exiled.API.Features.Components;
using Exiled.API.Features.Pickups.Projectiles;
using PluginAPI.Core;

namespace SCP294.CustomItems
{
    public class FakeGR : ICustomItem
    {
        public uint Id { get; set; } = 37;

        public string Name { get; set; } = "Una granata finta";

        public string Description { get; set; } = "";

        public float Weight { get; set; } = 2f;

        public ISpawn Spawn { get; set; } = new Spawn()
        {
            DoSpawn = false,
        };

        public ItemType Item { get; set; } = ItemType.GrenadeHE;

        public CustomItemType CustomItemType { get; set; } = CustomItemType.ExplosiveGrenade;

        public Vector3 Scale { get; set; } = Vector3.one;

        public IData CustomData { get; set; } = new ExplosiveGrenadeData()
        {
            DeafenDuration = 0,
            BurnDuration = 0,
            ConcussDuration = 0,
            PinPullTime = 0.5f,
            FuseTime = 3,
            MaxRadius = 0,
            Repickable = false,
            ScpDamageMultiplier = 1,
        };

        public static void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            if (ev.Projectile.IsActive)
            {
                Log.Debug("xd");
                SoundHandler.PlayAudio("pirots.ogg", 50, false, "troll", ev.Position, 3f);
            }
                
        }

    }
}
