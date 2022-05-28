using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class RegisterPage : Form
    {

        //db
        private DBClass db = DBClass.getInstance();

        // flag that check if there are legal inputs
        private bool usernameFlag = false;
        private bool passFlag = false;
        private bool idFlag = false;

        private const string ENGLISH_REGEX = @"^[a-zA-Z0-9 ]*$";
        private const int ID_SIZE = 9;
        private string errorStr;

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void txtBxUsername_Leave(object sender, EventArgs e)
        {
            validateUsernameInput();
        }

        private void txtBxPassword_Leave(object sender, EventArgs e)
        {
            validatePasswordInput();
        }

        private void txtBxId_Leave(object sender, EventArgs e)
        {
            validateIdInput();
        }

        public void validateUsernameInput()
        {
            string usrname = txtBxUsername.Text;
            int digitsAmount = usrname.Count(c => Char.IsNumber(c));

            //if the box is empty no need for the error
            if (String.IsNullOrEmpty(txtBxUsername.Text))
            {
                lblUsernameError.Visible = false;
                usernameFlag = false;
                return;
            }

            //check for string is not english text
            else if (!Regex.IsMatch(usrname, ENGLISH_REGEX))
            {
                errorStr = "שם משתמש חייב ליהיות באנגלית או מספרים";
                showErrorText("username", errorStr);
                return;
            }

            //check if input has 6-8 chars
            else if (usrname.Length < 6 || usrname.Length > 8)
            {
                errorStr = "שם משתמש חייב ליהיות בין 6 ל-8 תווים";
                showErrorText("username", errorStr);
                return;
            }

            //check if string has more than 2 numbers
            else if (digitsAmount > 2)
            {
                errorStr = "בשם משתמש יש מקסימום 2 ספרות";
                showErrorText("username", errorStr);
                return;
            }

            /*
             *  checked for english text
             *  checked for input size 
             *  checked for less than 2 digits
             *  
             *  the input is ok
             */

            lblUsernameError.Visible = false;   //place for bugs
            usernameFlag = true;
        }

        public void validatePasswordInput()
        {
            string password = txtBxPassword.Text;

            //if the box is empty no need for the error
            if (String.IsNullOrEmpty(txtBxPassword.Text))
            {
                passFlag = false;
                lblUsernameError.Visible = false;
                return;
            }

/*
            //check for string is not english text
            else if (!Regex.IsMatch(password, ENGLISH_REGEX))
            {
                errorStr = "שם משתמש חייב ליהיות באנגלית או מספרים";
                showErrorText("password", errorStr);
                return;
            }
*/

            //check if input has 8-10 chars
            else if (password.Length < 8 || password.Length > 10)
            {
                errorStr = "סיסמא חייבת ליהיות בין 8 ל-10 תווים";
                showErrorText("password", errorStr);
                return;
            }

            //check if string at least 1 number
            else if (!Helper.checkForOneSpecialChars(password))
            {
                errorStr = "בסיסמה יש חייב ליהיות לפחות תו מיוחד אחד";
                showErrorText("password", errorStr);
                return;
            }

            /*
             *  checked for english text
             *  checked for input size 
             *  checked for at least one letter digit and special char
             *  
             *  the input is ok
             */

            lblPasswordError.Visible = false;   //place for bugs
            passFlag = true;
        }

        public void validateIdInput()
        {
            if (string.IsNullOrEmpty(txtBxId.Text)) {
                idFlag = false;
                return;
            }

            if (txtBxId.Text.Length < ID_SIZE)
            {
                errorStr = "תעודת זהות צריכה ליהיות 9 ספרות";
                showErrorText("id", errorStr);
                return;
            }

            idFlag = true;
        }

        private void showErrorText(string box, string errorStr)
        {
            switch (box) {
                case "username":
                    lblUsernameError.Text = errorStr;
                    lblUsernameError.ForeColor = Color.Red;
                    //lblUsernameError.BackColor = Color.
                    lblUsernameError.Visible = true;
                    break;

                case "password":
                    lblPasswordError.Text = errorStr;
                    lblPasswordError.ForeColor = Color.Red;
                    //lblPasswordError.BackColor = Color.
                    lblPasswordError.Visible = true;
                    break;

                case "id":
                    lblIdError.Text = errorStr;
                    lblIdError.ForeColor = Color.Red;
                    //lblIdError.BackColor = Color.
                    lblIdError.Visible = true;
                    break;

                default:
                    Console.WriteLine("thrown in showErrorText(" + box + ", " + errorStr + ")");
                    break;
            }
        }

        

        private void RegisterPage_Load(object sender, EventArgs e)
        {
            lblUsernameError.Visible = false;
            lblPasswordError.Visible = false;
            lblIdError.Visible = false;
        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            string usr = txtBxUsername.Text;
            string pass = txtBxPassword.Text;
            string id = txtBxId.Text;

            //check for empty inputs
            if (checkForEmptyCheckbox())
                return;

            //we already have message for errors so well return
            if (!(usernameFlag && passFlag && idFlag))
                return;

            //check if theres the username in the DB
            if (db.usernameIsInDB(usr))
            {
                MessageBox.Show("משתמש כזה כבר קיים!");
                return;
            }

            if (db.idIsInDB(usr))
            {
                MessageBox.Show("קיים מתשמש בעל תעודת זהות כזאת!");
                return;
            }

            //if we get here we add the user to the xl sheet 
            Doctor dr = new Doctor(usr, pass, id);

            if (!db.addDrToDB(dr))
            {
                MessageBox.Show("בעיה בהרשמה.");
                return;
            }

            //log in into the dashboard
            //if were here than we finished using the DB 
            //  we'll close it so that we dont have an open process of excel
            DashBoard dshbrd = new DashBoard(dr);
            MessageBox.Show("תודה על ההרשמה " + dr.Username);
            db.Finish();
            dshbrd.Show();
            this.Hide();
        }
    
        private bool checkForEmptyCheckbox()
        {
            //check for empty username
            if (String.IsNullOrEmpty(txtBxUsername.Text))
            {
                MessageBox.Show("שם משתמש ריק!");
                return true;
            }

            //check for empty username
            if (String.IsNullOrEmpty(txtBxPassword.Text))
            {
                MessageBox.Show("סיסמה ריקה!");
                return true;
            }

            //check for empty username
            if (String.IsNullOrEmpty(txtBxId.Text))
            {
                MessageBox.Show("תעודת זהות ריקה!");
                return true;
            }

            return false;
        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginPage lp = new LoginPage();
            lp.Show();
            this.Hide();
        }

        private void RegisterPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        
    }
}
