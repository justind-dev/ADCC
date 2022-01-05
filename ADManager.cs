using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ADCC
{
    public class ADM
    {
        //This will be the class used to interact with Active Directory Services.
        //If you have multiple domains you will need to configure the domain controllers
        //in the seperate 
        public string adname;
        public ADM(string domainName)
        {
            adname = domainName;
        }
        //Unlock User Account
        public string Unlock(string userSAM)
        {
            var userDn = GetDistinguishedName(userSAM);

            if (userDn == null)
            {
                return "Could not find user.";
            }
            else
            {
                try
                {
                    DirectoryEntry uEntry = new DirectoryEntry(userDn);
                    uEntry.Properties["LockOutTime"].Value = 0; //unlock account

                    uEntry.CommitChanges(); //may not be needed but adding it anyways

                    uEntry.Close();

                    return uEntry.Name;
                }

                catch (DirectoryServicesCOMException E)
                {
                    //DoSomethingWith --> E.Message.ToString();
                    return E.Message;
                }
            }


        }

        //Returns user distinguised name given SAMAccount name
        public string GetDistinguishedName(string userSAM)
        {
            using (var pc = new PrincipalContext(ContextType.Domain, adname))
            {
                var user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, adname + "\\" + userSAM);
                if (user != null)
                {
                    return user.DistinguishedName;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
