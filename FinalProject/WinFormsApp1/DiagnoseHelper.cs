using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class DiagnoseHelper
    {

        public static int[] PERCENT_INDEX = { 1, 2, 4}; 
        //base levels for all MustHaves
        //wbc
        const double WBC_ADULT_MIN = 4500;
        const double WBC_ADULT_MAX = 11000;
        const double WBC_KIDS_MIN = 5500;
        const double WBC_KIDS_MAX = 15500;
        const double WBC_INFANT_MIN = 6000;
        const double WBC_INFANT_MAX = 17500;
        //Neut(in percents)
        const double NEUT_MIN = 28;
        const double NEUT_MAX = 54;
        //Lymph (in percents)
        const double LYMPH_MIN = 36;
        const double LYMPH_MAX = 52;
        //RBC
        const double RBC_MIN = 4.5;
        const double RBC_MAX = 6;
        //HCT (in percents)
        const double HCT_MALE_MIN = 37;
        const double HCT_MALE_MAX = 54;
        const double HCT_FEMALE_MIN = 33;
        const double HCT_FEMALE_MAX = 47;
        //Urea (in miligrams)
        const double UREA_MIN = 17;
        const double UREA_MAX = 43;
        const int UREA_EASTERN_MULTIPLIER = 10;   //percent (+10%)
        //HB (in miligrams)
        const double HB_MALE_MIN = 12;
        const double HB_MALE_MAX = 18;
        const double HB_FEMALE_MIN = 12;
        const double HB_FEMALE_MAX = 16;
        const double HB_KIDS_MIN = 11.5;
        const double HB_KIDS_MAX = 15.5;
        //crtn
        //infant: 0-2, kids:3-17, adults:18-59, elder:60+
        const double CRTN_ELDER_MIN = 0.6;
        const double CRTN_ELDER_MAX = 1.2;
        const double CRTN_ADULT_MIN = 0.6;
        const double CRTN_ADULT_MAX = 1;
        const double CRTN_KIDS_MIN = 0.5;
        const double CRTN_KIDS_MAX = 1;
        const double CRTN_INFANT_MIN = 0.2;
        const double CRTN_INFANT_MAX = 0.5;
        //iron
        const double IRON_MIN = 60;
        const double IRON_MAX = 160;
        const int IRON_FEMALE_MULTIPLIER = -20; //percent (-20%)
        //HDL (in miligrams)
        const double HDL_MALE_MIN = 29;
        const double HDL_MALE_MAX = 62;
        const double HDL_FEMALE_MIN = 34;
        const double HDL_FEMALE_MAX = 82;
        const int HDL_ETHEOPIAN_MULTIPLIER = 20; //percent (+20%)
        //AP (in units)
        const double AP_EASTERN_MIN = 60;
        const double AP_EASTERN_MAX = 120;
        const double AP_MIN = 30;
        const double AP_MAX = 90;

        //Dictionary to convert diagnosis to treatment
        Diag2TreatConverter d2t = new Diag2TreatConverter();

        //list order:
        //WBC =0, NEUT, LYMPH, RBC, HCT, UREA, HB, CRTN, IRON, HDL, AP
        public static Dictionary<string, int> Vitals = new Dictionary<string, int>() {
        //wbc, neut, lymph, rbc, hct, urea, hb, crtn, iron, hdl, ap
            {"wbc",     0},
            {"neut",    1},
            {"lymph",   2},
            {"rbc",     3},
            {"hct",     4},
            {"urea",    5},
            {"hb",      6},
            {"crtn",    7},
            {"iron",    8},
            {"hdl",     9},
            {"ap",      10},
        };

        //TODO: Create static helper functions!

        public static List<string> diagnose(Patient pt)
        {
            List<string> diagnoseLst = new List<string>();

            //well just move through each parameter very brute force

            if(pt.IsSmoker)
                diagnoseLst.Add(@"מעשן");

            //wbc
            switch (pt.AgeStage)
            {
                //infant
                case Patient.StageOfLife.INFANT:
                    if (pt.getMustHaves()[Vitals["wbc"]] < WBC_INFANT_MIN)  //low
                        addWbcMinStr(diagnoseLst);
                    else
                        if (pt.getMustHaves()[Vitals["wbc"]] > WBC_INFANT_MAX)     //High
                        addWbcMaxStr(pt, diagnoseLst);
                    break;
                //kids
                case Patient.StageOfLife.KIDS:
                    if (pt.getMustHaves()[Vitals["wbc"]] < WBC_KIDS_MIN)  //low
                        addWbcMinStr(diagnoseLst);
                    else
                        if (pt.getMustHaves()[Vitals["wbc"]] > WBC_KIDS_MAX)     //High
                        addWbcMaxStr(pt, diagnoseLst);
                    break;
                //adults
                case Patient.StageOfLife.ADULT:
                case Patient.StageOfLife.ELDER:
                    if (pt.getMustHaves()[Vitals["wbc"]] < WBC_ADULT_MIN)  //low
                        addWbcMinStr(diagnoseLst);
                    else
                        if (pt.getMustHaves()[Vitals["wbc"]] > WBC_ADULT_MAX)     //High
                        addWbcMaxStr(pt, diagnoseLst);
                    break;
            }

            //neut
            if (pt.getMustHaves()[Vitals["neut"]] < NEUT_MIN)
                //diagnoseLst.Add("זיהום חיידקי");
                diagnoseLst.Add("זיהום");

            else if (pt.getMustHaves()[Vitals["neut"]] > NEUT_MAX)
            {
                diagnoseLst.Add("הפרעה ביצירת הדם / תאי דם");
                diagnoseLst.Add("זיהום");   //tendency
                diagnoseLst.Add("סרטן");    //rare
            }

            //rbc
            if (pt.getMustHaves()[Vitals["rbc"]] < RBC_MIN) { 
                if(!pt.IsSmoker)
                    diagnoseLst.Add("מחלת ריאות");    
                else
                { 
                diagnoseLst.Add("אנמיה");
                diagnoseLst.Add("דימום");
                }
            }
            else if (pt.getMustHaves()[Vitals["rbc"]] > RBC_MAX)
            {
                diagnoseLst.Add("הפרעה ביצירת הדם / תאי דם");
                diagnoseLst.Add("זיהום");   //tendency
                diagnoseLst.Add("סרטן");    //rare
            }

            //hct
            if (pt.IsMale) { 
                if (pt.getMustHaves()[Vitals["hct"]] < HCT_MALE_MIN)
                {
                    addHctMin(diagnoseLst);
                }
                else if (pt.getMustHaves()[Vitals["hct"]] > HCT_MALE_MAX)
                {
                    diagnoseLst.Add("מעשן");
                }
            } 
            else
            {
                if (pt.getMustHaves()[Vitals["hct"]] < HCT_FEMALE_MIN)
                {
                    addHctMin(diagnoseLst);
                }
                else if (pt.getMustHaves()[Vitals["hct"]] > HCT_FEMALE_MAX)
                {
                    diagnoseLst.Add("מעשן");
                }

            }

            //urea
            if (pt.IsEastern)
            {

                if (pt.getMustHaves()[Vitals["urea"]] < UREA_MIN + UREA_MIN*(UREA_EASTERN_MULTIPLIER / 100.0))
                {
                    addUreaMin(pt, diagnoseLst);
                }
                else if (pt.getMustHaves()[Vitals["neut"]] > UREA_MAX + UREA_MAX * (UREA_EASTERN_MULTIPLIER / 100.0))
                {
                    addUreaMax(diagnoseLst);
                }
            }
            else
            {
                if (pt.getMustHaves()[Vitals["urea"]] < UREA_MIN)
                {
                    addUreaMin(pt, diagnoseLst);
                }
                else if (pt.getMustHaves()[Vitals["urea"]] > UREA_MAX)
                {
                    addUreaMax(diagnoseLst);
                }
            }


            //hb
            if(pt.AgeStage == Patient.StageOfLife.INFANT || pt.AgeStage == Patient.StageOfLife.KIDS)
            {
                if(pt.getMustHaves()[Vitals["hb"]] < HB_KIDS_MIN)
                {
                    addHbMin(diagnoseLst);
                }
            }
            else
            {
                if (pt.IsMale) { 
                    if (pt.getMustHaves()[Vitals["hb"]] < HB_MALE_MIN)
                    {
                        addHbMin(diagnoseLst);
                    }
                }
                else
                {
                    if (pt.getMustHaves()[Vitals["hb"]] < HB_FEMALE_MIN)
                    {
                        addHbMin(diagnoseLst);
                    }
                }
            }


            //crtn
            switch (pt.AgeStage)
            {
                case Patient.StageOfLife.INFANT:
                    if(pt.getMustHaves()[Vitals["crtn"]] < CRTN_INFANT_MIN)
                    {
                        addCrtnInfantMin(diagnoseLst);
                    }
                    else if(pt.getMustHaves()[Vitals["crtn"]] > CRTN_INFANT_MAX)
                    {
                        addCrtnInfantMax(diagnoseLst);
                    }
                    break;
                case Patient.StageOfLife.KIDS:
                    if (pt.getMustHaves()[Vitals["crtn"]] < CRTN_KIDS_MIN)
                    {
                        addCrtnInfantMin(diagnoseLst);
                    }
                    else if (pt.getMustHaves()[Vitals["crtn"]] > CRTN_KIDS_MAX)
                    {
                        addCrtnInfantMax(diagnoseLst);
                    }
                    break;
                case Patient.StageOfLife.ADULT:
                    if (pt.getMustHaves()[Vitals["crtn"]] < CRTN_ADULT_MIN)
                    {
                        addCrtnInfantMin(diagnoseLst);
                    }
                    else if (pt.getMustHaves()[Vitals["crtn"]] > CRTN_ADULT_MAX)
                    {
                        addCrtnInfantMax(diagnoseLst);
                    }
                    break;
                case Patient.StageOfLife.ELDER:
                    if (pt.getMustHaves()[Vitals["crtn"]] < CRTN_ELDER_MIN)
                    {
                        addCrtnInfantMin(diagnoseLst);
                    }
                    else if (pt.getMustHaves()[Vitals["crtn"]] > CRTN_ELDER_MAX)
                    {
                        addCrtnInfantMax(diagnoseLst);
                    }
                    break;
            }

            //iron
            if (pt.IsMale)
            {
                if (pt.getMustHaves()[Vitals["iron"]] < IRON_MIN)
                {
                    addIronMin(diagnoseLst);
                }
                else if (pt.getMustHaves()[Vitals["iron"]] > IRON_MAX)
                {
                    addIronMax(diagnoseLst);
                }
            }
            else
            {
                if (pt.getMustHaves()[Vitals["iron"]] < IRON_MIN + (IRON_MIN*IRON_FEMALE_MULTIPLIER/100.0))
                {
                    addIronMin(diagnoseLst);
                }
                else if (pt.getMustHaves()[Vitals["iron"]] > IRON_MAX + (IRON_MAX * IRON_FEMALE_MULTIPLIER / 100.0))
                {
                    addIronMax(diagnoseLst);
                }
            }

            //hdl
            if (pt.IsMale)
            {
                if (pt.IsEthopian)
                {
                    if (pt.getMustHaves()[Vitals["hdl"]] < HDL_MALE_MIN + (HDL_MALE_MIN*HDL_ETHEOPIAN_MULTIPLIER/100.0))
                    {
                        addHdlMin(diagnoseLst);
                    }
                    else if (pt.getMustHaves()[Vitals["hdl"]] > HDL_MALE_MAX + (HDL_MALE_MAX * HDL_ETHEOPIAN_MULTIPLIER / 100.0))
                    {
                     //   
                    }
                }
                else
                {
                    if (pt.getMustHaves()[Vitals["hdl"]] < HDL_MALE_MIN)
                    {
                        addHdlMin(diagnoseLst);
                    }
                    else if (pt.getMustHaves()[Vitals["hdl"]] > HDL_MALE_MAX)
                    {
                        //
                    }
                }
                
            }
            else
            {
                if (pt.IsEthopian)
                {
                    if (pt.getMustHaves()[Vitals["hdl"]] < HDL_FEMALE_MIN + (HDL_FEMALE_MIN * HDL_ETHEOPIAN_MULTIPLIER / 100.0))
                    {
                        addHdlMin(diagnoseLst);
                    }
                    else if (pt.getMustHaves()[Vitals["hdl"]] > HDL_FEMALE_MAX + (HDL_FEMALE_MAX * HDL_ETHEOPIAN_MULTIPLIER / 100.0))
                    {
                        //
                    }
                }
                else
                {
                    if (pt.getMustHaves()[Vitals["hdl"]] < HDL_FEMALE_MIN)
                    {
                        addHdlMin(diagnoseLst);
                    }
                    else if (pt.getMustHaves()[Vitals["hdl"]] > HDL_FEMALE_MAX)
                    {
                        //
                    }
                }
            }

            //ap
            if (pt.IsEastern)
            {
                if (pt.getMustHaves()[Vitals["ap"]] < AP_EASTERN_MIN)
                {
                    addApMin(diagnoseLst);
                }
                else if (pt.getMustHaves()[Vitals["ap"]] > AP_EASTERN_MAX)
                {
                    addApMax(diagnoseLst);

                }
            }
            else
            {
                if (pt.getMustHaves()[Vitals["ap"]] < AP_MIN)
                {
                    addApMin(diagnoseLst);
                }
                else if (pt.getMustHaves()[Vitals["ap"]] > AP_MAX)
                {
                    addApMax(diagnoseLst);

                }
            }

            //remove duplicates
            List<string> retLst = new HashSet<string>(diagnoseLst).ToList();

            return retLst;
        }

        private static void addApMin(List<string> diagnoseLst)
        {
            diagnoseLst.Add("חוסר בוויטמינים");
        }

        private static void addApMax(List<string> diagnoseLst)
        {
            diagnoseLst.Add("מחלת כבד");
            diagnoseLst.Add("מחלות בדרכי המרה");
            diagnoseLst.Add("הריון");
            diagnoseLst.Add("פעילות יתר של בלוטת התריס");
            diagnoseLst.Add("שימוש בתרופות שונות");
        }

        private static void addHdlMin(List<string> diagnoseLst)
        {
            diagnoseLst.Add("מחלות לב");
            diagnoseLst.Add("היפרליפידמיה )שומנים בדם(");
            diagnoseLst.Add("סוכרת מבוגרים");
        }

        private static void addIronMax(List<string> diagnoseLst)
        {
            diagnoseLst.Add("הרעלת ברזל");
        }

        private static void addIronMin(List<string> diagnoseLst)
        {
            //diagnoseLst.Add("תזונה לא מספקת");
            diagnoseLst.Add("דיאטה");
        }

        private static void addCrtnInfantMax(List<string> diagnoseLst)
        {
            diagnoseLst.Add("מחלת כליה");
            //diagnoseLst.Add("אי ספיקת כליות");
            diagnoseLst.Add("מחלת שריר");
            diagnoseLst.Add("דיאטה");
        }

        private static void addCrtnInfantMin(List<string> diagnoseLst)
        {
            diagnoseLst.Add("תת תזונה");
        }

        private static void addHbMin(List<string> diagnoseLst)
        {
            diagnoseLst.Add("אנמיה");   //shows
            diagnoseLst.Add("הפרעה המטולוגית");
            diagnoseLst.Add("מחסור בברזל");
            diagnoseLst.Add("דימום");
        }

        private static void addUreaMax(List<string> diagnoseLst)
        {
            diagnoseLst.Add("מחלת כליה");
            diagnoseLst.Add("התייבשות");
            diagnoseLst.Add("דיאטה");
        }

        private static void addUreaMin(Patient pt, List<string> diagnoseLst)
        {
            if (pt.IsPregnant)
                diagnoseLst.Add("הריון");
            diagnoseLst.Add("תת תזונה");
            diagnoseLst.Add("מחלת כבד");
            diagnoseLst.Add("דיאטה");
        }

        private static void addHctMin(List<string> diagnoseLst)
        {
            diagnoseLst.Add("דימום");
            diagnoseLst.Add("אנמיה");
        }

        private static void addWbcMinStr(List<string> diagnose)
        {
            diagnose.Add(@"מחלה ויראלית");
            diagnose.Add(@"כשל של מערכת החיסון");
            diagnose.Add(@"סרטן"); //rare.
        }

        private static void addWbcMaxStr(Patient pt, List<string> diagnose)
        {
            if (pt.IsFever)
                diagnose.Add(@"זיהום");
            
            diagnose.Add(@"מחלת דם");
            diagnose.Add(@"סרטן"); //rare.
            
        }


        /* public enum Vitals
         {
             WBC =0, NEUT, LYMPH, RBC, HCT, UREA, HB, CRTN, IRON, HDL, AP
         }*/
    }
}
