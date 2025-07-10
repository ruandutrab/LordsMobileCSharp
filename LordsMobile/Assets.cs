using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordsMobile
{
    class Assets
    {
        public class Challenge {
            public const string challenge = "challenge\\";
            public const string Chapter = challenge + "chapter.jpg";
        }

        public class MEmu
        {
            private const string memu = "assets\\memu\\";
            public const string Lords = memu + "lords.jpg";
            public const string Lords2 = memu + "lords2.jpg";
        }

        public class Chest
        {
            private const string chest = "assets\\chest\\";
            public const string chestOpen = chest + "chest.jpg";
            public const string Collect = chest + "collect.jpg";
            public const string Window = chest + "window.jpg";
            public const string x5 = chest + "5x.jpg";
        }

        public class Development
        {
            private const string dev = "assets\\dev\\";
            public const string Help = dev + "help.jpg";
            public const string Free = dev + "free.jpg";
            public const string Go = dev + "go.jpg";
            public const string Build = dev + "build.jpg";
            public const string Upgrade = dev + "upgrade.jpg";
            public const string NoCR = dev + "no_cr.jpg";
            public const string NoRes = dev + "no_res.jpg";
            public const string New = dev + "new.jpg";
        }

        public class Tutorial
        {
            private const string tutorial = "assets\\tutorial\\";
            public const string Create = tutorial + "create.jpg";
            public const string Important = tutorial + "important.jpg";
            public const string OathKeeper = tutorial + "oath_keeper.jpg";
            public const string Skirmish1 = tutorial + "skirmish1.jpg";
            public const string Confirm = tutorial + "confirm.jpg";
            public const string TapGo = tutorial + "tap_go.jpg";
            public const string MpFull1 = tutorial + "mp_full1.jpg";
            public const string MpFull2 = tutorial + "mp_full2.jpg";
            public const string MpFull3 = tutorial + "mp_full3.jpg";
            public const string Guild = tutorial + "guild.jpg";
            public const string Migrate = tutorial + "migrate.jpg";
        }

        public class Turf
        {
            private const string turf = "assets\\turf\\";
            public const string Statue = turf + "statue.jpg";
            public const string Statue2 = turf + "statue2.jpg";
            public const string Infirmary = turf + "infirmary.jpg";
            public const string Shelter = turf + "shelter.jpg";
            public const string Barracks = turf + "barracks.jpg";
        }

        public class Etc
        {
            private const string etc = "assets\\etc\\";
            public const string Kingdom = etc + "kingdom.jpg";
            public const string Turf = etc + "turf.jpg";
            public const string Leave = etc + "leave.jpg";
            public const string Oracle = etc + "oracle.jpg"; //0.55
            public const string LevelUp = etc + "level_up.jpg"; //0.85
            public const string LiveSupport = etc + "live_support.jpg";
            public const string Close = etc + "close.jpg"; //0.8
            public const string LevelUpTxt = etc + "level_up_txt.jpg";
        }

        public class Hud
        {
            private const string hud = "assets\\hud\\";
            public const string Army = hud + "army.jpg";
            public const string Help = hud + "help.jpg";
            public const string ExtraSupplies = hud + "extra_supplies.jpg";
        }

        public class Quest
        {
            private const string quest = "assets\\quest\\";
            public const string has_completed = quest + "has_completed.jpg";
            public const string has_quests = quest + "has_quests.jpg";
            public const string collect = quest + "collect.jpg";
            public const string start = quest + "start.jpg";
            public const string daily_comp = quest + "daily_comp.jpg";
            public const string daily_comp_select = quest + "daily_comp_select.jpg";
            public const string admin_comp = quest + "admin_comp.jpg";
            public const string admin_comp_select = quest + "admin_comp_select.jpg";
            public const string turf_comp = quest + "turf_comp.jpg";
            public const string turf_comp_select = quest + "turf_comp_select.jpg";
            public const string guild_comp = quest + "guild_comp.jpg";
            public const string guild_comp_select = quest + "guild_comp_select.jpg";
            public const string vip_comp = quest + "vip_comp.jpg";
            public const string vip_comp_select = quest + "vip_comp_select.jpg";
            public const string vip_chest = quest + "vip_chest.jpg";
            public const string auto_complete = quest + "auto_complete.jpg";
            public const string auto_complete_2 = quest + "auto_complete_2.jpg";
            public const string close_up_level = quest + "close_up_level.jpg";
            public const string open_chest_daily = quest + "open_chest_daily.jpg";
            public const string closed_chest = quest + "closed_chest.jpg";
            public const string open_chest = quest + "open_chest.jpg";
            public static readonly String[] muilt_auto_complete = new String[]
            {
                quest + "auto_complete_2.jpg",
                quest + "auto_complete.jpg"
            };
            //public static readonly String[] open_chest_daily = new String[]
            //{
            //    quest + "open_chest_daily.jpg",
            //    quest + "open_chest_daily2.jpg",
            //    quest + "open_chest_daily3.jpg"
            //};
            //public const string vip_claim = quest + "vip_claim.jpg";
        }

        public class Gather
        {
            private const string res = "assets\\res\\";
            public const string gather_res = res + "gather.jpg";
            public const string gather_mirage = res + "mirage\\mirage.jpg";
            //public const string mirage_ruins = res + "mirage\\ruins.jpg";
            public const string gathering_priority = res + "gathering_priority.jpg";
            public const string tier_select = res + "tier_select.jpg";
            public const string highest_tier = res + "highest_tier.jpg";
            public const string lowest_tier = res + "lowest_tier.jpg";
            public const string highest_tier_first = res + "highest_tier_first.jpg";
            public const string lowest_tier_first = res + "lowest_tier_first.jpg";
            public const string deploy = res + "deploy.jpg";
            public static readonly String[] mirage_ruins = new String[]
            {
                res + "mirage\\ruins.jpg",
                res + "mirage\\ruins_2.jpg"
            };
            public static readonly String[] Field = new String[]
            {
                res + "txt_field.jpg",
                res + "field.jpg"
            };
            public static readonly String[] Rocks = new String[]
            {
                res + "txt_rocks.jpg",
                res + "rocks.jpg"
            };
            public static readonly String[] Ore = new String[]
            {
                res + "txt_ore.jpg",
                res + "ore.jpg",
                res + "ore2.jpg"
            };
            public static readonly String[] Wood = new String[]
            {
                res + "txt_woods.jpg",
                res + "woods.jpg",
                res + "woods2.jpg"
            };
            public static readonly String[] Gold = new String[]
            {
                res + "txt_gold.jpg",
                res + "gold.jpg",
                res + "gold2.jpg"
            };
        }

        public class Attack
        {
            private const string attack = "assets\\attack\\";
            public const string being_attacked = attack + "being_attacked.jpg";
            public const string refuge = attack + "refuge.jpg";
            public const string enter = attack + "enter.jpg";
            public const string shelter_trops = attack + "shelter_trops.jpg";
            public const string confirm_shelter = attack + "confirm_shelter.jpg";
            // if have shields
            public const string has_shield = attack + "has_shield.jpg";
            public const string painel_shield = attack + "painel_shield.jpg";
            public const string shield_menu = attack + "shield_menu.jpg";
            public const string using_shield = attack + "using_shield.jpg";
        }

        public class Help
        {
            private const string path = "assets\\send_help\\";
            public const string help = path + "send_help.jpg";
            public const string help3x = path + "send_help_3x.jpg";
            public const string help_all = path + "help_all.jpg";
        }

        public class Panel
        {
            private const string path = "assets\\panel\\";
            public const string panel_finish_task = path + "panel_standby_task_finish.jpg";
            public const string free_button = path + "finish_task.jpg";
            public const string panel_Zzz = path + "panel_standby.jpg";
            public const string construction = path + "construction_queue_is_idle.jpg";
            public const string research = path + "research_is_idle.jpg";
            public const string barracks = path + "barracks_is_idle.jpg";
            public const string task_available = path + "task_available.jpg";
            public const string close_panel = path + "close_panel.jpg";
        }

        public class Construction
        {
            private const string path = "assets\\construction\\";
            public const string building_upgrade = path + "building_upgrade.jpg";
            public const string building_upgrade_2 = path + "building_upgrade_2.jpg";
            public const string start_upgrade = path + "start_upgrade.jpg";
            public const string complete_for_free = path + "complete_for_free.jpg";
            public const string request_help = path + "request_help.jpg";
            public const string material_not_available = path + "material_not_available.jpg";
            public const string idle_upgrade = path + "idle_upgrade.jpg";
            public const string quick_swap_menu = path + "quick_swap_menu.jpg";
            public const string apply_set = path + "apply_set.jpg";
            public const string help = path + "help.jpg";
        }

        public class Research
        {
            private const string path = "assets\\research\\";
            public const string academy = path + "academy.jpg";
            public const string recommended = path + "research_avaliable.jpg";
            public const string research_start = path + "research_start.jpg";
            public const string material_not_available = path + "material_not_available.jpg";
            public const string get_help = path + "get_help.jpg";
        }

        public class Barracks
        {
            private const string path = "assets\\barracks\\";
            public const string barracks = path + "barracks.jpg";
            public const string inf_t1 = path + "\\t1\\inf_t1.jpg";
            public const string arch_t1 = path + "\\t1\\arch_t1.jpg";
            public const string cav_t1 = path + "\\t1\\cav_t1.jpg";
            public const string balli_t1 = path + "\\t1\\balli_t1.jpg";

            public const string inf_t2 = path + "\\t2\\inf_t2.jpg";
            public const string arch_t2 = path + "\\t2\\arch_t2.jpg";
            public const string cav_t2 = path + "\\t2\\cav_t2.jpg";
            public const string balli_t2 = path + "\\t2\\balli_t2.jpg";

            public const string inf_t3 = path + "\\t3\\inf_t3.jpg";
            public const string arch_t3 = path + "\\t3\\arch_t3.jpg";
            public const string cav_t3 = path + "\\t3\\cav_t3.jpg";
            public const string balli_t3 = path + "\\t3\\balli_t3.jpg";

            public const string inf_t4 = path + "\\t4\\inf_t4.jpg";
            public const string arch_t4 = path + "\\t4\\arch_t4.jpg";
            public const string cav_t4 = path + "\\t4\\cav_t4.jpg";
            public const string balli_t4 = path + "\\t4\\balli_t4.jpg";

            public const string start_training = path + "start_training.jpg";
            public const string material_not_available = path + "material_not_available.jpg";
        }

        public class Castle
        {
            private const string path = "assets\\castle\\";
            public const string enter_castle = path + "enter_castle.jpg";
        }

        public class StartUp
        {
            private const string path = "assets\\startup\\";
            public static readonly String[] home_screen = new String[]
            {
                path + "Internal_home_screen.jpg",
                path + "external_home_screen.jpg"
            };
        }

        public class GuildGift
        {
            private const string path = "assets\\guild_gift\\";
            public const string gift = path + "available_gift.jpg";
            public const string enter_gift = path + "enter_gift.jpg";
            public const string open_gift = path + "open_gift.jpg";
            public const string clean_gift = path + "clean_gift.jpg";
            public const string menu_guild_gift = path + "menu_guild_gift.jpg";
            public const string open_all_gifts = path + "open_all_gifts.jpg";
            public const string clean_all_gifts = path + "clean_all_gifts.jpg";
        }

        public class Infirmary
        {
            private const string path = "assets\\infirmary\\";
            public const string infirmary = path + "infirmary.jpg";
            public const string heal_all = path + "heal_all.jpg";
            public const string material_not_available = path + "material_not_available.jpg";
            public const string heal = path + "heal.jpg";
        }

        public class TransmutationLab {
            private const string path = "assets\\transmutation_lab\\";
            public const string transmutation_lab = path + "transmutation_lab.jpg";
            public const string transmute = path + "transmute.jpg";
        }
    }
}
