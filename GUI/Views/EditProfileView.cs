using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class EditProfileView : UserControl
    {
        private TextBox txtName;

        public EditProfileView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel("EDIT PROFILE", 20, true);
            lblTitle.Location = new Point(50, 40);

            var lblName = UIHelpers.CreateLabel("New Name:", 12);
            lblName.Location = new Point(50, 100);

            txtName = UIHelpers.CreateTextBox();
            txtName.Text = Session.Account.Name;
            txtName.Location = new Point(50, 130);

            var btnSave = UIHelpers.CreateButton("Save Name");
            btnSave.Location = new Point(50, 180);
            btnSave.Click += BtnSave_Click;

            var btnBack = UIHelpers.CreateSecondaryButton("Cancel");
            btnBack.Location = new Point(50, 240);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new ProfileView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnBack);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            User user = UserService.SearchUser("email", Session.Account.Email);
            if (user != null)
            {
                user.Name = txtName.Text;
                Session.Account.Name = txtName.Text;
            }

            MessageBox.Show("Name updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            (this.ParentForm as Form1)?.NavigateTo(new ProfileView());
        }
    }
}
