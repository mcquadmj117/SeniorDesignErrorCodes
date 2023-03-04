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
using System.Runtime.CompilerServices;

namespace Seniordesign
{
    public partial class Form1 : Form
    {
        GamerCache gamerCache = new GamerCache();
        BadProcessCache bpc = new BadProcessCache();
        WMIPrcocess wmiProcess;
        bool wmiActive = false;
        List<string> critList = new List<string>();

        Regex Name_Extraction_Regex = new Regex("([^\\s]+)");
        private System.Windows.Forms.Timer timer1;
     

        private void timer1_Tick(object sender, EventArgs e)
        {
            //UpdateUserInterface();
            Thread thread1 = new Thread(UpdateUserInterface);
            thread1.Start();
        }

        public Form1()
        {
            InitializeComponent();
            InitTimer();
            
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

        public void InitTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 3000; // in miliseconds
    
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
                    this.CritNotLabel.Visible = true;
                    this.CritNotListBox.Visible = true;
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
            this.wmiProcess = new WMIPrcocess(this.gamerCache, this.bpc.BadProcesses);   
            //Thread thread1 = new Thread(UpdateUserInterface);
            //thread1.Start();
            this.label1.Text = "WMI in process. Click end to move on to the results. Or Pause to pause WMI session";
            this.timer1.Start();
        }

        public void UpdateUserInterface()
        {
            try
            {
                Console.WriteLine("updating interface");

                foreach (Gamer g in this.gamerCache.GamerDictionary.Values.ToList())
                {

                    //make variable for g.execution llog
                    for (int i = 0; i < this.listBox1.Items.Count; i++)
                    {
                        string status = "";

              
                        string name = Name_Extraction_Regex.Match(this.listBox1.Items[i].ToString()).ToString();
                        if (name == g.Name)
                        {
                            //update status
                            if (g.ExceptionLog.Count > 0)
                            {
                                status = g.Connected == true
                                   ? "Connected : " + g.ExceptionLog?.Where(el => el.CriticalMessage == false).Last()?.LogMessage?.ToString()
                                   : "Disconnected : " + g.ExceptionLog?.Where(el => el.CriticalMessage == false).Last()?.LogMessage?.ToString();
                            }
                            else
                            {
                                status = g.Connected == true
                                  ? "Connected : "
                                  : "Disconnected : ";
                            }
                            string previousVal = listBox1.Items[i].ToString();

                            string listString = g.Name + " : " + status;

                            if (listString.Replace(".", "") == previousVal.Replace(".", ""))
                            {
                                listString = previousVal + ".";
                                listString = listString.Replace(".....", "");
                            }


                            this.listBox1.Invoke(new Action(() => this.listBox1.Items.RemoveAt(i)));
                            this.listBox1.Invoke(new Action(() => this.listBox1.Items.Insert(i, listString)));
                            
                        }
                    }
                    //update critical list
                    if (g.ExceptionLog.Where(el => el.CriticalMessage == true).Count() > 0)
                    {

                        DateTime a = g.ExceptionLog?.Where(el => el.CriticalMessage == true).Last()?.Time ?? DateTime.Now;
                        DateTime b = new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 0, a.Kind);
                        string alteredTime = b.ToString();


                        string lastCriticalMessage = "";
                        lastCriticalMessage = g.ExceptionLog?.Where(el => el.CriticalMessage == true).Last()?.LogMessage?.ToString() + " : " + alteredTime;// take off seconds


                        //if(this.critList.Count == 0)
                        //{
                        //    this.critList.Add(lastCriticalMessage);
                        //}

                        // if (!(this.critList?.Last() == lastCriticalMessage))//fix logic 
                        if (!this.critList.Contains(lastCriticalMessage))
                        {
                            this.critList.Add(lastCriticalMessage);
                            this.CritNotListBox.Invoke(new Action(() => this.CritNotListBox.Items.Add(lastCriticalMessage)));
                        }


                    }
                }


        
                if (wmiActive) { 
                //{
                //    RuntimeHelpers.EnsureSufficientExecutionStack();
                //    this.UpdateUserInterface();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);    
                
           
            }
            
        }

        private void stop_Click(object sender, EventArgs e)
        {
            this.label1.Text = "WMI process ending... Please wait";
            this.ResumeButton.Visible = false;
            this.wmiActive = false;
            this.PauseButton.Visible = false;
            this.stop.Visible = false;
            if (wmiProcess != null) {
                wmiProcess.EndProcessRetrieval(this.wmiActive);
                WaitForGamersToDisconnect();
                this.wmiProcess = null;
            }

            Thread.Sleep(1000);
            this.Load_Results_Into_Excel.Visible = true;

            this.label1.Text = "WMI ended Click button to load your results into excel";
            this.UpdateUserInterface();
            this.timer1.Stop();


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
            this.wmiProcess = new WMIPrcocess(this.gamerCache, this.bpc.BadProcesses);
            //Thread thread1 = new Thread(UpdateUserInterface);
            //thread1.Start();
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


        private void SystemPause(object sender, EventArgs e)
        {
            this.stop.Visible = false;
            this.PauseButton.Visible = false;
            this.label1.Invoke(new Action(() => this.label1.Text = "resestting stack... \n App will continue shortly"));

            this.wmiActive = false;

            wmiProcess.EndProcessRetrieval(this.wmiActive);
            WaitForGamersToDisconnect();
            this.wmiProcess = null;

            this.stop.Enabled = true;
            this.stop.Visible = true;
            //need to work on proper garbage collection

            Thread.Sleep(1000);

            this.label1.Text = "Resuming...";
            this.wmiActive = true;
            this.ResumeButton.Visible = false;
            Thread.Sleep(1000);
            this.button1.Visible = false;
            this.wmiProcess = new WMIPrcocess(this.gamerCache, this.bpc.BadProcesses);
            Thread thread1 = new Thread(UpdateUserInterface);
            thread1.Start();
            this.label1.Text = "WMI in process click end to move results or Pause to pause WMI session";
            this.stop.Visible = true;
            this.PauseButton.Visible = true;


        }

    }
}
