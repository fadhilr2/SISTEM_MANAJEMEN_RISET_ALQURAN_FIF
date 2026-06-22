using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.common;
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
            var lblTitle = UIHelpers.CreateLabel(Messages.Get("gui.title.active_users"), 20, true);
            lblTitle.Location = new Point(50, 50);

            lstUsers = new ListBox();
            lstUsers.Location = new Point(50, 100);
            lstUsers.Size = new Size(900, 320);
            lstUsers.Font = new Font("Segoe UI", 12F);
            lstUsers.BackColor = UIHelpers.SecondaryColor;
            lstUsers.ForeColor = UIHelpers.TextColor;

            var btnSetRole = UIHelpers.CreateButton(Messages.Get("gui.btn.change_role"));
            btnSetRole.Location = new Point(750, 450);
            btnSetRole.Click += BtnSetRole_Click;

            var btnBack = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.back"));
            btnBack.Location = new Point(50, 450);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AdminView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lstUsers);
            this.Controls.Add(btnSetRole);
            this.Controls.Add(btnBack);
        }

        private void BtnSetRole_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItem == null)
            {
                MessageBox.Show(Messages.Get("admin.select_user"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (lstUsers.SelectedItem is UserListItem selectedItem)
            {
                User user = selectedItem.User;
                if (user.Role.Equals("visitor", StringComparison.OrdinalIgnoreCase)) user.Role = "researcher";
                else if (user.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase)) user.Role = "visitor";

                MessageBox.Show(Messages.Get("admin.role_updated"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void LoadData()
        {
            lstUsers.Items.Clear();
            foreach (User user in Lib.services.DataContext.Users.GetAll())
            {
                if (!user.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    lstUsers.Items.Add(new UserListItem { User = user });
                }
            }
        }

        private class UserListItem
        {
            public User User { get; set; }
            public override string ToString()
            {
                return $"[{User.Role}] {User.Name ?? User.Email} ({User.Email})";
            }
        }
    }
}
