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
            string starterFilesPath = FileWorker.CreateInitialDirectoryWithFiles();

            if (starterFilesPath != "" && starterFilesPath != null)
            {
                this.label1.Text = "Welcome to our senior design project: \n your starting files will be located at  " + starterFilesPath + " \n Please open these files and verify your initial values are set correctly";
            }
            else
            {
                this.label1.Text = "trouble loading start file";
            }

        }



        private void load_Click(object sender, EventArgs e)
        {
            bool loaded = true;
            try
            {
                //will probably want to load list of banned processes here
                bpc = FileWorker.LoadBadProcesses(bpc);
                gamerCache = FileWorker.LoadGamersFromExcel(gamerCache);
            }
            catch(Exception ex)
            {
                this.label1.Text = "Issue getting starting data due to following error \n " + ex.Message;
                loaded = false;
            }
            if (loaded)
            {
                if (gamerCache.GamerDictionary.Count > 0)
                {
                    this.button1.Visible = true;
                }
                else
                {
                    this.label1.Text = "No Gamers Loaded, please enter gamers into intial start file and restart application";
                }

                this.load.Visible = false;

                foreach (Gamer g in this.gamerCache.GamerDictionary.Values)
                {
                    string status = "waiting for wmi start";

                    this.listBox1.Items.Add(g.Name + " : " + status);
                }

                if (gamerCache.GamerDictionary.Count > 0)
                {
                    this.label1.Text = "Gamers Loaded: Click wmi to start getting processes";
                }
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.wmiActive = true;
            this.stop.Visible = true;
            this.PauseButton.Visible = true;
            this.button1.Visible = false;
            this.wmiProcess = new WMIPrcocess(this.gamerCache);   
            Thread thread1 = new Thread(UpdateUserInterface);
            thread1.Start();
            this.label1.Text = "WMI in process click end to move results or Pause to pause WMI session";
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
                            this.listBox1.Invoke(new Action(() => this.listBox1.Items.Insert(i, g.Name + " : wmi paused or complete : " + g.Processes.Keys.Count + "Different Processes recorded")));
                   
                        }
                    }
                }
                
            }
           
        }

        private void stop_Click(object sender, EventArgs e)
        {

            this.ResumeButton.Visible = false;
            this.wmiActive = false;
            this.PauseButton.Visible = false;
            this.stop.Visible = false;
            if (wmiProcess != null) {
                wmiProcess.EndProcessRetrieval(this.wmiActive);
                this.wmiProcess = null;
            }
            WaitForGamersToDisconnect();
            this.label1.Text = "WMI process ended Click button to load your results into excel";
            this.Load_Results_Into_Excel.Visible = true;
        }

      
        private void Load_Results_Into_Excel_Click(object sender, EventArgs e)
        {
            this.label1.Text = "Loading Results Please wait....";
            Thread.Sleep(1000);
            this.Load_Results_Into_Excel.Visible = false;
            string fileCreated = FileWorker.ViewResultsInExcel(gamerCache,bpc);
            Console.WriteLine("Load results to excel");
            if (fileCreated != null && fileCreated != "")
            {
                this.label1.Text = "Results have been loaded succesfully, \n you can view these results at " + fileCreated + ". \n Session complete. You may exit form application now";
            }
            else
            {
                this.label1.Text = "trouble loading results to excel file";
            }

            this.End_Session.Visible = true;
        }

        private void End_Session_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            this.stop.Visible = false;
            this.PauseButton.Visible = false;
            this.label1.Invoke(new Action(() => this.label1.Text = "Pausing... \n Please wait for processes to stop"));        
          
            this.wmiActive = false;
       
            wmiProcess.EndProcessRetrieval(this.wmiActive);
            WaitForGamersToDisconnect();
            this.wmiProcess = null;
          
            this.stop.Enabled = true;
            this.stop.Visible = true;
            //need to work on proper garbage collection

            Thread.Sleep(3000);
            this.label1.Text = "WMI process paused. Select End or Resume to Continue";
            this.ResumeButton.Visible = true;

        }

        private void ResumeButton_Click(object sender, EventArgs e)
        {
            this.label1.Text = "Resuming...";
            this.wmiActive = true;
            this.ResumeButton.Visible = false;
            Thread.Sleep(1000);
            this.button1.Visible = false;
            this.wmiProcess = new WMIPrcocess(this.gamerCache);
            Thread thread1 = new Thread(UpdateUserInterface);
            thread1.Start();
            this.label1.Text = "WMI in process click end to move results or Pause to pause WMI session";
            this.stop.Visible = true;
            this.PauseButton.Visible = true;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void WaitForGamersToDisconnect()
        {
            foreach(Gamer g in gamerCache.GamerDictionary.Values)
            {
                if(g.Connected == true)
                {
                    Thread.Sleep(1000);
                    WaitForGamersToDisconnect();
                       
                }
            }
        }

    }
}
