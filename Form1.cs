using System;
using System.Net;
using System.Net.NetworkInformation;
namespace ADCC
{
    public partial class Form1 : Form
    {
        readonly ADManager manager = new();
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void btn_unlock_userSAM_Click(object sender, EventArgs e)
        {
            if (textbox_userSAM.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to unlock them.");
                return;
            }
            else
            {
                MessageBox.Show(manager.UnlockUser(textbox_userSAM.Text, textBox_DomainName.Text.Trim()));
            }
        }

        private void btn_FindUserDN_Click(object sender, EventArgs e)
        {
            if (textBox_userDNFind.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to find.");
                return;
            }
            else
            {
                if (textBox_DomainName.Text == "")
                {
                    var userDistinguishedName = manager.GetDistinguishedName(textBox_userDNFind.Text.Trim(), "");
                    MessageBox.Show($"User DN is : {userDistinguishedName}");
                }
                else
                {
                    var userDistinguishedName = manager.GetDistinguishedName(textBox_userDNFind.Text.Trim(), 
                                                                             textBox_DomainName.Text.Trim());
                    MessageBox.Show($"User DN is : {userDistinguishedName}");
                }
            }
        }

    }
}