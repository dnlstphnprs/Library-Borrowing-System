namespace LibrarySystem
{
    partial class AdminAccess
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LogOutbtn = new System.Windows.Forms.Button();
            this.ViewUsers = new System.Windows.Forms.Button();
            this.ViewInv = new System.Windows.Forms.Button();
            this.ViewBB = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 45);
            this.panel1.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button2.Location = new System.Drawing.Point(424, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(38, 32);
            this.button2.TabIndex = 9;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(324, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "LIBRARY SYSTEM | ADMIN ACCESS";
            // 
            // LogOutbtn
            // 
            this.LogOutbtn.BackColor = System.Drawing.Color.White;
            this.LogOutbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LogOutbtn.Font = new System.Drawing.Font("Impact", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogOutbtn.Location = new System.Drawing.Point(12, 257);
            this.LogOutbtn.Name = "LogOutbtn";
            this.LogOutbtn.Size = new System.Drawing.Size(65, 34);
            this.LogOutbtn.TabIndex = 6;
            this.LogOutbtn.Text = "Log Out";
            this.LogOutbtn.UseVisualStyleBackColor = false;
            this.LogOutbtn.Click += new System.EventHandler(this.LogOutbtn_Click);
            // 
            // ViewUsers
            // 
            this.ViewUsers.BackColor = System.Drawing.Color.White;
            this.ViewUsers.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ViewUsers.Font = new System.Drawing.Font("Impact", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewUsers.Location = new System.Drawing.Point(14, 121);
            this.ViewUsers.Name = "ViewUsers";
            this.ViewUsers.Size = new System.Drawing.Size(165, 41);
            this.ViewUsers.TabIndex = 4;
            this.ViewUsers.Text = "View User Login Details";
            this.ViewUsers.UseVisualStyleBackColor = false;
            this.ViewUsers.Click += new System.EventHandler(this.ViewUsers_Click);
            // 
            // ViewInv
            // 
            this.ViewInv.BackColor = System.Drawing.Color.White;
            this.ViewInv.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ViewInv.Font = new System.Drawing.Font("Impact", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewInv.Location = new System.Drawing.Point(12, 73);
            this.ViewInv.Name = "ViewInv";
            this.ViewInv.Size = new System.Drawing.Size(167, 42);
            this.ViewInv.TabIndex = 8;
            this.ViewInv.Text = "View Books Inventory";
            this.ViewInv.UseVisualStyleBackColor = false;
            this.ViewInv.Click += new System.EventHandler(this.ViewInv_Click);
            // 
            // ViewBB
            // 
            this.ViewBB.BackColor = System.Drawing.Color.White;
            this.ViewBB.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ViewBB.Font = new System.Drawing.Font("Impact", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewBB.Location = new System.Drawing.Point(12, 168);
            this.ViewBB.Name = "ViewBB";
            this.ViewBB.Size = new System.Drawing.Size(167, 41);
            this.ViewBB.TabIndex = 9;
            this.ViewBB.Text = "View Issued Books";
            this.ViewBB.UseVisualStyleBackColor = false;
            this.ViewBB.Click += new System.EventHandler(this.ViewBB_Click);
            // 
            // AdminAccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(470, 301);
            this.Controls.Add(this.ViewBB);
            this.Controls.Add(this.ViewInv);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LogOutbtn);
            this.Controls.Add(this.ViewUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminAccess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdminAccess";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button LogOutbtn;
        private System.Windows.Forms.Button ViewUsers;
        private System.Windows.Forms.Button ViewInv;
        private System.Windows.Forms.Button ViewBB;
    }
}