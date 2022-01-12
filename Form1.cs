using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using CredentialManagement;
using System.Windows.Forms;
using System.Drawing;
using System.DirectoryServices.ActiveDirectory;

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
                if (string.IsNullOrWhiteSpace(cred.Item1) | string.IsNullOrWhiteSpace(cred.Item2))
                {
                    MessageBox.Show("There was an error fetching credentials for the selected domain.\n" +
                "Please be sure that you have specified those using the Windows Credential Manager");
                    toolStripComboBox1.SelectedIndex = 0;
                    this.user_DataGridView1.ClearSelection();
                    this.user_DataGridView1.DataSource = null;
                }
                else
                {
                    _adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName, cred.Item1, cred.Item2);
                    _manager.SetContext(_adcontext);
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
                return Tuple.Create("", "");
            }
        }

        private void toolStripButton_UserSearch_Click(object sender, EventArgs e)
        {
            if (_currentDomainContextName == null) { MessageBox.Show("Please select a domain."); return; }
            if (textbox_UserName.Text == "")
            {
                MessageBox.Show("Please enter something to search.");
                return;
            }
            else
            {
                _manager.SetContext(_adcontext);
                _manager.SetObjectOfInterestSearchTerm(textbox_UserName.Text);
                var searchData = _manager.QueryDirectoryUsers();
                var source = new BindingSource(searchData, null);
                user_DataGridView1.DataSource = source;
            }

        }

        private void toolStripButton_DeviceSearch_Click(object sender, EventArgs e)
        {
            if (_currentDomainContextName == null) { MessageBox.Show("Please select a domain."); return; }
            if (textBox_DeviceName.Text == "")
            {
                MessageBox.Show("Please enter something to search.");
                return;
            }
            else
            {
                _manager.SetContext(_adcontext);
                _manager.SetObjectOfInterestSearchTerm(textBox_DeviceName.Text);
                var cred = GetCredentialsByDomainController(_currentDomainContextName);
                var searchData = _manager.QueryDirectoryDevices(cred.Item1, cred.Item2);
                var source = new BindingSource(searchData, null);
                user_DataGridView1.DataSource = source;

            }

        }

        private void user_DataGridView1_CellMouseDown(Object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    this.user_DataGridView1.ClearSelection();
                    this.user_DataGridView1.Rows[rowSelected].Selected = true;
                    // you now have the selected row with the context menu showing for the user to delete etc.
                    ContextMenuStrip cm = new ContextMenuStrip();
                    this.ContextMenuStrip = cm;
                    cm.Items.Add(new ToolStripMenuItem("&Unlock", null, new System.EventHandler(this.unlockUser_Click)));
                    cm.Items.Add(new ToolStripMenuItem("&Reset Password", null, new System.EventHandler(this.resetPassword_Click)));
                    cm.Items.Add(new ToolStripMenuItem("&Get Groups", null, new System.EventHandler(this.getGroups_Click)));
                    cm.Items.Add(new ToolStripMenuItem("&Clone", null, new System.EventHandler(this.cloneUser_Click)));
                    cm.Show(Cursor.Position);
                }

            }


        }

        private void cloneUser_Click(object? sender, EventArgs e)
        {
            try
            {
                //get the username
                var userName = this.user_DataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                //Parse the OU so that newly created user will be in the same organizational unit
                List<string> ouData = this.user_DataGridView1.SelectedRows[0].Cells[4].Value.ToString().Split(",").ToList();
                ouData.RemoveAt(0);
                string userOU = string.Join(",", ouData);

                //Get the groups of the selected user which the new user will be apart of.
                _manager.SetUserOfInterestByIdentity(userName);
                var userGroups = _manager.GetUserGroupMemberships(null);

                //set the OU for the context to match current user
                SetContextContainer(userOU);

                //pass it into the manager
                _manager.SetContext(_adcontext);


                //This will be replaced with a dialog box accepting the following.
                _manager.CreateUser("testUserFirst5", "testUserLast5", "testuser005", "SecureOne!@", userOU);
                SetContextContainer("");
                _manager.SetUserOfInterestByIdentity("testuser005");
                _manager.AddUserToGroups(userGroups, "testuser005");
                MessageBox.Show($"User: {userName} successfully cloned to testuser005");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR OCCURED CLONING USER");
            }
        }

        private void getGroups_Click(object? sender, EventArgs e)
        {
            List<string> groupNames = new();
            var userName = this.user_DataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            _manager.SetContext(_adcontext);
            _manager.SetUserOfInterestByIdentity(userName);
            var userGroups = _manager.GetUserGroupMemberships(userName);
            if (userGroups.Count > 0)
            {
                foreach (var group in userGroups)
                {
                    groupNames.Add(group.Name);
                }
                var message = string.Join(Environment.NewLine, groupNames);
                MessageBox.Show(message, "USER GROUPS");
            }
        }
        private void resetPassword_Click(object? sender, EventArgs e)
        {
            var newPassword = "";
            if (InputBox.Show("Reset Password", "Please enter the new password", ref newPassword) == System.Windows.Forms.DialogResult.OK)
            {
                var userName = this.user_DataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                _manager.SetContext(_adcontext);
                _manager.SetUserOfInterestByIdentity(userName);
                if (_manager.SetPassword(newPassword))
                {
                    MessageBox.Show($"Password reset for {userName} successfully.");
                }
                else
                {
                    //Exception message shown via ActiveDirectorManager class.
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void unlockUser_Click(object? sender, EventArgs e)
        {
            _manager.SetContext(_adcontext);
            _manager.SetUserOfInterestByIdentity(this.user_DataGridView1.Rows[0].Cells[0].Value.ToString());
            if (_manager.UnlockUser())
            {
                MessageBox.Show($"User: {this.user_DataGridView1.Rows[0].Cells[0].Value.ToString()} is now unlocked");
            }
            else
            {
                MessageBox.Show($"User: {this.user_DataGridView1.Rows[0].Cells[0].Value.ToString()} is already unlocked.");

            }

        }

        private void SetContextContainer(string container)
        {
            if (_currentDomainContextName == _connectedDomain)
            {
                try
                {
                    if (string.IsNullOrEmpty(container))
                    {
                        _adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName);
                    }
                    else
                    {
                        _adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName, container);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR COULD NOT SET CONTEXT");
                }
            }
            else
            {
                var cred = GetCredentialsByDomainController(_currentDomainContextName);
                if (string.IsNullOrWhiteSpace(cred.Item1) | string.IsNullOrWhiteSpace(cred.Item2))
                {
                    MessageBox.Show("There was an error setting setting the location to the new OU.\n" +
                "Please be sure that the OU exists and you are still authenticated to Active Directory");
                    this.user_DataGridView1.DataSource = null;
                }
                else
                {
                    try
                    {
                        if (string.IsNullOrEmpty(container))
                        {
                            _adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName, cred.Item1, cred.Item2);
                        }
                        else
                        {
                            _adcontext = new PrincipalContext(ContextType.Domain, _currentDomainContextName, container, cred.Item1, cred.Item2);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR COULD NOT SET CONTEXT");
                    }
                }
            }
        }
    }
}