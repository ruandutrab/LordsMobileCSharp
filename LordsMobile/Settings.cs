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

        public static class Bot
        {
            public static DateTime TimeToCheckQuests { get; set; }
        }

        public static class Troops
        {
            public static bool TrainingTroopsT1 { get; set; } = true;
            public static bool TrainingTroopsT2 { get; set; }
            public static bool TrainingTroopsT3 { get; set; }
            public static bool TrainingTroopsT4 { get; set; }
            public static bool TrainingTroopsT5 { get; set; }
            public static string Troop { get; set; } = "Arch";

        }
    }

    class VmProfile
    {
        public string PlayerName { get; set; }
        public string VmName { get; set; }
        public int VmIdProcess { get; set; }
        public int VmIndex { get; set; }
        public bool Enabled { get; set; }
    }
    public class BotConfigs
    {
        public bool AutoShield { get; set; }
        public int TimeGuildGifts { get; set; } = 5;
    }

    
}
