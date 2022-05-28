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
    public partial class ImportUserPage : Form
    {
        
        private Label[] amountsArr;
        private VitalsExcelClass vitFile;
        private bool LoadFlag = false;
        private bool exportFlag = false;

        private TextBox[] txtBxRequiredArr;

        List<string> diagnostics;
        private List<string> treatments;
        Diag2TreatConverter d2t = new Diag2TreatConverter();

        Patient patient;

        //for the display
        private int rotateIndex = 0;
        private bool showDiagAndTreatFlag = false;

        public ImportUserPage()
        {
            InitializeComponent();

            txtBxRequiredArr = new TextBox[] { txtBxFirstName,txtBxLastName, txtBxId, txtBxAge};

            //create the label array
            amountsArr = new Label[]{ LblAmountWBC, LblAmountNeut, LblAmountLymph, LblAmountRBC,
                                        LblAmountHCT, LblAmountUrea, LblAmountHB, LblAmountCrtn,
                                         LblAmountIron, LblAmountHDL, LblAmountAP};
            //make them invisible
            foreach (var item in amountsArr)
            {
                item.Visible = false;
            }
            lblAccepted.Visible = false;
            lblCounter.Visible = false;
        }

        private void diagnoseBtn_Click(object sender, EventArgs e)
        {
            //TODO: refactor this to its own function.
            string fName, lName, id;
            int age;

            //reset the flag
            exportFlag = false;
            resetRotatorStuff();

            if (!checkRequiredInfo())
                return;

            //TODO: check the not required fields

            if (chkBxPregnant.Checked && rdoMale.Checked)
            {
                MessageBox.Show("לא יכול ליהיות זכר ובהריון!");
                return;
            }

            if (!LoadFlag)
            {
                MessageBox.Show("לא יובא קובץ אקסל!");
                return;
            }

            //get the outputs
            fName = txtBxFirstName.Text;
            lName = txtBxLastName.Text;
            id = txtBxId.Text;
            age = int.Parse(txtBxAge.Text);

            //create a patient object
            Patient pt = new Patient(fName, lName, id, age);

            //set 
            pt.setMustHaves(vitFile.Vitals);
            setSelectionsToPatient(pt);

            //calculate a diagnosis and get the treatment
            diagnostics = DiagnoseHelper.diagnose(pt);
            pt.setDiagLst(diagnostics); //can be full or null;
    
            treatments = d2t.createLstOfTreatments(diagnostics);    //creates a full or empty list
            pt.setTreatLst(treatments);
            //for testing;
            displayDiagAndTreat();

            //if we got here everything is ok so we set the patient
            patient = pt;
            exportFlag = true;
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

        private void btnImportFile_Click(object sender, EventArgs e)
        {
            VitalsExcelClass file = null;
            var filePath = string.Empty;
            LoadFlag = false;

            resetAcceptedLabel();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Excel Files | *.xls; *.xlsx; *.xlsm";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    file = new VitalsExcelClass(filePath,1,false);
                }
            }

            //test the input file for stuff


            if (file == null)
            {
                MessageBox.Show("בעיה");
                return;
            }


            vitFile = file;
            //do the tests
            if (!file.IsCorrectFormat)
            {
                MessageBox.Show("קלט לא בפורמט הנכון!");
                file.Exit();
                vitFile = null;
                GC.Collect();
                return;
            }

            //that means the format is correct
            //fill the labels
            setLabelToAccepted();
            fillLabels();


            //at the end exit because we dont need the excel open
            LoadFlag = true;
            vitFile.Finish();
        }

        private void fillLabels()
        {
            //ive already checked there are the same amount of elemnets but just to be carful
            if (amountsArr.Length != vitFile.Vitals.Count) return;

            for (int i = 0; i < amountsArr.Length; i++)
            {
                amountsArr[i].Text = vitFile.Vitals[i].ToString();
                if(DiagnoseHelper.PERCENT_INDEX.Contains(i))
                    amountsArr[i].Text += "%";

                amountsArr[i].Visible = true;
            }
        }

        private void setLabelToAccepted()
        {
            lblAccepted.Visible = true;
        }
        private void resetAcceptedLabel()
        {
            lblAccepted.Text = "נטען בהצלחה!";
            lblAccepted.ForeColor = Color.Green;
            lblAccepted.Visible = false;

            //reset amount labels
            foreach (var item in amountsArr)
            {
                item.Visible = false;
            }

            resetRotatorStuff();
        }

        private void resetRotatorStuff()
        {
            lblTreatment.Visible = false;
            lblDiagnose.Visible = false;
            lblCounter.Visible = false;

            rotateIndex = 0;
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

            if (!Helper.doesStrIsNums(age) && int.Parse(age) <= 1 )
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

            if(double.Parse(temp) >= 43 || double.Parse(temp) <= 20)
            {
                MessageBox.Show("אנא הכנס חום הגיוני (20-43)");
                return false;
            }

            return true;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            decRotator();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            incRotator();
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

        private string createOutOfStr(int length)
        {
            if (length == 0)
                return "0 מתוך 0";
            //get the current index from 1-length
           return ((rotateIndex) + 1).ToString() + " מתוך " + length.ToString();
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            exportMethod();
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
    }
}
