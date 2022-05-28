using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

using _Excel = Microsoft.Office.Interop.Excel; 


namespace WinFormsApp1
{
    public class ExcelClass
    {
        string path;
        private _Application excel = new _Excel.Application();
        private Workbooks workbooks;
        private Sheets worksheets;
        private Workbook wb;
        private Worksheet ws;

        public ExcelClass(string path, int sheet, bool local = true)
        {
            if (local)
                this.path = bingPathToAppDir(path);
            else
                this.path = path;

            /*wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[sheet];*/
            workbooks = excel.Workbooks;
            openWb(this.path, sheet);
        }

        public ExcelClass()
        {
            object misValue = System.Reflection.Missing.Value;
            workbooks = excel.Workbooks;
            wb = workbooks.Add(misValue);

            worksheets = wb.Worksheets;
            ws = (Worksheet)worksheets.get_Item(1);
        }


        public string readCell(int x, int y)
        {
            if (!checkLegalCell(x,y))
            {
                Console.WriteLine("x and y cant be 0 or less");
                return "";
            }

            if (ws.Cells[x, y].Value2 != null)
                return ws.Cells[x, y].Value2.ToString();
            else
                return "";

        }

        public void writeToCell(int x,int y, string str)
        {
            if (!checkLegalCell(x, y))
            {
                Console.WriteLine("x and y cant be 0 or less");
                return;
            }

            ws.Cells[x, y].Value2 = str;
            wb.Save();
        }

        public void save()
        {
            wb.Save();
        }
        public void saveAs(string path)
        {
            wb.SaveAs(path);
        }

        public void openWb(string path, int sheet)
        {
            wb = workbooks.Open(path);
            ws = wb.Worksheets[sheet];
        }
        public void Exit()
        { 
            wb.Close(0);
            excel.Quit();
        }


        public static string bingPathToAppDir(string localPath)
        {
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\" + localPath)));
            return directory.ToString();
        }

        public bool checkLegalCell(int x, int y)
        {
            if (x <= 0 || y <= 0)
                return false;

            return true;
        }

        public int getLastRow()
        {
            _Excel.Range last = ws.Cells.SpecialCells(_Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            _Excel.Range range = ws.get_Range("A1", last);

            return last.Row;
        }

        public int getLastColumn()
        {
            _Excel.Range last = ws.Cells.SpecialCells(_Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            _Excel.Range range = ws.get_Range("A1", last);

            return last.Column;
        }

        public void Finish()
        {
            /*   wb.Close(0);
               excel.Quit();

             */

            //test
            while (Marshal.ReleaseComObject(ws) > 0) ;
            while (Marshal.ReleaseComObject(wb) > 0);
            while (Marshal.ReleaseComObject(workbooks) > 0);
            while (Marshal.ReleaseComObject(excel) > 0) ;
            

            /*Marshal.ReleaseComObject(workbooks);
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(excel);*/
        }

        /*public void closeAll()
        {
            _Excel.Application s = Marshal.GetActiveObject(_Excel);
            Workbooks wbs = excel.Workbooks;
            foreach (Workbook wb in wbs)
            {
                Console.WriteLine(wb.Name); // print the name of excel files that are open
                wb.Save();
                wb.Close();
            }
        }*/

        public static string GetSaveFilePathWithDialog()
        {
            //creates save file dialog and returns a path
            string filePath;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Excel Files | *.xls; *.xlsx; *.xlsm";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = saveFileDialog.FileName;

                    /*if(expFile != null)
                        expFile.saveAs(filePath);*/
                    return filePath;
                }
            }
            return null;
        }

        ~ExcelClass()
        { 
            //Finish();
        }
        public string getPath { get { return path; } set { path = value; } }

        public _Application Test { get { return excel; } }
    }
}
