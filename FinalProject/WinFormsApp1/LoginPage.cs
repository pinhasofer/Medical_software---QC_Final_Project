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
    public partial class LoginPage : Form
    {
        //db
        private DBClass db = DBClass.getInstance();



        public LoginPage()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usr = txtBxUsername.Text;
            string pass = txtBxPassword.Text;

            Doctor dr = db.loginToDb(usr, pass);
            if (dr == null)
            {
                MessageBox.Show("בעיה בשם משתמש או סיסמא");
                return;
            }

            //registered
            DashBoard dshbrd = new DashBoard(dr);
            MessageBox.Show("ברוך הבא: " + dr.Username);
            db.Finish();
            dshbrd.Show();
            this.Hide();
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterPage rp = new RegisterPage();
            rp.Show();
            this.Hide();
        }

        private void LoginPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
