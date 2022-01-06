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
        private readonly CredentialManager credManager = new();
        private string _contextName;
        private PrincipalContext _currentContext;
        private readonly string connectedDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;

        public ActiveDirectoryManager(string domainContext)
        {
            if (domainContext == connectedDomain)
            {
                this._currentContext = new PrincipalContext(ContextType.Domain, domainContext);
                this._contextName = _currentContext.Name;
            }
            else
            {
                var cred = credManager.GetCredentialsByDomainController(domainContext);
                using var _currentContext = new PrincipalContext(ContextType.Domain, domainContext, cred.Item1, cred.Item2);
            }
        }

        //Update Principal Context from which this object works.
        public void SetContext(string domainContext)
        {
            if (domainContext == connectedDomain)
            {
                this._currentContext = new PrincipalContext(ContextType.Domain, domainContext);
            }
            else
            {
                var cred = credManager.GetCredentialsByDomainController(domainContext);
                this._currentContext = new PrincipalContext(ContextType.Domain, domainContext, cred.Item1, cred.Item2);
            }

        }

        //Finds and returns a user principal object given a sAMAccount name.
        public UserPrincipal SetUserOfInterestBysAMAccount(string searchCriteria)
        {
            var userOfInterest = UserPrincipal.FindByIdentity(_currentContext, IdentityType.SamAccountName, _currentContext.Name.Split().First() + "\\" + searchCriteria);
            if (userOfInterest == null)
            {
                return null;
            }
            return userOfInterest;
        }

        //Unlock User Account
        public bool UnlockUser(string userName)
        {
            var userOfInterest = SetUserOfInterestBysAMAccount(userName);

            if (userOfInterest == null)
            {
                return false;
            }
            else
            {
                if (userOfInterest.IsAccountLockedOut())
                {
                    userOfInterest.UnlockAccount();
                    return true;
                }
                else { return false; }
            }
        }

        //Set User Account Password
        public bool SetPassword(string userName, string password)
        {
            var userOfInterest = SetUserOfInterestBysAMAccount(userName);

            if (userOfInterest == null)  return false;
            userOfInterest.SetPassword(password);
            userOfInterest.Dispose();
            return true;
        }

        // Return the distinguished name of a user.
        public string GetDistinguishedName(string userSAM)
        {
            var userOfInterest = SetUserOfInterestBysAMAccount(userSAM);

            if (userOfInterest == null) return null;
            string distinguishedName = userOfInterest.DistinguishedName;
            userOfInterest.Dispose();
            return distinguishedName;

        }


    }
}
