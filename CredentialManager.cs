using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADCC
{
    internal class CredentialManager
    {
        static void CreateNewPassword(string PasswordName, string Password)
        {
            using var cred = new Credential();
            cred.Password = Password;
            cred.Target = PasswordName;
            cred.Type = CredentialType.Generic;
            cred.PersistanceType = PersistanceType.LocalComputer;
            cred.Save();
        }

        //Provided a domain  such as company.com or compan2.com pull the credentials for that.
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
                return null;
            }
        }

    }
}
