using Seniordesign.DataClasses_Enums;
using Seniordesign.Processes_Workers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Seniordesign
{
    public partial class Form1 : Form
    {
        GamerCache gamerCache = new GamerCache();
        BadProcessCache bpc = new BadProcessCache();
        WMIPrcocess wmiProcess;

        public Form1()
        {
            InitializeComponent();
        }

        private void load_Click(object sender, EventArgs e)
        {
            //will probably want to load list of banned processes here
            bpc = FileWorker.LoadBadProcesses(bpc);
            gamerCache = FileWorker.LoadGamersFromExcel(gamerCache);
            if (gamerCache.GamerDictionary.Count > 0)
            {
                this.button1.Visible = true;
            }

            this.load.Visible = false;

            foreach (Gamer g in this.gamerCache.GamerDictionary.Values)
            {
                this.listBox1.Items.Add(g.Name);
            }
         
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.stop.Visible = true;
            this.button1.Visible = false;
            this.wmiProcess = new WMIPrcocess(this.gamerCache);                     
        }

        private void stop_Click(object sender, EventArgs e)
        {
            this.stop.Enabled = false;
            wmiProcess.EndProcessRetrieval();
            //need to work on proper garbage collection
            this.wmiProcess = null;

            foreach (Gamer g in this.gamerCache.GamerDictionary.Values)
            {
                foreach (Process p in g.Processes)
                {
                    this.listBox1.Items.Add(p.ProcessName + ":" + p.ProcessId + ":"+ p.Time.ToString( )+ ":" + (p.Starting ? "Starting" : "Stopping") );
                }

                Console.WriteLine(g.Processes.ToString());
            }

            this.Load_Results_Into_Excel.Visible = true;
        }

        private void Load_Results_Into_Excel_Click(object sender, EventArgs e)
        {
            this.Load_Results_Into_Excel.Visible = false;
            FileWorker.ViewResultsInExcel(gamerCache,bpc);
            Console.WriteLine("Load results to excel");         
        }
    }
}
