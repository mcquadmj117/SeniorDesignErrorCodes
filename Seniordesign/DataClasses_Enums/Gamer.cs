using Seniordesign.DataClasses_Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seniordesign.DataClasses_Enums
{
    class Gamer
    {
        public string Name { get; set; }
        public string Computer_Name { get; set; }
        public string Mac_Address { get; set; }
        public string IP_Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Expected_Processes { get; set; } = new List<string>();

        public List<string> Bad_Processes { get; set; } = new List<string>();
        public Dictionary<string, List<Process>> Processes { get; set; } = new Dictionary<string, List<Process>>();

        public bool Connected { get; set; } = false;

        public List<LogItem> ExceptionLog { get; set; } = new List<LogItem>();


        internal void AddProcessToGamer(Process process, bool initial = false)
        {
            //will only store the first and latest instances of a process
            if (process != null)
            {
                string processNameFormatted = process.ProcessName;
                processNameFormatted = processNameFormatted.Replace(" ", "");
                processNameFormatted = processNameFormatted.Replace(".exe", "");
                processNameFormatted = processNameFormatted.ToLower();
                processNameFormatted = processNameFormatted.Trim();

                process.ProcessName = processNameFormatted;


                if (!this.Processes.ContainsKey(process.ProcessName))
                {
                   
                    this.Processes.Add(process.ProcessName, new List<Process>());
                    this.Processes[process.ProcessName].Add(process);
                  
                    foreach(string procString in this.Expected_Processes)
                    {
                        bool expectedProcFound = false;

                        if (process.ProcessName.Contains(procString))
                        {
                            expectedProcFound = true;
                            this.FoundProcs.Add(procString);
                        }

                        if (expectedProcFound)
                        {
                            LogItem li = new LogItem();
                            li.LogMessage = this.Name + " : " + "Possible Expected Proccess Found For Gamer : " + process.ProcessName;
                            li.GoodLog = true;
                            li.CriticalMessage = true;
                            li.Time = DateTime.Now;
                            this.ExceptionLog.Add(li);

                            if (!initial)
                            {
                                if (this.GetMissingProcesses().Count > 0)
                                {
                                    var missingProcs = this.GetMissingProcesses();
                                    LogItem li2 = new LogItem();
                                    li.LogMessage = this.Name + " : Expected Proccesses Still Missing" + " : " + String.Join(", ", missingProcs);
                                    li.GoodLog = false;
                                    li.CriticalMessage = true;
                                    li.Time = DateTime.Now;
                                    this.ExceptionLog.Add(li2);
                                }
                            }
                        }
                    }

                }
                else if(this.Processes[process.ProcessName].Count > 1)
                {
                    this.Processes[process.ProcessName][1] = process;
                }
                else
                {
                    this.Processes[process.ProcessName].Add(process);
                 
                }
                

            }
        }

        internal void AddExpectedProcessToGamer(String processString)
        {
            //will only store the first and latest instances of a process
            if (processString != null)
            {
                
                processString = processString.Replace(" ", "");
                processString = processString.Replace(".exe", "");
                processString = processString.ToLower();
                processString = processString.Trim();
                if (!this.Expected_Processes.Contains(processString)){
                    this.Expected_Processes.Add(processString);
                }

            }
        }


        public List<String> FoundProcs { get; set; } = new List<string>();
        public List<String> GetMissingProcesses()
        {
            List<string> missingProcs = new List<string>();
            foreach (string expectedproc in this.Expected_Processes)
            {
                if (!this.FoundProcs.Contains(expectedproc))
                {
                    missingProcs.Add(expectedproc);
                }
            }
           
            if(missingProcs.Count > 0)
            {
                return missingProcs;
            }
            else
            {
                return new List<string>();
            }
        }
    }
}

