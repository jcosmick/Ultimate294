using Exiled.Events.EventArgs.Map;
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
using UncomplicatedCustomItems.API.Features;

namespace SCP294.CustomItems
{
    public class ProGR : ICustomItem
    {
        public uint Id { get; set; } = 38;

        public string Name { get; set; } = "Una granata lenta ma letale";

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
            PinPullTime = 0.1f,
            FuseTime = 6,
            MaxRadius = 22,
            Repickable = false,
            ScpDamageMultiplier = 1.5f,
        };

        public static void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            if (ev.Projectile.FuseTime == 6)
            {
                SoundHandler.PlayAudio("kaboom.ogg", 50, false, "kaboom", ev.Position, 3f);
            }

        }

    }
}
