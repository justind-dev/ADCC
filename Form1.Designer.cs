namespace ADCC
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textbox_userSAM = new System.Windows.Forms.TextBox();
            this.btn_unlock_userSAM = new System.Windows.Forms.Button();
            this.textBox_userDNFind = new System.Windows.Forms.TextBox();
            this.btn_FindUserDN = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label_domainContext = new System.Windows.Forms.Label();
            this.ComboBox1 = new System.Windows.Forms.ComboBox();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.TabUsers = new System.Windows.Forms.TabPage();
            this.TabComputers = new System.Windows.Forms.TabPage();
            this.MainTabControl.SuspendLayout();
            this.TabUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // textbox_userSAM
            // 
            this.textbox_userSAM.Location = new System.Drawing.Point(113, 29);
            this.textbox_userSAM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textbox_userSAM.Name = "textbox_userSAM";
            this.textbox_userSAM.Size = new System.Drawing.Size(116, 23);
            this.textbox_userSAM.TabIndex = 1;
            this.textbox_userSAM.WordWrap = false;
            // 
            // btn_unlock_userSAM
            // 
            this.btn_unlock_userSAM.Location = new System.Drawing.Point(17, 25);
            this.btn_unlock_userSAM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_unlock_userSAM.Name = "btn_unlock_userSAM";
            this.btn_unlock_userSAM.Size = new System.Drawing.Size(88, 27);
            this.btn_unlock_userSAM.TabIndex = 2;
            this.btn_unlock_userSAM.Text = "Unlock";
            this.btn_unlock_userSAM.UseVisualStyleBackColor = true;
            this.btn_unlock_userSAM.Click += new System.EventHandler(this.btn_unlock_userSAM_Click);
            // 
            // textBox_userDNFind
            // 
            this.textBox_userDNFind.Location = new System.Drawing.Point(113, 62);
            this.textBox_userDNFind.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_userDNFind.Name = "textBox_userDNFind";
            this.textBox_userDNFind.Size = new System.Drawing.Size(116, 23);
            this.textBox_userDNFind.TabIndex = 3;
            // 
            // btn_FindUserDN
            // 
            this.btn_FindUserDN.Location = new System.Drawing.Point(17, 58);
            this.btn_FindUserDN.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_FindUserDN.Name = "btn_FindUserDN";
            this.btn_FindUserDN.Size = new System.Drawing.Size(88, 27);
            this.btn_FindUserDN.TabIndex = 4;
            this.btn_FindUserDN.Text = "Find DN";
            this.btn_FindUserDN.UseVisualStyleBackColor = true;
            this.btn_FindUserDN.Click += new System.EventHandler(this.btn_FindUserDN_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label_domainContext
            // 
            this.label_domainContext.AutoSize = true;
            this.label_domainContext.Location = new System.Drawing.Point(449, 15);
            this.label_domainContext.Name = "label_domainContext";
            this.label_domainContext.Size = new System.Drawing.Size(49, 15);
            this.label_domainContext.TabIndex = 8;
            this.label_domainContext.Text = "Domain";
            // 
            // ComboBox1
            // 
            this.ComboBox1.FormattingEnabled = true;
            this.ComboBox1.Location = new System.Drawing.Point(504, 12);
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(121, 23);
            this.ComboBox1.TabIndex = 10;
            this.ComboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MainTabControl.Controls.Add(this.TabUsers);
            this.MainTabControl.Controls.Add(this.TabComputers);
            this.MainTabControl.Location = new System.Drawing.Point(0, 37);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(637, 430);
            this.MainTabControl.TabIndex = 11;
            // 
            // TabUsers
            // 
            this.TabUsers.Controls.Add(this.btn_unlock_userSAM);
            this.TabUsers.Controls.Add(this.textbox_userSAM);
            this.TabUsers.Controls.Add(this.btn_FindUserDN);
            this.TabUsers.Controls.Add(this.textBox_userDNFind);
            this.TabUsers.Location = new System.Drawing.Point(4, 24);
            this.TabUsers.Name = "TabUsers";
            this.TabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.TabUsers.Size = new System.Drawing.Size(629, 402);
            this.TabUsers.TabIndex = 0;
            this.TabUsers.Text = "Users";
            this.TabUsers.UseVisualStyleBackColor = true;
            // 
            // TabComputers
            // 
            this.TabComputers.Location = new System.Drawing.Point(4, 24);
            this.TabComputers.Name = "TabComputers";
            this.TabComputers.Padding = new System.Windows.Forms.Padding(3);
            this.TabComputers.Size = new System.Drawing.Size(629, 402);
            this.TabComputers.TabIndex = 1;
            this.TabComputers.Text = "Computers";
            this.TabComputers.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 467);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.label_domainContext);
            this.Controls.Add(this.ComboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ActiveDirectory Command Center";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainTabControl.ResumeLayout(false);
            this.TabUsers.ResumeLayout(false);
            this.TabUsers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox textbox_userSAM;
        private Button btn_unlock_userSAM;
        private TextBox textBox_userDNFind;
        private Button btn_FindUserDN;
        private ContextMenuStrip contextMenuStrip1;
        private Label label_domainContext;
        private ComboBox ComboBox1;
        private TabControl MainTabControl;
        private TabPage TabUsers;
        private TabPage TabComputers;
    }
}