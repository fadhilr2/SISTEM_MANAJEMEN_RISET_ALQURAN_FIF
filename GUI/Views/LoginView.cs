using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.services;
using Lib.models;
using Lib.common;

namespace GUI.Views
{
    public class LoginView : UserControl
    {
        private TextBox txtEmail;
        private TextBox txtPassword;

        public LoginView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel("LOGIN", 20, true);
            lblTitle.Location = new Point((800 - 100) / 2, 60);

            int startX = (800 - 250) / 2;

            var lblEmail = UIHelpers.CreateLabel("Email:");
            lblEmail.Location = new Point(startX, 130);
            
            txtEmail = UIHelpers.CreateTextBox();
            txtEmail.Location = new Point(startX, 155);

            var lblPassword = UIHelpers.CreateLabel("Password:");
            lblPassword.Location = new Point(startX, 200);
            
            txtPassword = UIHelpers.CreateTextBox(isPassword: true);
            txtPassword.Location = new Point(startX, 225);

            var btnLogin = UIHelpers.CreateButton("Login");
            btnLogin.Location = new Point((800 - btnLogin.Width) / 2, 280);
            btnLogin.Click += BtnLogin_Click;

            var btnBack = UIHelpers.CreateSecondaryButton("Back");
            btnBack.Location = new Point((800 - btnBack.Width) / 2, 330);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new WelcomeView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnBack);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            bool isSuccess = AuthService.Login(txtEmail.Text, txtPassword.Text);
            if (isSuccess)
            {
                if (Session.Account.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    (this.ParentForm as Form1)?.NavigateTo(new AdminView());
                }
                else
                {
                    (this.ParentForm as Form1)?.NavigateTo(new HomeView());
                }
            }
            else
            {
                MessageBox.Show(Messages.Get("login.failed"), "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
