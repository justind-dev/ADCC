using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace ADCC;

public class ActiveDirectoryManager
{
    private PrincipalContext? _currentContext;
    private UserPrincipal? _objectOfInterest;
    private string? _objectName;

    public ActiveDirectoryManager(PrincipalContext context)
    {
        _currentContext = context;
        _objectOfInterest = null;
    }
    //set current user of interest by idenitity
    public void SetUserOfInterestByIdentity(string identityName)
    {
        _objectName = identityName;
        _objectOfInterest?.Dispose();
        _objectOfInterest = UserPrincipal.FindByIdentity(_currentContext, identityName);
    }
    // Set search term for query methods
    public void SetObjectOfInterestSearchTerm(string searchCriteria)
    {
        _objectName = searchCriteria;
    }
    public bool UnlockUser()
    {
        if (_objectOfInterest == null || !_objectOfInterest.IsAccountLockedOut()) return false;
        _objectOfInterest.UnlockAccount();
        return true;
    }
    public bool SetPassword(string password)
    {
        try
        {
            _objectOfInterest?.SetPassword(password);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error, could not reset password");
            return false;
        }
    }

    public string? GetDistinguishedName()
    {
        return _objectOfInterest?.DistinguishedName;
    }

    // Search active directory, returning a list of matching users.
    public BindingList<User> QueryDirectory()
    {
        var userMatches = new BindingList<User>();
        var searchString = String.Format("*{0}*", _objectName);
        var searchMaskDisplayname = new UserPrincipal(_currentContext) { DisplayName = searchString };
        var searchMaskUsername = new UserPrincipal(_currentContext) { SamAccountName = searchString };
        var searcherDisplayname = new PrincipalSearcher(searchMaskDisplayname);
        var searcherUsername = new PrincipalSearcher(searchMaskUsername);
        PrincipalSearchResult<Principal> taskDisplayName = searcherDisplayname.FindAll();
        PrincipalSearchResult<Principal> taskUsername = searcherUsername.FindAll();
        var allMatches = taskDisplayName.Union(taskUsername);

        foreach (UserPrincipal userPrincipal in allMatches)
            using (userPrincipal)
            {
                userMatches.Add(new User(userPrincipal.SamAccountName, userPrincipal.GivenName, userPrincipal.Surname, userPrincipal.IsAccountLockedOut() ? "True" : "False", userPrincipal.DistinguishedName));
            }
        userMatches = new BindingList<User>(userMatches.Distinct().ToList());
        return userMatches;
    }


    public void SetContext(PrincipalContext context)
    {
        // _currentContext?.Dispose(); There was issues with accessing things after being disposed. Needs testing.

        _currentContext = context;

        // _userOfInterest?.Dispose(); There was issues with accessing things after being disposed. Needs testing.

        _objectOfInterest = null;
    }
}