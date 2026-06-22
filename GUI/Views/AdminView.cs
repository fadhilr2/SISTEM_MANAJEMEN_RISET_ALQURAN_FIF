using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.common;
using Lib.services;

namespace GUI.Views
{
    public class AdminView : UserControl
    {
        public AdminView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel(Messages.Get("gui.title.admin"), 20, true);
            lblTitle.Location = new Point((1000 - 180) / 2, 60);

            var lblWelcome = UIHelpers.CreateLabel($"Welcome, Administrator!", 14);
            lblWelcome.Location = new Point(50, 110);

            var btnUsers = UIHelpers.CreateButton(Messages.Get("gui.btn.manage_users"));
            btnUsers.Location = new Point((1000 - btnUsers.Width) / 2, 180);
            btnUsers.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AdminUsersView());

            var btnRequests = UIHelpers.CreateButton(Messages.Get("gui.btn.manage_requests"));
            btnRequests.Location = new Point((1000 - btnRequests.Width) / 2, 240);
            btnRequests.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AdminRequestsView());

            var btnLogout = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.logout"));
            btnLogout.Location = new Point((1000 - btnLogout.Width) / 2, 320);
            btnLogout.Click += (s, e) => 
            {
                Session.Instance.Logout();
                (this.ParentForm as Form1)?.NavigateTo(new WelcomeView());
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblWelcome);
            this.Controls.Add(btnUsers);
            this.Controls.Add(btnRequests);
            this.Controls.Add(btnLogout);
        }
    }
}
