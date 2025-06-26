using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace LordsMobile
{
    class MEmuManager
    {
        public static string[] instances;
        public static Process[] processes;
        private static String installPath = "C:\\Program Files\\Microvirt\\MEmu";
        private static List<string> vms = new List<string>();
        private static List<string> allowedVMs = new List<string>();
        private static List<string> isRunningVMs = new List<string>();
        private static int runningVMs = 0;
        private static int lastVM = 0;

        const int SW_RESTORE = 9;
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
        //[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        //internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static void MoveWindowToDefinition(IntPtr hWnd, int x, int y)
        {
            if (GetWindowRect(hWnd, out RECT rect))
            {
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                MoveWindow(hWnd, x, y, width, height, true);
            }
        }


        public MEmuManager()
        {
        }

        public static int getRunningAmount()
        {
            return runningVMs;
        }

        public static void getVMs()
        {
            string[] dirs;
            try
            {
                dirs = Directory.GetDirectories("C:\\Program Files\\Microvirt\\MEmu\\MemuHyperv VMs");
            } catch(Exception ex)
            {
                dirs = Directory.GetDirectories("C:\\Program Files\\Microvirt\\MEmu\\MemuHyperv VMs");
                MEmuManager.installPath = "C:\\Program Files\\Microvirt\\MEmu";
            }
    
            var checkProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(installPath, "memuc.exe"),
                    Arguments = "listvms",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            checkProc.Start();
            string listOutput = checkProc.StandardOutput.ReadToEnd();
            checkProc.WaitForExit();

            // Quebrar por linhas
            string[] linhas = listOutput.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var linha in linhas)
            {
                string[] partes = linha.Split(',');
                if (partes.Length > 1)
                {
                    MEmuManager.vms.Add(partes[1]);
                }
            }

            foreach (var vm in MEmuManager.vms)
            {
                // Quais estão rodando...
                var checkRunningProc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(installPath, "memuc.exe"),
                        Arguments = $"isvmrunning i {vm}",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                checkRunningProc.Start();
                string running = checkRunningProc.StandardOutput.ReadToEnd();
                checkRunningProc.WaitForExit();

                string[] returnRunning = running.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var linha in returnRunning)
                {
                    string[] vmItem = linha.Split(',');
                    if (vmItem.Length > 1)
                    {
                        isRunningVMs.Add(vmItem[1]);
                    }
                }
            }
        }

        public static IntPtr getHandle(int vm)
        {
            if (processes[vm] == null)
                throw new InvalidOperationException($"Processo da VM no índice {vm} não foi inicializado.");

            return processes[vm].MainWindowHandle;
        }

        private static void resizeVM()
        {
            string args = $"setconfigex -i 0 custom_resolution {Statics.GAME_WIDTH} {Statics.GAME_HEIGHT} {Statics.GAME_DPI}";

            var checkProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(installPath, "memuc.exe"),
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            checkProc.Start();
            string stdout = checkProc.StandardOutput.ReadToEnd();
            string stderr = checkProc.StandardError.ReadToEnd();
            Debug.WriteLine(stdout);
            Debug.WriteLine(stderr);
            checkProc.WaitForExit();
        }

        private static async Task<int> startVM(string vm)
        {
            int vmInd = Array.IndexOf(instances, null);
            string memuNum = MEmuManager.lastVM.ToString();
            string newName = $"MEmu{memuNum}";
            bool vmAvailable = false;
            foreach (var item in vms)
            {
                if (isRunningVMs.Contains(item))
                    cloneVM(newName);
                else
                    vmAvailable = true;
            }


            int retries = 10;

            while (!vmAvailable && retries-- > 0)
            {
                var checkProc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(installPath, "memuc.exe"),
                        Arguments = "listvms",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                checkProc.Start();
                string listOutput = checkProc.StandardOutput.ReadToEnd();
                checkProc.WaitForExit();

                if (listOutput.Contains(newName))
                {
                    vmAvailable = true;
                    break;
                }

                Thread.Sleep(1000);
            }

            if (vmAvailable)
                playVM(memuNum);

            if (!vmAvailable)
            {
                Debug.WriteLine("Erro: a nova VM ainda não foi registrada após o clone.");
                return -1;
            }
            do
            {
                System.Threading.Thread.Sleep(1000);
                Process[] p = Process.GetProcesses();
                foreach (Process pr in p)
                {
                    if (pr.MainWindowTitle == $"MEmu{memuNum}" || pr.MainWindowTitle == "MEmu")
                        processes[vmInd] = pr;
                }
            } while (processes[vmInd] == null);
            Thread.Sleep(500);
            Debug.WriteLine("Process Found: " + processes[vmInd].MainWindowTitle);
            //resizeWindow(vmInd);
            //resizeVM();
            Thread.Sleep(500);
            instances[vmInd] = vm;
            return vmInd;
        }

        private static void playVM(string memuNum)
        {
            string args;

            if (memuNum == "0")
                args = $"start -n MEmu";
            else
                args = $"start -n MEmu{memuNum}";

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(installPath, "memuc.exe"),
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            string stdout = proc.StandardOutput.ReadToEnd();
            string stderr = proc.StandardError.ReadToEnd();
            Debug.WriteLine(stdout);
            Debug.WriteLine(stderr);
        }

        private static void cloneVM(string newName)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(installPath, "memuc.exe"),
                    Arguments = $"clone -i 0 -r {newName}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            string stdout = proc.StandardOutput.ReadToEnd();
            string stderr = proc.StandardError.ReadToEnd();
            Debug.WriteLine(stdout);
            Debug.WriteLine(stderr);
        }

        public static void resizeWindow(int vmInd)
        {
            //MoveWindow(processes[vmInd].MainWindowHandle, 0, 0, Statics.GAME_WIDTH, Statics.GAME_HEIGHT, true);
            MoveWindowToDefinition(processes[vmInd].MainWindowHandle, 0, 0);
        }

        public static async Task<int> startVMs()
        {
            int p = -1;
            int vmsLeft = Settings.maxVMs - MEmuManager.runningVMs;
            if (MEmuManager.runningVMs < Settings.maxVMs && MEmuManager.runningVMs < MEmuManager.allowedVMs.Count)
            {
                if (MEmuManager.lastVM < MEmuManager.allowedVMs.Count)
                {
                    p = await MEmuManager.startVM(MEmuManager.allowedVMs[MEmuManager.lastVM]);
                    MEmuManager.lastVM++;
                    MEmuManager.runningVMs++;
                }
                else
                {
                    MEmuManager.lastVM = 0;
                    p = await MEmuManager.startVM(MEmuManager.allowedVMs[MEmuManager.lastVM]);
                    MEmuManager.lastVM++;
                    MEmuManager.runningVMs++;
                }
            }
            return p;
        }

        public static List<string> getMEmuIDs()
        {
            return MEmuManager.vms;
        }

        public static void setAllowedVMs(string vm)
        {
            MEmuManager.allowedVMs.Add(vm);
        }

        public static List<string> getAllowedVMs()
        {
            return MEmuManager.allowedVMs;
        }

        public static void setMaxInstances(int numInstances)
        {
            MEmuManager.instances = new string[numInstances];
            MEmuManager.processes = new Process[numInstances];
            for (int i = 0; i < MEmuManager.instances.Length; i++)
            {
                MEmuManager.instances[i] = null;
                MEmuManager.processes[i] = null;
            }
            Settings.maxVMs = numInstances;
        }

        public static void killVM(int vm)
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = installPath + "\\MEmu.exe",
                    Arguments = "-clone:" + MEmuManager.instances[vm],
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            MEmuManager.instances[vm] = null;
            MEmuManager.processes[vm] = null;
            proc.Start();
            MEmuManager.runningVMs--;
        }

        public static void killAll()
        {
            for (int vm = 0; vm < processes.Length; vm++)
            {
                MEmuManager.killVM(vm);
            }
        }

        public static void bringToFront(int vm)
        {
            Process process = MEmuManager.processes[vm];
            IntPtr handle = process.MainWindowHandle;
            if (IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }
            SetForegroundWindow(handle);
        }
        //public static int startSpecificVM(string vmName)
        //{
        //    int vmInd = Array.IndexOf(instances, null);

        //    if (vmInd == -1)
        //        return -1; // sem espaço

        //    if (string.IsNullOrWhiteSpace(vmName))
        //        return -1;

        //    setMaxInstances(instances.Length); // Garante que 'processes' esteja inicializado

        //    // Clona se ainda não existir
        //    var checkProc = new Process
        //    {
        //        StartInfo = new ProcessStartInfo
        //        {
        //            FileName = Path.Combine(installPath, "memuc.exe"),
        //            Arguments = $"listvms",
        //            RedirectStandardOutput = true,
        //            UseShellExecute = false,
        //            CreateNoWindow = true
        //        }
        //    };
        //    checkProc.Start();
        //    string output = checkProc.StandardOutput.ReadToEnd();
        //    checkProc.WaitForExit();

        //    if (!output.Contains(vmName))
        //    {
        //        var clone = new Process
        //        {
        //            StartInfo = new ProcessStartInfo
        //            {
        //                FileName = Path.Combine(installPath, "memuc.exe"),
        //                Arguments = $"clone -i 0 -r {vmName}",
        //                UseShellExecute = false,
        //                RedirectStandardOutput = true,
        //                RedirectStandardError = true,
        //                CreateNoWindow = true
        //            }
        //        };
        //        clone.Start();
        //        clone.WaitForExit();
        //        Thread.Sleep(5000); // tempo para a VM ser registrada
        //    }

        //    // Start VM
        //    var startProc = new Process
        //    {
        //        StartInfo = new ProcessStartInfo
        //        {
        //            FileName = Path.Combine(installPath, "memuc.exe"),
        //            Arguments = $"start -n {vmName}",
        //            UseShellExecute = false,
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true,
        //            CreateNoWindow = true
        //        }
        //    };
        //    startProc.Start();
        //    startProc.WaitForExit();

        //    // Aguarda o processo aparecer
        //    int tentativas = 20;
        //    while (tentativas-- > 0)
        //    {
        //        Process[] all = Process.GetProcesses();
        //        foreach (var p in all)
        //        {
        //            if (p.MainWindowTitle == vmName)
        //            {
        //                processes[vmInd] = p;
        //                instances[vmInd] = vmName;
        //                resizeWindow(vmInd); // Ajusta o tamanho se necessário
        //                return vmInd;
        //            }
        //        }
        //        Thread.Sleep(1000);
        //    }

        //    return -1;
        //}
        public static int startSpecificVM(string vmName)
        {
            int vmInd = Array.IndexOf(instances, null);

            if (vmInd == -1 || string.IsNullOrWhiteSpace(vmName))
                return -1;

            setMaxInstances(instances.Length); // garante inicialização

            // Verifica se a VM já existe
            var checkProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(installPath, "memuc.exe"),
                    Arguments = $"listvms",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            checkProc.Start();
            string listOutput = checkProc.StandardOutput.ReadToEnd();
            checkProc.WaitForExit();

            // Tenta encontrar a VM e seu índice
            string[] linhas = listOutput.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int vmIndex = -1;

            foreach (string linha in linhas)
            {
                string[] partes = linha.Split(',');
                if (partes.Length > 1 && partes[1] == vmName)
                {
                    vmIndex = int.Parse(partes[0]); // achou o índice
                    break;
                }
            }

            // Se não encontrou, clona
            if (vmIndex == -1)
            {
                var clone = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(installPath, "memuc.exe"),
                        Arguments = $"clone -i 0 -r {vmName}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                clone.Start();
                clone.WaitForExit();

                Thread.Sleep(5000); // aguarda o sistema registrar a nova VM

                // Refaz leitura da lista
                checkProc.Start();
                listOutput = checkProc.StandardOutput.ReadToEnd();
                checkProc.WaitForExit();

                linhas = listOutput.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string linha in linhas)
                {
                    string[] partes = linha.Split(',');
                    if (partes.Length > 1 && partes[1] == vmName)
                    {
                        vmIndex = int.Parse(partes[0]);
                        break;
                    }
                }
            }

            if (vmIndex == -1)
                return -1; // falha total

            // Inicia VM pelo índice
            var startProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(installPath, "memuc.exe"),
                    Arguments = $"start -i {vmIndex}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            startProc.Start();
            startProc.WaitForExit();

            // Aguarda processo aparecer
            int tentativas = 20;
            while (tentativas-- > 0)
            {
                var proc = Process.GetProcesses().FirstOrDefault(p => p.MainWindowTitle.Contains(vmName));
                if (proc != null)
                {
                    processes[vmInd] = proc;
                    instances[vmInd] = vmName;
                    resizeWindow(vmInd);
                    return vmInd;
                }
                Thread.Sleep(1000);
            }

            return -1;
        }
    }
}
