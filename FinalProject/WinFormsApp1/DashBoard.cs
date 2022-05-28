using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;   // this's for the rounded corners

namespace WinFormsApp1
{

    public partial class DashBoard : Form
    {
        private Doctor loggedDr;

        //*************************** for rounded corners ***************************//
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
           (
               int nLeftRect,     // x-coordinate of upper-left corner
               int nTopRect,      // y-coordinate of upper-left corner
               int nRightRect,    // x-coordinate of lower-right corner
               int nBottomRect,   // y-coordinate of lower-right corner
               int nWidthEllipse, // width of ellipse
               int nHeightEllipse // height of ellipse
           );
        //*************************************************************************//

        // TODO: set ability to drag the Application
        //*************************** for drag func ***************************//
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        //*********************************************************************//

        private const int BORDER_RADIUS = 30;
        private const int BORDER_RADIUS_HEIGHT = 20;
        private const int BORDER_SIZE = 2;
        private Color borderColor = Color.RoyalBlue;
        public DashBoard()
        {
            InitializeComponent();
            loggedDr = null;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, BORDER_RADIUS
                , BORDER_RADIUS_HEIGHT));

            ///// position the nav panel
            pnlNav.Height = btnHome.Height;
            pnlNav.Top = btnHome.Top;
            pnlNav.Left = btnHome.Left;
            /////////////////////////////

            // click the home button
            homeBtnMethod();
        }

        public DashBoard(Doctor dr)
        {
            //we brought the logged doctor with us!
            loggedDr = dr;

            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, BORDER_RADIUS
                , BORDER_RADIUS_HEIGHT));

            ///// position the nav panel
            pnlNav.Height = btnHome.Height;
            pnlNav.Top = btnHome.Top;
            pnlNav.Left = btnHome.Left;
            /////////////////////////////
            ///
            lblName.Text = dr.Username;
            homeBtnMethod();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            homeBtnMethod();
        }

        private void homeBtnMethod()
        {
            //initialize the click in the navigator
            //highlight the buttom.
            pnlNav.Height = btnHome.Height; //move highlight to current clicked button
            pnlNav.Top = btnHome.Top;
            pnlNav.Left = btnHome.Left;
            btnHome.BackColor = Helper.BTN_HIGHLIGHT_COLOR;

            //open a different form inside the panel.
            lblTitle.Text = "דף בית";
            switchFormInPanel(new HomePage());
        }



        private void btnHome_Leave(object sender, EventArgs e)
        {
            btnHome.BackColor = Helper.NAV_PANEL_BG_COLOR;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }



        private void switchFormInPanel(Form next)
        {
            this.pnlFormLoader.Controls.Clear();
            //HomePage hmpg = new HomePage() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };

            next.Dock = DockStyle.Fill;
            next.TopLevel = false;
            next.TopMost = true;
            next.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(next);
            next.Show();
        }

        private void btnPatientInfo_Click(object sender, EventArgs e)
        {
            //initialize the clik in the navigator
            //highlight the buttom.
            pnlNav.Height = btnPatientInfo.Height; //move highlight to current clicked button
            pnlNav.Top = btnPatientInfo.Top;
            pnlNav.Left = btnPatientInfo.Left;
            btnPatientInfo.BackColor = Helper.BTN_HIGHLIGHT_COLOR;

            //open a different form inside the panel.
            lblTitle.Text = "הכנסת פרטים ידנית";

            switchFormInPanel(new UserInputPage());
        }

        private void btnPatientInfo_Leave(object sender, EventArgs e)
        {
            btnPatientInfo.BackColor = Helper.NAV_PANEL_BG_COLOR;
        }

        private void btnImportPage_Click(object sender, EventArgs e)
        {
            //initialize the click in the navigator
            //highlight the buttom.
            pnlNav.Height = btnImportPage.Height; //move highlight to current clicked button
            pnlNav.Top = btnImportPage.Top;
            pnlNav.Left = btnImportPage.Left;
            btnImportPage.BackColor = Helper.BTN_HIGHLIGHT_COLOR;

            //open a different form inside the panel.
            lblTitle.Text = "ייבוא מסמך אקסל";

            switchFormInPanel(new ImportUserPage());
        }

        private void btnImportPage_Leave(object sender, EventArgs e)
        {
            btnImportPage.BackColor = Helper.NAV_PANEL_BG_COLOR;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            //initialize the click in the navigator
            //highlight the buttom.
            pnlNav.Height = btnAbout.Height; //move highlight to current clicked button
            pnlNav.Top = btnAbout.Top;
            pnlNav.Left = btnAbout.Left;
            btnAbout.BackColor = Helper.BTN_HIGHLIGHT_COLOR;

            //open a different form inside the panel.
            lblTitle.Text = "אודות";
            switchFormInPanel(new AboutPage());
        }

        private void btnAbout_Leave(object sender, EventArgs e)
        {
            btnAbout.BackColor = Helper.NAV_PANEL_BG_COLOR;
        }
    }

}
