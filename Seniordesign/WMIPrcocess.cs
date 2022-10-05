using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            StartProcessesGather();
        }
        public void StartProcessesGather()
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


        public GamerCache GetGamerWithProcesses()
        {
            return gamerCache;
        }
    }

}
