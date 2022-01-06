using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Configuration;

namespace ADCC
{
    public partial class Form1 : Form
    {
        private readonly string connectedDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        private string currentDomainContext;
        public ActiveDirectoryManager manager;

        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            currentDomainContext = connectedDomain.ToString();
            GetDomainsFromSettings();
        }
        private void btn_unlock_userSAM_Click(object sender, EventArgs e)
        {
            if (currentDomainContext == null) { MessageBox.Show("Please select a domain."); return; }
            if (textbox_userSAM.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to unlock them.");
                return;
            }
            else
            {
                try
                {
                    if (manager.UnlockUser(textbox_userSAM.Text))
                    {
                        MessageBox.Show($"User {textbox_userSAM.Text} unlocked.");
                    }
                    else { MessageBox.Show($"User {textbox_userSAM.Text} already unlocked or does not exist."); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex}");
                }
            }
        }

        private void btn_FindUserDN_Click(object sender, EventArgs e)
        {
            if (currentDomainContext == null) { MessageBox.Show("Please select a domain."); return; }
            if (textBox_userDNFind.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to find.");
                return;
            }
            else
            {
                var userDistinguishedName = manager.GetDistinguishedName(textBox_userDNFind.Text.Trim());
                MessageBox.Show($"User DN is : {userDistinguishedName}");
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox1.Text == null) { return; }
            currentDomainContext = ComboBox1.Text.ToString().ToLower().Trim();
            manager.SetContext(currentDomainContext);
        }

        private void GetDomainsFromSettings()
        {
            ComboBox1.Items.Clear();
            ComboBox1.Items.Add(connectedDomain.ToString().Trim());
            foreach (string domain in ConfigurationManager.AppSettings["domains"].Split(","))
            {
                ComboBox1.Items.Add(domain);

            }
            ComboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            currentDomainContext = connectedDomain.ToString();
            manager = new(currentDomainContext);
            ComboBox1.Text = currentDomainContext;

        }

    }
}