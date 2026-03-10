using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;
using Utilla.Behaviours;

namespace Utilla
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    internal class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger;

        public Plugin()
        {
            Logger = base.Logger;

            DontDestroyOnLoad(this);

            Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, PluginInfo.GUID);
            Events.GameInitialized += OnGameInitialized;
        }

        public void OnGameInitialized(object sender, EventArgs args)
        {
            DontDestroyOnLoad(new GameObject($"{PluginInfo.Name} {PluginInfo.Version}", typeof(UtillaNetworkController), typeof(GamemodeManager), typeof(ConductBoardManager)));
        }
    }
}
