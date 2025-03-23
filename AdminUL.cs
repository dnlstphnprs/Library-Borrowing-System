using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class AdminUL : Form
    {
        SqlDataAdapter adapter;
        DataTable dt;
        public string connstring = "Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False";
        public AdminUL()
        {
            InitializeComponent();
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            AdminAccess AAForm = new AdminAccess();
            AAForm.Show();
            this.Close();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string query = "SELECT * FROM userlogin";
                adapter = new SqlDataAdapter(query, connstring);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                dt = new DataTable();
                adapter.Fill(dt);  
                dataGridView1.DataSource = dt;  //nagdidisplay ng data

                // tinatago yung password column for privacy
                dataGridView1.Columns["password"].Visible = false;

                // pag autosize para fit lahat ng laman
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                adapter.Update(dt);  //ise-save yung changes
                MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int userID;

            if (!int.TryParse(uidTB.Text, out userID))
            {
                MessageBox.Show("Please enter a valid User ID", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();

                // iche-check kung nag-eexist yung user
                string checkQuery = "SELECT COUNT(*) FROM userlogin WHERE UID = @UID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@UID", userID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        MessageBox.Show("Book does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // idedelete if meron
                string deleteQuery = "DELETE FROM userlogin WHERE UID = @UID";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@UID", userID);
                    deleteCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Book deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                AdminAccess AAForm = new AdminAccess();
                AAForm.Show();
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminAccess AAForm = new AdminAccess();
            AAForm.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // sinisigurado na row 0 yung nakaselect
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Autofill kada click
                uidTB.Text = row.Cells["UID"].Value.ToString();
            }
        }

        private void AdminUL_Load(object sender, EventArgs e)
        {
            uidTB.ReadOnly = true;
        }
    }
}
