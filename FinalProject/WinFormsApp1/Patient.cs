using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    /*
    struct BloodTestValues
    {
        float ap, hdl, iron, crtn, hb, urea, hct, rbc, lymph, neut, wbc;

        public BloodTestValues(float ap, float hdl, float iron, float crtn, float hb,
            float urea, float hct, float rbc, float lymph, float neut, float wbc)
        {
            this.ap = ap;
            this.hdl = hdl;
            this.iron = iron;
            this.crtn = crtn;
            this.hb = hb;
            this.urea = urea;
            this.hct = hct;
            this.rbc = rbc;
            this.lymph = lymph;
            this.neut = neut;
            this.wbc = wbc;
        }

        public BloodTestValues(List<float> valsLst)
        {
            //supposed to be 11 vals
            if(valsLst == null && valsLst.Count != 11)
            {
                //
            }

            this.ap = valsLst[0];
            this.hdl = valsLst[1];
            this.iron = valsLst[2];
            this.crtn = valsLst[3];
            this.hb = valsLst[4];
            this.urea = valsLst[5];
            this.hct = valsLst[6];
            this.rbc = valsLst[7];
            this.lymph = valsLst[8];
            this.neut = valsLst[9];
            this.wbc = valsLst[10];
        }
    }
    */

    public class Patient
    {
        //patient info
        private string firstName;
        private string lastName;
        private string id;
        private int age;
        
        private StageOfLife ageStage;

        //added info
        private bool smoker;
        private bool pregnant;


        //must have for excel
        private const int MUST_HAVE_AMOUNT = 11;

        ////wbc, neut, lymph, rbc, hct, urea, hb, crtn, iron, hdl, ap; total: 11
        private List<double> healthParams;

        private List<string> treatLst;
        private List<string> diagLst;

        private double temp;
        private (int,int) bp;  //blood pressure - tuple
        private int pulse;

        private bool eastern;
        private bool etheopian;

        //(male == false) -> female
        private bool male;
        private bool onMeds;
        private bool fever;
        private bool diarrhea;
        private bool vommit;


        private ExcelClass inputFile;

        public Patient(string firstName, string lastName, string id, int age)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.id = id;
            this.age = age;

            //set from outside
            this.temp = -1;


            ageStage = calcStageOfLife();

            healthParams = new List<double>(MUST_HAVE_AMOUNT);
            diagLst = null;
            treatLst = null;
            inputFile = null;
        }

        public void setMustHaves(List<double> inputs)
        {
            if (healthParams != null && inputs != null)
            {
                healthParams.Clear();
                healthParams = inputs;
            }
        }

        public void setDiagLst(List<string> strLst)
        {
            if (strLst != null)
            {
                if(diagLst != null)
                {
                    diagLst.Clear();
                    diagLst = strLst;
                }
                else
                {
                    diagLst = new List<string>();
                    diagLst = strLst;
                }
                
            }
        }

        public void setTreatLst(List<string> strLst)
        {
            if(strLst != null)
            {
                if (treatLst != null)
                {
                    treatLst.Clear();
                    treatLst = strLst;
                }
                else
                {
                    treatLst = new List<string>();
                    treatLst = strLst;
                }

            }
        }

        public List<double> getMustHaves()
        {
            return healthParams;
        }

        public List<string> getTreatLst()
        {
            return treatLst;
        }

        public List<string> getDiagLst()
        {
            return diagLst;
        }

        public string FirstName { get { return firstName; } set { firstName = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        public string Id { get { return id; } set { id = value; } }
        public int Age { get { return age; } set { age = value; } }

        public double Temp { get { return temp; } set { temp = value; fever = checkFever(); } }

        public bool IsSmoker { get { return smoker; } set { smoker = value; } }
        public bool IsMale { get { return male; } set { male = value; } }
        public bool IsOnMeds { get { return onMeds; } set { onMeds = value; } }
        public bool IsEastern { get { return eastern; } set { eastern = value; } }
        public bool IsEthopian { get { return etheopian; } set { etheopian = value; } }
        public StageOfLife AgeStage { get { return ageStage; } set { ageStage = value; } }
        public bool IsFever { get { return fever;  } set { fever = value; } }
        public bool IsPregnant { get { return pregnant; } set {pregnant = value; } }
        public bool IsDiarrhea { get { return diarrhea; } set { diarrhea = value; } }

        public bool IsVommit { get {return vommit; }  set { vommit = value; } }

        public StageOfLife calcStageOfLife()
        {
            if (age >= 0 && age <= 3)
                return StageOfLife.INFANT;
            if (age >= 4 && age <= 17)
                return StageOfLife.KIDS;
            if (age >= 18 && age <= 59)
                return StageOfLife.ADULT;

            //no need for another if because only one option but just in case
            return StageOfLife.ELDER;
        }

        public bool checkFever()
        {
            if (temp >= 0)
            {
                if (temp > 38)
                    return true;

                return false;
            }

            return false;
        }

        

        public enum StageOfLife
        {
            INFANT,
            KIDS,
            ADULT,
            ELDER
        }
    }
}
