using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;

namespace GUI.Views
{
    public class HomeView : UserControl
    {
        public HomeView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel("HOME PAGE", 20, true);
            lblTitle.Location = new Point((800 - 160) / 2, 40);

            var lblWelcome = UIHelpers.CreateLabel($"Welcome, {Session.Account?.Name}!", 14);
            lblWelcome.Location = new Point(50, 90);

            var btnAllResearch = UIHelpers.CreateButton("View All Research");
            btnAllResearch.Location = new Point((800 - btnAllResearch.Width) / 2, 140);
            btnAllResearch.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());

            var btnProfile = UIHelpers.CreateButton("My Profile");
            btnProfile.Location = new Point((800 - btnProfile.Width) / 2, 190);
            btnProfile.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new ProfileView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblWelcome);
            this.Controls.Add(btnAllResearch);
            this.Controls.Add(btnProfile);

            int nextY = 240;
            if (Session.Account != null && Session.Account.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase))
            {
                var btnAddResearch = UIHelpers.CreateButton("Add New Research");
                btnAddResearch.Location = new Point((800 - btnAddResearch.Width) / 2, nextY);
                btnAddResearch.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AddResearchView());
                this.Controls.Add(btnAddResearch);
                nextY += 50;
            }

            var btnLogout = UIHelpers.CreateSecondaryButton("Logout");
            btnLogout.Location = new Point((800 - btnLogout.Width) / 2, nextY + 20);
            btnLogout.Click += (s, e) => 
            {
                Session.Account = null;
                (this.ParentForm as Form1)?.NavigateTo(new WelcomeView());
            };
            this.Controls.Add(btnLogout);
        }
    }
}
