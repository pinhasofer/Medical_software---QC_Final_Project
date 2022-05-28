using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class VitalsExcelClass
    {
        private const int NAMES_INDEX = 1;
        private const int NUMBERS_INDEX = 2;
        private const int MAX_ITEM_IN_COL = 11;
        private List<double> vitals;
        private List<string> labels = new List<string>{"WBC", "Neut", "Lymph", "RBC", "HCT", "Urea",
                                                        "Hb", "Crtn", "Iron", "HDL", "AP"};
        private bool correctFormat = false;
        
        

        private const int DEFAULT_SHEET = 1;
        private ExcelClass file;

        public VitalsExcelClass(string path, int sheet = 1,bool local = true)
        {
            file = new ExcelClass(path, sheet, local);
            correctFormat = checkCorrectFormat();
            if (correctFormat)
            {
                vitals = createVitList();
            }
        }

        private List<double> createVitList()
        {
            List<double> lst = new List<double>();
            for (int i = 1; i <= MAX_ITEM_IN_COL; i++)
            {
                lst.Add(double.Parse(file.readCell(NUMBERS_INDEX, i)));
            }

            return lst;
        }

        public VitalsExcelClass(string path)
        {
            file = new ExcelClass(path, DEFAULT_SHEET);
            correctFormat = checkCorrectFormat();
        }

        public VitalsExcelClass(ExcelClass inpFile)
        {
            file = inpFile;
            correctFormat = checkCorrectFormat();
            if (correctFormat)
            {
                vitals = createVitList();
            }
        }

        public bool validateNumbersOrder()
        {
            string currNum;

            int col = file.getLastColumn();
            if (col != MAX_ITEM_IN_COL) return false;

            for (int i = 1; i < MAX_ITEM_IN_COL; i++)
            {
                currNum = file.readCell(NUMBERS_INDEX, i);
                if (!isDouble(currNum)) return false;

                if (double.Parse(currNum) < 0) return false;

                if (Helper.percentsIndex.Contains(i-1))
                {
                    if (double.Parse(currNum) < 0 || double.Parse(currNum) > 100)
                        return false;
                }


            }


            
            return true;
        }

        private bool isDouble(string currNum)
        {
            double dblVal;
            return double.TryParse(currNum, out dblVal);
        }

        public bool validateNamesOrder()
        {
            int col = file.getLastColumn();
            if (col != MAX_ITEM_IN_COL) return false;

            for (int i = 1; i <= MAX_ITEM_IN_COL; i++)
                if (file.readCell(NAMES_INDEX, i) != labels[i-1]) return false;

            //checked all the names and they are a match
            return true;
        }

        public bool checkCorrectFormat()
        {
            return (validateNamesOrder() && validateNumbersOrder());
        }

        public bool IsCorrectFormat { get { return correctFormat; } set { correctFormat = value; } }

        public List<double> Vitals { get { return vitals; } set { vitals = value; } }

        public void Exit()
        {
            if (file != null)
                file.Exit();
        }

        public void Finish()
        {
            if (file != null) { 
                file.Exit();
                file.Finish();
                file = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
