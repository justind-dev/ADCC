using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADCC
{
    public class User
    {
        public string SamAccountName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string LockedOut { get; set; }
        public string DistinguishedName { get; set; }

        public User(string samAccountName, string givenName, string surname, string lockedOut, string distinguishedName)
        {
            SamAccountName = samAccountName;
            GivenName = givenName;
            Surname = surname;
            LockedOut = lockedOut;
            DistinguishedName = distinguishedName;
        }
    }
}
