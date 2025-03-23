using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class AdminBB : Form
    {
        SqlDataAdapter adapter;
        DataTable dt;
        public string connstring = "Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False";
        public AdminBB()
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
                string query = "SELECT * FROM BorrowingSystem";
                adapter = new SqlDataAdapter(query, connstring);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;  //ididisplay yung data

                // autosize para magkasya lahat ng content
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = unTB.Text.Trim(); // kukunin yung username sa text box
            string bookID = biTB.Text.Trim(); // kukunin yung book ID sa text box

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a valid Borrower's Username.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();

                // ichecheck kung hiniram nung binigay na username yung book id nung libro
                string checkQuery = "SELECT COUNT(*) FROM BorrowingSystem WHERE username = @username";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        MessageBox.Show("Book does not exist for this user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // idedelete yung record
                string deleteQuery = "DELETE FROM BorrowingSystem WHERE username = @username AND bookID = @bookID";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@username", username);
                    deleteCmd.Parameters.AddWithValue("@bookID", bookID);
                    deleteCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Book deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                AdminAccess AAForm = new AdminAccess();
                AAForm.Show();
                this.Close();
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                adapter.Update(dt);  // ise-save yung changes na manual ginawa sa datagridview
                MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                AdminAccess AAForm = new AdminAccess();
                AAForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (e.RowIndex >= 0) // sinisigurado na row 0 yung selected once wala pang naka-select
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // autofill sa textbox kapag na-click
                biTB.Text = row.Cells["BookID"].Value.ToString();
                unTB.Text = row.Cells["username"].Value.ToString();
            }
        }

        private void AdminBB_Load(object sender, EventArgs e)
        {
            unTB.ReadOnly = true; //bawal na i-edit yung textbox ng username
            biTB.ReadOnly = true; //bawal na i-edit yung textbox ng username
        }
    }
}
