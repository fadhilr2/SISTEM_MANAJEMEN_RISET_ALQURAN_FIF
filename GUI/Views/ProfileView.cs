using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class ProfileView : UserControl
    {
        private User profileUser;
        private bool isMyProfile;

        public ProfileView(string username = null)
        {
            if (username == null)
            {
                profileUser = Session.Account;
                isMyProfile = true;
            }
            else
            {
                profileUser = UserService.SearchUser("name", username);
                isMyProfile = false;
            }

            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            if (profileUser == null)
            {
                var lblNotFound = UIHelpers.CreateLabel(Messages.Get("paper.not_found"), 16);
                lblNotFound.Location = new Point(50, 50);
                this.Controls.Add(lblNotFound);
                
                var btnBack1 = UIHelpers.CreateSecondaryButton("Back");
                btnBack1.Location = new Point(50, 100);
                btnBack1.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());
                this.Controls.Add(btnBack1);
                return;
            }

            var lblTitle = UIHelpers.CreateLabel(isMyProfile ? "MY PROFILE" : $"{profileUser.Name.ToUpper()}'S PROFILE", 20, true);
            lblTitle.Location = new Point(50, 40);

            var lblName = UIHelpers.CreateLabel($"Name: {profileUser.Name}", 14);
            lblName.Location = new Point(50, 90);

            var lblEmail = UIHelpers.CreateLabel($"Email: {profileUser.Email}", 14);
            lblEmail.Location = new Point(50, 120);

            var lblRole = UIHelpers.CreateLabel($"Role: {profileUser.Role}", 14);
            lblRole.Location = new Point(50, 150);

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblName);
            this.Controls.Add(lblEmail);
            this.Controls.Add(lblRole);

            if (isMyProfile)
            {
                var btnEdit = UIHelpers.CreateButton("Edit My Profile");
                btnEdit.Location = new Point(50, 200);
                btnEdit.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new EditProfileView());
                this.Controls.Add(btnEdit);
            }

            var lblPapers = UIHelpers.CreateLabel("Authored Papers:", 14, true);
            lblPapers.Location = new Point(350, 40);
            this.Controls.Add(lblPapers);

            var lstResearch = new ListBox();
            lstResearch.Location = new Point(350, 80);
            lstResearch.Size = new Size(400, 200);
            lstResearch.Font = new Font("Segoe UI", 12F);
            lstResearch.BackColor = UIHelpers.SecondaryColor;
            lstResearch.ForeColor = UIHelpers.TextColor;
            foreach (Paper paper in Lib.services.DataContext.Papers.GetAll())
            {
                if (paper.Author.Equals(profileUser.Name, StringComparison.OrdinalIgnoreCase))
                {
                    lstResearch.Items.Add(paper.Title);
                }
            }
            this.Controls.Add(lstResearch);

            var btnBack = UIHelpers.CreateSecondaryButton("Back");
            btnBack.Location = new Point(50, 350);
            btnBack.Click += (s, e) => 
            {
                if (isMyProfile) (this.ParentForm as Form1)?.NavigateTo(new HomeView());
                else (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());
            };
            this.Controls.Add(btnBack);
        }
    }
}
