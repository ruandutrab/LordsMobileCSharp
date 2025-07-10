using LordsMobile.Scripts;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using static LordsMobile.Statics;
using static System.Windows.Forms.AxHost;

namespace LordsMobile
{
    class Bot
    {
        private static bool running = false;
        private static MainForm _mainForm;
        private static bool EnableClean = true;
        private static bool SentinelActive = true;

        public static void startForProfile(State state, VmProfile profile)
        {
            _mainForm.StatusUpdate("Iniciando bot para perfil individual...");

            if (state.state == "Loading")
                IsStartUp(state, profile);

            runBot(state);
        }

        public static void SetMainFormInstance(MainForm form)
        {
            _mainForm = form;
        }

        public static void DisableSentinel()
        {
            SentinelActive = false;
            _mainForm.StatusUpdate("Sentinel disabled");
        }

        public static void Sentinel(State state)
        {
            while (SentinelActive)
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

        private static void IsStartUp(State state, VmProfile profile)
        {
            _mainForm.StatusUpdate("Loading...");
            state.clearScreen(EnableClean);
            Thread.Sleep(2000);
            if(!state.v.ExistMultPoints(Assets.StartUp.home_screen, 0.75))
            {
                MEmuManager.runLordsMobile(profile);
                //while (!state.v.ExistPoint(Assets.MEmu.Lords2, 0.8))
                //{
                //    //MEmuManager.resizeWindow(state.processIndex);
                //    _mainForm.StatusUpdate("Looking for Lords Mobile");
                //    Thread.Sleep(2000);
                //}
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
            }
            else
            {
                state.state = "Game running...";
                _mainForm.StatusUpdate("Game running...");
                running = true;
                state.clearScreen(EnableClean);
            }
                Thread.Sleep(1000);
        }

        private static void GatherMirage(State state)
        {
            if (state.c.vClick(state.v.matchTemplateForSentinel(Assets.Gather.gather_mirage, 0.75)))
            {
                _mainForm.StatusUpdate("Starting mirage farming");
                string tier = "lowest";
                Action[] moveToOre = new Action[]
                {
                    () => state.c.vMoveDown(false),
                    () => state.c.vMoveDown(false),
                    () => state.c.vMoveDown(false),
                    () => state.c.vMoveDown(false),
                    () => state.c.vMoveDown(false),
                    () => state.c.vMoveDown(false),
                    () => state.c.vMoveLeft(false),
                    () => state.c.vMoveLeft(false)
                };
                Thread.Sleep(3000);
                foreach (Action action in moveToOre)
                {
                    action();
                    Thread.Sleep(1000);
                    if (state.v.ExistMultPoints(Assets.Gather.mirage_ruins, 0.75))
                    {
                        _mainForm.StatusUpdate("Found mirage ore");
                        if (state.c.vClick(state.v.MuiltMatchTemplate(Assets.Gather.mirage_ruins, 0.75)))
                        {
                            Thread.Sleep(200);
                            if (state.c.vClick(state.v.matchTemplate(Assets.Gather.gather_res, 0.75)))
                            {
                                _mainForm.StatusUpdate("Gathering mirage ore");
                                Thread.Sleep(200);
                                if (state.v.ExistPoint(Assets.Gather.gathering_priority, 0.75)) // Não está com a prioridade de coleta selecionado
                                {
                                    Thread.Sleep(200);
                                    if (state.c.vClick(state.v.matchTemplate(Assets.Gather.tier_select, 0.75)))
                                    {
                                        _mainForm.StatusUpdate("Setting gathering priority");
                                        Thread.Sleep(200);
                                        switch (tier)
                                        {
                                            case "lowest":
                                                state.c.vClick(state.v.matchTemplate(Assets.Gather.lowest_tier_first, 0.75));
                                                break;
                                            case "highest":
                                                state.c.vClick(state.v.matchTemplate(Assets.Gather.highest_tier_first, 0.75));
                                                break;
                                        }
                                    }
                                } 
                                else if (tier == "lowest" && state.v.ExistPoint(Assets.Gather.lowest_tier, 0.75))
                                {
                                    Thread.Sleep(200);
                                    state.c.vClick(state.v.matchTemplate(Assets.Gather.lowest_tier, 0.75));
                                } 
                                else if (tier == "highest" && state.v.ExistPoint(Assets.Gather.highest_tier, 0.75))
                                {
                                    Thread.Sleep(200);
                                    state.c.vClick(state.v.matchTemplate(Assets.Gather.highest_tier, 0.75));
                                } 
                                else if (state.c.vClick(state.v.matchTemplate(Assets.Gather.tier_select, 0.75)))
                                {
                                    _mainForm.StatusUpdate("Setting gathering priority");
                                    Thread.Sleep(200);
                                    switch (tier)
                                    {
                                        case "lowest":
                                            state.c.vClick(state.v.matchTemplate(Assets.Gather.lowest_tier_first, 0.75));
                                            break;
                                        case "highest":
                                            state.c.vClick(state.v.matchTemplate(Assets.Gather.highest_tier_first, 0.75));
                                            break;
                                    }
                                }
                                state.c.vClick(state.v.matchTemplate(Assets.Gather.deploy, 0.75));
                            }
                        }
                        Thread.Sleep(2000);
                        state.clearScreen(EnableClean);
                        return;
                    }

                }
            }
            //state.c.vClick(state.v.matchTemplateForSentinel(Assets.StartUp.initial_screen, 0.75));
            Thread.Sleep(500);
            //state.SearchOnMap();
        }

        private static void runBot(State state)
        {
            //if (state.state == "Loading" && running)
            //    IsStartUp(state);

            //Tutorial tutorial = new Tutorial(state);
            while (true && running) {

                //Thread.Sleep(1000);
                //if (tutorial.hasTutorial())
                //    tutorial.doTutorial();
                Thread.Sleep(2000);

                // Gather mirage
                GatherMirage(state);

                // Verificar baú
                OpenAChest(state);

                // Enviar ajuda
                SendHelpAll(state);

                // Verifica o painel de tarefas
                TaskPanel(state);

                // Verifica as tarefas disponíveis no painel
                while (state.v.ExistPoint(Assets.Panel.panel_Zzz, 0.75))
                {
                    state.c.vClick(Statics.Panel.PANEL_BUTTON);
                    _mainForm.StatusUpdate("Checking dashboard tasks...");
                    Thread.Sleep(200);
                    state.c.vClick(state.v.matchTemplate(Assets.Panel.task_available, 0.80));
                    Thread.Sleep(200);

                    // Centro de pesquisa
                    Academy(state);

                    // Centro de treinamento militar
                    Barracks(state);

                    // Enfermaria
                    Infirmary(state);

                    // Transmutação
                    TransmutationLab(state);

                    // Upgrade das contruções
                    ConstructionUpgrade(state);
                }

                // Presentes disponíveis
                GuildGifts(state);

                // Verifica se existem quests disponíveis
                if (Settings.Bot.TimeToCheckQuests <= DateTime.Now)
                {
                    Quests(state);
                    Settings.Bot.TimeToCheckQuests = DateTime.Now.AddMinutes(5);
                }

                _mainForm.StatusUpdate("Start: " + state.start.ToString() + ", End: " + state.start.AddMinutes(Settings.duration).ToString() + ", Done? " + state.hasTimeElapsed());
                _mainForm.StatusUpdate("Waiting for time to pass.");
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Laboratório de transmutação
        /// </summary>
        private static void TransmutationLab(State state)
        {
            if (state.v.ExistPoint(Assets.TransmutationLab.transmutation_lab, 0.75))
            {
                Thread.Sleep(200);
                state.c.vClick(Statics.TransmutationLab.POS1);
                if (state.v.ExistPoint(Assets.TransmutationLab.transmute, 0.75))
                {
                    state.c.vClick(state.v.matchTemplate(Assets.TransmutationLab.transmute, 0.75));
                    Thread.Sleep(200);
                    goto final;
                }
                Thread.Sleep(200);
                state.c.vClick(Statics.TransmutationLab.POS2);
                if (state.v.ExistPoint(Assets.TransmutationLab.transmute, 0.75))
                {
                    state.c.vClick(state.v.matchTemplate(Assets.TransmutationLab.transmute, 0.75));
                    Thread.Sleep(200);
                    goto final;
                }
                Thread.Sleep(200);
                state.c.vClick(Statics.TransmutationLab.POS3);
                if (state.v.ExistPoint(Assets.TransmutationLab.transmute, 0.75))
                {
                    state.c.vClick(state.v.matchTemplate(Assets.TransmutationLab.transmute, 0.75));
                    Thread.Sleep(200);
                    goto final;
                }
            final:
                state.clearScreen(EnableClean);
            }
        }

        /// <summary>
        /// Painel com as tarefas disponíveis
        /// </summary>
        private static void TaskPanel(State state)
        {
            if (state.c.vClick(state.v.matchTemplate(Assets.Panel.panel_finish_task, 0.75)))
            {
                _mainForm.StatusUpdate("Checking task pane");
                Thread.Sleep(200);
                if(state.c.vClick(state.v.matchTemplate(Assets.Panel.free_button, 0.75)))
                {
                    Thread.Sleep(200);
                    state.c.vClick(state.v.matchTemplate(Assets.Panel.close_panel, 0.75));
                }
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
                Thread.Sleep(100);
                _mainForm.StatusUpdate("Opening chest");
                state.c.vClick(state.v.matchTemplate(Assets.Chest.Collect, 0.75));
                Thread.Sleep(100);
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
            Thread.Sleep(200);
            if (state.c.vClick(state.v.matchTemplate(Assets.Construction.building_upgrade, 0.80)))
            {
                _mainForm.StatusUpdate("Building ready to upgrade");
                Thread.Sleep(200);
                if (state.c.vClick(state.v.matchTemplate(Assets.Construction.start_upgrade, 0.80)))
                {
                    Thread.Sleep(200);
                    if (state.c.vClick(state.v.matchTemplate(Assets.Construction.quick_swap_menu, 0.80)))
                    {
                        Thread.Sleep(200);
                        if (state.c.vClick(state.v.matchTemplate(Assets.Construction.apply_set, 0.80)))
                        {
                            Thread.Sleep(200);
                            if (state.c.vClick(state.v.matchTemplate(Assets.Construction.start_upgrade, 0.80)))
                            {
                                Thread.Sleep(200);
                                if (state.c.vClick(state.v.matchTemplate(Assets.Construction.material_not_available, 0.80)))
                                {
                                    _mainForm.StatusUpdate("No material available");
                                }
                                _mainForm.StatusUpdate("Upgrading...");
                                Thread.Sleep(200);
                                
                            }
                        }
                    } 
                    else
                    {
                        Thread.Sleep(200);
                        if (state.c.vClick(state.v.matchTemplate(Assets.Construction.material_not_available, 0.75)))
                        {
                            _mainForm.StatusUpdate("No material available");
                            Thread.Sleep(200);
                        }
                        _mainForm.StatusUpdate("Upgrading...");
                    }
                    if (state.c.vClick(state.v.matchTemplate(Assets.Construction.help, 0.80)))
                    {
                        _mainForm.StatusUpdate("Request help");
                    }
                }
                state.clearScreen(EnableClean);
            }
            
            if (state.v.ExistPoint(Assets.Construction.building_upgrade_2, 0.75))
            {
                _mainForm.StatusUpdate("Building ready to upgrade");
                Thread.Sleep(300);
                state.c.vClick(Statics.Building.CENTER_WINDOW);
                Thread.Sleep(500);
                if (state.c.vClick(state.v.matchTemplate(Assets.Construction.idle_upgrade, 0.80)))
                {
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
                _mainForm.StatusUpdate("Training troops");
                if (Settings.Troops.TrainingTroopsT1)
                {
                    switch (Settings.Troops.Troop)
                    {
                        case "Inf":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.inf_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Arch":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.arch_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Cav":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.cav_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Balli":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.balli_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
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
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                    }
                    Thread.Sleep(4000);
                    goto final;
                }

                state.c.vDragMouse(new Point(480, 440), new Point(480, 196)); // T2
                Thread.Sleep(1000);
                if (Settings.Troops.TrainingTroopsT2)
                {
                    switch (Settings.Troops.Troop)
                    {
                        case "Inf":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.inf_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Arch":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.arch_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Cav":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.cav_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Balli":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.balli_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
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
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                    }
                    Thread.Sleep(4000);
                    goto final;
                }

                state.c.vDragMouse(new Point(479, 440), new Point(479, 195)); // T3
                Thread.Sleep(1000);
                if (Settings.Troops.TrainingTroopsT3)
                {
                    switch (Settings.Troops.Troop)
                    {
                        case "Inf":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.inf_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Arch":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.arch_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Cav":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.cav_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Balli":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.balli_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
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
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                    }
                    Thread.Sleep(4000);
                    goto final;
                }
                state.c.vDragMouse(new Point(481, 440), new Point(481, 194)); // T4
                Thread.Sleep(1000);
                if (Settings.Troops.TrainingTroopsT4)
                {
                    switch (Settings.Troops.Troop)
                    {
                        case "Inf":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.inf_t4, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Arch":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.arch_t4, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Cav":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.cav_t4, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Balli":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.balli_t4, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        default:
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.inf_t4, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                    }
                    Thread.Sleep(4000);
                    goto final;
                }
                state.c.vDragMouse(new Point(480, 440), new Point(480, 193)); // T5
                if (Settings.Troops.TrainingTroopsT5)
                {
                    switch (Settings.Troops.Troop)
                    {
                        case "Inf":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.inf_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Arch":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.arch_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Cav":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.cav_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                        case "Balli":
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.balli_t1, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(new Point(811, 401));
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
                            state.c.vClick(new Point(811, 401));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.start_training, 0.75));
                            Thread.Sleep(500);
                            state.c.vClick(state.v.matchTemplate(Assets.Barracks.material_not_available, 0.75));
                            Thread.Sleep(200);
                            state.clearScreen(EnableClean);
                            break;
                    }
                    Thread.Sleep(4000);
                    goto final;
                }
                Thread.Sleep(1000);
            }
        final:;
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
                    state.c.vClick(state.v.matchTemplate(Assets.Infirmary.heal));
                    Thread.Sleep(100);
                    state.c.vClick(state.v.matchTemplate(Assets.Infirmary.material_not_available));
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
                            state.c.vClick(GuildGift.GUILD_GIFT); // Guild Gift
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
                            state.c.vClick(GuildGift.BONUS_CHEST); // Bonus Chest
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

        /// <summary>
        ///  Questes do jogo, bau vip, etc
        /// </summary>
        private static void Quests(State state)
        {
            if (state.v.ExistPoint(Assets.Quest.has_quests, 0.75))
            {
                Thread.Sleep(200);
                state.c.vClick(state.v.matchTemplate(Assets.Quest.has_quests, 0.75));
                Thread.Sleep(200);
                // Abre o baú de recompensas
                if (state.c.vClick(state.v.matchTemplate(Assets.Quest.vip_comp, 0.95)) || 
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.vip_comp_select, 0.95)))
                {
                    Thread.Sleep(200);
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.vip_chest, 0.75));
                }
                Thread.Sleep(200);
                // Coleta recompensas da quest de guild
                if (state.c.vClick(state.v.matchTemplate(Assets.Quest.guild_comp, 0.85)) ||
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.guild_comp_select, 0.85)))
                {
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.collect, 0.75));
                    Thread.Sleep(2000);
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.start, 0.75));
                    while (state.v.ExistMultPoints(Assets.Quest.muilt_auto_complete, 0.75)) // Vai coletar todas as recompensas
                    {
                        state.c.vClick(state.v.MuiltMatchTemplate(Assets.Quest.muilt_auto_complete, 0.75)); // Coleta recompensas
                        Thread.Sleep(500);
                    }
                }
                // Coleta recompensas do Admin quest
                if (state.c.vClick(state.v.matchTemplate(Assets.Quest.admin_comp, 0.95)) ||
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.admin_comp_select, 0.95)))
                {
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.collect, 0.75));
                    Thread.Sleep(2000);
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.start, 0.75));
                    Thread.Sleep(200);
                    while (state.v.ExistMultPoints(Assets.Quest.muilt_auto_complete, 0.75)) // Vai coletar todas as recompensas
                    {
                        state.c.vClick(state.v.MuiltMatchTemplate(Assets.Quest.muilt_auto_complete, 0.75)); // Coleta recompensas
                        Thread.Sleep(500);
                    }
                }
                // Missões territoriais
                if (false) {
                    if (state.c.vClick(state.v.matchTemplate(Assets.Quest.turf_comp, 0.85)) ||
                        state.c.vClick(state.v.matchTemplate(Assets.Quest.turf_comp_select, 0.85)))
                    {
                        while (state.v.ExistPoint(Assets.Quest.collect, 0.75)) // Vai coletar todas as recompensas
                        {
                            Thread.Sleep(200);
                            state.c.vClick(state.v.matchTemplate(Assets.Quest.collect, 0.75)); // Coleta recompensas
                            Thread.Sleep(200);
                            if (state.v.ExistPoint(Assets.Quest.close_up_level, 0.75)) // Se ainda tiver recompensas
                                state.c.vClick(state.v.matchTemplate(Assets.Quest.close_up_level, 0.75));
                            Thread.Sleep(1000);
                        }
                    }
                }
                // Missões diárias
                if (state.c.vClick(state.v.matchTemplate(Assets.Quest.daily_comp, 0.85)) ||
                    state.c.vClick(state.v.matchTemplate(Assets.Quest.daily_comp_select, 0.85)))
                {
                    Thread.Sleep(200);
                    while (state.v.ExistPoint(Assets.Quest.collect, 0.80)) // Vai coletar todas as recompensas
                    {
                        Thread.Sleep(400);
                        state.c.vClick(state.v.matchTemplate(Assets.Quest.collect, 0.80)); // Coleta recompensas
                    }
                    Thread.Sleep(1000);
                    if (state.v.matchLocationTemplate(Assets.Quest.closed_chest, 0.80, false, Statics.Quests.CHEST_LOCATION1).X == -1)
                    {
                        Thread.Sleep(200);
                        if (state.v.matchLocationTemplate(Assets.Quest.open_chest, 0.80, false, Statics.Quests.CHEST_LOCATION1).X == -1)
                        {
                            Thread.Sleep(200);
                            state.c.vClick(Statics.Quests.OPEN_CHEST1);
                        }
                    }
                    Thread.Sleep(1000);
                    if (state.v.matchLocationTemplate(Assets.Quest.closed_chest, 0.80, false, Statics.Quests.CHEST_LOCATION2).X == -1)
                    {
                        Thread.Sleep(200);
                        if (state.v.matchLocationTemplate(Assets.Quest.open_chest, 0.80, false, Statics.Quests.CHEST_LOCATION2).X == -1)
                        {
                            Thread.Sleep(200);
                            state.c.vClick(Statics.Quests.OPEN_CHEST2);
                        }
                    }
                    Thread.Sleep(1000);
                    if (state.v.matchLocationTemplate(Assets.Quest.closed_chest, 0.80, false, Statics.Quests.CHEST_LOCATION3).X == -1)
                    {
                        Thread.Sleep(200);
                        if (state.v.matchLocationTemplate(Assets.Quest.open_chest, 0.80, false, Statics.Quests.CHEST_LOCATION3).X == -1)
                        {
                            Thread.Sleep(200);
                            state.c.vClick(Statics.Quests.OPEN_CHEST3);
                        }
                    }
                    Thread.Sleep(1000);
                    if (state.v.matchLocationTemplate(Assets.Quest.closed_chest, 0.80, false, Statics.Quests.CHEST_LOCATION4).X == -1)
                    {
                        Thread.Sleep(200);
                        if (state.v.matchLocationTemplate(Assets.Quest.open_chest, 0.80, false, Statics.Quests.CHEST_LOCATION4).X == -1)
                        {
                            Thread.Sleep(200);
                            state.c.vClick(Statics.Quests.OPEN_CHEST4);
                        }
                    }
                    Thread.Sleep(1000);
                    if (state.v.matchLocationTemplate(Assets.Quest.closed_chest, 0.80, false, Statics.Quests.CHEST_LOCATION5).X == -1)
                    {
                        Thread.Sleep(200);
                        if (state.v.matchLocationTemplate(Assets.Quest.open_chest, 0.80, false, Statics.Quests.CHEST_LOCATION5).X == -1)
                        {
                            Thread.Sleep(200);
                            state.c.vClick(Statics.Quests.OPEN_CHEST5);
                        }
                    }
                }
            }
            state.clearScreen(EnableClean);
        }
    }
}
