using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class AdminAccess : Form
    {
        public AdminAccess()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ViewInv_Click(object sender, EventArgs e)
        {
            AdminInv adminInv = new AdminInv();
            adminInv.Show();
            this.Close();
        }

        private void ViewUsers_Click(object sender, EventArgs e)
        {
            AdminUL adminUL = new AdminUL();
            adminUL.Show();
            this.Close();
        }

        private void LogOutbtn_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();  
            form1.Show();
            this.Close();
        }

        private void ViewBB_Click(object sender, EventArgs e)
        {
            AdminBB admin = new AdminBB();
            admin.Show();
            this.Close();
        }
    }
}
