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
            var lblTitle = UIHelpers.CreateLabel(Messages.Get("gui.title.edit_profile"), 20, true);
            lblTitle.Location = new Point(50, 40);

            var lblName = UIHelpers.CreateLabel(Messages.Get("gui.label.new_name"), 12);
            lblName.Location = new Point(50, 100);

            txtName = UIHelpers.CreateTextBox();
            txtName.Text = Session.Instance.Account.Name;
            txtName.Location = new Point(50, 130);

            var btnSave = UIHelpers.CreateButton(Messages.Get("gui.btn.save"));
            btnSave.Location = new Point(50, 190);
            btnSave.Click += BtnSave_Click;

            var btnBack = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.cancel"));
            btnBack.Location = new Point(50, 250);
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
                MessageBox.Show(Messages.Get("gui.msg.empty_name"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            User user = UserService.SearchUser("email", Session.Instance.Account.Email);
            if (user != null)
            {
                string oldName = user.Name;
                user.Name = txtName.Text;
                Session.Instance.Account.Name = txtName.Text;

                // Cascade the name update to all authored papers so they don't disappear
                if (oldName != null && !oldName.Equals(txtName.Text, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var paper in Lib.services.DataContext.Papers.GetAll())
                    {
                        if (paper.Author.Equals(oldName, StringComparison.OrdinalIgnoreCase))
                        {
                            paper.Author = txtName.Text;
                        }
                    }
                }
            }

            MessageBox.Show(Messages.Get("gui.msg.name_updated"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            (this.ParentForm as Form1)?.NavigateTo(new ProfileView());
        }
    }
}
