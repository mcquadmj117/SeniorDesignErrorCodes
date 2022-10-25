using Seniordesign.DataClasses_Enums;
using Seniordesign.Processes_Workers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Seniordesign
{
    public partial class Form1 : Form
    {
        GamerCache gamerCache = new GamerCache();
        BadProcessCache bpc = new BadProcessCache();
        WMIPrcocess wmiProcess;
        bool wmiActive = false;

        Regex Name_Extraction_Regex = new Regex("([^\\s]+)");


        public Form1()
        {
            InitializeComponent();
            this.label1.Text = "Welcome to our senior design project: Click load to load the gamers from excel sheet into cache";
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
                string status = "waiting for wmi start";

                this.listBox1.Items.Add(g.Name +" : " + status);
            }

            this.label1.Text = "Gamers Loaded: Click wmi to start getting processes";

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.stop.Visible = true;
            this.button1.Visible = false;
            this.wmiProcess = new WMIPrcocess(this.gamerCache);
            this.wmiActive = true;
            Thread thread1 = new Thread(UpdateUserInterface);
            thread1.Start();
            this.label1.Text = "WMI in process click stop to end";
        }

        public void UpdateUserInterface()
        {
          
            Console.WriteLine("updating interface");

            foreach(Gamer g in this.gamerCache.GamerDictionary.Values.ToList())
            {

                
                for (int i = 0; i < this.listBox1.Items.Count; i++)
                {
                    string status = "";
                    string name = Name_Extraction_Regex.Match(this.listBox1.Items[i].ToString()).ToString();
                    if(name == g.Name){
                       
                        if (g.ExceptionLog.Count > 0){
                             status = g.Connected == true
                                ? "Connected : " + g.ExceptionLog?.Last()?.LogMessage?.ToString()
                                : "Disconnected : " + g.ExceptionLog?.Last()?.LogMessage?.ToString();
                        }
                        else
                        {
                             status = g.Connected == true
                               ? "Connected : "
                               : "Disconnected : ";
                        }
                        string previousVal = listBox1.Items[i].ToString();

                        string listString = g.Name + " : " + status;

                        if(listString.Replace(".","") == previousVal.Replace(".", ""))
                        {
                            listString = previousVal + ".";
                          listString =   listString.Replace(".....", "");
                        }


                        this.listBox1.Invoke(new Action(() => this.listBox1.Items.RemoveAt(i)));
                        this.listBox1.Invoke(new Action(() => this.listBox1.Items.Insert(i, listString)));
                    }
                }
 
            }


            Thread.Sleep(3000);
            if(wmiActive)
            {
                this.UpdateUserInterface();
            }
            else
            {
                foreach (Gamer g in this.gamerCache.GamerDictionary.Values.ToList())
                {
                    for (int i = 0; i < this.listBox1.Items.Count; i++)
                    {
                        string status = "";
                        string name = Name_Extraction_Regex.Match(this.listBox1.Items[i].ToString()).ToString();
                        if (name == g.Name)
                        {
                            status = g.Connected == true
                              ? "Connected : "
                              : "Disconnected : ";
                            this.listBox1.Invoke(new Action(() => this.listBox1.Items.RemoveAt(i)));
                            this.listBox1.Invoke(new Action(() => this.listBox1.Items.Insert(i, g.Name + " : wmi complete : " + g.Processes.Count + " Process Instances recorded")));
                   
                        }
                    }
                }
            }
           
        }

        private void stop_Click(object sender, EventArgs e)
        {
            this.label1.Text = "WMI process ended Click button to load your results into excel";
            this.wmiActive = false;
            this.stop.Enabled = false;
            wmiProcess.EndProcessRetrieval();
            //need to work on proper garbage collection
            this.wmiProcess = null;

            

            this.Load_Results_Into_Excel.Visible = true;
        }

        private void Load_Results_Into_Excel_Click(object sender, EventArgs e)
        {
            this.Load_Results_Into_Excel.Visible = false;
            FileWorker.ViewResultsInExcel(gamerCache,bpc);
            Console.WriteLine("Load results to excel");
            this.label1.Text = "Results loaded, session complete. You may exit form application now";
        }

     
    }
}
