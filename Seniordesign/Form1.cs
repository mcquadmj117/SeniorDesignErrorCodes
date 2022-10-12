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
        WMIPrcocess wmiProcess;

        public Form1()
        {
            InitializeComponent();
        }

        private void load_Click(object sender, EventArgs e)
        {
            gamerCache = Preload.LoadGamersFromExcel(gamerCache);
            if (gamerCache.GamerDictionary.Count > 0)
            {
                this.button1.Visible = true;
            }
            this.load.Visible = false;
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
            this.wmiProcess = null;

            foreach (Gamer g in this.gamerCache.GamerDictionary.Values)
            {
                foreach (Process p in g.Processes)
                {
                    this.listBox1.Items.Add(p.ProcessName + ":" + p.ProcessId + ":"+ p.Time.ToString( )+ ":" + (p.Starting ? "Starting" : "Stopping") );
                }

                Console.WriteLine(g.Processes.ToString());
            }
        
        }

     
    }
}
