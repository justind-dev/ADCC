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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textbox_userSAM
            // 
            this.textbox_userSAM.Location = new System.Drawing.Point(119, 32);
            this.textbox_userSAM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textbox_userSAM.Name = "textbox_userSAM";
            this.textbox_userSAM.Size = new System.Drawing.Size(116, 23);
            this.textbox_userSAM.TabIndex = 1;
            this.textbox_userSAM.WordWrap = false;
            // 
            // btn_unlock_userSAM
            // 
            this.btn_unlock_userSAM.Location = new System.Drawing.Point(23, 28);
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
            this.textBox_userDNFind.Location = new System.Drawing.Point(119, 61);
            this.textBox_userDNFind.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_userDNFind.Name = "textBox_userDNFind";
            this.textBox_userDNFind.Size = new System.Drawing.Size(116, 23);
            this.textBox_userDNFind.TabIndex = 3;
            // 
            // btn_FindUserDN
            // 
            this.btn_FindUserDN.Location = new System.Drawing.Point(23, 58);
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
            this.label_domainContext.Location = new System.Drawing.Point(449, 35);
            this.label_domainContext.Name = "label_domainContext";
            this.label_domainContext.Size = new System.Drawing.Size(49, 15);
            this.label_domainContext.TabIndex = 8;
            this.label_domainContext.Text = "Domain";
            // 
            // ComboBox1
            // 
            this.ComboBox1.FormattingEnabled = true;
            this.ComboBox1.Location = new System.Drawing.Point(504, 32);
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(121, 23);
            this.ComboBox1.TabIndex = 10;
            this.ComboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersToolStripMenuItem,
            this.groupsToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(637, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.usersToolStripMenuItem.Text = "Common Tasks";
            // 
            // groupsToolStripMenuItem
            // 
            this.groupsToolStripMenuItem.Name = "groupsToolStripMenuItem";
            this.groupsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.groupsToolStripMenuItem.Text = "Reports";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 430);
            this.Controls.Add(this.ComboBox1);
            this.Controls.Add(this.label_domainContext);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btn_FindUserDN);
            this.Controls.Add(this.textBox_userDNFind);
            this.Controls.Add(this.btn_unlock_userSAM);
            this.Controls.Add(this.textbox_userSAM);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "ActiveDirectory Command Center";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private MenuStrip menuStrip1;
        private ToolStripMenuItem usersToolStripMenuItem;
        private ToolStripMenuItem groupsToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
    }
}