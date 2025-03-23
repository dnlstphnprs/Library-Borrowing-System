using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LibrarySystem
{ 
    public partial class Form1 : Form
    {
        private SqlConnection connect = new SqlConnection(@"Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False");
        private string connstring = @"Data Source=dnl;Initial Catalog=LBS;Integrated Security=True;Encrypt=False";
        public Form1()
        {
            InitializeComponent();
        }

        public static class SessionManager
        {
            public static string Username { get; set; } // get = kukunin yung na-input na data set = iseset yung data na nakuha
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(loginUN.Text) || string.IsNullOrWhiteSpace(loginPW.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(connstring)) 
                {
                    connect.Open();

                    // kukunin yung username, login
                    string selectData = "SELECT username, userRole FROM userlogin WHERE username = @username AND password = @password";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@username", loginUN.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", loginPW.Text.Trim());
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) //chinecheck niya kung nag-eexist yung user
                            {
                                // ginagamit 'to para sa next form (para sa user) naka-display yung username ng nag login
                                SessionManager.Username = reader["username"].ToString();
                                string userRole = reader["userRole"].ToString(); //babasahin sa table yung Role ng user

                                MessageBox.Show("Login Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Redirection base sa role nila
                                if (userRole == "Admin")
                                {
                                    AdminAccess AAForm = new AdminAccess();
                                    AAForm.Show();
                                    this.Hide();                                
                                }
                                else if (userRole == "User")
                                {
                                    MainForm MForm = new MainForm();
                                    MForm.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Unknown role detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                this.Hide(); // tinatago yung form
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Username/Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to Database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SignUpLabel_Click(object sender, EventArgs e)
        {
            RegForm Rform = new RegForm();
            Rform.Show();
            this.Hide();
        }

        private void showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (showPass.Checked)
            {
                loginPW.UseSystemPasswordChar = false; // Ipapakita yung characters if checked
            }
            else
            {
                loginPW.UseSystemPasswordChar = true;  // Itatago yung characters since hindi naka check
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
