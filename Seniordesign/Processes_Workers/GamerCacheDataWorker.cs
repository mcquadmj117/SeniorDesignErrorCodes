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
            List<string> distinctProcessNameList = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.OrderBy(pt=>pt.Time).Select(p => p.ProcessName).Distinct().ToList();
            return distinctProcessNameList;
        }
        public static List<string> GetExclusiveProcessNames(string gamerName, GamerCache gc) 
        {
            List<string> exclusiveProcessNames = new List<string>();
            List<string> targetGamerProcessNameList = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.OrderBy(pt => pt.Time).Select(p=>p.ProcessName).Distinct().ToList();
            foreach (string s in targetGamerProcessNameList)
            {
               bool distinctFlag = true;
                foreach (Gamer g in gc.GamerDictionary.Values.Where(g => g.Name != gamerName).ToList()) 
                {
                    if (g.Processes.Select(p => p.ProcessName).Distinct().ToList().Contains(s))
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
            List<string> targetGamerProcessNameList = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.OrderBy(pt => pt.Time).Select(p => p.ProcessName).Distinct().ToList();
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

            List<string> targetGamerProcessNameList = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.OrderBy(pt => pt.Time).Select(p => p.ProcessName).Distinct().ToList();
         
            foreach(string p in targetGamerProcessNameList)
            {
                string targetProcessTime = "no time available";
                Process proc = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.FirstOrDefault(pr => pr.ProcessName == p);
                targetProcessTime = proc.Time.ToString("hh:mm:ss:ff") ?? targetProcessTime;
                firstInstanceTimeList.Add(targetProcessTime);
            }
            return firstInstanceTimeList;
        }

        public static List<string> GetExecPathForFirstInstances(string gamerName, GamerCache gc)
        {
            List<string> processPathList = new List<string>();

            List<string> targetGamerProcessNameList = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.OrderBy(pt => pt.Time).Select(p => p.ProcessName).Distinct().ToList();

            foreach (string p in targetGamerProcessNameList)
            {
                string targetProcessPath = "no Path Available";
                Process proc = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).Processes.FirstOrDefault(pr => pr.ProcessName == p);
                targetProcessPath = proc.ExecPath?.ToString() ?? targetProcessPath;
                processPathList.Add(targetProcessPath);
            }
            return processPathList;
        }

        public static List<LogItem> GetLogsForGamer(string gamerName, GamerCache gc)
        {
            List<LogItem> targetGamerLogList = gc.GamerDictionary.Values.FirstOrDefault(g => g.Name == gamerName).ExceptionLog.OrderBy(li => li.Time).ToList();
            return targetGamerLogList;

        }
    }
}
