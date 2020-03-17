namespace Beta_Loader
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.label2 = new MetroFramework.Controls.MetroLabel();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.label4 = new MetroFramework.Controls.MetroLabel();
			this.Inject = new MetroFramework.Controls.MetroButton();
			this.SuspendLayout();
			// 
			// metroLabel1
			// 
			this.metroLabel1.AutoSize = true;
			this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.metroLabel1.Location = new System.Drawing.Point(17, 34);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(78, 19);
			this.metroLabel1.Style = MetroFramework.MetroColorStyle.Purple;
			this.metroLabel1.TabIndex = 5;
			this.metroLabel1.Text = "Username :";
			this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(101, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 19);
			this.label2.Style = MetroFramework.MetroColorStyle.Purple;
			this.label2.TabIndex = 6;
			this.label2.Text = "user";
			this.label2.Theme = MetroFramework.MetroThemeStyle.Dark;
			// 
			// metroLabel2
			// 
			this.metroLabel2.AutoSize = true;
			this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.metroLabel2.Location = new System.Drawing.Point(17, 73);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(58, 19);
			this.metroLabel2.Style = MetroFramework.MetroColorStyle.Purple;
			this.metroLabel2.TabIndex = 7;
			this.metroLabel2.Text = "Expires :";
			this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(81, 73);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 19);
			this.label4.Style = MetroFramework.MetroColorStyle.Purple;
			this.label4.TabIndex = 8;
			this.label4.Text = "expire";
			this.label4.Theme = MetroFramework.MetroThemeStyle.Dark;
			// 
			// Inject
			// 
			this.Inject.FontSize = MetroFramework.MetroButtonSize.Medium;
			this.Inject.Location = new System.Drawing.Point(98, 121);
			this.Inject.Name = "Inject";
			this.Inject.Size = new System.Drawing.Size(113, 45);
			this.Inject.Style = MetroFramework.MetroColorStyle.Purple;
			this.Inject.TabIndex = 9;
			this.Inject.Text = "Inject";
			this.Inject.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.Inject.UseSelectable = true;
			this.Inject.Click += new System.EventHandler(this.Inject_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(300, 189);
			this.Controls.Add(this.Inject);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.metroLabel2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.metroLabel1);
			this.Name = "Main";
			this.Resizable = false;
			this.ShadowType = MetroFramework.Forms.MetroFormShadowType.SystemShadow;
			this.Style = MetroFramework.MetroColorStyle.Purple;
			this.Theme = MetroFramework.MetroThemeStyle.Dark;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
			this.Load += new System.EventHandler(this.Main_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel label2;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel label4;
        private MetroFramework.Controls.MetroButton Inject;
    }
}