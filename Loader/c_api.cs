using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;

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
                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    ServicePointManager.ServerCertificateValidationCallback = c_encryption.pin_public_key;

                    program_key = c_program_key;
                    iv_key = c_encryption.iv_key();
                    enc_key = c_encryption_key;

                    var values = new NameValueCollection {
                        ["version"] = c_encryption.encrypt(c_version, enc_key),
                        ["session_iv"] = c_encryption.encrypt(iv_key, enc_key),
                        ["api_version"] = c_encryption.encrypt("3.0b", enc_key),
                        ["program_key"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(program_key))
                    };

                    string result = Encoding.UTF8.GetString(web.UploadValues(api_link + "handler.php?type=init", values));

                    ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

                    switch (result) {
                        case "program_doesnt_exist":
                            MessageBox.Show("The program doesnt exist", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                            break;

                        case string xd when xd.Equals(c_encryption.encrypt("wrong_version", enc_key)):
                            MessageBox.Show("Wrong program version", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                            break;

                        case string xd when xd.Equals(c_encryption.encrypt("old_api_version", enc_key)):
                            MessageBox.Show("Please download the newest API files on the auth's website", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Invalid API/Encryption key or the session expired", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }
        public static bool c_login(string c_username, string c_password, string c_hwid = "default") {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            try {
                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    ServicePointManager.ServerCertificateValidationCallback = c_encryption.pin_public_key;

                    var values = new NameValueCollection {
                        ["username"] = c_encryption.encrypt(c_username, enc_key, iv_key),
                        ["password"] = c_encryption.encrypt(c_password, enc_key, iv_key),
                        ["hwid"] = c_encryption.encrypt(c_hwid, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(program_key))
                    };

                    string result = c_encryption.decrypt(Encoding.UTF8.GetString(web.UploadValues(api_link + "handler.php?type=login", values)), enc_key, iv_key);

                    ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

                    switch (result) {
                        case "invalid_username":
                            MessageBox.Show("Invalid username", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_password":
                            MessageBox.Show("Invalid password", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "user_is_banned":
                            MessageBox.Show("The user is banned", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "no_sub":
                            MessageBox.Show("Your subscription is over", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_hwid":
                            MessageBox.Show("Invalid HWID", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case string _sw when _sw.Contains("logged_in"):
                            string[] s = result.Split('|');

                            c_userdata.username = s[1];
                            c_userdata.email = s[2];

                            c_userdata.expires = c_encryption.unix_to_date(Convert.ToDouble(s[3]));

                            c_userdata.rank = Convert.ToInt32(s[4]);

                            stored_pass = c_encryption.encrypt(c_password, enc_key, iv_key);

                            MessageBox.Show("Logged in!!", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;

                        default:
                            MessageBox.Show("Invalid API/Encryption key or the session expired", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
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

                    ServicePointManager.ServerCertificateValidationCallback = c_encryption.pin_public_key;

                    var values = new NameValueCollection {
                        ["username"] = c_encryption.encrypt(c_username, enc_key, iv_key),
                        ["email"] = c_encryption.encrypt(c_email, enc_key, iv_key),
                        ["password"] = c_encryption.encrypt(c_password, enc_key, iv_key),
                        ["token"] = c_encryption.encrypt(c_token, enc_key, iv_key),
                        ["hwid"] = c_encryption.encrypt(c_hwid, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(program_key))
                    };

                    string result = c_encryption.decrypt(Encoding.UTF8.GetString(web.UploadValues(api_link + "handler.php?type=register", values)), enc_key, iv_key);

                    ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

                    switch (result) {
                        case "user_already_exists":
                            MessageBox.Show("User already exists", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "email_already_exists":
                            MessageBox.Show("Email already exists", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_email_format":
                            MessageBox.Show("Invalid email format", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_token":
                            MessageBox.Show("Invalid token", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "maximum_users_reached":
                            MessageBox.Show("Maximum users of the program was reached, please contact the program owner", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "used_token":
                            MessageBox.Show("Already used token", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "success":
                            MessageBox.Show("Success!!", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;

                        default:
                            MessageBox.Show("Invalid API/Encryption key or the session expired", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
                return false;
            }
        }
        public static bool c_activate(string c_username, string c_password, string c_token) {
            try {
                using (var web = new WebClient()) {
                    web.Proxy = null;
                    web.Headers["User-Agent"] = user_agent;

                    ServicePointManager.ServerCertificateValidationCallback = c_encryption.pin_public_key;

                    var values = new NameValueCollection {
                        ["username"] = c_encryption.encrypt(c_username, enc_key, iv_key),
                        ["password"] = c_encryption.encrypt(c_password, enc_key, iv_key),
                        ["token"] = c_encryption.encrypt(c_token, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(program_key))
                    };

                    string result = c_encryption.decrypt(Encoding.UTF8.GetString(web.UploadValues(api_link + "handler.php?type=activate", values)), enc_key, iv_key);

                    ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

                    switch (result) {
                        case "invalid_username":
                            MessageBox.Show("Invalid username", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_password":
                            MessageBox.Show("Invalid password", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "user_is_banned":
                            MessageBox.Show("The user is banned", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "invalid_token":
                            MessageBox.Show("Invalid token", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "used_token":
                            MessageBox.Show("Already used token", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;

                        case "success":
                            MessageBox.Show("Success!!", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;

                        default:
                            MessageBox.Show("Invalid API/Encryption key or the session expired", "cAuth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
                return false;
            }
        }
        public static bool c_all_in_one(string c_token, string c_hwid = "default") {
            if (c_hwid == "default") c_hwid = WindowsIdentity.GetCurrent().User.Value;

            if (c_login(c_token, c_token, c_hwid))
                return true;

            else if (c_register(c_token, c_token + "@email.com", c_token, c_token, c_hwid)) {
                MessageBox.Show("Success!!, Restarting...");
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

                    ServicePointManager.ServerCertificateValidationCallback = c_encryption.pin_public_key;

                    var values = new NameValueCollection {
                        ["var_name"] = c_encryption.encrypt(c_var_name, enc_key, iv_key),
                        ["username"] = c_encryption.encrypt(c_userdata.username, enc_key, iv_key),
                        ["password"] = stored_pass,
                        ["hwid"] = c_encryption.encrypt(c_hwid, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(program_key))
                    };

                    string result = c_encryption.decrypt(Encoding.UTF8.GetString(web.UploadValues(api_link + "handler.php?type=var", values)), enc_key, iv_key);

                    ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

                    return result;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
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

                    ServicePointManager.ServerCertificateValidationCallback = c_encryption.pin_public_key;

                    var values = new NameValueCollection {
                        ["username"] = c_encryption.encrypt(c_userdata.username, enc_key, iv_key),
                        ["message"] = c_encryption.encrypt(c_message, enc_key, iv_key),
                        ["iv_input"] = c_encryption.encrypt(iv_input, enc_key),
                        ["program_key"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(program_key))
                    };

                    string result = Encoding.UTF8.GetString(web.UploadValues(api_link + "handler.php?type=log", values));

                    ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }
        
        private static string api_link = "https://cauth.me/api/";

        private static string user_agent = "Mozilla cAuth";
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
        public static string byte_arr_to_str(byte[] ba) {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] str_to_byte_arr(String hex) {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0;i < NumberChars;i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string EncryptString(string plainText, byte[] key, byte[] iv) {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;

            using (MemoryStream memoryStream = new MemoryStream()) {
                using (ICryptoTransform aesEncryptor = encryptor.CreateEncryptor()) {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write)) {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);

                        cryptoStream.FlushFinalBlock();

                        byte[] cipherBytes = memoryStream.ToArray();

                        return byte_arr_to_str(cipherBytes);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, byte[] key, byte[] iv) {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;

            using (MemoryStream memoryStream = new MemoryStream()) {
                using (ICryptoTransform aesDecryptor = encryptor.CreateDecryptor()) {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write)) {
                        byte[] cipherBytes = str_to_byte_arr(cipherText);

                        cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                        cryptoStream.FlushFinalBlock();

                        byte[] plainBytes = memoryStream.ToArray();

                        return Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);
                    }
                }
            }
        }
        public static string iv_key() => 
            Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal));

        public static string sha256(string randomString) =>
            byte_arr_to_str(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(randomString)));

        public static string encrypt(string message, string enc_key, string iv = "default_iv") {
            byte[] _key = Encoding.UTF8.GetBytes(sha256(enc_key).Substring(0, 32));

            if (iv == "default_iv")
                return EncryptString(message, _key, Encoding.UTF8.GetBytes("1514834626578394"));
            else
                return EncryptString(message, _key, Encoding.UTF8.GetBytes(sha256(iv).Substring(0, 16)));
        }

        public static string decrypt(string message, string enc_key, string iv = "default_iv") {
            byte[] _key = Encoding.UTF8.GetBytes(sha256(enc_key).Substring(0, 32));

            if (iv == "default_iv") 
                return DecryptString(message, _key, Encoding.UTF8.GetBytes("1514834626578394"));         
            else 
                return DecryptString(message, _key, Encoding.UTF8.GetBytes(sha256(iv).Substring(0, 16)));
        }

        public static DateTime unix_to_date(double unixTimeStamp) =>
            new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();

        public static bool pin_public_key(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            if (null == certificate)
                return false;

            String pk = certificate.GetPublicKeyString();
            if (pk.Equals("3082010A0282010100C7429D4B4591E50FE4B3ABDA72DB3F3EA578E12B9CD4E228E4EDFAC3F9681F354C913386A13E88181D1B14D91723FB50770C5DC94FCA59D4DEE4F6632041EFE76C3B6BCFF6B8F5B38AF92547D04BD08AF71087B094F5DFE8760C8CD09A3771836807588B02282BEC7C4CD73EE7C650C0A7C7F36F2FA56DA17E892B2760C4C75950EA5C90CD4EA301EC0CBC36B8372FE8515A7131CC6DF13A97D95B94C6A92AC4E5BFF217FCB20B3C01DB085229E919555D426D919E9A9F0D4C599FE7473FA7DBDE9B33279E2FC29F6CE09FA1269409E4A82175C8E0B65723DB6F856A53E3FD11363ADD63D1346790A3E4D1E454D1714ECED9815A0F85C5019C0D4DC3D58234C10203010001"))
                return true;

            // Bad dog
            return false;
        }
    }
}
