using System.Net.NetworkInformation;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using CredentialManagement;

namespace ADCC;

public partial class Form1 : Form
{
    private readonly string localComputerDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
    private string currentDomainContextName;
    private ActiveDirectoryManager manager;
    private PrincipalContext context;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        currentDomainContextName = localComputerDomain;
        GetDomainsFromSettings();
    }

    private void ButtonUnlockUser(object sender, EventArgs e)
    {
        if (currentDomainContextName == null)
        {
            MessageBox.Show("Please select a domain.");
            return;
        }

        if (textbox_userSAM.Text == "")
        {
            MessageBox.Show("Please enter the sAMAccountName for the user to unlock them.");
            return;
        }

        manager.SetUserOfInterestByIdentity(textbox_userSAM.Text);
        MessageBox.Show(manager.UnlockUser()
            ? $"User {textbox_userSAM.Text} unlocked."
            : $"User {textbox_userSAM.Text} already unlocked or does not exist.");
    }

    private void ButtonFindUserDn(object sender, EventArgs e)
    {
        if (currentDomainContextName == null)
        {
            MessageBox.Show("Please select a domain.");
            return;
        }

        if (textBoxUserDnFind.Text == "")
        {
            MessageBox.Show("Please enter the sAMAccountName for the user to find.");
            return;
        }

        manager.SetUserOfInterestByIdentity(textBoxUserDnFind.Text);
        var userDistinguishedName = manager.GetDistinguishedName();
        MessageBox.Show($"User DN is : {userDistinguishedName}");
    }

    private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ComboBox1.Text == null)
        {
            return;
        }

        currentDomainContextName = ComboBox1.Text.ToLower().Trim();
        if (currentDomainContextName == localComputerDomain)
        {
            context = new PrincipalContext(ContextType.Domain, currentDomainContextName);
            manager.SetContext(context);
        }
        else
        {
            var cred = GetCredentialsByDomainController(currentDomainContextName);
            if (cred != null)
            {
                context = new PrincipalContext(ContextType.Domain, currentDomainContextName, cred.Item1,
                    cred.Item2);
                manager.SetContext(context);
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
        ComboBox1.Items.Add(localComputerDomain.Trim());
        foreach (var domain in ConfigurationManager.AppSettings["domains"].Split(","))
        {
            ComboBox1.Items.Add(domain);
        }

        ComboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        currentDomainContextName = localComputerDomain;
        
        var context = new PrincipalContext(ContextType.Domain, currentDomainContextName);
        manager = new ActiveDirectoryManager(context);
        ComboBox1.Text = currentDomainContextName;
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