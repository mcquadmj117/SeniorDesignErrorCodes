using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Management;
using System.Windows.Forms;
using Seniordesign.DataClasses_Enums;


namespace Seniordesign.Processes_Workers
{
    class WMIPrcocess
    {
        public delegate void WatchProc(Gamer g);
    
        public bool endProcessRetrieval = false;

        public List<Thread> threadList = new List<Thread>();

        public WMIPrcocess(GamerCache gamerCache)
        {

            //foreach (Gamer gamer in gamerCache.GamerDictionary.Values)
            //{
            //    try
            //    {
            //        EstablishInitialManagementScopeConnection2(gamer);

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("error when trying to connect to " + gamer.Name + "'s machine: " + ex.Message);
            //        LogItem li = new LogItem();
            //        li.LogMessage = ex.Message;
            //        li.Time = DateTime.Now;
            //        gamer.ExceptionLog.Add(li);
            //    }
            //}


            // Thread threadObject = new Thread(RunProcessWatching);

     
            foreach (Gamer gamer in gamerCache.GamerDictionary.Values)
            {
                threadList.Add(new Thread(() => EstablishInitialManagementScopeConnection(gamer)) { IsBackground = true, Name = gamer.Name });

                // new Thread(RunProcessWatching(gamer.Name)) { IsBackground = true, Name = gamer.Name }.Start();
            }

            foreach (Thread t in threadList)
            {
                t.Start();
            }



            #region WORKING LOCAL WMI STARTUP FUNCTIONS
            //// this.gamerCache = gamerCache;
            //foreach (Gamer gamer in gamerCache.GamerDictionary.Values)
            //{
            //    EstablishInitialManagementScopeConnection2(gamer);
            //}

            //// Thread threadObject = new Thread(RunProcessWatching);
            //foreach (Gamer gamer in gamerCache.GamerDictionary.Values)
            //{
            //    threadList.Add(new Thread(() => RunProcessWatching2(gamer)) { IsBackground = true, Name = gamer.Name });

            //    // new Thread(RunProcessWatching(gamer.Name)) { IsBackground = true, Name = gamer.Name }.Start();
            //}

            //foreach (Thread t in threadList)
            //{
            //    t.Start();
            //}
            //Console.WriteLine("Waiting for process events");
            ////do
            ////{
            ////  //  Thread.Sleep(5000);
            ////} while (true);
            #endregion
        }

        private void EstablishInitialManagementScopeConnection(Gamer g, int loopCount = -1, ManagementScope scope = null)
        {
            try
            {
                if (!this.endProcessRetrieval)
                {
                    if (scope == null || !scope.IsConnected)
                    {

                        string myCompName = System.Environment.MachineName;

                        ConnectionOptions options = new ConnectionOptions();
                        if (g.Computer_Name != myCompName)
                        {
                            options.Password = g.Password;
                            options.Username = g.Username;
                        }

                        options.EnablePrivileges = true;
                        options.Impersonation = System.Management.ImpersonationLevel.Impersonate;
                        string machineName = g.Computer_Name;

                        scope = new ManagementScope("\\\\" + machineName + "\\root\\cimv2", options);

                        scope.Connect();
                    }
                    if (scope.IsConnected)
                    {
                        g.Connected = true;
                        LogItem connected = new LogItem();
                        connected.Time = DateTime.Now;
                        connected.GoodLog = true;
                        connected.LogMessage = g.Name + " CONNECTED: Grabbing Current Processes : Pulse Count: " + loopCount.ToString();
                        g.ExceptionLog.Add(connected);
                        Console.WriteLine(g.Name + " CONNECTED: Grabbing Current Processes");
                    }

                    ObjectQuery query = new ObjectQuery("SELECT Caption, ProcessID, ExecutablePath FROM Win32_Process");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                    int processCount = 0;
                    foreach (ManagementObject queryObj in searcher.Get())
                    {

                        Process tempProcess = new DataClasses_Enums.Process();
                        
                        tempProcess.ProcessName = queryObj["Caption"].ToString();
                        tempProcess.Time = DateTime.Now;
                        tempProcess.Starting = true;
                        tempProcess.ProcessId = queryObj["PROCESSID"]?.ToString();
                        tempProcess.ExecPath = queryObj["ExecutablePath"]?.ToString() != null ? queryObj["ExecutablePath"]?.ToString() : "path not available";

                        if (g.Processes == null)
                        {
                            g.Processes = new Dictionary<string, List<Process>>();
                        }
                        g.AddProcessToGamer(tempProcess);
                        processCount++;
           
                    }

                    Console.WriteLine("retrieved " + processCount + " current running processes from" + g.Name);

                    if (loopCount >= 0 && loopCount < 5 && !this.endProcessRetrieval)
                    {
                        loopCount++;
                        Thread.Sleep(5000);
                        EstablishInitialManagementScopeConnection(g, loopCount, scope != null && scope.IsConnected ? scope : null );
                    }
                    else if(!this.endProcessRetrieval)
                    {
                        if(loopCount >= 0)
                        {
                            LogItem li = new LogItem();
                            li.LogMessage = "Pulse has reached 5 rotations : Attempting to Switch to Watching Convention";
                            li.GoodLog = true;
                            li.Time = DateTime.Now;
                            g.ExceptionLog.Add(li);
                        }

                        RunProcessWatching(g);
                    }
                }
            }
            catch (Exception ex)
            {
                g.Connected = false;
                LogItem li = new LogItem();
                var message = "Process Retrieval for " +g.Name+ " :( Failed Due to error: " + ex.Message + "setting pulse convention count to 0";
                li.LogMessage = message;
                li.Time = DateTime.Now;
                g.ExceptionLog.Add(li);
                Console.WriteLine("Process Retrieval for " + g.Name + " :( Failed Due to error: " + ex.Message + "setting pulse convention count to 0");
       
                Thread.Sleep(5000);
                EstablishInitialManagementScopeConnection(g, 0);


            }
        }

        public void RunProcessWatching(Gamer g)
        {            
                try
                {
                
                if (!this.endProcessRetrieval)
                {
                   
                    string myCompName = System.Environment.MachineName;

                    ConnectionOptions options = new ConnectionOptions();
                    if (g.Computer_Name != myCompName)
                    {
                        options.Password = g.Password;
                        options.Username = g.Username;
                    }

                    options.Impersonation = System.Management.ImpersonationLevel.Impersonate;
                    options.EnablePrivileges = true;


                    string machineName = g.Computer_Name;

                    ManagementScope scope = new ManagementScope("\\\\" + machineName + "\\root\\cimv2", options);



                    scope.Connect();

                    if (scope.IsConnected)
                    {
                        LogItem connected = new LogItem();
                        connected.Time = DateTime.Now;
                        connected.GoodLog = true;
                        connected.LogMessage = g.Name + " CONNECTED  Process Watching";
                        g.Connected = true;
                        g.ExceptionLog.Add(connected);
                        g.Connected = true;
                    }


                    EventQuery queryString = new EventQuery("SELECT * FROM __InstanceCreationEvent WITHIN .025 WHERE TargetInstance ISA 'Win32_Process'");

                  

                    ManagementEventWatcher watcher =
               new ManagementEventWatcher(scope, queryString);
                    //startWatch.Start();
                    ManagementBaseObject e = null;
                    while (!this.endProcessRetrieval)
                    {
                       
                            e = watcher.WaitForNextEvent();
                        
                        var proc = GetProcessInfo(e);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Retrieved Process From watch:");
                        Console.WriteLine("+ {0} {1} {2} ({3}) {4} [{5}]", g.Name, proc.ProcessName, proc.ExecPath, proc.PID, proc.CommandLine, proc.User);
                        //            Console.WriteLine("+ {0} ({1}) {2} > {3} ({4}) {5}", proc.ProcessName, proc.PID, proc.CommandLine, pproc.ProcessName, pproc.PID, pproc.CommandLine);
                        Process tempProcess = new Process();
                        tempProcess.ProcessName = proc.ProcessName;
                        tempProcess.Time = proc.TimeCreated;
                        tempProcess.Starting = true;
                        tempProcess.ProcessId = proc.PID;
                        tempProcess.ExecPath = proc.ExecPath;
                        g.AddProcessToGamer(tempProcess);
                    }

                    if (this.endProcessRetrieval)
                    {
                        watcher.Stop();
                    }

                }
            }
            catch (Exception ex)
            {
                g.Connected = false;
                LogItem li = new LogItem();
                li.LogMessage = "watch failed for " + g.Name+ " , switching to pulse convention due to following error:: " + ex.Message;
                li.Time = DateTime.Now;
                g.ExceptionLog.Add(li);
                Console.WriteLine("watch failed for " + g.Name + " , switching to pulse convention due to following error:: " + ex.Message);
                Thread.Sleep(5000);
                EstablishInitialManagementScopeConnection(g, 0);
            }
        }

        static ProcessInfo GetProcessInfo(ManagementBaseObject mbo)
        {
            ProcessInfo p = new ProcessInfo();
            if (mbo != null)
            {


                try
                {
                    var targetInstance = (System.Management.ManagementBaseObject)mbo.Properties["TargetInstance"]?.Value;
                    p.ProcessName = targetInstance?.Properties["Caption"]?.Value?.ToString();
                    p.PID = targetInstance?.Properties["ProcessId"]?.Value?.ToString();
                    p.ExecPath = targetInstance?.Properties["ExecutablePath"]?.Value?.ToString() != null
                        ? targetInstance?.Properties["ExecutablePath"]?.Value?.ToString()
                        : "Path Not Available";
                    //todo convert uint64 for timecreated
                    // p.TimeCreated = DateTime.FromBinary((Int64)e.NewEvent?.Properties["TIME_CREATED"]?.Value);
                    p.TimeCreated = DateTime.Now;
                }
                catch (Exception ex) { throw ex; }
               
            }
            return p;


        }

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
            public string ExecPath { get; set; }
            public string UserName { get; set; }
            public string UserDomain { get; set; }
            public DateTime TimeCreated { get; set; }
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
