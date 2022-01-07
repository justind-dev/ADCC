using System.Net.NetworkInformation;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using CredentialManagement;

namespace ADCC
{
    public partial class Form1 : Form
    {
        private readonly string _connectedDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        private string _currentDomainContextName;
        private ActiveDirectoryManager _manager;
        private PrincipalContext _currentContext;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _currentDomainContextName = _connectedDomain;
            GetDomainsFromSettings();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text == null)
            {
                return;
            }

            _currentDomainContextName = toolStripComboBox1.Text.ToLower().Trim();
            if (_currentDomainContextName == _connectedDomain)
            {
                _currentContext = new PrincipalContext(ContextType.Domain, _currentDomainContextName);
                _manager.SetContext(_currentContext);
            }
            else
            {
                var (item1, item2) = GetCredentialsByDomainController(_currentDomainContextName);
                if (!string.IsNullOrWhiteSpace(item1) && !string.IsNullOrWhiteSpace(item2))
                {
                    _currentContext = new PrincipalContext(ContextType.Domain, _currentDomainContextName, item1,
                        item2);
                    _manager.SetContext(_currentContext);
                    return;
                }
                MessageBox.Show("There was an error fetching credentials for the selected domain.\n" +
                                "Please be sure that you have specified those using the Windows Credential Manager");
                toolStripComboBox1.SelectedIndex = 0;
            }
        }

        private void GetDomainsFromSettings()
        {
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Items.Add(_connectedDomain.ToString().Trim());
            foreach (var domain in ConfigurationManager.AppSettings["domains"].Split(","))
            {
                toolStripComboBox1.Items.Add(domain);
            }

            toolStripComboBox1.SelectedIndexChanged += toolStripComboBox1_SelectedIndexChanged;
            _currentDomainContextName = _connectedDomain;
            var adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName);
            _manager = new ActiveDirectoryManager(adcontext);
            toolStripComboBox1.Text = _currentDomainContextName;
        }

        private static Tuple<string, string> GetCredentialsByDomainController(string DomainName)
        {
            using var cred = new Credential();
            cred.Target = DomainName;
            if (!cred.Exists()) return new Tuple<string, string>("","");
            cred.Load();
            return Tuple.Create(cred.Username, cred.Password);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //populate grid after searching using the textbox_userSAM contents.
            if (_currentDomainContextName == null)
            {
                MessageBox.Show("Please select a domain.");
                return;
            }

            if (textbox_userSAM.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to unlock them.");
                return;
            }

            _manager.SetContext(_currentContext);
            _manager.SetObjectOfInterestSearchTerm(textbox_userSAM.Text);
            var userData = _manager.QueryDirectory();
            var source = new BindingSource(userData, null);
            user_DataGridView1.DataSource = source;
        }
    }
}