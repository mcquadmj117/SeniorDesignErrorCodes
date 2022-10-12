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
        //private GamerCache gamerCache;

        public bool endProcessRetrieval = false;

        public List<Thread> threadList = new List<Thread>();

        public WMIPrcocess(GamerCache gamerCache)
        {
            // this.gamerCache = gamerCache;
            foreach (Gamer gamer in gamerCache.GamerDictionary.Values)
            {
                RunInitialProcessesRetrieval(gamer);
            }
          
            // Thread threadObject = new Thread(RunProcessWatching);
            foreach (Gamer gamer in gamerCache.GamerDictionary.Values) {
                threadList.Add( new Thread(() => RunProcessWatching(gamer)){ IsBackground = true, Name = gamer.Name });

               // new Thread(RunProcessWatching(gamer.Name)) { IsBackground = true, Name = gamer.Name }.Start();
            }

        foreach(Thread t in threadList)
            {
                t.Start();
            }
            Console.WriteLine("Waiting for process events");
            //do
            //{
            //  //  Thread.Sleep(5000);
            //} while (true);
            
        }
        private void RunInitialProcessesRetrieval(Gamer g)
        {
            try
            {
               ///  foreach (Gamer g in gamerCache.GamerDictionary.Values)
                //{
                   // processesGathered = false;

                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\Cimv2",
                        "SELECT * FROM Win32_Process");

                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        Process tempProcess = new Process();
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine("Win32_Process instance");
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine("Caption: {0}", queryObj["Caption"]);
                        tempProcess.ProcessName = queryObj["Caption"].ToString();
                        tempProcess.Time = DateTime.Now;
                        tempProcess.Starting = true;
                        tempProcess.ProcessId = queryObj["PROCESSID"]?.ToString();
                    if (g.Processes == null)
                    {
                        g.Processes = new List<Process>();
                    }
                        g.Processes.Add(tempProcess);
                    }

                   // processesGathered = true;
               // }
            }

            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        public void RunProcessWatching(Gamer g)
        {
            try
            {
                string queryString = "SELECT * FROM __InstanceCreationEvent WITHIN .025 WHERE TargetInstance ISA 'Win32_Process'";
                var startWatch = new ManagementEventWatcher(@"\\.\root\CIMV2",queryString);
                // startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
                startWatch.EventArrived += (sender, eventArgs) =>
                {
                    try
                    {
                        if (this.endProcessRetrieval)
                        {
                            startWatch.Stop();
                        }
                        var proc = GetProcessInfo(eventArgs);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("+{0} {1} ({2}) {3} [{4}]",g.Name, proc.ProcessName, proc.PID, proc.CommandLine, proc.User);
                        //            Console.WriteLine("+ {0} ({1}) {2} > {3} ({4}) {5}", proc.ProcessName, proc.PID, proc.CommandLine, pproc.ProcessName, pproc.PID, pproc.CommandLine);
                        Process tempProcess = new Process();
                        tempProcess.ProcessName = proc.ProcessName;
                        tempProcess.Time = DateTime.Now;
                        tempProcess.Starting = true;
                        tempProcess.ProcessId = proc.PID;
                        g.Processes.Add(tempProcess);
                                       
                      
                    }
                    catch (Exception ex)
                    {
                        g.ExceptionLog.Add(ex.ToString());
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(ex);
                        throw;
                    }
                };

                startWatch.Start();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("+ Started Process in GREEN");


     
            }
            catch (Exception ex)
            {
                g.ExceptionLog.Add(ex.ToString());
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ex);
                throw;
            }
        }

        //static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        //{
        //    try
        //    {
        //        var proc = GetProcessInfo(e);
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.WriteLine("+{0} {1} ({2}) {3} [{4}]", proc.ProcessName, proc.PID, proc.CommandLine, proc.User);
        //        //            Console.WriteLine("+ {0} ({1}) {2} > {3} ({4}) {5}", proc.ProcessName, proc.PID, proc.CommandLine, pproc.ProcessName, pproc.PID, pproc.CommandLine);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Yellow;
        //        Console.WriteLine(ex);
        //    }
        //}

        static ProcessInfo GetProcessInfo(EventArrivedEventArgs e)
        {
            ProcessInfo p = new ProcessInfo();
            try
            {
                var targetInstance = ((System.Management.ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value);
                p.ProcessName = targetInstance.Properties["Caption"].Value.ToString();
                p.PID = targetInstance.Properties["ProcessId"].Value.ToString();
            }

            catch (Exception ex) { throw ex; }
            return p;
        }


        //public GamerCache GetGamerWithProcesses()
        //{
        //    foreach(Thread t in this.threadList)
        //    {
        //        t.Abort();
        //    }
        //    return gamerCache;
        //}

        public void EndProcessRetrieval()
        {
            this.endProcessRetrieval = true;
            foreach (Thread t in this.threadList)
            {
                t.Abort();
            }
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
