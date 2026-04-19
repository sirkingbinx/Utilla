using BepInEx;
using System;
using System.Linq;
using Utilla.Models;

namespace Utilla
{
    public class PluginInfo
    {
        // Hi - ghosty
        public const string Name = "Utilla";
        public const string GUID = "org.legoandmars.gorillatag.utilla"; // this used to be com.*, my bad
        public const string Version = "1.6.29";

        public const string VersionURL = "https://github.com/sirkingbinx/Utilla/blob/master/Version.txt?raw=true";
        public const string InfoURL = "https://raw.githubusercontent.com/developer9998/Utilla-Info/main";

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
