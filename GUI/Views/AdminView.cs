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
            var lblTitle = UIHelpers.CreateLabel("ADMIN PAGE", 20, true);
            lblTitle.Location = new Point((800 - 180) / 2, 40);

            var lblWelcome = UIHelpers.CreateLabel($"Welcome, Administrator!", 14);
            lblWelcome.Location = new Point(50, 90);

            var btnUsers = UIHelpers.CreateButton("View All Users");
            btnUsers.Location = new Point((800 - btnUsers.Width) / 2, 150);
            btnUsers.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AdminUsersView());

            var btnRequests = UIHelpers.CreateButton("View Registration Requests");
            btnRequests.Location = new Point((800 - btnRequests.Width) / 2, 210);
            btnRequests.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AdminRequestsView());

            var btnLogout = UIHelpers.CreateSecondaryButton("Logout");
            btnLogout.Location = new Point((800 - btnLogout.Width) / 2, 290);
            btnLogout.Click += (s, e) => 
            {
                Session.Account = null;
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
