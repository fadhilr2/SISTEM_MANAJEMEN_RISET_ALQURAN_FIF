using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class AdminRequestsView : UserControl
    {
        private ListBox lstRequests;

        public AdminRequestsView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel(Messages.Get("gui.title.registration_requests"), 20, true);
            lblTitle.Location = new Point(50, 50);

            lstRequests = new ListBox();
            lstRequests.Location = new Point(50, 100);
            lstRequests.Size = new Size(900, 320);
            lstRequests.Font = new Font("Segoe UI", 12F);
            lstRequests.BackColor = UIHelpers.SecondaryColor;
            lstRequests.ForeColor = UIHelpers.TextColor;

            var btnAccept = UIHelpers.CreateButton(Messages.Get("gui.btn.accept"));
            btnAccept.BackColor = Color.SeaGreen;
            btnAccept.Location = new Point(50, 450);
            btnAccept.Click += BtnAccept_Click;

            var btnReject = UIHelpers.CreateButton(Messages.Get("gui.btn.reject"));
            btnReject.BackColor = Color.IndianRed;
            btnReject.Location = new Point(270, 450);
            btnReject.Click += BtnReject_Click;

            var btnBack = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.back"));
            btnBack.Location = new Point(750, 450);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AdminView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lstRequests);
            this.Controls.Add(btnAccept);
            this.Controls.Add(btnReject);
            this.Controls.Add(btnBack);
        }

        private void LoadData()
        {
            lstRequests.Items.Clear();
            if (!Lib.services.DataContext.Requests.GetAll().Any())
            {
                lstRequests.Items.Add(Messages.Get("admin.no_requests"));
                return;
            }

            foreach (User user in Lib.services.DataContext.Requests.GetAll())
            {
                lstRequests.Items.Add(user.Email);
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            string email = GetSelectedEmail();
            if (email != null)
            {
                User request = Lib.services.DataContext.Requests.GetAll().FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (request != null)
                {
                    Lib.services.DataContext.Users.Add(request);
                    Lib.services.DataContext.Requests.Remove(request);
                    MessageBox.Show(Messages.Get("admin.accept"), "Accepted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            string email = GetSelectedEmail();
            if (email != null)
            {
                User request = Lib.services.DataContext.Requests.GetAll().FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (request != null)
                {
                    Lib.services.DataContext.Requests.Remove(request);
                    MessageBox.Show(Messages.Get("admin.reject"), "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }

        private string GetSelectedEmail()
        {
            if (lstRequests.SelectedItem != null)
            {
                string text = lstRequests.SelectedItem.ToString();
                if (text == Messages.Get("admin.no_requests")) return null;
                return text;
            }
            MessageBox.Show(Messages.Get("admin.select_user"), "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }
    }
}
