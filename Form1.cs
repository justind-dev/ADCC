using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using CredentialManagement;
using System.ComponentModel;

namespace ADCC
{
    public partial class Form1 : Form
    {
        private readonly string _connectedDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        private string _currentDomainContextName;
        private ActiveDirectoryManager _manager;
        private PrincipalContext _adcontext;
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _currentDomainContextName = _connectedDomain.ToString();
            GetDomainsFromSettings();
        }
        private void btn_unlock_userSAM_Click(object sender, EventArgs e)
        {
            if (_currentDomainContextName == null) { MessageBox.Show("Please select a domain."); return; }
            if (textbox_userSAM.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to unlock them.");
                return;
            }
            else
            {
                _manager.SetUserOfInterestByIdentity(textbox_userSAM.Text);
                if (_manager.UnlockUser())
                {
                    MessageBox.Show($"User {textbox_userSAM.Text} unlocked.");
                }
                else { MessageBox.Show($"User {textbox_userSAM.Text} already unlocked or does not exist."); }


            }
        }

        private void btn_FindUserDN_Click(object sender, EventArgs e)
        {
            if (_currentDomainContextName == null) { MessageBox.Show("Please select a domain."); return; }
            if (textbox_userSAM.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to find.");
                return;
            }
            else
            {
                _manager.SetUserOfInterestByIdentity(textbox_userSAM.Text);
                var userDistinguishedName = _manager.GetDistinguishedName();
                MessageBox.Show($"User DN is : {userDistinguishedName}");
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text == null) { return; }
            _currentDomainContextName = toolStripComboBox1.Text.ToString().ToLower().Trim();
            if (_currentDomainContextName == _connectedDomain)
            {
                _adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName);
                _manager.SetContext(_adcontext);
            }
            else
            {
                var cred = GetCredentialsByDomainController(_currentDomainContextName);
                if (cred != null)
                {
                    _adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName, cred.Item1, cred.Item2);
                    _manager.SetContext(_adcontext);
                }
                else
                {
                    MessageBox.Show("There was an error fetching credentials for the selected domain.\n" +
                                    "Please be sure that you have specified those using the Windows Credential Manager");
                    toolStripComboBox1.SelectedIndex = 0;
                }
            }
        }

        private void GetDomainsFromSettings()
        {
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Items.Add(_connectedDomain.ToString().Trim());
            foreach (string domain in ConfigurationManager.AppSettings["domains"].Split(","))
            {
                toolStripComboBox1.Items.Add(domain);

            }
            toolStripComboBox1.SelectedIndexChanged += toolStripComboBox1_SelectedIndexChanged;
            _currentDomainContextName = _connectedDomain.ToString();
            var adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName);
            _manager = new ActiveDirectoryManager(adcontext);
            toolStripComboBox1.Text = _currentDomainContextName;

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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //populate grid after searching using the textbox_userSAM contents.
            if (_currentDomainContextName == null) { MessageBox.Show("Please select a domain."); return; }
            if (textbox_userSAM.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to unlock them.");
                return;
            }
            else
            {
                _manager.SetContext(_adcontext);
                _manager.SetUserOfInterestByIdentity(textbox_userSAM.Text);
                var userData = _manager.GetUserData();
                var source = new BindingSource(userData, null);
                user_DataGridView1.DataSource = source;
            }

        }
    }
}