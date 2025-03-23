using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibrarySystem
{
    
    public partial class RegForm : Form
    {
        
        private SqlConnection connect = new SqlConnection(@"Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False");
        public RegForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginLabel_Click(object sender, EventArgs e)
        {
            Form1 lform = new Form1();
            lform.Show();
            this.Hide();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            if (RegFN.Text == "" || RegUN.Text == "" || RegPW.Text == "" || roleComboBox.SelectedItem == null) //if null, error
            {
                MessageBox.Show("Please fill all blank fields and select a role", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string selectedRole = roleComboBox.SelectedItem.ToString();

                // required ang authentication code if si admin yung magreregister
                if (selectedRole == "Admin")
                {
                    if (AdminCodeTB.Text.Trim() != "307011") //eto yung code, if iba nakalagay error, 
                    {
                        MessageBox.Show("Invalid admin code. Registration denied.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        // ichecheck kung nag-eexist si user
                        String checkUsername = "SELECT COUNT(*) FROM userlogin WHERE username = @username";

                        using (SqlCommand checkCMD = new SqlCommand(checkUsername, connect))
                        {
                            checkCMD.Parameters.AddWithValue("@username", RegUN.Text.Trim());
                            int count = (int)checkCMD.ExecuteScalar();

                            if (count >= 1)
                            {
                                MessageBox.Show(RegUN.Text.Trim() + " is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                // ipapasok yung lahat ng details sa database 
                                String insertData = "INSERT INTO userlogin (fullname, username, password, userRole) " +
                                                    "VALUES(@fullname, @username, @password, @userRole)";

                                using (SqlCommand insertCMD = new SqlCommand(insertData, connect))
                                {
                                    insertCMD.Parameters.AddWithValue("@fullname", RegFN.Text.Trim());
                                    insertCMD.Parameters.AddWithValue("@username", RegUN.Text.Trim());
                                    insertCMD.Parameters.AddWithValue("@password", RegPW.Text.Trim());
                                    insertCMD.Parameters.AddWithValue("@userRole", selectedRole);

                                    insertCMD.ExecuteNonQuery();

                                    MessageBox.Show("Registered successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // after registragion, redirection sa login form
                                    Form1 lForm = new Form1();
                                    lForm.Show();
                                    this.Hide();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting to Database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }
    }
}