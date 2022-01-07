using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ADCC
{
    public class ActiveDirectoryManager
    {
        private PrincipalContext _currentContext;
        private UserPrincipal _userOfInterest;
        public ActiveDirectoryManager(PrincipalContext context)
        {
            _currentContext = context;
            _userOfInterest = null;
        }
        public void SetUserOfInterestByIdentity(string searchCriteria)
        {
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
            if (_userOfInterest == null) return false;
            _userOfInterest.SetPassword(password);
            _userOfInterest.Dispose();
            return true;
        }

        public string GetDistinguishedName()
        {
            if (_userOfInterest == null) return null;
            string distinguishedName = _userOfInterest.DistinguishedName;
            _userOfInterest.Dispose();
            return distinguishedName;

        }
        public void SetContext(PrincipalContext context)
        {
            _currentContext = context;
            _userOfInterest = null;
        }
    }
}
