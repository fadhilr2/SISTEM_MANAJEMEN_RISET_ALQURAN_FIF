using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;

namespace GUI.Views
{
    public class AdminUsersView : UserControl
    {
        private ListBox lstUsers;

        public AdminUsersView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel("ACTIVE USERS", 20, true);
            lblTitle.Location = new Point(50, 50);

            lstUsers = new ListBox();
            lstUsers.Location = new Point(50, 100);
            lstUsers.Size = new Size(700, 200);
            lstUsers.Font = new Font("Segoe UI", 12F);
            lstUsers.BackColor = UIHelpers.SecondaryColor;
            lstUsers.ForeColor = UIHelpers.TextColor;

            var btnBack = UIHelpers.CreateSecondaryButton("Back");
            btnBack.Location = new Point(50, 350);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AdminView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lstUsers);
            this.Controls.Add(btnBack);
        }

        private void LoadData()
        {
            lstUsers.Items.Clear();
            foreach (User user in Lib.services.DataContext.Users.GetAll())
            {
                if (!user.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    lstUsers.Items.Add($"[{user.Role}] {user.Name} ({user.Email})");
                }
            }
        }
    }
}
