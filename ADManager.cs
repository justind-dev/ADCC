using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;
using System.Net.NetworkInformation;

namespace ADCC
{
    public class ADManager
    {
        // This will be the class used to interact with Active Directory Services.
        // ADManager utilizes the CredManager class to interact with the CredentialManagement API
        // for retrieving passwords to domain controllers that you have added in.
        // domains are configurable in the application by using the application settings file:
        // <settings file name here>
        // example domains have been added. Please replace with your own. You will also 
        // need to add credentials for these domains in Windows Credential Manager in the
        // format credential name: domain.com username: username@domain.com

        //Used for interacting with Windows Credential Manager API
        private readonly CredManager credManager = new();

        //This gets the default domain that the computer is currently connected to
        private string connectedDomain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        public ADManager()
        {
            
        }
        //Unlock User Account
        public string Unlock(string userName, string domainName)
        {
            //get the credential for the passed domain name

            var cred = credManager.GetCredentialsByDomainController(domainName);

            using var pc = new PrincipalContext(ContextType.Domain, domainName, cred.Item1, cred.Item2);
            var user = UserPrincipal.FindByIdentity(pc, domainName + "\\" + userName);

            if (user == null) { user.Dispose(); return "User account not found"; }
            else
            {
                if (user.IsAccountLockedOut())
                {
                    user.UnlockAccount();
                    user.Dispose();
                    return userName + " is now unlocked.";
                }
                else
                {
                    user.Dispose();
                    return userName + " is already unlocked";
                }
            }
        }

        // Returns user distinguised name given SAMAccount name - Not really used currently but might be later...
        public string GetDistinguishedName(string userSAM, string domainName )
        {

            if (domainName != "")
            {
                var cred = credManager.GetCredentialsByDomainController(domainName);
                using var pc = new PrincipalContext(ContextType.Domain, domainName, cred.Item1, cred.Item2);
                var user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, domainName.Split().First() + "\\" + userSAM);
                if (user != null)
                {
                    return user.DistinguishedName.ToString();
                }
                else
                {
                    return "User not found";
                }
            }
            else // use the default connected domain.
            {
                using var pc = new PrincipalContext(ContextType.Domain, connectedDomain);
                var user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, userSAM);
                if (user != null)
                {
                    return user.DistinguishedName.ToString();
                }
                else
                {
                    return "User not found";
                }
            }

        }
    }
}
