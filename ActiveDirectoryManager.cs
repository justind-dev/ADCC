using System.ComponentModel;
using System.DirectoryServices.AccountManagement;

namespace ADCC;

public class ActiveDirectoryManager
{
    private PrincipalContext? _currentContext;
    private UserPrincipal? _userOfInterest;

    public ActiveDirectoryManager(PrincipalContext context)
    {
        _currentContext = context;
        _userOfInterest = null;
    }

    public void SetUserOfInterestByIdentity(string searchCriteria)
    {
        _userOfInterest?.Dispose();
        _userOfInterest = UserPrincipal.FindByIdentity(_currentContext, searchCriteria);
    }

    public bool UnlockUser()
    {
        if (_userOfInterest == null || !_userOfInterest.IsAccountLockedOut()) return false;
        _userOfInterest.UnlockAccount();
        return true;
    }

    public bool SetPassword(string password)
    {
        _userOfInterest?.SetPassword(password);
        return true;
    }

    public string? GetDistinguishedName()
    {
        return _userOfInterest?.DistinguishedName;
    }

    public BindingList<User> GetUserData()
    {
        if (_userOfInterest == null) { return null; }
        else
        {
            var userList = new List<User>
            {
                new User(_userOfInterest.SamAccountName,
                                _userOfInterest.Name,
                                _userOfInterest.Surname,
                                _userOfInterest.IsAccountLockedOut() ? "True" : "False",
                                _userOfInterest.DistinguishedName)
            };

            var userData = new BindingList<User>(userList);
            return userData;
        }
    }
    public void SetContext(PrincipalContext context)
    {
        // _currentContext?.Dispose(); There was issues with accessing things after being disposed. Needs testing.

        _currentContext = context;

        // _userOfInterest?.Dispose(); There was issues with accessing things after being disposed. Needs testing.

        _userOfInterest = null;
    }
}