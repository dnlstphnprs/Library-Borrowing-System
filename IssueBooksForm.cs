using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LibrarySystem.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace LibrarySystem
{
    public partial class IssueBooksForm : Form
    {
        public string connstring = "Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False";
        public IssueBooksForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM books", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt; //eto yung nagdidisplay ng table sa DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void label4_Click(object sender, EventArgs e)
        {
            MainForm mForm = new MainForm();
            mForm.Show();
            this.Hide();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0) // sinisigurado na row 0 yung selected once wala pang naka-select
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // autofill sa textbox para i-click
                biTB.Text = row.Cells["BookID"].Value.ToString();
                bnTB.Text = row.Cells["BookName"].Value.ToString();
            }
        }

        private void IssueBooksForm_Load(object sender, EventArgs e)
        {
            unTB.Text = "" + SessionManager.Username;
            unTB.ReadOnly = true;
            biTB.ReadOnly = true;
            bnTB.ReadOnly = true;
        }

        private void borrowBookButton_Click(object sender, EventArgs e)
        {
            int bookID; // variable para sa book id, yan yung parang id para mahanap yung libro kapag hihiramin

            // error message kapag blangko mga text
            if (string.IsNullOrWhiteSpace(unTB.Text) || string.IsNullOrWhiteSpace(biTB.Text) || string.IsNullOrWhiteSpace(bnTB.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // kung wala yung user id = error
            if (!int.TryParse(biTB.Text, out bookID))
            {
                MessageBox.Show("Please enter a valid Book ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();

                    // Step 1: ichecheck kung 3 times na nakahiram si user
                    // "check1" SQL command para icheck kung ilang beses na humiram yung user
                    string check1 = "SELECT COUNT(*) FROM BorrowingSystem WHERE username = @username";
                    using (SqlCommand checkUserCmd = new SqlCommand(check1, conn))
                    {
                        checkUserCmd.Parameters.AddWithValue("@username", unTB.Text.Trim());
                        int borrowCount = (int)checkUserCmd.ExecuteScalar();

                        if (borrowCount >= 3) // if 3 na, limit na at bawal na humiram
                        {
                            MessageBox.Show("You already borrowed 3 books,\nplease return one before you can borrow another.",
                                "LIMIT REACHED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Step 2: ichecheck kung nahiram na ni user yung iisang libro
                    // "check2" = SQL command used para i-check kung magkakaroon ng duplicate na record
                    string check2 = "SELECT COUNT(*) FROM BorrowingSystem WHERE username = @username AND bookID = @bookID";
                    using (SqlCommand checkDuplicateCmd = new SqlCommand(check2, conn))
                    {
                        checkDuplicateCmd.Parameters.AddWithValue("@username", unTB.Text.Trim());
                        checkDuplicateCmd.Parameters.AddWithValue("@bookID", bookID);
                        int duplicateCount = (int)checkDuplicateCmd.ExecuteScalar();

                        if (duplicateCount > 0)
                        {
                            MessageBox.Show("You have already borrowed this book.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Step 3: ichecheck niya kung avaialable pa yung libro
                    // "check3" = SQL command used para icheck kung ilang kopya yung available sa libro
                    string check3 = "SELECT AvailableCopies FROM Books WHERE BookId = @BookId";
                    using (SqlCommand checkBookCmd = new SqlCommand(check3, conn))
                    {
                        checkBookCmd.Parameters.AddWithValue("@BookId", bookID);
                        object result = checkBookCmd.ExecuteScalar();

                        if (result == null) // kapag hindi mahanap yung libro
                        {
                            MessageBox.Show("Book ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        int availableBooks = Convert.ToInt32(result);

                        if (availableBooks <= 0) // kapag wala na extrang kopya
                        {
                            MessageBox.Show("No available copies of this book.", "Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Step 4: hihiramin na yung libro
                    using (SqlTransaction transaction = conn.BeginTransaction()) // sisimulan na yung transaction
                    {
                        try
                        {
                            // iu-update yung number ng libro (-1)
                            string update1 = "UPDATE Books SET AvailableCopies = AvailableCopies - 1 WHERE BookID = @BookID";
                            using (SqlCommand updateBookCmd = new SqlCommand(update1, conn, transaction))
                            {
                                updateBookCmd.Parameters.AddWithValue("@BookID", bookID);
                                updateBookCmd.ExecuteNonQuery();
                            }

                            // ipapasok yung libro na nahiram pati kung sino yung humiram
                            string update2 = "INSERT INTO BorrowingSystem (username, bookID, bookName) VALUES (@username, @bookID, @bookName)";
                            using (SqlCommand insertBorrowCmd = new SqlCommand(update2, conn, transaction))
                            {
                                insertBorrowCmd.Parameters.AddWithValue("@username", unTB.Text.Trim());
                                insertBorrowCmd.Parameters.AddWithValue("@bookID", bookID);
                                insertBorrowCmd.Parameters.AddWithValue("@bookName", bnTB.Text.Trim());

                                insertBorrowCmd.ExecuteNonQuery();
                            }

                            // itutuloy yung transaction
                            transaction.Commit();

                            MessageBox.Show("Book borrowed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            MainForm mForm = new MainForm();
                            mForm.Show();
                            this.Hide();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // rollback kapag may error
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

        private void button4_Click(object sender, EventArgs e)
        {
            MainForm mForm = new MainForm();
            mForm.Show();
            this.Hide();
        }
    }
}