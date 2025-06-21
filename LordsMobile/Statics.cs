using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordsMobile
{
    class Statics
    {
        //public const int GAME_WIDTH = 1015;
        //public const int GAME_HEIGHT = 582;
        public const int GAME_WIDTH = 981;
        public const int GAME_HEIGHT = 582;
        public const int GAME_DPI = 194;
        public static readonly Point GAME_MID = new Point(490, 291);
        public static readonly Point CLOSE_MAIN = new Point(940, 66);
        public static readonly Point CLOSE_LEVEL_UP = new Point(655, 65);

        public enum MOVEMENTS: int
        {
            Left=0, Up=1, Right=2, Down=3
        }

        public class Screen1
        {
            public static readonly Point CASTLE = new Point(805, 101);
            public static readonly Point BARRACKS = new Point(774, 179);
            public static readonly Point WALL = new Point(660, 180);
            public static readonly Point WATCHTOWER = new Point(515, 165);
            public static readonly Point SHIP = new Point(324, 433);
            public static readonly Point TILE_1 = new Point(709, 285);
            public static readonly Point TILE_2 = new Point(827, 281);
            public static readonly Point TILE_3 = new Point(592, 406);
            public static readonly Point TILE_4 = new Point(722, 390);
        }

        public class Screen3
        {
            public static readonly Point BARRACKS = new Point(626, 339);
            public static readonly Point SHELTER = new Point(544, 158);
            public static readonly Point INFIRMARY = new Point(777, 317);
            public static readonly Point VAULT = new Point(509, 244);
        }

        public class Hud
        {
            public static readonly Point GUILD = new Point(456, 538);
            public static readonly Point SETTINGS = new Point(696, 547);
            public static readonly Point EXTRA_SUPPLIES_COLLECT = new Point(490, 424);
        }

        public class NumPad
        {
            public static readonly Point ONE = new Point(662, 259);
            public static readonly Point TWO = new Point(730, 259);
            public static readonly Point THREE = new Point(794, 259);
            public static readonly Point FOUR = new Point(662, 306);
            public static readonly Point FIVE = new Point(730, 306);
            public static readonly Point SIX = new Point(794, 306);
            public static readonly Point SEVEN = new Point(662, 354);
            public static readonly Point EIGHT = new Point(730, 354);
            public static readonly Point NINE = new Point(794, 354);
            public static readonly Point ZERO = new Point(662, 406);
            public static readonly Point CONFIRM = new Point(777, 406);
        }

        public class Chest
        {
            public static readonly Point CHEST = new Point(873, 424);
            public static readonly Point CLAIM = new Point(488, 401);
        }

        public class Gather
        {
            public static readonly Point GATHER = new Point(490, 309);
            public static readonly Point ASSEMBLE = new Point(721, 358);
            public static readonly Point START = new Point(730, 465);
        }

        public class Tutorial
        {
            public static readonly Point AGREE = new Point(620, 370);
            public static readonly Point BUILD_BARRACKS = new Point(414, 476);
            public static readonly Point BUILD_BARRACKS2 = new Point(511, 371);
            public static readonly Point BUILD_BARRACKS3 = new Point(483, 134);
            public static readonly Point FREE_SPEED = new Point(301, 168);
            public static readonly Point BUILD_BARRACKS5 = new Point(413, 472);
            public static readonly Point BUILD_CASTLE = new Point(411, 476);
            public static readonly Point BUILD_CASTLE2 = new Point(514, 313);
            public static readonly Point BUILD_CASTLE3 = new Point(651, 69);
            public static readonly Point BUILD_CASTLE4 = new Point(414, 478);
            public static readonly Point CLOSE_VIP = new Point(652, 143);
            public static readonly Point GO_TRAIN_BALLISTA = new Point(415, 475);
            public static readonly Point BARRACKS = new Point(550, 424);
            public static readonly Point GO_SKIRMISH1 = new Point(412, 474);
            public static readonly Point CASTLE_SKIRMISH1 = new Point(447, 302);
            public static readonly Point SCOUT_SKIRMISH1 = new Point(415, 411);
            public static readonly Point BATTLETIPS_SKIRMISH1 = new Point(479, 221);
            public static readonly Point HEROES_SKIRMISH1 = new Point(384, 155);
            public static readonly Point TROOPS_SKIRMISH1 = new Point(370, 390);
        }

        public class Panel 
        {
            public static readonly Point PANEL_BUTTON = new Point(20, 185);

            public static readonly Rectangle PANEL_AREA = new Rectangle(181, 384, 72, 19);
        }

        public class Castle
        {
            public static readonly Point CASTLE = new Point(530, 290);
            public static readonly Rectangle ARMY_LIMIT = new Rectangle(576, 318, 138, 31);
        }

        public class Building
        {
            public static readonly Point CENTER_WINDOW = new Point(491, 305);
            public static readonly Point BUILD = new Point(850, 540);
            public static readonly Point UPGRADE = new Point(350, 560);

            public static readonly Point LUMBER_MILL = new Point(485, 139);
            public static readonly Point QUARRY = new Point(482, 259);
            public static readonly Point MINES = new Point(474, 372);
            public static readonly Point FARM = new Point(493, 473);
        }

        public class Barracks
        {
            public static readonly Rectangle INF_T1_AMT = new Rectangle(181, 384, 72, 19);
            public static readonly Rectangle ARCH_T1_AMT = new Rectangle(364, 384, 71, 19);
            public static readonly Rectangle CAV_T1_AMT = new Rectangle(549, 383, 71, 20);
            public static readonly Rectangle BALLI_T1_AMT = new Rectangle(732, 383, 72, 20);
            // Remover ...
            public static readonly Point BALLISTA = new Point(727, 294);
            public static readonly Rectangle GRUNT_AMT = new Rectangle(215, 365, 60, 18);
            public static readonly Rectangle BALLISTA_AMT = new Rectangle(700, 365, 60, 18);
            public static readonly Rectangle ARCHER_AMT = new Rectangle(380, 365, 60, 18);
            public static readonly Rectangle CATAPHRACT_AMT = new Rectangle(540, 365, 60, 18);            
            public static readonly Point GRUNT = new Point(241, 294);
            public static readonly Point ARCHER = new Point(404, 294);
            public static readonly Point CATAPHRACT = new Point(568, 294);
            public static readonly Point TRAIN = new Point(666, 537);
        }

        public class GuildGift
        {
            public static readonly Point GIFT = new Point(455, 580);
        }

        public class Quests
        {
            public static readonly Point VIP_COMP = new Point(785, 155);
            public static readonly Point GUILD_COMP = new Point(635, 155);
            public static readonly Point ADMIN_COMP = new Point(490, 155);
            public static readonly Point TURF_COMP = new Point(330, 155);
            public static readonly Point DAILY_COMP = new Point(190, 155);
        }
        public class Settings
        {
            public class Account
            { 
                public static readonly Rectangle ROI = new Rectangle(367, 228, 227, 40);
                public static readonly Point POINT = new Point(290, 198);
            }
        }

        public class Avoid
        {
            public static readonly Point[] Hud = new Point[]
            {

            };

            public static readonly Point[] TurfKingdom = new Point[]
            {
                new Point(0, 495),
                new Point(125, Statics.GAME_HEIGHT)
            };
        }

        public class Talents
        {
            public static readonly Rectangle ROI = new Rectangle(465, 280, 55, 20);
            public class Row1
            {
                public static int SCREEN = 1;
                public static readonly Point FOOD_PRODUCTION_I = new Point(571, 269);
            }

            public class Row2
            {
                public static int SCREEN = 2;
                public static readonly Point STONE_PRODUCTION_I = new Point(571, 274);
                public static readonly Point CONSTRUCTION_SPEED_I = new Point(736, 274);
            }

            public class Row3
            {
                public static int SCREEN = 3;
                public static readonly Point TIMBER_PRODUCTION_I = new Point(574, 291);
                public static readonly Point RESEARCH_I = new Point(737, 291);
            }

            public class Row4
            {
                public static int SCREEN = 4;
                public static readonly Point ORE_PRODUCTION_I = new Point(575, 299);
                public static readonly Point GOLD_PRODUCTION_I = new Point(738, 303);
            }

            public class Row5
            {
                public static int SCREEN = 5;
                public static readonly Point FOOD_PRODUCTION_I = new Point(577, 312);
                public static readonly Point MAX_LOAD_I = new Point(739, 315);
            }

            public class Row6
            {
                public static int SCREEN = 6;
                public static readonly Point STONE_PRODUCTION_I = new Point(573, 324);
                public static readonly Point GATHERING_I = new Point(739, 326);
            }
        }
    }
}
