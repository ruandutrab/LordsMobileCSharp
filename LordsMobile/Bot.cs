using LordsMobile.Scripts;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using static System.Windows.Forms.AxHost;

namespace LordsMobile
{
    class Bot
    {
        private static bool running = false;
        public static State[] states;
        private static Thread[] threads;
        private static MainForm _mainForm;
        private static DateTime dtLastReadTroops;
        private static TroopsTier1 troopsTier1 = new TroopsTier1();
        private static bool EnableClean = true;

        //public static async void start()
        //{
        //    states = new State[Settings.maxVMs];
        //    threads = new Thread[Settings.maxVMs];
        //    running = true;

        //    while (running)
        //    {
        //        int s = Array.IndexOf(states, null);
        //        if (s != -1)
        //        {
        //            int pId = await MEmuManager.startVMs();
        //            if (pId != -1)
        //            {
        //                State st = new State(MEmuManager.getHandle(pId));
        //                st.processIndex = pId;
        //                states[s] = st;
        //                threads[s] = new Thread(() => runBot(st));
        //                threads[s].Start();

        //                if (!runningCheckAttack)
        //                {
        //                    var tSentinel = new Thread(() => Sentinel(st));
        //                    tSentinel.Start();
        //                    runningCheckAttack = true;
        //                }
        //            }
        //        }
        //        Thread.Sleep(10000);
        //    }
        //}

        public static void startForProfile(State state)
        {
            _mainForm.StatusUpdate("Iniciando bot para perfil individual...");

            if (state.state == "Loading")
                IsStartUp(state);

            runBot(state);
        }

        public static void SetMainFormInstance(MainForm form)
        {
            _mainForm = form;
        }

        

        public static void Sentinel(State state)
        {
            while (true)
            {
                if (state.state != "Loading")
                    if (state.v.ExistPoint(Assets.Attack.being_attacked, 0.8))
                    {
                        EnableClean = false;
                        Thread.Sleep(100);
                        if(!state.v.ExistPoint(Assets.Attack.has_shield, 0.7)) // Se não estiver de escudo
                        {
                            _mainForm.StatusUpdate("Sentinel: Detected attack, using shield");
                            Thread.Sleep(100);
                            if (state.c.vClick(state.v.matchTemplateForSentinel(Assets.Attack.painel_shield, 0.7))) // Abre o painel de escudo
                            {
                                Thread.Sleep(100);
                                if (state.c.vClick(state.v.matchTemplateForSentinel(Assets.Attack.shield_menu, 0.7))) // Abre menu de escudo
                                {
                                    Thread.Sleep(100);
                                    state.c.vClick(state.v.matchTemplateForSentinel(Assets.Attack.using_shield, 0.7)); // Usa o escudo
                                }
                            }
                        }
                        else
                        {
                            _mainForm.StatusUpdate("Sentinel: Detected attack, but shield is active");
                        }
                        EnableClean = true;
                        state.clearScreen(EnableClean);
                    }
                Thread.Sleep(5000);
            }
            
        }

        private static void IsStartUp(State state)
        {
            _mainForm.StatusUpdate("Loading...");
            Thread.Sleep(2000);
            while (!state.v.ExistPoint(Assets.MEmu.Lords2, 0.8))
            {
                //MEmuManager.resizeWindow(state.processIndex);
                _mainForm.StatusUpdate("Looking for Lords Mobile");
                Thread.Sleep(2000);
            }
            if (state.c.vClick(state.v.matchTemplate(Assets.MEmu.Lords2, 0.8)))
                Thread.Sleep(1000);
            if (state.state == "Loading")
            {
                running = true;
                state.state = "Launching Lords Mobile";
                while (!state.v.ExistPoint(Assets.Etc.Close, 0.8) && !state.v.ExistPoint(Assets.Etc.Oracle, 0.55) && running)
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
            while (true && running) {

                //Thread.Sleep(1000);
                //if (tutorial.hasTutorial())
                //    tutorial.doTutorial();
                Thread.Sleep(2000);


                // Verificar baú
                OpenAChest(state);

                // Enviar ajuda
                SendHelpAll(state);

                // Verifica o painel de tarefas
                TaskPanel(state);

                // Verifica as tarefas disponíveis no painel
                while (state.v.ExistPoint(Assets.Painel.painel_Zzz, 0.75))
                {
                    state.c.vClick(Statics.Panel.PANEL_BUTTON);
                    _mainForm.StatusUpdate("Checking dashboard tasks...");
                    Thread.Sleep(200);
                    state.c.vClick(state.v.matchTemplate(Assets.Painel.task_available, 0.80));
                    Thread.Sleep(200);

                    // Centro de pesquisa
                    Academy(state);

                    // Centro de treinamento militar
                    Barracks(state);

                    // Enfermaria
                    Infirmary(state);

                    // Upgrade das contruções
                    ConstructionUpgrade(state);
                }

                // Presentes disponíveis
                GuildGifts(state);

                _mainForm.StatusUpdate("Start: " + state.start.ToString() + ", End: " + state.start.AddMinutes(Settings.duration).ToString() + ", Done? " + state.hasTimeElapsed());
                _mainForm.StatusUpdate("Waiting for time to pass.");
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Painel com as tarefas disponíveis
        /// </summary>
        private static void TaskPanel(State state)
        {
            if (state.c.vClick(state.v.matchTemplate(Assets.Painel.painel_finish_task, 0.75)))
            {
                _mainForm.StatusUpdate("Checking task pane");
                Thread.Sleep(500);
                state.c.vClick(state.v.matchTemplate(Assets.Painel.free_button, 0.75));
                _mainForm.StatusUpdate("Completing task");
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Abrir um baú
        /// </summary>
        private static void OpenAChest(State state)
        {
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
        }

        /// <summary>
        /// Envia ajuda para todos
        /// </summary>
        private static void SendHelpAll(State state)
        {
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
        }

        /// <summary>
        /// Verifica construções disponíveis para upgrade
        /// </summary>
        private static void ConstructionUpgrade(State state)
        {
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
                        TaskPanel(state);
                    else
                        state.c.vClick(state.v.matchTemplate(Assets.Construction.request_help, 0.75));
                    _mainForm.StatusUpdate("Request help");
                    Thread.Sleep(500);
                    state.clearScreen(EnableClean);
                }
                Thread.Sleep(4000);
            }
        }

        /// <summary>
        /// Centro de pesquisa
        /// </summary>
        private static void Academy(State state)
        {
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
                Thread.Sleep(500);
                TaskPanel(state);
            }
        }

        /// <summary>
        /// Treina soldados na caserna
        /// </summary>
        private static void Barracks(State state)
        {
            if (state.v.ExistPoint(Assets.Barracks.barracks, 0.75))
            {
                var troops = new TroopsDB();
                if (dtLastReadTroops <= DateTime.Now)
                {

                    troopsTier1.InfT1 = troops.ParseIntOrDefault(state.v.readText(Statics.Barracks.INF_T1_AMT));
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
        }

        /// <summary>
        /// Verifica se existe soldados para curar na enfermaria
        /// </summary>
        private static void Infirmary(State state)
        {
            
            if (state.v.ExistPoint(Assets.Infirmary.infirmary, 0.75))
            {
                Thread.Sleep(100);
                if (state.c.vClick(state.v.matchTemplate(Assets.Infirmary.heal_all)))
                {
                    Thread.Sleep(100);
                    state.c.vClick(state.v.matchTemplate(Assets.Infirmary.material_not_available));
                    Thread.Sleep(100);
                    state.c.vClick(state.v.matchTemplate(Assets.Infirmary.heal));
                    Thread.Sleep(100);
                    state.clearScreen(EnableClean);
                }
            }
        }

        /// <summary>
        /// Obtém os presentes da guilda e limpa a tela
        /// </summary>
        private static void GuildGifts(State state)
        {
            if (state.v.ExistPoint(Assets.GuildGift.gift, 0.75)) // Verifica se existe presentes
            {
                Thread.Sleep(200);
                if (state.c.vClick(Statics.GuildGift.GUILD_BUTTON)) // Acessa o menu da guild
                {
                    Thread.Sleep(200);
                    if (state.c.vClick(state.v.matchTemplate(Assets.GuildGift.menu_guild_gift, 0.75))) // Vai para aba de presentes
                    {
                        Thread.Sleep(200);
                        if (state.c.vClick(state.v.matchTemplate(Assets.GuildGift.enter_gift, 0.75))) // Acessa aba de presentes
                        {
                            Thread.Sleep(200);
                            if (state.c.vClick(state.v.matchTemplate(Assets.GuildGift.open_all_gifts, 0.75))) // Recolhe todos os presentes
                            {
                                Thread.Sleep(200);
                                state.c.vClick(state.v.matchTemplate(Assets.GuildGift.clean_all_gifts, 0.75)); // Remove todos os presentes
                            }
                            else
                            {
                                while (state.v.ExistPoint(Assets.GuildGift.open_gift, 0.75)) // Recolhe e remove 1 a 1
                                {
                                    Thread.Sleep(500);
                                    state.c.vClick(state.v.matchTemplate(Assets.GuildGift.open_gift, 0.75));
                                    Thread.Sleep(500);
                                    state.c.vClick(state.v.matchTemplate(Assets.GuildGift.clean_gift, 0.75));
                                }
                            }
                            Thread.Sleep(200);
                            // Aguardando dados para adicionar...
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
