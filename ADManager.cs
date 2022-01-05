using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ADCC
{
    public class ADManager
    {
        //This will be the class used to interact with Active Directory Services.

        public string domainName;
        public ADManager(string DomainName)
        {
            domainName = DomainName;
        }
        //Unlock User Account
        public string Unlock(string userName)
        {

            using var pc = new PrincipalContext(ContextType.Domain, domainName);
            var user = UserPrincipal.FindByIdentity(pc, domainName + "\\" + userName);

            if (user == null) return "User account not found";
            else
            {
                if (user.IsAccountLockedOut())
                {
                    user.UnlockAccount();
                    return userName + " is now unlocked.";
                }
                else
                {
                    return userName + " is already unlocked";
                }
            }
        }

        //Returns user distinguised name given SAMAccount name
        public string GetDistinguishedName(string userSAM)
        {
            using var pc = new PrincipalContext(ContextType.Domain, domainName);
            var user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, domainName + "\\" + userSAM);
            if (user != null)
            {
                return user.DistinguishedName.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
