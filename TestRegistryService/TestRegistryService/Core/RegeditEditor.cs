using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace TestRegistryService.Core
{
    public static class RegeditEditor
    {
        public static bool IsAdmin()
        {
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            return isAdmin;
        }

        public static string AddRegistryKey(string companyName, string productName, string registryKeyName, string registryKeyValue)
        {
            if (string.IsNullOrEmpty(companyName)) throw new ArgumentNullException(nameof(companyName));
            if (string.IsNullOrEmpty(productName)) throw new ArgumentNullException(nameof(productName));
            if (string.IsNullOrEmpty(registryKeyName)) throw new ArgumentNullException(nameof(registryKeyName));
            if (string.IsNullOrEmpty(registryKeyValue)) throw new ArgumentNullException(nameof(registryKeyValue));

            RegistryKey key = Registry.LocalMachine;
            key = key.CreateSubKey("SOFTWARE");
            key = key.CreateSubKey(companyName);
            key = key.CreateSubKey(productName);
            key.SetValue(registryKeyName, registryKeyValue);
            return key.ToString();
        }

        public static bool ChangePermissionToUser(string userName, string registryKey, RegistryRights registryRights = RegistryRights.ReadKey)
        {
            string user = userName;
            RegistrySecurity rs = new RegistrySecurity();

            rs.AddAccessRule(new RegistryAccessRule(user,
                registryRights,
                InheritanceFlags.None,
                PropagationFlags.None,
                AccessControlType.Allow));
            string rootKey = registryKey.Replace($"{Registry.LocalMachine.ToString()}\\", string.Empty);
            RegistryKey key = Registry.LocalMachine.OpenSubKey(rootKey, true);

            try
            {
                key.SetAccessControl(rs);
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            return true;
        }
    }
}
