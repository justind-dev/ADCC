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

    public void SetContext(PrincipalContext context)
    {
        _currentContext?.Dispose();

        _currentContext = context;

        _userOfInterest?.Dispose();

        _userOfInterest = null;
    }
}