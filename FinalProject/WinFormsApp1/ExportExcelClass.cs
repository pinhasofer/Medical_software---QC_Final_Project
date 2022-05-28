using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    
    public class ExportExcelClass
    {
        private string[] headersArr = new string[]
    {
        "First name",
        "Last name",
        "ID",
        "Age",
        "temp",
        "gender",
        "smoker",
        "etheopean",
        "eastern",
        "onMeds",
        "pregnant",
        "diarheaa",
        "vommit",
        "WBC",
        "Neut",
        "Lymph",
        "RBC",
        "HCT",
        "Urea",
        "Hb",
        "Crtn",
        "Iron",
        "HDL",
        "AP",
        "Diagnosis",
        "Recommendation",
    };  //amount 26;
        private int headersArrLength;
        //remember excel  indexes start at 1.
        private int DIAG_INDEX = 25;   //second to last index
        private int TREAT_INDEX = 26;   //last index

        private const int HEADER_INDEX = 1;
        private const int FILL_INDEX = 2;

        private const int SHEET = 1;

        private string path;

        ExcelClass expFile;
        Patient pt;

        public ExportExcelClass(Patient pt = null, bool local = false)
        {
            this.pt = pt;
            headersArrLength = headersArr.Length;

            if (path != null)
            {
                expFile = new ExcelClass();
                if (fillExcelFile())
                {

                }
            }                
        }

        public ExportExcelClass(String path, Patient pt = null)
        {
            this.pt = pt;
            headersArrLength = headersArr.Length;

            this.path = path;
            if (path != null)
            {
                //ExcelClass ctor has local parameter in the end set to true and we nee false
                expFile = new ExcelClass();
                if (fillExcelFile())
                {
                    //
                }
            }
        }


        

        public bool fillExcelFile()
        {
            if (pt == null)
            {
                MessageBox.Show("אין קובץ פציינט");
                return false;
            }

            //write the headers
            for (int i = 1; i <= headersArrLength; i++)
            {
                expFile.writeToCell(HEADER_INDEX, i, headersArr[i - 1]);
            }

            //write the patient info
            expFile.writeToCell(FILL_INDEX,  1, pt.FirstName);
            expFile.writeToCell(FILL_INDEX,  2, pt.LastName);
            expFile.writeToCell(FILL_INDEX,  3, pt.Id);
            expFile.writeToCell(FILL_INDEX,  4, pt.Age.ToString());

            expFile.writeToCell(FILL_INDEX,  5, lineOrNumber(pt.Temp));

            //expFile.writeToCell(FILL_INDEX,  6, pt.Pulse.toString());
            //expFile.writeToCell(FILL_INDEX,  7, pt.Rate.toString());

            expFile.writeToCell(FILL_INDEX,  6, maleOrFemale(pt.IsMale));
            expFile.writeToCell(FILL_INDEX,  7, yesOrNo(pt.IsSmoker));
            expFile.writeToCell(FILL_INDEX, 8, yesOrNo(pt.IsEthopian));
            expFile.writeToCell(FILL_INDEX, 9, yesOrNo(pt.IsEastern));
            expFile.writeToCell(FILL_INDEX, 10, yesOrNo(pt.IsOnMeds));
            expFile.writeToCell(FILL_INDEX, 11, yesOrNo(pt.IsPregnant));
            expFile.writeToCell(FILL_INDEX, 12, yesOrNo(pt.IsDiarrhea));
            expFile.writeToCell(FILL_INDEX, 13, yesOrNo(pt.IsVommit));


            /*
            //vitals
            expFile.writeToCell(FILL_INDEX, 14, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 15, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 16, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 17, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 18, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 19, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 20, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 21, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 22, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 23, pt.FirstName);
            expFile.writeToCell(FILL_INDEX, 24, pt.FirstName);
            */

            //fill vitals
            int vitalsLength = pt.getMustHaves().Count;
            for (int i = 0; i < vitalsLength; i++)
            {
                expFile.writeToCell(FILL_INDEX, 14 + i, pt.getMustHaves()[i].ToString());
            }

            //diag list
            if (pt.getDiagLst().Count == 0 && pt.getTreatLst().Count == 0)
            {

                expFile.writeToCell(FILL_INDEX, DIAG_INDEX, "---");
                expFile.writeToCell(FILL_INDEX, TREAT_INDEX, "---");
            }
            else
            {
                for (int j = 0; j < pt.getDiagLst().Count; j++)
                {
                    expFile.writeToCell(FILL_INDEX + j, DIAG_INDEX, pt.getDiagLst()[j]);
                }

                //treatList
                for (int j = 0; j < pt.getTreatLst().Count; j++)
                {
                    expFile.writeToCell(FILL_INDEX + j, TREAT_INDEX, pt.getTreatLst()[j]);
                }
            }

            //end
            return true;
        }

        public string yesOrNo(bool b)
        {
            if (b)
                return "Yes";

            return "No";
        }

        public string maleOrFemale(bool b)
        {
            if (b)
                return "Male";

            return "Female";
        }

        public string lineOrNumber(double dbl)
        {
            if (dbl == -1)
                return "---";

            return dbl.ToString();
        }

        public void setPt(Patient pt)
        {
            this.pt = pt;
        }

        public bool saveAs()
        {
            if (expFile == null && path == null)
                return false;

            expFile.saveAs(path);
            Finish();

            return true;
        }

        public void Finish()
        {

            //expFile.Test.Visible = true;//test
            expFile.Exit();
            expFile.Finish();
            expFile = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void finishImportFile(ExportExcelClass eec)
        {
            eec = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
