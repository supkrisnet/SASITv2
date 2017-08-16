using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utiles.Crypt
{
    public class Offusque
    {
        private static string DiskId
        {
            get
            {
                ManagementObject _obj = new ManagementObject(@"win32_logicaldisk.deviceid=""C:""");
                _obj.Get();
                return _obj["VolumeSerialNumber"].ToString();
            }
        }

        private static string MACAddress
        {
            get
            {
                List<string> li = new List<string>();
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
                    {
                        li.Add(string.Format("{0}", nic.GetPhysicalAddress()));
                    }
                }

                return string.Join("$", li.ToArray());
            }
        }

        private static string CpuId
        {
            get
            {
                StringBuilder _obj = new StringBuilder();

                try
                {
                    ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select ProcessorID From Win32_processor");
                    ManagementObjectCollection mbsList = mbs.Get();

                    foreach (ManagementObject mo in mbsList)
                    {
                        _obj.Append(mo["ProcessorID"].ToString());
                    }

                    return _obj.ToString();
                }
                catch (Exception) { return _obj.ToString(); }
            }
        }


        private static string SerialNumber
        {
            get
            {
                List<string> _obj = new List<string>();

                try
                {
                    ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_OperatingSystem");
                    ManagementObjectCollection mbsList = mbs.Get();

                    foreach (ManagementObject mo in mbsList)
                    {
                        try
                        {
                            _obj.Add(mo["SerialNumber"].ToString());
                        }
                        catch
                        { }
                        try
                        {
                            _obj.Add(mo["OSLanguage"].ToString());
                        }
                        catch { }

                        try
                        {
                            _obj.Add(mo["InstallDate"].ToString());
                        }
                        catch { }
                    }

                    return string.Join("#", _obj.ToArray());
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }
            }
        }

        private static string EncryptionKey = string.Format("{4}{0}{1}{2}{3}{5}{6}", Environment.UserDomainName, Environment.UserName, Environment.MachineName, DiskId, CpuId, MACAddress, SerialNumber);
        private static byte[] SaltKey = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        public static string Encrypt(string t, bool trace = false)
        {
            if (trace)
            {
                Trace.WriteLine(EncryptionKey);
            }

            byte[] _tmp = Encoding.Unicode.GetBytes(t);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, SaltKey);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(_tmp, 0, _tmp.Length);
                        cs.Close();
                    }
                    t = Convert.ToBase64String(ms.ToArray());
                }
            }

            return t;
        }

        public static string Decrypt(string t, bool trace = false)
        {
            if (trace)
            {
                Trace.WriteLine(EncryptionKey);
            }

            t = t.Replace(" ", "+");
            byte[] _tmp = Convert.FromBase64String(t);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, SaltKey);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(_tmp, 0, _tmp.Length);
                        cs.Close();
                    }
                    t = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return t;
        }
    }
}
