using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using LordsMobile.Scripts;
using System.Drawing;

namespace LordsMobile
{
    class Bot
    {
        private static bool running = false;
        private static bool runningCheckAttack = false;
        public static State[] states;
        private static Random rnd = new Random();
        private static Thread[] threads;
        private static MainForm _mainForm;
        private static DateTime dtLastReadTroops;
        private static TroopsTier1 troopsTier1 = new TroopsTier1();
        private static string armyLimit;
        private static bool EnableClean;

        public static void start()
        {
            states = new State[Settings.maxVMs];
            threads = new Thread[Settings.maxVMs];
            running = true;

            while (running)
            {
                int s = Array.IndexOf(states, null);
                int pId = MEmuManager.startVMs();
                if (s != -1)
                {
                    State st = new State(MEmuManager.getHandle(pId));
                    st.processIndex = pId;
                    states[s] = st;
                    threads[s] = new Thread(() => runBot(states[s]));
                    threads[s].Start();
                    if (!runningCheckAttack)
                    {
                        threads[s] = new Thread(() => Sentinel(states[s]));
                        runningCheckAttack = true;
                    }
                    threads[s].Start();
                }
                Thread.Sleep(10000);
            }
        }
        public static void SetMainFormInstance(MainForm form)
        {
            _mainForm = form;
        }

        

        private static void Sentinel(State state)
        {
            while (true)
            {
                if (state.v.ExistPoint(Assets.Attack.being_attacked, 0.8))
                {
                    EnableClean = false;
                    Thread.Sleep(100);
                    if(state.c.vClick(state.v.matchTemplateForSentinel(Assets.Attack.refuge, 0.7)))
                    {
                        Thread.Sleep(100);
                        if (state.c.vClick(state.v.matchTemplateForSentinel(Assets.Attack.enter, 0.7)))
                        {
                            Thread.Sleep(100);
                            if (state.c.vClick(state.v.matchTemplateForSentinel(Assets.Attack.shelter_trops, 0.7)))
                            {
                                Thread.Sleep(100);
                                if (state.c.vClick(state.v.matchTemplateForSentinel(Assets.Attack.shelter_trops, 0.7)))
                                {
                                    state.c.vClick(state.v.matchTemplateForSentinel(Assets.Attack.confirm_shelter, 0.7));
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(5000);
                EnableClean = true;
            }
            
        }

        private static void IsStartUp(State state)
        {
            _mainForm.StatusUpdate("Loading...");
            while (state.v.matchTemplate(Assets.MEmu.Lords2, 0.8).X == -1)
            {
                MEmuManager.resizeWindow(state.processIndex);
                _mainForm.StatusUpdate("Looking for Lords Mobile");
                Thread.Sleep(2000);
            }
            if (state.c.vClick(state.v.matchTemplate(Assets.MEmu.Lords2, 0.8)))
                Thread.Sleep(1000);
            if (state.state == "Loading")
            {
                state.state = "Launching Lords Mobile";
                //while (state.v.matchTemplate(Assets.Etc.Close, 0.8).X == -1 && state.v.matchTemplate(Assets.Etc.Oracle, 0.55).X == -1 && running)
                while (!state.v.ExistPoint(Assets.StartUp.initial_screen, 0.8))
                {
                    _mainForm.StatusUpdate("Stuck Launching");
                    state.clearScreen(EnableClean);
                    Debug.WriteLine(state.processIndex);
                    MEmuManager.resizeWindow(state.processIndex);
                    Thread.Sleep(1500);
                }
                state.clearScreen(EnableClean);
            }
            Thread.Sleep(1000);
        }

        private static void StartFarming(State state)
        {
            _mainForm.StatusUpdate("Starting farming");
            state.c.vClick(state.v.matchTemplateForSentinel(Assets.StartUp.initial_screen, 0.75));
            Thread.Sleep(500);
            state.SearchOnMap();
        }

        private static void runBot(State state)
        {
            if (state.state == "Loading" && running)
                IsStartUp(state);

            Tutorial tutorial = new Tutorial(state);
            while(true && running) { 
                
            //Thread.Sleep(1000);
            //if (tutorial.hasTutorial())
            //    tutorial.doTutorial();
                Thread.Sleep(2000);
                
                
                /*
                 * Verificar baú
                 */
                if (state.c.vClick(state.v.matchTemplate(Assets.Chest.chestOpen, 0.75)) || state.c.vClick(state.v.matchTemplate(Assets.Chest.x5, 0.75)))
                {
                    _mainForm.StatusUpdate("Collecting chest...");
                    Thread.Sleep(1000);
                    _mainForm.StatusUpdate("Opening chest");
                    state.c.vClick(state.v.matchTemplate(Assets.Chest.Collect, 0.75));
                    Thread.Sleep(1000);
                    _mainForm.StatusUpdate("Claim chest");
                    state.c.vClick(state.v.matchTemplate(Assets.Chest.Window, 0.75));
                    state.clearScreen(EnableClean);
                }

                /*
                 * Enviar ajuda
                 */
                if (state.c.vClick(state.v.matchTemplate(Assets.Help.help, 0.75)) ||
                    state.c.vClick(state.v.matchTemplate(Assets.Help.help3x, 0.75)))
                {
                    _mainForm.StatusUpdate("Sending help");
                    Thread.Sleep(100);
                    state.c.vClick(state.v.matchTemplate(Assets.Help.help_all, 0.75));
                    _mainForm.StatusUpdate("Help sent");
                    Thread.Sleep(100);
                    state.clearScreen(EnableClean);
                }
            painel:

                /*
                 * Verifica o painel de tarefas
                 */
                bool taskAvailable = false;
                if (state.c.vClick(state.v.matchTemplate(Assets.Painel.painel_finish_task, 0.75)))
                {
                    _mainForm.StatusUpdate("Checking task pane");
                    Thread.Sleep(500);
                    state.c.vClick(state.v.matchTemplate(Assets.Painel.free_button, 0.75));
                    _mainForm.StatusUpdate("Completing task");
                    Thread.Sleep(4000);
                    taskAvailable = true;
                }

                while (state.v.ExistPoint(Assets.Painel.painel_Zzz, 0.75) || taskAvailable)
                {
                    state.c.vClick(Statics.Panel.PANEL_BUTTON);
                    _mainForm.StatusUpdate("Checking dashboard tasks...");
                    Thread.Sleep(200);
                    state.c.vClick(state.v.matchTemplate(Assets.Painel.task_available, 0.80));
                    Thread.Sleep(200);

                    /*
                     * Centro de pesquisa
                     */
                    if (state.v.ExistPoint(Assets.Research.academy, 0.75))
                    {
                        _mainForm.StatusUpdate("Conducting research");
                        state.c.vClick(state.v.matchTemplate(Assets.Research.recommended, 0.65));
                        Thread.Sleep(500);
                        _mainForm.StatusUpdate("Searching...");
                        state.c.vClick(state.v.matchTemplate(Assets.Research.research_start, 0.75));
                        _mainForm.StatusUpdate("No material available");
                        state.c.vClick(state.v.matchTemplate(Assets.Research.material_not_available, 0.75));
                        Thread.Sleep(500);
                        state.c.vClick(state.v.matchTemplate(Assets.Research.get_help, 0.75));
                        state.clearScreen(EnableClean);
                        Thread.Sleep(4000);
                        goto painel;
                    }

                    /*
                     * Centro de treinamento militar
                     */
                    if (state.v.ExistPoint(Assets.Barracks.barracks, 0.75))
                    {
                        var troops = new Troops();
                        if (dtLastReadTroops <= DateTime.Now)
                        {

                            troopsTier1.InfT1 =  troops.ParseIntOrDefault(state.v.readText(Statics.Barracks.INF_T1_AMT));
                            troopsTier1.ArchT1 = troops.ParseIntOrDefault(state.v.readText(Statics.Barracks.ARCH_T1_AMT));
                            troopsTier1.CavT1 = troops.ParseIntOrDefault(state.v.readText(Statics.Barracks.CAV_T1_AMT));
                            troopsTier1.BalliT1 = troops.ParseIntOrDefault(state.v.readText(Statics.Barracks.BALLI_T1_AMT));
                            
                            dtLastReadTroops = DateTime.Now.AddHours(1);
                        }
                        else
                        {
                            troopsTier1 = troops.GetTroops("");
                        }

                        string troopName = troops.GetMinTroopType(troopsTier1);
                        _mainForm.StatusUpdate("Training troops");
                        switch (troopName)
                        {
                            case "InfT1":
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.inf_t1, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(new Point(847, 401));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                                Thread.Sleep(200);
                                state.clearScreen(EnableClean);
                                break;
                            case "ArchT1":
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.arch_t1, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(new Point(847, 401));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                                Thread.Sleep(200);
                                state.clearScreen(EnableClean);
                                break;
                            case "CavT1":
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.cav_t1, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(new Point(847, 401));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                                Thread.Sleep(200);
                                state.clearScreen(EnableClean);
                                break;
                            case "BalliT1":
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.balli_t1, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(new Point(847, 401));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                                Thread.Sleep(200);
                                state.clearScreen(EnableClean);
                                break;
                            default:
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.inf_t1, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(new Point(847, 401));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                                Thread.Sleep(500);
                                state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                                Thread.Sleep(200);
                                state.clearScreen(EnableClean);
                                break;
                        }
                        Thread.Sleep(4000);
                    }

                    /*
                     * Upgrade das contruções.
                     */
                    
                    if (state.v.ExistPoint(Assets.Construction.building_upgrade_2, 0.75))
                    {
                        _mainForm.StatusUpdate("Building ready to upgrade");
                        Thread.Sleep(300);
                        state.c.vClick(Statics.Building.CENTER_WINDOW);
                        Thread.Sleep(300);
                        if (state.c.vClick(state.v.matchTemplate(Assets.Construction.idle_upgrade, 0.80)))
                        {
                            Thread.Sleep(300);
                            if (state.c.vClick(state.v.matchTemplate(Assets.Construction.building_upgrade, 0.75)))
                                _mainForm.StatusUpdate("Upgrading...");
                            Thread.Sleep(300);
                            if (state.c.vClick(state.v.matchTemplate(Assets.Construction.start_upgrade, 0.75)) ||
                                state.c.vClick(Statics.Building.BUILD))
                                _mainForm.StatusUpdate("No material available");
                            //state.c.vClick(Statics.Building.UPGRADE);
                            state.c.vClick(state.v.matchTemplate(Assets.Construction.material_not_available, 0.75));
                            Thread.Sleep(1000);
                            if (state.c.vClick(state.v.matchTemplate(Assets.Construction.complete_for_free, 0.75)))
                                goto painel;
                            else
                                state.c.vClick(state.v.matchTemplate(Assets.Construction.request_help, 0.75));
                            _mainForm.StatusUpdate("Request help");
                            Thread.Sleep(500);
                            state.clearScreen(EnableClean);
                        }
                        Thread.Sleep(4000);
                        taskAvailable = false;
                    }
                }



                //state.c.vClick(state.v.matchTemplate(Assets.Development.Free, 0.65));
                //state.c.vClick(state.v.matchTemplate(Assets.Development.Help, 0.6));
                //if (state.c.vClick(state.v.matchTemplate(Assets.Hud.Help, 0.65)))
                //{
                //    state.c.vClick(new Point(491, 534));
                //    state.clearScreen();
                //}

                //if (state.v.matchTemplate(Assets.Chest.Collect, 0.65).X != -1)
                //{
                //    state.c.vClick(Statics.Chest.CHEST);
                //    state.c.vClick(Statics.Chest.CLAIM);
                //}

                //if (state.c.vClick(state.v.matchTemplate(Assets.Hud.ExtraSupplies, 0.75)))
                //{
                //    state.c.vClick(Statics.Hud.EXTRA_SUPPLIES_COLLECT);
                //}

                //if (DateTime.Now > state.lastShelterCheck.AddMinutes(5))
                //    state.lastShelterCheck = Core.mainChecks(state, state.screen);


                /**
                 *  ~~~   Quest Checks   ~~~
                 * 
                 */
                //if (state.c.vClick(state.v.matchTemplate(Assets.Quest.HasCompleted, 0.65)) ||
                //    state.c.vClick(state.v.matchTemplate(Assets.Quest.HasQuests, 0.65)))
                //{
                //    if (state.c.vClick(state.v.matchTemplate(Assets.Quest.Turf, 0.61)))
                //    {
                //        while (state.c.vClick(state.v.matchTemplate(Assets.Quest.Collect, 0.8)))
                //        {
                //            Debug.WriteLine("Turf");
                //            if (state.v.matchTemplate(Assets.Etc.LevelUpTxt, 0.55).X != -1)
                //                state.c.vClick(Statics.CLOSE_LEVEL_UP);
                //        }
                //    }
                //    if (state.c.vClick(state.v.matchTemplate(Assets.Quest.Admin, 0.6)))
                //    {
                //        while (state.c.vClick(state.v.matchTemplate(Assets.Quest.Collect, 0.8)))
                //        {
                //            Debug.WriteLine("Admin");
                //            if (state.v.matchTemplate(Assets.Etc.LevelUpTxt, 0.55).X != -1)
                //                state.c.vClick(Statics.CLOSE_LEVEL_UP);
                //        }
                //        state.c.vClick(state.v.matchTemplate(Assets.Quest.Start, 0.5));
                //    }
                //    if (state.c.vClick(state.v.matchTemplate(Assets.Quest.Guild, 0.6)))
                //    {
                //        while (state.c.vClick(state.v.matchTemplate(Assets.Quest.Collect, 0.8)))
                //        {
                //            Debug.WriteLine("Guild");
                //            if (state.v.matchTemplate(Assets.Etc.LevelUpTxt, 0.55).X != -1)
                //                state.c.vClick(Statics.CLOSE_LEVEL_UP);
                //        }
                //        state.c.vClick(state.v.matchTemplate(Assets.Quest.Start, 0.5));
                //    }
                //    if (state.c.vClick(state.v.matchTemplate(Assets.Quest.VIP, 0.61)))
                //    {
                //        if (state.v.matchTemplate(Assets.Quest.VIPClaim, 0.7).X != -1)
                //            state.c.vClick(state.v.matchTemplate(Assets.Quest.VIPChest, 0.6));
                //    }
                //}



                //state.clearScreen();
                //if (state.v.matchTemplate(Assets.Hud.Army, 0.6).X == -1)
                //{
                //    Gather g = new Gather(state);
                //    state.lastResource = g.gather(rnd.Next(0, 5));
                //}

                //while (state.v.matchTemplate(Assets.Development.NoCR, 0.55).X != -1)
                //{
                //    state.goTo();
                //    Build b = new Build(state);
                //    b.build();
                //    state.screen = -1;

                //    state.c.vClick(state.v.matchTemplate(Assets.Development.Free, 0.65));
                //    state.c.vClick(state.v.matchTemplate(Assets.Development.Help, 0.6));
                //}

                //state.c.vClick(state.v.matchTemplate(Assets.Development.Free, 0.65));
                //state.c.vClick(state.v.matchTemplate(Assets.Development.Help, 0.6));

                //if (state.v.matchTemplate(Assets.Chest.Collect, 0.65).X != -1)
                //{
                //    state.c.vClick(Statics.Chest.CHEST);
                //    state.c.vClick(Statics.Chest.CLAIM);
                //}
                //StartFarming(state);
                GuildGifts(state);
                _mainForm.StatusUpdate("Start: " + state.start.ToString() + ", End: " + state.start.AddMinutes(Settings.duration).ToString() + ", Done? " + state.hasTimeElapsed());
                _mainForm.StatusUpdate("Waiting for time to pass.");
                Thread.Sleep(1000);
            }
        }

        private static void GuildGifts(State state)
        {
            if (state.v.ExistPoint(Assets.GuildGift.gift, 0.75))
            {
                Thread.Sleep(200);
                if (state.c.vClick(Statics.GuildGift.GIFT))
                {
                    Thread.Sleep(200);
                    if (state.c.vClick(state.v.matchTemplate(Assets.GuildGift.enter_gift, 0.75)))
                    {
                        Thread.Sleep(200);
                        while(state.v.ExistPoint(Assets.GuildGift.open_gift, 0.75))
                        {
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.GuildGift.open_gift, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.GuildGift.clean_gift, 0.75));
                        }
                    }
                }
                state.clearScreen(EnableClean);
            }
        }
        public static void stop()
        {
            foreach (Thread t in threads)
            {
                t.Abort();
            }
            Debug.WriteLine("Stopping!");
            running = false;
        }
    }
}
