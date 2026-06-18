using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;

namespace GUI.Views
{
    public class WelcomeView : UserControl
    {
        public WelcomeView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel("Sistem Manajemen Riset Al-Qur'an FIF", 18, true);
            lblTitle.Location = new Point(100, 60);
            // Center the label manually roughly based on 800px width
            lblTitle.Location = new Point((800 - 550) / 2, 80);

            var btnLogin = UIHelpers.CreateButton("Login");
            btnLogin.Location = new Point((800 - btnLogin.Width) / 2, 160);
            btnLogin.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new LoginView());

            var btnRegister = UIHelpers.CreateSecondaryButton("Register");
            btnRegister.Location = new Point((800 - btnRegister.Width) / 2, 220);
            btnRegister.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new RegisterView());

            var btnExit = UIHelpers.CreateSecondaryButton("Exit");
            btnExit.Location = new Point((800 - btnExit.Width) / 2, 280);
            btnExit.Click += (s, e) => Application.Exit();

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnExit);
        }
    }
}
