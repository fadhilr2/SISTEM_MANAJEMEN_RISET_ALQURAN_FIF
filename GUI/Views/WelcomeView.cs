using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.common;

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
            // Center the label manually roughly based on 1000px width
            lblTitle.Location = new Point((1000 - 550) / 2, 130);

            var btnLogin = UIHelpers.CreateButton(Messages.Get("gui.btn.login"));
            btnLogin.Location = new Point((1000 - btnLogin.Width) / 2, 230);
            btnLogin.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new LoginView());

            var btnRegister = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.register"));
            btnRegister.Location = new Point((1000 - btnRegister.Width) / 2, 290);
            btnRegister.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new RegisterView());

            var btnExit = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.exit"));
            btnExit.Location = new Point((1000 - btnExit.Width) / 2, 350);
            btnExit.Click += (s, e) => Application.Exit();

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnExit);
        }
    }
}
