using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using CredentialManagement;

namespace ADCC
{
    public partial class Form1 : Form
    {
        private readonly string connectedDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        private string currentDomainContextName;
        private ActiveDirectoryManager manager;
        private PrincipalContext adcontext;
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            currentDomainContextName = connectedDomain.ToString();
            GetDomainsFromSettings();
        }
        private void btn_unlock_userSAM_Click(object sender, EventArgs e)
        {
            if (currentDomainContextName == null) { MessageBox.Show("Please select a domain."); return; }
            if (textbox_userSAM.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to unlock them.");
                return;
            }
            else
            {
                manager.SetUserOfInterestByIdentity(textbox_userSAM.Text);
                if (manager.UnlockUser())
                {
                    MessageBox.Show($"User {textbox_userSAM.Text} unlocked.");
                }
                else { MessageBox.Show($"User {textbox_userSAM.Text} already unlocked or does not exist."); }


            }
        }

        private void btn_FindUserDN_Click(object sender, EventArgs e)
        {
            if (currentDomainContextName == null) { MessageBox.Show("Please select a domain."); return; }
            if (textBox_userDNFind.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to find.");
                return;
            }
            else
            {
                manager.SetUserOfInterestByIdentity(textBox_userDNFind.Text);
                var userDistinguishedName = manager.GetDistinguishedName();
                MessageBox.Show($"User DN is : {userDistinguishedName}");
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox1.Text == null) { return; }
            currentDomainContextName = ComboBox1.Text.ToString().ToLower().Trim();
            if (currentDomainContextName == connectedDomain)
            {
                adcontext = new PrincipalContext(ContextType.Domain, currentDomainContextName);
                manager.SetContext(adcontext);
            }
            else
            {
                var cred = GetCredentialsByDomainController(currentDomainContextName);
                if (cred != null)
                {
                    adcontext = new PrincipalContext(ContextType.Domain, currentDomainContextName, cred.Item1, cred.Item2);
                    manager.SetContext(adcontext);
                }
                else
                {
                    MessageBox.Show("There was an error fetching credentials for the selected domain.\n" +
                                    "Please be sure that you have specified those using the Windows Credential Manager");
                    ComboBox1.SelectedIndex = 0;
                }
            }
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
            currentDomainContextName = connectedDomain.ToString();
            var adcontext = new PrincipalContext(ContextType.Domain, currentDomainContextName);
            manager = new ActiveDirectoryManager(adcontext);
            ComboBox1.Text = currentDomainContextName;

        }

        public Tuple<string, string> GetCredentialsByDomainController(string DomainName)
        {
            using var cred = new Credential();
            cred.Target = DomainName;
            if (cred.Exists())
            {
                cred.Load();
                return Tuple.Create(cred.Username, cred.Password);
            }
            else
            {
                return null;
            }
        }

    }
}