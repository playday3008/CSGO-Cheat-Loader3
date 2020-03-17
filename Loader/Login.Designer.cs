namespace Beta_Loader
{
    partial class Login
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.llogin = new MetroFramework.Controls.MetroButton();
			this.OpenRegister = new MetroFramework.Controls.MetroButton();
			this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.Username = new MetroFramework.Controls.MetroTextBox();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.Password = new MetroFramework.Controls.MetroTextBox();
			((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
			this.SuspendLayout();
			// 
			// llogin
			// 
			this.llogin.FontSize = MetroFramework.MetroButtonSize.Medium;
			this.llogin.Location = new System.Drawing.Point(32, 127);
			this.llogin.Name = "llogin";
			this.llogin.Size = new System.Drawing.Size(97, 37);
			this.llogin.Style = MetroFramework.MetroColorStyle.Purple;
			this.llogin.TabIndex = 6;
			this.llogin.Text = "Login";
			this.llogin.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.llogin.UseSelectable = true;
			this.llogin.Click += new System.EventHandler(this.llogin_Click);
			// 
			// OpenRegister
			// 
			this.OpenRegister.Location = new System.Drawing.Point(163, 127);
			this.OpenRegister.Name = "OpenRegister";
			this.OpenRegister.Size = new System.Drawing.Size(97, 37);
			this.OpenRegister.Style = MetroFramework.MetroColorStyle.Purple;
			this.OpenRegister.TabIndex = 7;
			this.OpenRegister.Text = "Register";
			this.OpenRegister.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.OpenRegister.UseSelectable = true;
			this.OpenRegister.Click += new System.EventHandler(this.OpenRegister_Click);
			// 
			// metroStyleManager1
			// 
			this.metroStyleManager1.Owner = null;
			this.metroStyleManager1.Style = MetroFramework.MetroColorStyle.Purple;
			this.metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Dark;
			// 
			// metroLabel1
			// 
			this.metroLabel1.AutoSize = true;
			this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.metroLabel1.Location = new System.Drawing.Point(32, 19);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(78, 19);
			this.metroLabel1.Style = MetroFramework.MetroColorStyle.Purple;
			this.metroLabel1.TabIndex = 8;
			this.metroLabel1.Text = "Username :";
			this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
			// 
			// Username
			// 
			// 
			// 
			// 
			this.Username.CustomButton.Image = null;
			this.Username.CustomButton.Location = new System.Drawing.Point(206, 1);
			this.Username.CustomButton.Name = "";
			this.Username.CustomButton.Size = new System.Drawing.Size(21, 21);
			this.Username.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.Username.CustomButton.TabIndex = 1;
			this.Username.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.Username.CustomButton.UseSelectable = true;
			this.Username.CustomButton.Visible = false;
			this.Username.Lines = new string[0];
			this.Username.Location = new System.Drawing.Point(32, 41);
			this.Username.MaxLength = 32767;
			this.Username.Name = "Username";
			this.Username.PasswordChar = '\0';
			this.Username.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.Username.SelectedText = "";
			this.Username.SelectionLength = 0;
			this.Username.SelectionStart = 0;
			this.Username.ShortcutsEnabled = true;
			this.Username.Size = new System.Drawing.Size(228, 23);
			this.Username.Style = MetroFramework.MetroColorStyle.Purple;
			this.Username.TabIndex = 9;
			this.Username.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Username.UseSelectable = true;
			this.Username.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.Username.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// metroLabel2
			// 
			this.metroLabel2.AutoSize = true;
			this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.metroLabel2.Location = new System.Drawing.Point(32, 67);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(74, 19);
			this.metroLabel2.Style = MetroFramework.MetroColorStyle.Purple;
			this.metroLabel2.TabIndex = 10;
			this.metroLabel2.Text = "Password :";
			this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
			// 
			// Password
			// 
			// 
			// 
			// 
			this.Password.CustomButton.Image = null;
			this.Password.CustomButton.Location = new System.Drawing.Point(206, 1);
			this.Password.CustomButton.Name = "";
			this.Password.CustomButton.Size = new System.Drawing.Size(21, 21);
			this.Password.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
			this.Password.CustomButton.TabIndex = 1;
			this.Password.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
			this.Password.CustomButton.UseSelectable = true;
			this.Password.CustomButton.Visible = false;
			this.Password.Lines = new string[0];
			this.Password.Location = new System.Drawing.Point(32, 89);
			this.Password.MaxLength = 32767;
			this.Password.Name = "Password";
			this.Password.PasswordChar = '*';
			this.Password.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.Password.SelectedText = "";
			this.Password.SelectionLength = 0;
			this.Password.SelectionStart = 0;
			this.Password.ShortcutsEnabled = true;
			this.Password.Size = new System.Drawing.Size(228, 23);
			this.Password.Style = MetroFramework.MetroColorStyle.Purple;
			this.Password.TabIndex = 11;
			this.Password.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Password.UseSelectable = true;
			this.Password.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
			this.Password.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
			// 
			// Login
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(299, 193);
			this.Controls.Add(this.Password);
			this.Controls.Add(this.metroLabel2);
			this.Controls.Add(this.Username);
			this.Controls.Add(this.metroLabel1);
			this.Controls.Add(this.OpenRegister);
			this.Controls.Add(this.llogin);
			this.Name = "Login";
			this.Resizable = false;
			this.ShadowType = MetroFramework.Forms.MetroFormShadowType.SystemShadow;
			this.Style = MetroFramework.MetroColorStyle.Purple;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroButton llogin;
        private MetroFramework.Controls.MetroButton OpenRegister;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox Username;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox Password;
    }
}

