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
        private string currentDomainContext;
        public ActiveDirectoryManager manager;
        private PrincipalContext adcontext;
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
            if (currentDomainContext == null) { MessageBox.Show("Please select a domain."); return; }
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
            currentDomainContext = ComboBox1.Text.ToString().ToLower().Trim();
            if (currentDomainContext == connectedDomain)
            {
                adcontext = new PrincipalContext(ContextType.Domain, currentDomainContext);
                manager.SetContext(adcontext);
            }
            else
            {
                var cred = GetCredentialsByDomainController(currentDomainContext);
                adcontext = new PrincipalContext(ContextType.Domain, currentDomainContext, cred.Item1, cred.Item2);
                manager.SetContext(adcontext);
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
            currentDomainContext = connectedDomain.ToString();
            var adcontext = new PrincipalContext(ContextType.Domain, currentDomainContext);
            manager = new ActiveDirectoryManager(adcontext);
            ComboBox1.Text = currentDomainContext;

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