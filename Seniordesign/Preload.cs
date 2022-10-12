﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace Seniordesign
{
    class Preload
    {
        public static GamerCache LoadGamersFromExcel(GamerCache gamerCache)
        {
          

                // using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel 97-2003 Workbook|*.xls|Excel Workbook|*.xlsx" })
                //{
                //if (ofd.ShowDialog() == DialogResult.OK)
                // {
                Excel.Application ExcelApp = new Excel.Application();

                if (ExcelApp == null)
                {
                    Console.WriteLine("Excel is not installed!!");
                    return gamerCache;
                }

                string filepath = Directory.GetCurrentDirectory() + "\\ExcelFiles\\Seniordesign.xlsx";

                Excel.Workbook workbook = ExcelApp.Workbooks.Open(filepath);
         
                Excel._Worksheet worksheet = workbook.Sheets[1];
                Excel.Range range = worksheet.UsedRange;
                int rowCount = range.Rows.Count;
                int colCount = 8;

            try
            {
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
                                    g.Mac_Address= range.Cells[i, j].Value2.ToString() ?? "";
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
                                    g.Processes.Add(new Process { ProcessName = pn});
                                }
                                    break;
                            
                            }
                    }
                    gamerCache.GamerDictionary.Add(g.Name, g);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //  GC.Collect();
                // GC.WaitForPendingFinalizers();

                //release com objects to fully kill excel process from running in the background
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);

                //close and release
                workbook.Close(0);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

                //quit and release
                ExcelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApp);
                // }
                // }
            }
            return gamerCache;
        }

    }
}
