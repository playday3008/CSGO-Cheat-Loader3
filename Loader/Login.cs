﻿using c_auth;
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
    public partial class Login : MetroFramework.Forms.MetroForm
    {
        public Login() {
            InitializeComponent();
            c_api.c_init("1.0", "your program key", "your response encryption key"); //initialize the auth
        }

        private void OpenRegister_Click(object sender, EventArgs e) {
            Register form = new Register(); //creates new form
            form.Show(); //show this form
        }

        private void llogin_Click(object sender, EventArgs e) {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //bugs sometimes

            bool response = c_api.c_login(Username.Text, Password.Text); //login using the first and the second textbox

            if (response) {
                c_api.c_log("Successfully logged in"); //log a message to the panel

                new Main().Show();
                this.Hide();
            }
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e) =>
            Application.Exit(); //close the app if the form is closed
    }
}
