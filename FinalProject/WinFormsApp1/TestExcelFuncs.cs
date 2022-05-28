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
    public partial class TestExcelFuncs : Form
    {
        public TestExcelFuncs()
        {
            InitializeComponent();
        }

        private void btnXLRead_Click(object sender, EventArgs e)
        {
            readFileFromXL();
        }

        private void readFileFromXL()
        {
            string MOTHER_PATH = AppDomain.CurrentDomain.BaseDirectory;
            string path = ExcelClass.bingPathToAppDir(@"test.xlsx");
            ExcelClass excel = new ExcelClass(path, 1);

            excel.writeToCell(1, 1, "12345");

            MessageBox.Show(excel.readCell(1, 1));

            excel.Exit();
        }
    }
}
