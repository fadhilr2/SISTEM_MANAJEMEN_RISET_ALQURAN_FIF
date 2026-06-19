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
            var lblTitle = UIHelpers.CreateLabel(Messages.Get("gui.title.login"), 20, true);
            lblTitle.Location = new Point((1000 - 100) / 2, 100);

            int startX = (1000 - 250) / 2;

            var lblEmail = UIHelpers.CreateLabel(Messages.Get("gui.label.email"));
            lblEmail.Location = new Point(startX, 170);
            
            txtEmail = UIHelpers.CreateTextBox();
            txtEmail.Location = new Point(startX, 195);

            var lblPassword = UIHelpers.CreateLabel(Messages.Get("gui.label.password"));
            lblPassword.Location = new Point(startX, 240);
            
            txtPassword = UIHelpers.CreateTextBox(isPassword: true);
            txtPassword.Location = new Point(startX, 265);

            var btnLogin = UIHelpers.CreateButton(Messages.Get("gui.btn.login"));
            btnLogin.Location = new Point((1000 - btnLogin.Width) / 2, 330);
            btnLogin.Click += BtnLogin_Click;

            var btnBack = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.back"));
            btnBack.Location = new Point((1000 - btnBack.Width) / 2, 380);
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
                if (Session.Instance.Account.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
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
