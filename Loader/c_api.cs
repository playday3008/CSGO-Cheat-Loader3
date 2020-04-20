using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Text;
using System.Net;
using System.IO;

namespace c_auth
{
    public class c_api
    {
        private static string program_key { get; set; }
        private static string enc_key { get; set; }
        private static string iv_key { get; set; }

        private static string iv_input { get; set; }
        public static void c_init(string c_version, string c_program_key, string c_encryption_key) {
            try {
                if (c_encryption.ssl_cert(api_link, user_agent) != "oek2twC+UlJnvPdW/1eZPmTnKZUvDd4VsYcyGVOo5E0=")
                    Environment.Exit(0);

                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    program_key = c_program_key;
                    iv_key = c_encryption.iv_key();
                    enc_key = c_encryption_key;

                    var values = new NameValueCollection {
                        ["version"] = c_encryption.encrypt(c_version, enc_key),
                        ["session_iv"] = c_encryption.encrypt(iv_key, enc_key),
                        ["api_version"] = c_encryption.encrypt("2.8b", enc_key),
                        ["program_key"] = c_encryption.base64_encode(program_key)
                    };

                    string result = Encoding.Default.GetString(web.UploadValues(api_link + "handler.php?type=init", values));

                    switch (result) {
                        case "program_doesnt_exist":
                            MessageBox.Show("The program doesnt exist", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                            break;

                        case string xd when xd.Equals(c_encryption.encrypt("wrong_version", enc_key)):
                            MessageBox.Show("Wrong program version", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                            break;

                        case string xd when xd.Equals(c_encryption.encrypt("old_api_version", enc_key)):
                            MessageBox.Show("Please download the newest API files on the auth's website", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                            break;

                        default:
                            string[] s = c_encryption.decrypt(result, enc_key).Split('|');
                            iv_input = s[1];
                            break;
                    }
                }
            }
            catch (CryptographicException) {
                MessageBox.Show("Invalid encryption key", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
            }
        }
        public static bool c_login(string c_username, string c_password, string c_hwid = "default") {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try {
                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    var values = new NameValueCollection {
                        ["username"] = c_encryption.encrypt(c_username, enc_key, iv_key),
                        ["password"] = c_encryption.encrypt(c_password, enc_key, iv_key),
                        ["hwid"] = c_encryption.encrypt(c_hwid, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.base64_encode(program_key)
                    };

                    string result = c_encryption.decrypt(Encoding.Default.GetString(web.UploadValues(api_link + "handler.php?type=login", values)), enc_key, iv_key);

                    switch (result) {
                        case "invalid_username":
                            MessageBox.Show("Invalid username", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_password":
                            MessageBox.Show("Invalid password", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "user_is_banned":
                            MessageBox.Show("The user is banned", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "no_sub":
                            MessageBox.Show("Your subscription is over", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_hwid":
                            MessageBox.Show("Invalid HWID", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case string _sw when _sw.Contains("logged_in"):
                            string[] s = result.Split('|');

                            c_userdata.username = s[1];
                            c_userdata.email = s[2];

                            c_userdata.expires = c_encryption.unix_to_date(Convert.ToDouble(s[3]));

                            c_userdata.rank = Convert.ToInt32(s[4]);

                            stored_pass = c_encryption.encrypt(c_password, enc_key, iv_key);

                            MessageBox.Show("Logged in!!", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;

                        default:
                            MessageBox.Show("invalid encryption key/iv or session expired", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
        }
        public static bool c_register(string c_username, string c_email, string c_password, string c_token, string c_hwid = "default") {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try {
                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    var values = new NameValueCollection {
                        ["username"] = c_encryption.encrypt(c_username, enc_key, iv_key),
                        ["email"] = c_encryption.encrypt(c_email, enc_key, iv_key),
                        ["password"] = c_encryption.encrypt(c_password, enc_key, iv_key),
                        ["token"] = c_encryption.encrypt(c_token, enc_key, iv_key),
                        ["hwid"] = c_encryption.encrypt(c_hwid, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.base64_encode(program_key)
                    };

                    string result = c_encryption.decrypt(Encoding.Default.GetString(web.UploadValues(api_link + "handler.php?type=register", values)), enc_key, iv_key);

                    switch (result) {
                        case "user_already_exists":
                            MessageBox.Show("User already exists", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "email_already_exists":
                            MessageBox.Show("Email already exists", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_email_format":
                            MessageBox.Show("Invalid email format", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_token":
                            MessageBox.Show("Invalid token", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "maximum_users_reached":
                            MessageBox.Show("Maximum users of the program was reached, please contact the program owner", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "used_token":
                            MessageBox.Show("Already used token", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "success":
                            MessageBox.Show("Success!!", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;

                        default:
                            MessageBox.Show("invalid encryption key/iv or session expired", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
        }
        public static bool c_activate(string c_username, string c_password, string c_token) {
            try {
                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    var values = new NameValueCollection {
                        ["username"] = c_encryption.encrypt(c_username, enc_key, iv_key),
                        ["password"] = c_encryption.encrypt(c_password, enc_key, iv_key),
                        ["token"] = c_encryption.encrypt(c_token, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.base64_encode(program_key)
                    };

                    string result = c_encryption.decrypt(Encoding.Default.GetString(web.UploadValues(api_link + "handler.php?type=activate", values)), enc_key, iv_key);

                    switch (result) {
                        case "invalid_username":
                            MessageBox.Show("Invalid username", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_password":
                            MessageBox.Show("Invalid password", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "user_is_banned":
                            MessageBox.Show("The user is banned", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_token":
                            MessageBox.Show("Invalid token", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "used_token":
                            MessageBox.Show("Already used token", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "success":
                            MessageBox.Show("Success!!", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;

                        default:
                            MessageBox.Show("invalid encryption key/iv or session expired", "FireFrame Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return false;
            }
        }
        public static bool c_all_in_one(string c_token, string c_hwid = "default") {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            if (c_login(c_token, c_token, c_hwid))
                return true;

            else if (c_register(c_token, c_token + "@email.com", c_token, c_token, c_hwid)) {
                MessageBox.Show("success, restarting...");
                Environment.Exit(0);
                return true;
            }

            return false;
        }
        private static string stored_pass { get; set; }
        public static string c_var(string c_var_name, string c_hwid = "default") {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try {
                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    var values = new NameValueCollection {
                        ["var_name"] = c_encryption.encrypt(c_var_name, enc_key, iv_key),
                        ["username"] = c_encryption.encrypt(c_userdata.username, enc_key, iv_key),
                        ["password"] = stored_pass,
                        ["hwid"] = c_encryption.encrypt(c_hwid, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.base64_encode(program_key)
                    };

                    string result = c_encryption.decrypt(Encoding.Default.GetString(web.UploadValues(api_link + "handler.php?type=var", values)), enc_key, iv_key);

                    return result;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
                return "";
            }
        }
        public static void c_log(string c_message) {
            if (c_userdata.username == null) c_userdata.username = "NONE";

            try {
                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    var values = new NameValueCollection {
                        ["username"] = c_encryption.encrypt(c_userdata.username, enc_key, iv_key),
                        ["message"] = c_encryption.encrypt(c_message, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.base64_encode(program_key)
                    };

                    string result = Encoding.Default.GetString(web.UploadValues(api_link + "handler.php?type=log", values));
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                Environment.Exit(0);
            }
        }

        private static string api_link = "https://firefra.me/auth/api/";

        private static string user_agent = "Mozilla FireFrame";
    }
    public class c_userdata
    {
        public static string username { get; set; }
        public static string email { get; set; }
        public static DateTime expires { get; set; }
        public static int rank { get; set; }
    }
    public class c_encryption
    {
        public static string base64_encode(string _) =>
            System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(_));

        public static string EncryptString(string plainText, byte[] key, byte[] iv) {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            MemoryStream memoryStream = new MemoryStream();

            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            string cipherText = string.Empty;
            try {
                byte[] plainBytes = Encoding.Default.GetBytes(plainText);

                cryptoStream.Write(plainBytes, 0, plainBytes.Length);

                cryptoStream.FlushFinalBlock();

                byte[] cipherBytes = memoryStream.ToArray();

                cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
            }
            finally {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return cipherText;
        }

        public static string DecryptString(string cipherText, byte[] key, byte[] iv) {
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
            try {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                cryptoStream.FlushFinalBlock();

                byte[] plainBytes = memoryStream.ToArray();

                plainText = Encoding.Default.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return plainText;
        }

        public static string iv_key() =>
            Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal));

        public static string encrypt(string message, string enc_key, string iv = "default_iv") {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.Default.GetBytes(enc_key));

            if (iv == "default_iv")
                return EncryptString(message, key, new byte[16] { 0x1, 0x5, 0x1, 0x4, 0x8, 0x3, 0x4, 0x6, 0x2, 0x6, 0x5, 0x7, 0x8, 0x3, 0x9, 0x4 });
            else
                return EncryptString(message, key, Encoding.Default.GetBytes(Convert.ToBase64String(mySHA256.ComputeHash(Encoding.Default.GetBytes(iv))).Substring(0, 16)));
        }

        public static string decrypt(string message, string enc_key, string iv = "default_iv") {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.Default.GetBytes(enc_key));

            if (iv == "default_iv")
                return DecryptString(message, key, new byte[16] { 0x1, 0x5, 0x1, 0x4, 0x8, 0x3, 0x4, 0x6, 0x2, 0x6, 0x5, 0x7, 0x8, 0x3, 0x9, 0x4 });
            else
                return DecryptString(message, key, Encoding.Default.GetBytes(Convert.ToBase64String(mySHA256.ComputeHash(Encoding.Default.GetBytes(iv))).Substring(0, 16)));
        }

        public static DateTime unix_to_date(double unixTimeStamp) =>
            new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();

        public static string ssl_cert(string url, string user_agent) {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.UserAgent = user_agent;
            ((HttpWebResponse)httpWebRequest.GetResponse()).Close();
            X509Certificate2 cert2 = new X509Certificate2(httpWebRequest.ServicePoint.Certificate);
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.Default.GetBytes(cert2.GetPublicKeyString())));
        }
    }
}
