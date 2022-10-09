using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Management;
using System.Windows.Forms;

namespace Seniordesign
{
    class WMIPrcocess
    {
        private GamerCache gamerCache;

        public bool processesGathered = false;

        public WMIPrcocess(GamerCache gamerCache)
        {
            this.gamerCache = gamerCache;
            RunInitialProcessesRetrieval();
            List<string> processList = new List<string>();
           // Thread threadObject = new Thread(RunProcessWatching);
            new Thread(RunProcessWatching) { IsBackground = true, Name = "worker" }.Start();
            Console.WriteLine("Waiting for process events");
            do
            {
                Thread.Sleep(5000);
            } while (true);
            
        }
        private void RunInitialProcessesRetrieval()
        {
            try
            {
                foreach (Gamer g in gamerCache.GamerDictionary.Values)
                {
                    processesGathered = false;

                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\Cimv2",
                        "SELECT * FROM Win32_Process");

                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine("Win32_Process instance");
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine("Caption: {0}", queryObj["Caption"]);
                        if (!g.Processes.Contains(queryObj["Caption"].ToString()))
                        {
                            g.Processes.Add(queryObj["Caption"].ToString());
                        }
                    }

                    processesGathered = true;
                }
            }

            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        private void RunProcessWatching()
        {
            try
            {
                string queryString = "SELECT * FROM __InstanceCreationEvent WITHIN .025 WHERE TargetInstance ISA 'Win32_Process'";
                var startWatch = new ManagementEventWatcher(@"\\.\root\CIMV2",queryString);
                startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
                startWatch.Start();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("+ Started Process in GREEN");

         
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ex);
            }
        }

        static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            try
            {
                var proc = GetProcessInfo(e);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("+ {0} ({1}) {2} [{3}]", proc.ProcessName, proc.PID, proc.CommandLine, proc.User);
                //            Console.WriteLine("+ {0} ({1}) {2} > {3} ({4}) {5}", proc.ProcessName, proc.PID, proc.CommandLine, pproc.ProcessName, pproc.PID, pproc.CommandLine);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ex);
            }
        }

        static ProcessInfo GetProcessInfo(EventArrivedEventArgs e)
        {
            ProcessInfo p = new ProcessInfo();
            try
            {
                var targetInstance = ((System.Management.ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value);
                p.ProcessName = targetInstance.Properties["Caption"].Value.ToString();
                p.PID = targetInstance.Properties["ProcessId"].Value.ToString();
            }

            catch (ManagementException) {};
            return p;
        }


        public GamerCache GetGamerWithProcesses()
        {
            return gamerCache;
        }

        internal class ProcessInfo
        {
            public string ProcessName { get; set; }
            public string PID { get; set; }
            public string CommandLine { get; set; }
            public string UserName { get; set; }
            public string UserDomain { get; set; }
            public string User
            {
                get
                {
                    if (string.IsNullOrEmpty(UserName))
                    {
                        return "";
                    }
                    if (string.IsNullOrEmpty(UserDomain))
                    {
                        return UserName;
                    }
                    return string.Format("{0}\\{1}", UserDomain, UserName);
                }
            }
        }
    }

}
