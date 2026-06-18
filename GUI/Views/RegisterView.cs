using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class RegisterView : UserControl
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;

        public RegisterView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel("REGISTER", 20, true);
            lblTitle.Location = new Point((800 - 140) / 2, 40);

            int startX = (800 - 250) / 2;

            var lblEmail = UIHelpers.CreateLabel("Email:");
            lblEmail.Location = new Point(startX, 100);
            
            txtEmail = UIHelpers.CreateTextBox();
            txtEmail.Location = new Point(startX, 125);

            var lblPassword = UIHelpers.CreateLabel("Password:");
            lblPassword.Location = new Point(startX, 170);
            
            txtPassword = UIHelpers.CreateTextBox(isPassword: true);
            txtPassword.Location = new Point(startX, 195);

            var lblConfirmPassword = UIHelpers.CreateLabel("Confirm Password:");
            lblConfirmPassword.Location = new Point(startX, 240);
            
            txtConfirmPassword = UIHelpers.CreateTextBox(isPassword: true);
            txtConfirmPassword.Location = new Point(startX, 265);

            var btnRegister = UIHelpers.CreateButton("Register");
            btnRegister.Location = new Point((800 - btnRegister.Width) / 2, 320);
            btnRegister.Click += BtnRegister_Click;

            var btnBack = UIHelpers.CreateSecondaryButton("Back");
            btnBack.Location = new Point((800 - btnBack.Width) / 2, 370);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new WelcomeView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnBack);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(Messages.Get("register.password_mismatch"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AuthService.Register(txtEmail.Text, txtPassword.Text);
            MessageBox.Show(Messages.Get("register.success"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            (this.ParentForm as Form1)?.NavigateTo(new WelcomeView());
        }
    }
}
