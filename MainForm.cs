using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LibrarySystem.Form1;

namespace LibrarySystem
{
    public partial class MainForm : Form
    {
        private SqlConnection connect = new SqlConnection(@"Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False");
        public MainForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void LogOutbtn_Click(object sender, EventArgs e)
        {
            Form1 lForm = new Form1();
            lForm.Show();
            this.Close();
        }

        private void BBbtn_Click(object sender, EventArgs e)
        {
            IssueBooksForm ibForm = new IssueBooksForm();
            ibForm.Show();
            this.Hide();
        }

        private void RBbtn_Click(object sender, EventArgs e)
        {
            ReturnBook rbForm = new ReturnBook();
            rbForm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
