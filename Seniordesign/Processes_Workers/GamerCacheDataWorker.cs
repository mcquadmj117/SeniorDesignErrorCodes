using Seniordesign.DataClasses_Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seniordesign.Processes_Workers
{
    static class GamerCacheDataWorker
    {
       public static List<string> GetDistinctProcessesNames(string gamerName, GamerCache gc)
        {
            List<string> distinctProcessNameList = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.Keys.ToList();
            //foreach (List<Process> listOfProcesses in  gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.Values)
            //{
            //    foreach(Process p in listOfProcesses)
            //    {
            //        distinctProcessNameList.Add(p.ProcessName);
            //    }
            //}
           //     gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.Values.OrderBy(pt=>pt.Time).Select(p => p.ProcessName).Distinct().ToList();
            return distinctProcessNameList;
        }
        public static List<string> GetExclusiveProcessNames(string gamerName, GamerCache gc) 
        {
            List<string> exclusiveProcessNames = new List<string>();
            List<string> targetGamerProcessNameList = GetDistinctProcessesNames(gamerName, gc);
            foreach (string s in targetGamerProcessNameList)
            {
               bool distinctFlag = true;
                foreach (Gamer g in gc.GamerDictionary.Values.Where(g => g.Name != gamerName).ToList()) 
                {
                    if (g.Processes.Keys.ToList().Contains(s))
                    {
                        distinctFlag = false;
                    }
                }
                if(distinctFlag == true)
                {
                    exclusiveProcessNames.Add(s);
                }

            }
                return exclusiveProcessNames;
        }
        public static List<string> GetBadProcessNames(string gamerName, GamerCache gc, BadProcessCache bpc)
        {
            List<string> gamersBadProcesses = new List<string>();
            List<string> targetGamerProcessNameList = GetDistinctProcessesNames(gamerName,gc);
            foreach(string s in targetGamerProcessNameList)
            {
                string ps = s;
                ps = ps.Replace(" ", "");
                ps =  ps.Replace(".exe", "");
                ps = ps.ToLower();
                ps = ps.Trim();
              if(bpc.BadProcesses.Contains(ps))
                {
                    gamersBadProcesses.Add(s);
                }
            }
            return gamersBadProcesses;
        }

        public static List<string> GetFirstInstanceTimeList(string gamerName, GamerCache gc)
        {
            List<string> firstInstanceTimeList = new List<string>();

            List<string> targetGamerProcessNameList = GetDistinctProcessesNames(gamerName, gc);
         
            foreach(string p in targetGamerProcessNameList)
            {
                string targetProcessTime = "no time available";
                Process proc = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes[p].First();
                targetProcessTime = proc.Time.ToString("hh:mm:ss:ff") ?? targetProcessTime;
                firstInstanceTimeList.Add(targetProcessTime);
            }
            return firstInstanceTimeList;
        }

        public static List<string> GetLastInstanceTimeList(string gamerName, GamerCache gc)
        {
            List<string> lastInstanceTimeList = new List<string>();

            List<string> targetGamerProcessNameList = GetDistinctProcessesNames(gamerName, gc);

            foreach (string p in targetGamerProcessNameList)
            {
                string targetProcessTime = "no time available";
                Process proc = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes[p].Last();
                targetProcessTime = proc.Time.ToString("hh:mm:ss:ff") ?? targetProcessTime;
                lastInstanceTimeList.Add(targetProcessTime);
            }
            return lastInstanceTimeList;
        }

        public static List<string> GetExecPathForFirstInstances(string gamerName, GamerCache gc)
        {
            List<string> processPathList = new List<string>();

            List<string> targetGamerProcessNameList = GetDistinctProcessesNames(gamerName, gc);

            foreach (string p in targetGamerProcessNameList)
            {
                string targetProcessPath = "no Path Available";
                Process proc = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes[p].First();
                targetProcessPath = proc.ExecPath?.ToString() ?? targetProcessPath;
                processPathList.Add(targetProcessPath);
            }
            return processPathList;
        }

        public static List<LogItem> GetLogsForGamer(string gamerName, GamerCache gc)
        {
            List<LogItem> targetGamerLogList = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).ExceptionLog.OrderBy(li => li.Time).ToList();
            List<LogItem> targetGamerExpectedProcessLogList = GetExpectedProcessLogsForGamer(gamerName, gc);

           targetGamerExpectedProcessLogList.AddRange(targetGamerLogList);

            return targetGamerExpectedProcessLogList;

        }

        public static List<LogItem> GetExpectedProcessLogsForGamer(string gamerName, GamerCache gc)
        {
            List<LogItem> loglist = new List<LogItem>();
           List<string> expectedProcesses = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Expected_Processes;
            if (expectedProcesses.Count > 0)
            {
                List<string> distinctProcessStrings = GetDistinctProcessesNames(gamerName, gc);
                foreach (string xProc in expectedProcesses)
                {
                    bool foundFlag = false;
                    foreach (string proc in distinctProcessStrings)
                    {
                        
                            if (proc.ToLower().Replace(".exe", "").Replace(" ","").Equals(xProc.ToLower().Replace(".exe", "").Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                            {
                                foundFlag = true;
                                break;
                            }
                                        
                    }

                    if (foundFlag == true)
                    {
                        LogItem li = new LogItem();
                        li.GoodLog = true;
                        li.LogMessage = "Expected process " + xProc + " was found";
                        li.Time = default;
                        loglist.Add(li);
                    }
                    else
                    {
                        LogItem li = new LogItem();
                        li.GoodLog = false;
                        li.LogMessage = "Expected process " + xProc + " was not found";
                        li.Time = default;
                        loglist.Add(li);
                    }

                }
            }
            return loglist;

        }
    }
}
