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


        internal void AddProcessToGamer(Process process)
        {
            //will only store the first and latest instances of a process
            if (process != null)
            {

                if (!this.Processes.ContainsKey(process.ProcessName))
                {
                    // this.Processes[process.ProcessName] = new List<Process> { };
                    this.Processes.Add(process.ProcessName, new List<Process>());
                    this.Processes[process.ProcessName].Add(process);
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

               if (!this.Expected_Processes.Contains(processString)){
                    this.Expected_Processes.Add(processString);
                }

            }
        }
    }
}

