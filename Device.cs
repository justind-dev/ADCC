using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//devicePrincipal.Name, devicePrincipal.Description, devicePrincipal.LastLogon.ToString(), devicePrincipal.Enabled.ToString(), devicePrincipal.DistinguishedName
namespace ADCC
{
    public class Device
    {
        public string Name { get; set; }
        public string DNSName { get; set; }
        public string Description { get; set; }
        public string OperatingSystem { get; set; }
        public string LastLogonTimestamp { get; set; }
        public string DistinguishedName { get; set; }

        public Device(string deviceName, 
                      string deviceDNSName, 
                      string deviceDescription, 
                      string deviceOperatingSystem, 
                      string deviceLastLogonTimestamp, 
                      string distinguishedName)
        {
            Name = deviceName;
            DNSName = deviceDNSName;
            Description = deviceDescription;
            OperatingSystem = deviceOperatingSystem;
            LastLogonTimestamp = deviceLastLogonTimestamp;
            DistinguishedName = distinguishedName;
        }
    }
}
