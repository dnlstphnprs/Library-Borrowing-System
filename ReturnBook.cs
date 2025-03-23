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
using static LibrarySystem.Form1;

namespace LibrarySystem
{
    public partial class ReturnBook : Form
    {
        public string connstring = "Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False";
        public ReturnBook()
        {
            InitializeComponent();
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM BorrowingSystem", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt; // dinidisplay yung data sa DataGridView

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void uidTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            MainForm mForm = new MainForm();
            mForm.Show();
            this.Hide();
        }

        private void returnBookButton_Click(object sender, EventArgs e)
        {
            string username;
            int bookID;

            // if null, error
            if (string.IsNullOrWhiteSpace(unTB.Text) || string.IsNullOrWhiteSpace(biTB.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            username = unTB.Text.Trim();  // kukuni username as string

            if (!int.TryParse(biTB.Text, out bookID)) // iko-convert yung BookID sa integer
            {
                MessageBox.Show("Please enter a valid Book ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();

                    // Step 1: Check if the user has borrowed this book
                    string check1 = "SELECT COUNT(*) FROM BorrowingSystem WHERE username = @username AND bookID = @bookID";
                    using (SqlCommand checkCmd = new SqlCommand(check1, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        checkCmd.Parameters.AddWithValue("@bookID", bookID);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            MessageBox.Show("No record found for this Username and Book ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Step 2: Process return using a transaction
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // eto yung i-uupdate yung available copies kapag sinoli na (+1)
                            string update1 = "UPDATE Books SET AvailableCopies = AvailableCopies + 1 WHERE BookID = @BookID";
                            using (SqlCommand updateCmd = new SqlCommand(update1, conn, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@BookID", bookID);
                                updateCmd.ExecuteNonQuery();
                            }

                            // buburahin yung record sa listahan ng humiran at kung ano yung hiniram na book
                            string delete1 = "DELETE FROM BorrowingSystem WHERE username = @username AND bookID = @bookID";
                            using (SqlCommand deleteCmd = new SqlCommand(delete1, conn, transaction))
                            {
                                deleteCmd.Parameters.AddWithValue("@username", username);
                                deleteCmd.Parameters.AddWithValue("@bookID", bookID);
                                deleteCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show("Book returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            MainForm mForm = new MainForm();
                            mForm.Show();
                            this.Hide();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error: " + ex.Message, "Transaction Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to Database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ReturnBook_Load(object sender, EventArgs e)
        {
            unTB.Text = "" + SessionManager.Username;
            unTB.ReadOnly = true;
            biTB.ReadOnly = true;
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
    }
}
