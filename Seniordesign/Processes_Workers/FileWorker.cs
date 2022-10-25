using Seniordesign.DataClasses_Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace Seniordesign.Processes_Workers
{
    class FileWorker
    {
        //TODO these can be used later to set consistent paths at class level and give user feedcback on file locations in interface
        public string ExcelStarterFile = "";
        public string ExcelOutOutFile = "";
        public string BadProcessInputFile = "";

        public static BadProcessCache LoadBadProcesses(BadProcessCache badProcesses)
        {
            string filepath = Directory.GetCurrentDirectory() + "\\CSVFiles\\BadProcesses.csv";
            string[] lines = System.IO.File.ReadAllLines(filepath);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                foreach (string column in columns)
                {
                    if (!string.IsNullOrWhiteSpace(column)) { }
                    string badProcessString = column;
                    badProcessString = badProcessString.Replace(" ", "");
                    badProcessString = badProcessString.Replace(".exe", "");
                    badProcessString = badProcessString.ToLower();
                    badProcessString = badProcessString.Trim();


                    badProcesses.BadProcesses.Add(badProcessString);
                }
            }
            return badProcesses;
        }

        public static GamerCache LoadGamersFromExcel(GamerCache gamerCache)
        {


            // using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel 97-2003 Workbook|*.xls|Excel Workbook|*.xlsx" })
            //{
            //if (ofd.ShowDialog() == DialogResult.OK)
            // {

            try { 
               
                Excel.Application ExcelApp = new Excel.Application();
             
                if (ExcelApp == null)
                {
                Console.WriteLine("Excel is not installed!!");
                Gamer g = new Gamer() { Name = "TestName", };
                gamerCache.GamerDictionary.Add(g.Name, g);
                    return gamerCache;
                }

                string filepath = Directory.GetCurrentDirectory() + "\\ExcelFiles\\Seniordesign.xlsx";

                Excel.Workbook workbook = ExcelApp.Workbooks.Open(filepath);

                Excel._Worksheet worksheet = workbook.Sheets[1];
           
                    Excel.Range range = worksheet.UsedRange;
                try
                {
                    int rowCount = range.Rows.Count;
                    int colCount = 8;


                    for (int i = 2; i <= rowCount; i++)
                    {
                        Gamer g = new Gamer();
                        for (int j = 1; j <= colCount; j++)
                        {
                            switch (j)
                            {
                                case 1:
                                    g.Name = range.Cells[i, j]?.Value2?.ToString() ?? "";
                                    break;
                                case 2:
                                    g.Computer_Name = range.Cells[i, j]?.Value2?.ToString() ?? "";
                                    break;
                                case 3:
                                    g.Mac_Address = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 4:
                                    g.IP_Address = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 5:
                                    g.Username = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 6:
                                    g.Password = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 7:
                                    g.Game_Executable = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 8:
                                    List<string> processNames = new List<string>();
                                    if (range.Cells[i, j]?.Value2?.ToString() != null && range.Cells[i, j]?.Value2?.ToString() != string.Empty)
                                    {
                                        processNames = new List<string> { range.Cells[i, j]?.Value2?.ToString() };
                                    }
                                    //to refactor to get a list of processes
                                    List<Process> processes = new List<Process>();
                                    foreach (string pn in processNames)
                                    {
                                        g.Processes.Add(new Process { ProcessName = pn });
                                    }
                                    break;

                            }
                        }
                        
                            gamerCache.GamerDictionary.Add(g.Name, g);
                        
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);

                    //close and release
                    workbook.Close(0);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

                    //quit and release
                    ExcelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            return gamerCache;
        }



        public static GamerCache ViewResultsInExcel(GamerCache gamerCache, BadProcessCache bpc)
        {
            try
            {
                int fileNum = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\ExcelFiles").Length;
                Excel.Application ExcelApp = new Excel.Application();

                string filepath = Directory.GetCurrentDirectory() + "\\ExcelFiles\\Seniordesign.xlsx";

                Excel.Workbook workbook = ExcelApp.Workbooks.Open(filepath);
                try {
                    foreach (Gamer g in gamerCache.GamerDictionary.Values)
                    {
                        Excel._Worksheet worksheet = workbook.Worksheets.Add();
                        try
                        {
                            worksheet.Name = (g.Name + "_results");
                            worksheet.Cells[1, 1].Value = "All_Processes_Ran";
                            worksheet.Cells[1, 2].Value = "First_Instance_Time";
                            worksheet.Cells[1, 3].Value = "Process_Executable_Path";
                            worksheet.Cells[1, 4].Value = "Participant_Exclusive_Processes_Ran";
                            worksheet.Cells[1, 5].Value = "Identified_Bad_Processes";
                            worksheet.Cells[1, 6].Value = "Log and Errors";
                            worksheet.Cells[1, 7].Value = "Time of Log/Error";


                            //getting distinct Process Names 
                            List<string> tempStringList = GamerCacheDataWorker.GetDistinctProcessesNames(g.Name, gamerCache);
                            //placing values into first excel column
                            for(int i = 2; i < tempStringList.Count + 2; i++ ) 
                            {
                                worksheet.Cells[i, 1].Value = tempStringList[i - 2];
                            }

                            tempStringList = GamerCacheDataWorker.GetFirstInstanceTimeList(g.Name, gamerCache);
                            for (int i = 2; i < tempStringList.Count + 2; i++)
                            {
                                worksheet.Cells[i, 2].Value = tempStringList[i - 2];
                            }

                            tempStringList = GamerCacheDataWorker.GetExecPathForFirstInstances(g.Name, gamerCache);
                            for (int i = 2; i < tempStringList.Count + 2; i++)
                            {
                                worksheet.Cells[i, 3].Value = tempStringList[i - 2];
                            }

                            tempStringList = GamerCacheDataWorker.GetExclusiveProcessNames(g.Name, gamerCache);
                            for (int i = 2; i < tempStringList.Count + 2; i++)
                            {
                                worksheet.Cells[i, 4].Value = tempStringList[i - 2];
                                //todo make column yellow
                            }


                            //does not work right now
                            tempStringList = GamerCacheDataWorker.GetBadProcessNames(g.Name, gamerCache, bpc);
                            for (int i = 2; i < tempStringList.Count + 2; i++)
                            {
                                worksheet.Cells[i, 5].Value = tempStringList[i - 2];
                            }

                            //does not work right now
                           // tempStringList 
                              List<LogItem> logList  = GamerCacheDataWorker.GetLogsForGamer(g.Name, gamerCache).ToList();
                            for (int i = 2; i < logList.Count + 2; i++)
                            {
                                worksheet.Cells[i, 6].Value = logList[i - 2].LogMessage;
                                if(logList[i - 2].GoodLog)
                                {
                                    worksheet.Cells[i, 6].Interior.Color = Excel.XlRgbColor.rgbLightGreen;                                  
                                }
                                else
                                {
                                    worksheet.Cells[i, 6].Interior.Color = Excel.XlRgbColor.rgbPink;
                                }
                                worksheet.Cells[i, 7].Value = logList[i - 2].Time.ToString("hh:mm:ss:ff");
                            }

                            //tempStringList = GamerCacheDataWorker.GetLogsForGamer(g.Name, gamerCache).Select(li => li.Time.ToString("hh:mm:ss:ff")).ToList();
                            //for (int i = 2; i < tempStringList.Count + 2; i++)
                            //{
                            //    worksheet.Cells[i, 7].Value = tempStringList[i - 2];
                            //    if()
                            //}

                            Console.WriteLine("created sheet for " + g.Name );
                           
                        }
                        catch
                        {
                            throw;
                        }                        
                        finally {
                            //  workbook.SaveAs(newFilePath, Excel.XlFileFormat.xlWorkbookNormal, "", "", false, false,
                            // Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, "", "", "");
                           
                            string newFilePath = Directory.GetCurrentDirectory() + "\\ExcelFiles\\SeniordesignResults"+fileNum.ToString() +".xlsx";
                            workbook.SaveAs(newFilePath);
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                        }
                  
                    }
                }
                catch (System.Exception) {
                    throw;
                }
                finally
                {
                               
                    string newFilePath = Directory.GetCurrentDirectory() + "\\ExcelFiles\\SeniordesignResults" + fileNum.ToString() + ".xlsx";
                    workbook.SaveAs(newFilePath);


                    //close and release
                    workbook.Close(0);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

                    //quit and release
                    ExcelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp);
                      
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //todo gamercache to excel
            return gamerCache;
        }
    }
}
