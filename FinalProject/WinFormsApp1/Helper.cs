using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class Helper
    {
        //public static Color BG_COLOR { get { return BG_COLOR; } set { Color.FromArgb(46, 51, 53); } }
        public static Color BG_COLOR            = Color.FromArgb(46, 51, 53);
        public static Color NAV_PANEL_BG_COLOR  = Color.FromArgb(24, 30, 54);
        public static Color BTN_HIGHLIGHT_COLOR =  Color.FromArgb(46, 51, 73);


        public static void changeForm(Form curr, string name)
        {
            switch (name)
            {
                case "RegisterPage":
                    RegisterPage rp = new RegisterPage();
                    rp.Show();
                    curr.Hide();
                    break;

                default: throw new ArgumentException();
            }
            
        }

        public static bool doesStrContainNums(string str)
        {
            return str.Any(char.IsDigit);
        }

        public static bool doesStrIsNums(string str)
        {
            return str.All(char.IsDigit);
        }

        public static bool checkForOneSpecialChars(string str)
        {
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"" + " ";
            foreach (char ch in specialCh)
            {
                if (str.Contains(ch))
                    return true;
            }

            return false;
        }

        //modulu func
        public static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static bool isStrDouble(string str)
        {
            double dbl;

            return double.TryParse(str, out dbl);
        }


        public static string[] txtBxStrArr = { "WBC", "Neut", "Lymph", "RBC", "HCT",
                                    "Urea", "Hb", "Crtn", "Iron", "HDL", "AP" };
        public static int[] percentsIndex = { 1, 2, 4 };
        public static bool checkTxtBxs(TextBox[] txtBxArr, string[] txtBxStrArr)
        {
            

            if (txtBxArr == null)
                return false;

            int i = 0;
            foreach (var item in txtBxArr)
            {

                if (string.IsNullOrEmpty(item.Text))
                {
                    MessageBox.Show(txtBxStrArr[i] + " אנא מלא את כל הפרטים.");
                    return false;
                }

                if (!Helper.isStrDouble(item.Text))
                {
                    MessageBox.Show(txtBxStrArr[i] + " צריך למלא מספר.");
                    return false;
                }

                if (double.Parse(item.Text) < 0)
                {
                    MessageBox.Show(txtBxStrArr[i] + " צריך ליהיות חיובי.");
                    return false;
                }

                if (Helper.percentsIndex.Contains(i))
                {
                    if (double.Parse(item.Text) < 0 || double.Parse(item.Text) > 100)
                    {
                        MessageBox.Show(txtBxStrArr[i] + " צריך ליהיות באחוזים(0-100).");
                        return false;
                    }
                }


                i++;
            }

            return true;
        }

    }
}
