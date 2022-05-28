using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Diag2TreatConverter
    {
        private string[] diagnosisStrs =
        {
            "אנמיה",
            "דיאטה",
            "דימום",
            "היפרליפידמיה )שומנים בדם(",
            "הפרעה ביצירת הדם / תאי דם",
            "הפרעה המטולוגית",
            "הרעלת ברזל",
            "התייבשות",
            "זיהום",
            "חוסר בוויטמינים",
            "מחלה ויראלית",
            "מחלות בדרכי המרה",
            "מחלות לב",
            "מחלת דם",
            "מחלת כבד",
            "מחלת כליה",
            "מחסור בברזל",
            "מחלת שריר",
            "מעשן",
            "מחלת ריאות",
            "פעילות יתר של בלוטת התריס",
            "סוכרת מבוגרים",
            "סרטן",
            "צריכה מוגברת של בשר",
            "שימוש בתרופות שונות",
            "תת תזונה",
            "הריון",
            "כשל של מערכת החיסון"
        };

        private string[] treatmentStrs =
        {
            @"שני כדורי 10 מ'ג של B12 ביום למשך חודש",
            "לתאם פגישה עם תזונאי",
            "להתפנות בדחיפות לבית החולים",
            "לתאם פגישה עם תזונאי, כדור 5 מ'ג של סימוביל ביום למשך שבוע",
            "כדור 10 מ'ג של B12 ביום למשך חודש \nכדור 5 מ'ג של חומצה פולית ביום למשך חודש",
            "זריקה של הורמון לעידוד ייצור תאי הדם האדומים",
            "להתפנות לבית החולים",
            "מנוחה מוחלטת בשכיבה, החזרת נוזלים בשתייה",
            "אנטיביוטיקה ייעודית",
            "הפנייה לבדיקת דם לזיהוי הוויטמינים החסרים",
            "לנוח בבית",
            "הפנייה לטיפול כירורגי",
            "לתאם פגישה עם תזונאי",
            "שילוב של ציקלופוספאמיד וקורטיקוסרואידים",
            "הפנייה לאבחנה ספציפית לצורך קביעת טיפול",
            "איזון את רמות הסוכר בדם",
            "שני כדורי 10 מ'ג של B12 ביום למשך חודש",
            "שני כדורי 5 מ'ג של כורכום c3 של אלטמן ביום למשך חודש",
            "להפסיק לעשן",
            "להפסיק לעשן / הפנייה לצילום רנטגן של הריאות",
            "Propylthiouracil להקטנת פעילות בלוטת התריס",
            "התאמת אינסולין למטופל",
            "אנטרקטיניב - Entrectinib",
            "לתאם פגישה עם תזונאי",
            "הפנייה לרופא המשפחה לצורך בדיקת התאמה בין התרופות",
            "לתאם פגישה עם תזונאי",
            "ללדת",
            "לא נתון טיפול כרגע"
        };

        private Dictionary<string, string> diag2Dict;

        public Diag2TreatConverter()
        {
            diag2Dict = new Dictionary<string, string>();

            //test just in case
            if(diagnosisStrs.Length == treatmentStrs.Length)
            {
                for(int i =0; i < diagnosisStrs.Length; i++)
                {
                    //fill the dictionary
                    diag2Dict[diagnosisStrs[i]] = treatmentStrs[i];
                }
            }
        }

        public string getTreatment(string diag)
        {
            return diag2Dict[diag];
        }

        public List<string> createLstOfTreatments(List<string> lst)
        {
            List<string> treatLst = new List<string>();

            /*if (!(lst.Count > 0))
                return null;*/
                
            foreach (var item in lst)
            {
                treatLst.Add(getTreatment(item));
            }

            return treatLst;
            
        }

    }
}
