using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;
using Lib.common;

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
            var lblTitle = UIHelpers.CreateLabel(Messages.Get("gui.title.home"), 20, true);
            lblTitle.Location = new Point((1000 - 160) / 2, 60);

            var lblWelcome = UIHelpers.CreateLabel($"Welcome, {Session.Instance.Account?.Name}!", 14);
            lblWelcome.Location = new Point(50, 110);

            var btnAllResearch = UIHelpers.CreateButton(Messages.Get("gui.btn.explore_research"));
            btnAllResearch.Location = new Point((1000 - btnAllResearch.Width) / 2, 180);
            btnAllResearch.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());

            var btnProfile = UIHelpers.CreateButton(Messages.Get("gui.title.profile"));
            btnProfile.Location = new Point((1000 - btnProfile.Width) / 2, 240);
            btnProfile.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new ProfileView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblWelcome);
            this.Controls.Add(btnAllResearch);
            this.Controls.Add(btnProfile);

            int nextY = 300;
            if (Session.Instance.Account != null && Session.Instance.Account.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase))
            {
                var btnAddResearch = UIHelpers.CreateButton(Messages.Get("gui.btn.add_research"));
                btnAddResearch.Location = new Point((1000 - btnAddResearch.Width) / 2, nextY);
                btnAddResearch.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AddResearchView());
                this.Controls.Add(btnAddResearch);
                nextY += 50;
            }

            var btnLogout = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.logout"));
            btnLogout.Location = new Point((1000 - btnLogout.Width) / 2, nextY + 30);
            btnLogout.Click += (s, e) => 
            {
                Session.Instance.Logout();
                (this.ParentForm as Form1)?.NavigateTo(new WelcomeView());
            };
            this.Controls.Add(btnLogout);
        }
    }
}
