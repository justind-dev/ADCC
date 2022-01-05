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
            this.textbox_userSAM = new System.Windows.Forms.TextBox();
            this.btn_unlock_userSAM = new System.Windows.Forms.Button();
            this.textBox_userDNFind = new System.Windows.Forms.TextBox();
            this.btn_FindUserDN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textbox_userSAM
            // 
            this.textbox_userSAM.Location = new System.Drawing.Point(93, 12);
            this.textbox_userSAM.Name = "textbox_userSAM";
            this.textbox_userSAM.Size = new System.Drawing.Size(100, 20);
            this.textbox_userSAM.TabIndex = 1;
            this.textbox_userSAM.WordWrap = false;
            // 
            // btn_unlock_userSAM
            // 
            this.btn_unlock_userSAM.Location = new System.Drawing.Point(12, 10);
            this.btn_unlock_userSAM.Name = "btn_unlock_userSAM";
            this.btn_unlock_userSAM.Size = new System.Drawing.Size(75, 23);
            this.btn_unlock_userSAM.TabIndex = 2;
            this.btn_unlock_userSAM.Text = "Unlock";
            this.btn_unlock_userSAM.UseVisualStyleBackColor = true;
            this.btn_unlock_userSAM.Click += new System.EventHandler(this.btn_unlock_userSAM_Click);
            // 
            // textBox_userDNFind
            // 
            this.textBox_userDNFind.Location = new System.Drawing.Point(93, 42);
            this.textBox_userDNFind.Name = "textBox_userDNFind";
            this.textBox_userDNFind.Size = new System.Drawing.Size(100, 20);
            this.textBox_userDNFind.TabIndex = 3;
            // 
            // btn_FindUserDN
            // 
            this.btn_FindUserDN.Location = new System.Drawing.Point(12, 39);
            this.btn_FindUserDN.Name = "btn_FindUserDN";
            this.btn_FindUserDN.Size = new System.Drawing.Size(75, 23);
            this.btn_FindUserDN.TabIndex = 4;
            this.btn_FindUserDN.Text = "Find DN";
            this.btn_FindUserDN.UseVisualStyleBackColor = true;
            this.btn_FindUserDN.Click += new System.EventHandler(this.btn_FindUserDN_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 118);
            this.Controls.Add(this.btn_FindUserDN);
            this.Controls.Add(this.textBox_userDNFind);
            this.Controls.Add(this.btn_unlock_userSAM);
            this.Controls.Add(this.textbox_userSAM);
            this.Name = "Form1";
            this.Text = "ActiveDirectory Command Center";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox textbox_userSAM;
        private Button btn_unlock_userSAM;
        private TextBox textBox_userDNFind;
        private Button btn_FindUserDN;
    }
}