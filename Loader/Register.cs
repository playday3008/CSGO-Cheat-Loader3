using c_auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beta_Loader
{
    public partial class Register : MetroFramework.Forms.MetroForm
    {

        public Register() =>
            InitializeComponent();

        public bool response { get; set; }

        private void rregister_Click(object sender, EventArgs e) {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //bugs sometimes

            if (Redeem.Checked) {
                //The redeem arguments goes in the order: username, password, token
                response = c_api.c_activate(Username.Text, Password.Text, Token.Text); //redeem a token using textboxes
            }
            else {
                // The register arguments goes in the order: username, password, email, token
                response = c_api.c_register(Username.Text, Email.Text, Password.Text, Token.Text); //register using textboxes
            }
            if (response) {
                MessageBox.Show("Registered/Activated successfuly");
            }
            else {

            }
        }

        private void Redeem_CheckedChanged(object sender, EventArgs e) {
            if (Redeem.Checked)
                rregister.Text = "Activate"; //changing the text bcs yes lol
            else
                rregister.Text = "Register";
        }
    }
}
