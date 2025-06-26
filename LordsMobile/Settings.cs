using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LordsMobile
{
    public class Settings
    {
        public static string namePrefix = "";
        public static string guildName = "";
        public static int duration = 5; // Em minutos
        public static string hiveCoordK = "";
        public static string hiveCoordX = "";
        public static string hiveCoordY = "";
        public static int maxVMs = 0;
        public static int maxGrunts = 0;
        public static int maxArchers = 0;
        public static int maxCataphracts = 0;
        public static int maxBallistas = 0;
        public static string army_limit = "";

        public Thread Threads { get; set; }
        public string MemuInstance { get; set; }
        public string AccountName { get; set; }
    }

    class VmProfile
    {
        public string VmName { get; set; }
        public string PlayerName { get; set; }
        public bool Enabled { get; set; }
    }
    public class BotConfigs
    {
        public bool AutoShield { get; set; }
        public int TimeGuildGifts { get; set; } = 5;
    }

    public class Troops
    {
        public List<int> MaxTroops { get; set; }
        public bool TrainT1 { get; set; }
    }
}
