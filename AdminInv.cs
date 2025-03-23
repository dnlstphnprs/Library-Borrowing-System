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
    public partial class AdminInv : Form
    {
        SqlDataAdapter adapter;
        DataTable dt;
        public string connstring = "Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False";
        public AdminInv()
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
                string query = "SELECT * FROM Books";
                adapter = new SqlDataAdapter(query, connstring);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                dt = new DataTable();
                adapter.Fill(dt); 
                dataGridView1.DataSource = dt;  // pang-display ng table sa datagridview

                // pang auto fit
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                adapter.Update(dt);  // nagssave ng changes sa database
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
            int bookID;

            if (!int.TryParse(biTB.Text, out bookID))
            {
                MessageBox.Show("Please enter a valid Book ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();

                // hahanapin yung libro
                string checkQuery = "SELECT COUNT(*) FROM Books WHERE bookID = @bookID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@bookID", bookID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        MessageBox.Show("Book does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // idedelete if nahanap
                string deleteQuery = "DELETE FROM Books WHERE bookID = @bookID";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@bookID", bookID);
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
            // sinisigurado na row 0 yung selected once wala pang naka-select
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // autofill sa textbox kapag na-click
                biTB.Text = row.Cells["BookID"].Value.ToString();
            }
        }

        private void AdminInv_Load(object sender, EventArgs e)
        {
            biTB.ReadOnly = true; // uneditable na yung textbox
        }
    }
}
