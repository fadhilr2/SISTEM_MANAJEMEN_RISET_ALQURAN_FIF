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
                profileUser = Session.Instance.Account;
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
                var lblNotFound = UIHelpers.CreateLabel(Messages.Get("user.not_found"), 16);
                lblNotFound.Location = new Point(50, 50);
                this.Controls.Add(lblNotFound);
                
                var btnBack1 = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.back"));
                btnBack1.Location = new Point(50, 100);
                btnBack1.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());
                this.Controls.Add(btnBack1);
                return;
            }

            var lblTitle = UIHelpers.CreateLabel(isMyProfile ? Messages.Get("gui.title.profile") : $"{profileUser.Name.ToUpper()} - {Messages.Get("gui.title.profile")}", 20, true);
            lblTitle.Location = new Point(50, 40);

            var lblName = UIHelpers.CreateLabel($"{Messages.Get("gui.label.name")} {profileUser.Name}", 14);
            lblName.Location = new Point(50, 100);

            var lblEmail = UIHelpers.CreateLabel($"{Messages.Get("gui.label.email")} {profileUser.Email}", 14);
            lblEmail.Location = new Point(50, 135);

            var lblRole = UIHelpers.CreateLabel($"{Messages.Get("gui.label.role")} {profileUser.Role}", 14);
            lblRole.Location = new Point(50, 170);

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblName);
            this.Controls.Add(lblEmail);
            this.Controls.Add(lblRole);

            if (isMyProfile)
            {
                var btnEdit = UIHelpers.CreateButton(Messages.Get("gui.btn.edit_profile"));
                btnEdit.Location = new Point(50, 220);
                btnEdit.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new EditProfileView());
                this.Controls.Add(btnEdit);
            }

            var lblPapers = UIHelpers.CreateLabel(Messages.Get("gui.label.authored_papers"), 14, true);
            lblPapers.Location = new Point(450, 40);
            this.Controls.Add(lblPapers);

            var lstResearch = new ListBox();
            lstResearch.Location = new Point(450, 80);
            lstResearch.Size = new Size(500, 320);
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

            var btnBack = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.back"));
            btnBack.Location = new Point(50, 450);
            btnBack.Click += (s, e) => 
            {
                if (isMyProfile) (this.ParentForm as Form1)?.NavigateTo(new HomeView());
                else (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());
            };
            this.Controls.Add(btnBack);
        }
    }
}
