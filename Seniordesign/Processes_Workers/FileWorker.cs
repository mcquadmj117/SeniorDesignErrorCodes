using Seniordesign.DataClasses_Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace Seniordesign.Processes_Workers
{
    class FileWorker
    {

        //public string ExcelStarterFile = "";
        //public string ExcelOutOutFile = "";
        //public string BadProcessInputFile = "";

        public static string CreateInitialDirectoryWithFiles()
        {
            try
            {
                string newFolder = "ErrorCodes";
                string path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                newFolder);
                if (!System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    catch (IOException ie)
                    {
                        throw;
                    }               
                }
                if (!File.Exists(path + "\\ErrorCodesStartingFile.xlsx")) {
                    Excel.Application excelApp = new Excel.Application();
                    if (excelApp == null)
                    {
                        MessageBox.Show("Excel is not properly installed!!");
                        return "";
                    }
                    object misValue = System.Reflection.Missing.Value;
                    Excel.Workbook workbook = excelApp.Workbooks.Add(misValue);
                    var sheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.get_Item(1);
                    try
                    {

                        sheet.Cells[1, 1].Value = "Name(Must Be unique)";
                        sheet.Cells[1, 2].Value = "Computer_Name";
                        sheet.Cells[1, 3].Value = "IP_Address(optional)";
                        sheet.Cells[1, 4].Value = "Username";
                        sheet.Cells[1, 5].Value = "Password";
                        sheet.Cells[1, 6].Value = "Game_Executable";
                        sheet.Cells[1, 7].Value = "Inserted_Processes(seperate by comma)";
                        sheet.Columns.AutoFit();
                        sheet.Rows.AutoFit();
                        
   


                        workbook.SaveAs(path + "\\ErrorCodesStartingFile.xlsx");

                        workbook.Close(true, misValue, misValue);
                    }

                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        excelApp.Quit();

                        Marshal.ReleaseComObject(sheet);
                        Marshal.ReleaseComObject(workbook);
                        Marshal.ReleaseComObject(excelApp);
                    }
                }

                if(File.Exists(path + "\\ErrorCodesStartingFile.xlsx")){                   
                    return (path + "\\ErrorCodesStartingFile.xlsx");
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine( e.Message);
                return "";
            }

    
        }

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

            try { 
               
                Excel.Application ExcelApp = new Excel.Application();
             
                if (ExcelApp == null)
                {
                Console.WriteLine("Excel is not installed!!");
                Gamer g = new Gamer() { Name = "TestName", };
                gamerCache.GamerDictionary.Add(g.Name, g);
                    return gamerCache;
                }


                string path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "ErrorCodes");

                path = path + "\\ErrorCodesStartingFile.xlsx";

                Excel.Workbook workbook = ExcelApp.Workbooks.Open(path);

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
                                    g.IP_Address = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 4:
                                    g.Username = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 5:
                                    g.Password = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 6:
                                    g.Game_Executable = range.Cells[i, j].Value2.ToString() ?? "";
                                    break;
                                case 7:
                                    string processNames = "";
                                    List<string> processStrings = new List<string>();
                                    if (range.Cells[i, j]?.Value2?.ToString() != null && range.Cells[i, j]?.Value2?.ToString() != string.Empty)
                                    {
                                        processNames =  range.Cells[i, j]?.Value2?.ToString();
                                        processStrings = processNames.Split(',').ToList();
                                    }
                                 
                                                     
                                    foreach (string pn in processStrings)
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

        public static string ViewResultsInExcel(GamerCache gamerCache, BadProcessCache bpc)
        {

            string fileCreatedPath = "";

            try
            {

                string startPath = System.IO.Path.Combine(
              Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
              "ErrorCodes");

                string resultPath = System.IO.Path.Combine(startPath, "Results");

                if (!Directory.Exists(resultPath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(resultPath);
                    }
                    catch (IOException ie)
                    {
                        throw;
                    }
                }

                    int fileNum = Directory.GetFiles(resultPath).Length;
                Excel.Application ExcelApp = new Excel.Application();


                Excel.Workbook workbook = ExcelApp.Workbooks.Open(startPath + "\\ErrorCodesStartingFile.xlsx");
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
                            worksheet.Columns.AutoFit();
                            worksheet.Rows.AutoFit();
                            fileCreatedPath = resultPath + "\\ErrorCodeResults"+fileNum.ToString() +".xlsx";
                            workbook.SaveAs(fileCreatedPath);
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                        }
                  
                    }
                }
                catch (System.Exception) {
                    throw;
                }
                finally
                {
                               
                    fileCreatedPath = resultPath + "\\ErrorCodeResults" + fileNum.ToString() + ".xlsx";
                    workbook.SaveAs(fileCreatedPath);


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
            return fileCreatedPath;
        }

    }
}
