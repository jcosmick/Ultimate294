using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using MapEditorReborn.API.Features.Objects;
using MEC;
using SCP294.Classes;
using SCP294.CustomItems;
using SCP294.Types;
using System;
using System.Collections.Generic;
using UncomplicatedCustomItems.API;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace SCP294
{
    public class SCP294 : Plugin<Config.Config>
    {
        public override string Name => "Ultimate294";
        public override string Author => "creepycats & jcosmick & ThunderMatt";
        public override Version Version => new Version(1, 3, 0);

        public override PluginPriority Priority => PluginPriority.Highest;

        public static SCP294 Instance { get; set; }

        public Dictionary<SchematicObject, bool> SpawnedSCP294s { get; set; } = new Dictionary<SchematicObject, bool>();
        public Dictionary<SchematicObject, int> SCP294UsesLeft { get; set; } = new Dictionary<SchematicObject, int>();
        public Dictionary<SchematicObject, LightSourceObject> SCP294LightSources { get; set; } = new Dictionary<SchematicObject, LightSourceObject>();
        public List<string> PlayersNear294 { get; set; } = new List<string>();
        public Dictionary<ushort, DrinkInfo> CustomDrinkItems = new Dictionary<ushort, DrinkInfo>();
        public DrinkManager DrinkManager = new DrinkManager();
        public RarityManager RarityManager = new RarityManager();
        public Dictionary<string, float> PlayerVoicePitch = new Dictionary<string, float>();

        private Harmony _harmony;

        private CoroutineHandle hintCoroutine;

        public Dictionary<ReferenceHub, OpusComponent> Encoders = new Dictionary<ReferenceHub, OpusComponent>();

        public override void OnEnabled()
        {
            Instance = this;
            Log.Info($"{Name} v{Version} - made by creepycats");
            if (Config.Debug)
                Log.Info("Registering events...");
            RegisterEvents();
            Manager.Register(new Revolver());

            DrinkManager.LoadBaseDrinks();

            hintCoroutine = Timing.RunCoroutine(SCP294Object.Handle294Hint());

            _harmony = new Harmony("SCP294");
            _harmony.PatchAll();

            Log.Info("Plugin Enabled!");
        }
        public override void OnDisabled()
        {
            if (Config.Debug)
                Log.Info("Unregistering events...");
            UnregisterEvents();

            DrinkManager.UnloadAllDrinks();

            Timing.KillCoroutines(hintCoroutine);

            _harmony.UnpatchAll();
            _harmony = null;

            Log.Info("Disabled Plugin Successfully");
        }

        // NotesToSelf
        // OBJECT.EVENT += FUNCTION > Add Function to Callback
        // OBJECT.EVENT -= FUNCTION > Remove Function from Callback

        private handlers.serverHandler ServerHandler;
        private handlers.playerHandler PlayerHandler;

        public void RegisterEvents()
        {
            ServerHandler = new handlers.serverHandler();
            PlayerHandler = new handlers.playerHandler();

            Server.RoundStarted += ServerHandler.WaitingForPlayers;

            Player.ChangingItem += PlayerHandler.ChangingItem;
            Player.UsedItem += PlayerHandler.UsedItem;
            Player.Joined += PlayerHandler.Joined;
            Player.FlippingCoin += PlayerHandler.OnCoinFlip;
        }
        public void UnregisterEvents()
        {
            Server.RoundStarted -= ServerHandler.WaitingForPlayers;

            Player.ChangingItem -= PlayerHandler.ChangingItem;
            Player.UsedItem -= PlayerHandler.UsedItem;
            Player.Joined -= PlayerHandler.Joined;
            Player.FlippingCoin -= PlayerHandler.OnCoinFlip;
        }
    }
}