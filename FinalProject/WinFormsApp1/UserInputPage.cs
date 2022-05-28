using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class UserInputPage : Form
    {

        private List<string> diagnostics;
        private List<string> treatments;
        Diag2TreatConverter d2t = new Diag2TreatConverter();
        private bool showDiagAndTreatFlag = false;
        private bool exportFlag = false;
        public int rotateIndex = 0;

        Patient patient;

        public UserInputPage()
        {
            InitializeComponent();

            lblCounter.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TODO: refactor this to its own function.
            string fName, lName, id, temp;
            int age;
            //wbc, neut, lymph, rbc, hct, urea, hb, crtn, iron, hdl, ap;
            List<double> mustHave = new List<double>(11);

            exportFlag = false;
            resetRotatorStuff();

            if (!checkRequiredInfo())
                return;


            if (chkBxPregnant.Checked && rdoMale.Checked)
            {
                MessageBox.Show("לא יכול ליהיות זכר ובהריון!");
                return;
            }

            //get the outputs
            fName = txtBxFirstName.Text;
            lName = txtBxLastName.Text;
            id = txtBxId.Text;
            age = int.Parse(txtBxAge.Text);
            temp = txtBxAge.Text;

            //get the must haves
            TextBox[] txtBxArr = { txtBxWBC, txtBxNeut, txtBxLymph, txtBxRBC,
                                    txtBxHCT, txtBxUrea, txtBxHb, txtBxCrtn,
                                      txtBxIron, txtBxHDL, txtBxAP };

            string[] txtBxStrArr = { "WBC", "Neut", "Lymph", "RBC", "HCT",
                                    "Urea", "Hb", "Crtn", "Iron", "HDL", "AP" };

            if (!Helper.checkTxtBxs(txtBxArr, txtBxStrArr))
                return;

            for (int i = 0; i < txtBxArr.Length; i++)
            {
                mustHave.Add(double.Parse(txtBxArr[i].Text));
            }

            //create a patient object
            Patient pt = new Patient(fName, lName, id, age);

            //set 
            pt.setMustHaves(mustHave);
            setSelectionsToPatient(pt);

            //calculate a diagnosis and get the treatment
            
            diagnostics = DiagnoseHelper.diagnose(pt);

            pt.setDiagLst(diagnostics); //can be full or null;

            treatments = d2t.createLstOfTreatments(diagnostics);    //creates a full or empty list
            pt.setTreatLst(treatments);


            displayDiagAndTreat();

            //got here everything is ok
            patient = pt;
            exportFlag = true;
        }

        

        private void setSelectionsToPatient(Patient pt)
        {
            //set Male or Female
            if (rdoMale.Checked)
                pt.IsMale = true;
            else if (rdoFemale.Checked)
                pt.IsMale = false;

            //set the temp
            if (!string.IsNullOrEmpty(txtBxTemp.Text))
                pt.Temp = double.Parse(txtBxTemp.Text);

            if (chkBxSmoker.Checked)
                pt.IsSmoker = true;
            else
                pt.IsSmoker = false;

            if (chkBxEtheopian.Checked)
                pt.IsEthopian = true;
            else
                pt.IsEthopian = false;

            if (chkBxEastern.Checked)
                pt.IsEastern = true;
            else
                pt.IsEastern = false;

            if (chkBxOnMeds.Checked)
                pt.IsOnMeds = true;
            else
                pt.IsOnMeds = false;

            if (chkBxPregnant.Checked)
                //we already cleared the male and pregnant
                pt.IsPregnant = true;
            else
                pt.IsPregnant = false;

            if (chkBxDiarrhea.Checked)
                pt.IsDiarrhea = true;
            else
                pt.IsDiarrhea = false;

            if (chkBxVommit.Checked)
                pt.IsVommit = true;
            else
                pt.IsVommit = false;

        }

        private void displayDiagAndTreat()
        {
            int length = diagnostics.Count;
            int testLength = treatments.Count;
            if (length != testLength)
                return;


            if (length == 0)
            {
                lblDiagnose.Text = "הכל תקין";
                lblTreatment.Text = "אין טיפול";
            }
            else
            {
                lblDiagnose.Text = diagnostics[rotateIndex];
                lblTreatment.Text = treatments[rotateIndex];
            }

            lblCounter.Text = createOutOfStr(length);
            lblCounter.Visible = true;
            lblDiagnose.Visible = true;
            lblTreatment.Visible = true;
            showDiagAndTreatFlag = true;

        }

        private string createOutOfStr(int length)
        {
            if (length == 0)
                return "0 מתוך 0";
            //get the current index from 1-length
            return ((rotateIndex) + 1).ToString() + " מתוך " + length.ToString();
        }

        private bool checkRequiredInfo()
        {
            string fName = txtBxFirstName.Text;
            string lName = txtBxLastName.Text;
            string id = txtBxId.Text;
            string age = txtBxAge.Text;
            string temp = txtBxTemp.Text;

            //check empty textboxes
            if (string.IsNullOrEmpty(fName))
            {
                MessageBox.Show("אנא הכנס שם פרטי");
                return false;
            }

            if (string.IsNullOrEmpty(lName))
            {
                MessageBox.Show("אנא הכנס שם משפחה");
                return false;
            }

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("אנא הכנס תעודת זהות");
                return false;
            }

            if (string.IsNullOrEmpty(age))
            {
                MessageBox.Show("אנא הכנס גיל");
                return false;
            }

            //check name for numbers
            if (Helper.doesStrContainNums(fName) || Helper.checkForOneSpecialChars(fName))
            {
                MessageBox.Show("אנא הכנס שם פרטי תקין");
                return false;
            }

            if (Helper.doesStrContainNums(lName) || Helper.checkForOneSpecialChars(lName))
            {
                MessageBox.Show("אנא הכנס שם משפחה תקין");
                return false;
            }

            //check if id and age are only numbers
            if (!Helper.doesStrIsNums(id) || id.Length < 9)
            {
                MessageBox.Show("אנא הכנס תעודת זהות תקינה");
                return false;
            }

            if (!Helper.doesStrIsNums(age) && int.Parse(age) <= 1)
            {
                MessageBox.Show("אנא הכנס גיל  תקין");
                return false;
            }

            //add more checks if we needed
            
            //temp is required
            if (string.IsNullOrEmpty(temp))
            {
                MessageBox.Show("אנא הכנס חום");
                return false;
            }

            if (!Helper.isStrDouble(temp))
            {
                MessageBox.Show("אנא הכנס חום נכון");
                return false;
            }

            if (double.Parse(temp) >= 43 || double.Parse(temp) <= 20)
            {
                MessageBox.Show("אנא הכנס חום הגיוני (20-43)");
                return false;
            }

            return true;
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            incRotator();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            decRotator();
        }

        private void incRotator()
        {
            if (!checksForRotator())
                return;

            int length = diagnostics.Count;
            rotateIndex = Helper.mod(++rotateIndex, length);
            lblCounter.Text = createOutOfStr(length);
            lblDiagnose.Text = diagnostics[rotateIndex];
            lblTreatment.Text = treatments[rotateIndex];
        }

        private void decRotator()
        {
            if (!checksForRotator())
                return;

            int length = diagnostics.Count;
            rotateIndex = Helper.mod(--rotateIndex, length);
            //get the current index from 1-length
            lblCounter.Text = ((rotateIndex) + 1).ToString() + " מתוך " + length.ToString();
            lblDiagnose.Text = diagnostics[rotateIndex];
            lblTreatment.Text = treatments[rotateIndex];
        }

        public bool checksForRotator()
        {
            //check for null or empty strings
            if (diagnostics == null || diagnostics.Count == 0)
                return false;
            if (treatments == null || treatments.Count == 0)
                return false;
            //check the flag
            if (!showDiagAndTreatFlag)
                return false;

            return true;
        }

        private void resetRotatorStuff()
        {
            lblTreatment.Visible = false;
            lblDiagnose.Visible = false;
            lblCounter.Visible = false;

            rotateIndex = 0;
        }

        private void exportMethod()
        {
            if (!exportFlag)
            {
                MessageBox.Show("לא ניתן לייצא");
                return;
            }

            if (patient == null)
            {
                MessageBox.Show("אין אובייקט פציינט");
                return;
            }

            string path = ExcelClass.GetSaveFilePathWithDialog();
            if (path == null)
            {
                MessageBox.Show("בעיה בהשגת הPATH");
                return;
            }

            ExportExcelClass eec = new ExportExcelClass(path, patient);
            if (!eec.saveAs())
            {
                MessageBox.Show("בעיה בשמירה");
                return;
            }

            MessageBox.Show("נשמר בהצלחה!");
            ExportExcelClass.finishImportFile(eec);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            exportMethod();
        }

        
    }
}
