using System.Net.NetworkInformation;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using CredentialManagement;

namespace ADCC;

public partial class Form1 : Form
{
    private readonly string _localComputerDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
    private string _currentDomainContextName;
    private ActiveDirectoryManager _manager;
    private PrincipalContext _context;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        _currentDomainContextName = _localComputerDomain;
        GetDomainsFromSettings();
    }

    private void ButtonUnlockUser(object sender, EventArgs e)
    {
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

        _manager.SetUserOfInterestByIdentity(textbox_userSAM.Text);
        MessageBox.Show(_manager.UnlockUser()
            ? $"User {textbox_userSAM.Text} unlocked."
            : $"User {textbox_userSAM.Text} already unlocked or does not exist.");
    }

    private void ButtonFindUserDn(object sender, EventArgs e)
    {
        if (_currentDomainContextName == null)
        {
            MessageBox.Show("Please select a domain.");
            return;
        }

        if (textBoxUserDnFind.Text == "")
        {
            MessageBox.Show("Please enter the sAMAccountName for the user to find.");
            return;
        }

        _manager.SetUserOfInterestByIdentity(textBoxUserDnFind.Text);
        var userDistinguishedName = _manager.GetDistinguishedName();
        MessageBox.Show($"User DN is : {userDistinguishedName}");
    }

    private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ComboBox1.Text == null) return;
        _currentDomainContextName = ComboBox1.Text.ToLower().Trim();
        
        if (_currentDomainContextName == _localComputerDomain)
        {
            _context = new PrincipalContext(ContextType.Domain, _currentDomainContextName);
            _manager.SetContext(_context);
        }
        else
        {
            var cred = GetCredentialsByDomainController(_currentDomainContextName);
            if (cred != null)
            {
                _context = new PrincipalContext(ContextType.Domain, _currentDomainContextName, cred.Item1,
                    cred.Item2);
                _manager.SetContext(_context);
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
        ComboBox1.Items.Add(_localComputerDomain.Trim());
        foreach (var domain in ConfigurationManager.AppSettings["domains"].Split(","))
        {
            ComboBox1.Items.Add(domain);
        }

        ComboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        _currentDomainContextName = _localComputerDomain;
        
        var context = new PrincipalContext(ContextType.Domain, _currentDomainContextName);
        _manager = new ActiveDirectoryManager(context);
        ComboBox1.Text = _currentDomainContextName;
    }

    private static Tuple<string, string> GetCredentialsByDomainController(string DomainName)
    {
        using var cred = new Credential();
        cred.Target = DomainName;
        if (!cred.Exists()) return null;
        cred.Load();
        return Tuple.Create(cred.Username, cred.Password);
    }
}