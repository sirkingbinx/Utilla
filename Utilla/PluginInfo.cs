using BepInEx;
using System;
using System.Linq;
using Utilla.Models;

namespace Utilla
{
    public class PluginInfo
    {
        public const string Name = "Utilla";
        public const string GUID = "org.legoandmars.gorillatag.utilla"; // this used to be com.*, my bad
        public const string Version = "1.6.28";

        public const string UtillaRepoURL = "https://raw.githubusercontent.com/sirkingbinx/Utilla/refs/heads/master/Version.txt";



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
