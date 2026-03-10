using BepInEx;
using System;
using System.Linq;
using Utilla.Models;

namespace Utilla
{
    public class PluginInfo
    {
        public const string Name = "Utilla";
        public const string GUID = "com.legoandmars.gorillatag.utilla";
        public const string Version = "1.6.28";

        public const string UtillaRepoURL = "https://github.com/sirkingbinx/Utilla";



        public BaseUnityPlugin Plugin { get; set; }
        public Gamemode[] Gamemodes { get; set; }
        public Action<string> OnGamemodeJoin { get; set; }
        public Action<string> OnGamemodeLeave { get; set; }

        public override string ToString()
        {
            return $"{Plugin.Info.Metadata.Name} [{string.Join(", ", Gamemodes.Select(x => x.DisplayName))}]";
        }
    }
}
