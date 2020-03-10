using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_auth
{
    class c_api
    {
        public static string program_key { get; set; }

        public static string enc_key { get; set; }

        private static string api_link = "http://firefra.me/auth/api/"; //maybe you'll make your own auth based on mine

        private static string user_agent = "Mozilla FireFrame"; //my ddos protection needs Mozilla in front

        public static void c_init(string c_version)
        {
            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = web.DownloadString(api_link + "init.php?version=" + c_version + "&program_key=" + program_key);

                if (c_encryption.ssl_cert(api_link, user_agent) != "oek2twC+UlJnvPdW/1eZPmTnKZUvDd4VsYcyGVOo5E0=")
                    Environment.Exit(0);

                if (result == "program_doesnt_exist")
                {
                    MessageBox.Show("the program doesnt exist");
                    Environment.Exit(0);
                }
                else if (result == c_encryption.encrypt("wrong_version"))
                {
                    MessageBox.Show("wrong program version");
                    Environment.Exit(0);
                }
                else if (result == c_encryption.encrypt("started_program"))
                {
                    //guess 
                }
                else
                {
                    MessageBox.Show("invalid encryption key");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
            }
        }
        public static bool c_login(string c_username, string c_password, string c_hwid = "default")
        {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = c_encryption.decrypt(web.DownloadString(api_link + "login.php?program_key=" + program_key + "&username=" + c_username + "&password=" + c_password + "&hwid=" + c_hwid));

                if (result == "invalid_username")
                {
                    MessageBox.Show("invalid username");
                    return false;
                }
                else if (result == "invalid_password")
                {
                    MessageBox.Show("invalid password");
                    return false;
                }
                else if (result == "no_sub")
                {
                    MessageBox.Show("no sub");
                    return false;
                }
                else if (result == "invalid_hwid")
                {
                    MessageBox.Show("invalid hwid");
                    return false;
                }
                else if (result.Contains("logged_in"))
                {
                    string[] s = result.Split('|');

                    c_userdata.username = s[1];
                    c_userdata.email = s[2];
                    c_userdata.expires = c_encryption.unix_to_date(Convert.ToDouble(s[3]));
                    c_userdata.rank = Convert.ToInt32(s[4]);

                    shit_pass = c_encryption.encrypt(c_password);

                    MessageBox.Show("logged in!");
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
            return true;
        }
        public static bool c_register(string c_username, string c_email, string c_password, string c_token, string c_hwid = "default")
        {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = c_encryption.decrypt(web.DownloadString(api_link + "register.php?program_key=" + program_key + "&username=" + c_username + "&email=" + c_email + "&password=" + c_password + "&token=" + c_token + "&hwid=" + c_hwid));

                if (result == "user_already_exists")
                {
                    MessageBox.Show("user already exists");
                    return false;
                }
                else if (result == "email_already_exists")
                {
                    MessageBox.Show("email already exists");
                    return false;
                }
                else if (result == "invalid_email_format")
                {
                    MessageBox.Show("invalid email format");
                    return false;
                }
                else if (result == "invalid_token")
                {
                    MessageBox.Show("invalid token");
                    return false;
                }
                else if (result == "maximum_users_reached")
                {
                    MessageBox.Show("maximum users reached");
                    return false;
                }
                else if (result == "used_token")
                {
                    MessageBox.Show("used token");
                    return false;
                }
                else if (result == "success")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("invalid encryption key");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
        }
        public static bool c_activate(string c_username, string c_password, string c_token)
        {
            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = c_encryption.decrypt(web.DownloadString(api_link + "activate.php?program_key=" + program_key + "&username=" + c_username + "&password=" + c_password + "&token=" + c_token));

                if (result == "invalid_username")
                {
                    MessageBox.Show("invalid username");
                    return false;
                }
                else if (result == "invalid_password")
                {
                    MessageBox.Show("invalid password");
                    return false;
                }
                else if (result == "invalid_token")
                {
                    MessageBox.Show("invalid token");
                    return false;
                }
                else if (result == "used_token")
                {
                    MessageBox.Show("used token");
                    return false;
                }
                else if (result == "success")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("invalid encryption key");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
        }
        public static bool c_all_in_one(string c_token, string c_hwid = "default")
        {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            if (c_login(c_token, c_token, c_hwid))
                return true;

            else if (c_register(c_token, c_token + "@email.com", c_token, c_token, c_hwid))
            {
                MessageBox.Show("success, restarting...");
                Environment.Exit(0);
                return true;
            }

            return false;
        }
        private static string shit_pass { get; set; }
        public static string c_var(string c_var_name, string c_hwid = "default")
        {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try
            {
                WebClient web = new WebClient();
                web.Headers["User-Agent"] = user_agent;
                web.Proxy = null;

                string result = c_encryption.decrypt(web.DownloadString(api_link + "var.php?program_key=" + program_key + "&var_name=" + c_var_name + "&username=" + c_userdata.username + "&password=" + c_encryption.decrypt(shit_pass) + "&hwid=" + c_hwid));

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return "";
            }
        }
    }
    class c_userdata
    {
        public static string username { get; set; }
        public static string email { get; set; }
        public static DateTime expires { get; set; }
        public static int rank { get; set; }
    }
    class c_encryption
    {
        public static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            MemoryStream memoryStream = new MemoryStream();

            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            return cipherText;
        }

        public static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            MemoryStream memoryStream = new MemoryStream();

            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            string plainText = String.Empty;
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                cryptoStream.FlushFinalBlock();

                byte[] plainBytes = memoryStream.ToArray();

                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return plainText;
        }

        public static string encrypt(string message)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(c_api.enc_key));

            byte[] iv = new byte[16] { 0x1, 0x5, 0x1, 0x4, 0x8, 0x3, 0x4, 0x6, 0x2, 0x6, 0x5, 0x7, 0x8, 0x3, 0x9, 0x4 };

            return EncryptString(message, key, iv);
        }

        public static string decrypt(string message)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(c_api.enc_key));

            byte[] iv = new byte[16] { 0x1, 0x5, 0x1, 0x4, 0x8, 0x3, 0x4, 0x6, 0x2, 0x6, 0x5, 0x7, 0x8, 0x3, 0x9, 0x4 };

            return DecryptString(message, key, iv);
        }

        public static DateTime unix_to_date(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static string ssl_cert(string url, string user_agent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = user_agent;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Close();

            X509Certificate cert = request.ServicePoint.Certificate;
            X509Certificate2 cert2 = new X509Certificate2(cert);

            SHA256 x = SHA256Managed.Create();
            byte[] retUrn = x.ComputeHash(Encoding.ASCII.GetBytes(cert2.GetPublicKeyString()));

            return Convert.ToBase64String(retUrn);
        }
    }
}
