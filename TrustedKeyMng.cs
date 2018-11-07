namespace SSH
{
    using System.Collections.Generic;
    using Microsoft.Win32;
    using System;
    using System.IO;
    using Newtonsoft.Json;

    // Class for managing the keys 
    public class TrustedKeyMng
    {
        public Dictionary<string, string> Keys = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public Dictionary<string, string> GetKeys()
        {
            if(Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var hostkeys = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

                var personalFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var poshSshFolderPath = Path.Combine(personalFolderPath, ".poshssh");
                var poshSshKeysPath = Path.Combine(poshSshFolderPath, "known_hosts");

                if(File.Exists(poshSshKeysPath))
                {
                    var keysText = File.ReadAllText(poshSshKeysPath);
                    hostkeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(keysText);
                }

                return hostkeys;
            }
            else if(Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var hostkeys = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                var poshSoftKey = Registry.CurrentUser.OpenSubKey(@"Software\PoshSSH", true);
                if (poshSoftKey != null)
                {
                    var hosts = poshSoftKey.GetValueNames();
                    foreach (var host in hosts)
                    {
                        var hostkey = poshSoftKey.GetValue(host).ToString();
                        hostkeys.Add(host, hostkey);
                    }
                }
                else
                {
                    using (var softKey = Registry.CurrentUser.OpenSubKey(@"Software", true))
                    {
                        if (softKey != null) softKey.CreateSubKey("PoshSSH");
                    }
                }
                return hostkeys;
            }
            else
            {
                throw new Exception("Platform not supported");
            }
        }

        public bool SetKey(string host, string fingerprint)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var hostkeys = GetKeys();
                hostkeys[host] = fingerprint;

                var personalFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var poshSshFolderPath = Path.Combine(personalFolderPath, ".poshssh");
                if (!Directory.Exists(poshSshFolderPath))
                    Directory.CreateDirectory(poshSshFolderPath);

                var poshSshKeysPath = Path.Combine(poshSshFolderPath, "known_hosts");

                var keysText = JsonConvert.SerializeObject(hostkeys);
                File.WriteAllText(poshSshKeysPath, keysText);

                return true;
            }
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var poshSoftKey = Registry.CurrentUser.OpenSubKey(@"Software\PoshSSH", true);
                if (poshSoftKey != null)
                {
                    poshSoftKey.SetValue(host, fingerprint);
                    return true;
                }
                var softKey = Registry.CurrentUser.OpenSubKey(@"Software", true);
                if (softKey == null) return true;
                softKey.CreateSubKey("PoshSSH");
                softKey.SetValue(host, fingerprint);
                return true;
            }
            else
            {
                throw new Exception("Platform not supported");
            }
        }
    }
}
