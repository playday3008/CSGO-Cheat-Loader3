using c_auth;
using ManualMapInjection.Injection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beta_Loader
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        public Main() =>
            InitializeComponent();

        private void Main_FormClosed(object sender, FormClosedEventArgs e) =>
            Application.Exit(); //close the app if the form is closed

        private void Inject_Click(object sender, EventArgs e) {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //bugs sometimes

            Process target = Process.GetProcessesByName("csgo").FirstOrDefault(); // csgo process check

            if (target != null) // check if csgo is open
            {
                try //try to do a function
                {
                    using(WebClient mac = new WebClient()){
                        mac.Proxy = null;
                        mac.Headers.Add("User-Agent", c_api.c_var("user_agent")); //add user-agent headers, should be Mozilla
                        byte[] crypted = mac.DownloadData(c_api.c_var("download")); //download the dll and save it to bytes, should be the download link
                        byte[] decrypted = AES.DecryptAES(crypted, Encoding.UTF8.GetBytes(c_api.c_var("crypt_pass"))); // decrypt the dll, crypt pass value = XSaBw6JkWFZh7PBgLQb5TndqzEkm
                        var injector = new ManualMapInjector(target) { AsyncInjection = true }; //initializing the injector
                        Inject.Text = $"hmodule = 0x{injector.Inject(decrypted).ToInt64():x8}"; //inject the dll
                        MessageBox.Show("Success!!!");
                        c_api.c_log("injected successfully");
                        Application.Exit();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Exception happened : " + ex); //check some exceptions
                    Application.Exit();
                }
            }
            else {
                MessageBox.Show("Please open CSGO"); //error open csgo
            }
        }

        private void Main_Load(object sender, EventArgs e) {
            label2.Text = c_userdata.username;
            label4.Text = c_userdata.expires.ToString();
        }
    }
}
