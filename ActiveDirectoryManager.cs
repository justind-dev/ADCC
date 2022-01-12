using System.Collections;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Net;

namespace ADCC;

public class ActiveDirectoryManager
{
    private PrincipalContext? _currentContext;
    private UserPrincipal? _objectOfInterest;
    public string? _objectName;

    public ActiveDirectoryManager(PrincipalContext context)
    {
        _currentContext = context;
        _objectOfInterest = null;
        _objectName = null;
    }
    //set current user of interest by idenitity
    public void SetUserOfInterestByIdentity(string identityName)
    {
        _objectName = identityName;
        _objectOfInterest?.Dispose();
        try
        {
            _objectOfInterest = UserPrincipal.FindByIdentity(_currentContext, identityName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR SETTING USER OF INTEREST");
            _objectName = null;
            _objectOfInterest = null;
        }
    }
    //// Set search term for query methods
    public void SetObjectOfInterestSearchTerm(string searchCriteria)
    {
        _objectName = searchCriteria;
    }

    // unlock current user of interest account
    public bool UnlockUser()
    {
        if (_objectOfInterest == null || !_objectOfInterest.IsAccountLockedOut()) return false;
        _objectOfInterest.UnlockAccount();
        return true;
    }

    // set current object of interest password
    public bool SetPassword(string password)
    {
        try
        {
            _objectOfInterest?.SetPassword(password);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR OCCURED DURING PASSWORD RESET");
            return false;
        }
    }

    public string GetOrganizationUnityForUser(string userName)
    {
        string ou;
        if (string.IsNullOrEmpty(userName))
        {
            ou = "";
            return ou;
        }
        ou = "";
        return ou;
    }
    //create new user
    public bool CreateUser(string uFirstName, string uLastName, string usAMAccountName, string uPassword, string uOULocation)
    {
        try
        {
            _currentContext = new PrincipalContext(ContextType.Domain, _currentContext.Name, uOULocation);

            using (var user = new UserPrincipal(_currentContext)
            {
                GivenName = uFirstName,
                Surname = uLastName,
                DisplayName = uFirstName + " " + uLastName,
                Name = uFirstName + " " + uLastName,
                SamAccountName = usAMAccountName,
                UserPrincipalName = usAMAccountName + "@" + _currentContext.Name,
                PasswordNotRequired = false,
                Enabled = true
            })
            {
                user.SetPassword(uPassword);
                user.Save();
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR OCCURED DURING ACCOUNT CREATION");
            return false;
        }
    }

    //get user group memberships
    public List<GroupPrincipal> GetUserGroupMemberships(string userName)
    {
        List<GroupPrincipal> result = new List<GroupPrincipal>();
        try
        {
            PrincipalSearchResult<Principal> groups = (String.IsNullOrWhiteSpace(userName) ? _objectOfInterest.GetAuthorizationGroups() :
                                                      //if username was passed, use that instead.
                                                      UserPrincipal.FindByIdentity(_currentContext, userName).GetAuthorizationGroups());
            foreach (Principal p in groups)
            {
                if (p is GroupPrincipal)
                {
                    result.Add((GroupPrincipal)p);
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR OCCURED RETRIEVING GROUPS");
            return result;
        }
    }
    //get user group memberships -- NOT WORKING CURRENTLY
    public void AddUserToGroups(List<GroupPrincipal> userGroups, string userName)
    {
        try
        {
            SetUserOfInterestByIdentity(userName);

            if (userGroups.Count > 0)
            {
                foreach (GroupPrincipal group in userGroups)
                {
                    var members = group.GetMembers();
                    var gp = GroupPrincipal.FindByIdentity(_currentContext,group.Name);
                    if (members.Contains(_objectOfInterest))
                    {
                        gp.Dispose();
                        continue;
                    }
                    else
                    {
                        gp.Members.Add(_currentContext, IdentityType.SamAccountName, _objectOfInterest.SamAccountName.ToString());
                        gp.Save();
                        gp.Dispose();
                    }


                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR OCCURED ADDING USER TO GROUPS");
        }

    }

    // Search active directory, returning a list of matching users.
    public BindingList<User> QueryDirectoryUsers()
    {
        var userMatches = new BindingList<User>();
        var searchString = String.Format("*{0}*", _objectName);
        var searchMaskDisplayname = new UserPrincipal(_currentContext) { DisplayName = searchString };
        var searchMaskUsername = new UserPrincipal(_currentContext) { SamAccountName = searchString };
        var searcherDisplayname = new PrincipalSearcher(searchMaskDisplayname);
        var searcherUsername = new PrincipalSearcher(searchMaskUsername);
        PrincipalSearchResult<Principal> taskDisplayName = searcherDisplayname.FindAll();
        PrincipalSearchResult<Principal> taskUsername = searcherUsername.FindAll();
        var allMatches = (taskDisplayName.Union(taskUsername)).Distinct();

        foreach (UserPrincipal userPrincipal in allMatches)
            using (userPrincipal)
            {
                userMatches.Add(new User(userPrincipal.SamAccountName, userPrincipal.GivenName, userPrincipal.Surname, userPrincipal.IsAccountLockedOut() ? "True" : "False", userPrincipal.DistinguishedName));
            }

        userMatches = new BindingList<User>(userMatches.ToList());
        return userMatches;

    }

    // Search active directory, returning a list of matching computer objects
    // This one works a little differntly in that it uses a directory entry instead of a PrincipalContext
    public BindingList<Device> QueryDirectoryDevices(string username, string password)
    {
        var deviceMatches = new BindingList<Device>();
        string adPath = @"LDAP://" + _currentContext.Name;
        DirectoryEntry directoryEntry;
        try
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                directoryEntry = new(adPath);

            }
            else
            {
                directoryEntry = new(adPath);
                directoryEntry.Username = username;
                directoryEntry.Password = password;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR OCCURED CONNECTING TO DIRECTORY");
            return null;
        }

        string searchFilter = "(&(objectCategory=computer)(name=*" + _objectName + "*))";
        DirectorySearcher searcher = new DirectorySearcher(directoryEntry, searchFilter);
        String[] propertiesToLoad = new string[] { "name", "dNSHostName", "description", "operatingSystem", "lastLogonTimestamp", "distinguishedName" };
        foreach (string property in propertiesToLoad)
        {
            searcher.PropertiesToLoad.Add(property);
        }
        try
        {
            SearchResultCollection DirectorySearchResults = searcher.FindAll();

            if (DirectorySearchResults == null)
            {
                return null;
            }
            else
            {
                foreach (SearchResult result in DirectorySearchResults)
                {
                    if (String.IsNullOrEmpty(result.Properties["name"][0].ToString()))
                    {
                        return null;
                    }
                    else
                    {
                        //Check that values exist, if they do set the variable to the value or set it to "" if not.
                        string deviceName = Convert.ToBoolean(result.Properties["name"].Count > 0) ? result.Properties["name"][0].ToString() : "";
                        string deviceDNSName = Convert.ToBoolean(result.Properties["dNSHostName"].Count > 0) ? result.Properties["dNSHostName"][0].ToString() : "";
                        string deviceDescription = Convert.ToBoolean(result.Properties["description"].Count > 0) ? result.Properties["description"][0].ToString() : "";
                        string deviceOS = Convert.ToBoolean(result.Properties["operatingSystem"].Count > 0) ? result.Properties["operatingSystem"][0].ToString() : "";
                        string deviceLastLogon = Convert.ToBoolean(result.Properties["lastLogonTimestamp"].Count > 0) ? result.Properties["lastLogonTimestamp"][0].ToString() : "";

                        //Conver ADSI timestamp (deviceLastLogon) to human readable
                        string deviceLastLogonTime = DateTime.FromFileTime(Int64.Parse(deviceLastLogon)).ToString();

                        string deviceDistinguishedName = Convert.ToBoolean(result.Properties["distinguishedName"].Count > 0) ? result.Properties["distinguishedName"][0].ToString() : "";
                        // Add to the device matches list
                        deviceMatches.Add(new Device(deviceName,
                                                     deviceDNSName,
                                                     deviceDescription,
                                                     deviceOS,
                                                     deviceLastLogonTime,
                                                     deviceDistinguishedName));

                    }
                }

                deviceMatches = new BindingList<Device>(deviceMatches.Distinct().ToList());
                return deviceMatches;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR OCCURED SEARCHING DIRECTORY");
            return null;
        }
    }

    public void SetContext(PrincipalContext context)
    {
        _currentContext = context;
        _objectOfInterest = null;
        _objectName = null;
    }
}