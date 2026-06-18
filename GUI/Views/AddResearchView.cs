using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class AddResearchView : UserControl
    {
        private TextBox txtTitle;
        private TextBox txtAbstract;

        public AddResearchView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblHeader = UIHelpers.CreateLabel("ADD NEW RESEARCH", 20, true);
            lblHeader.Location = new Point(50, 30);

            var lblTitle = UIHelpers.CreateLabel("Title:", 12);
            lblTitle.Location = new Point(50, 80);

            txtTitle = UIHelpers.CreateTextBox();
            txtTitle.Location = new Point(50, 110);
            txtTitle.Size = new Size(700, 30);

            var lblAbstract = UIHelpers.CreateLabel("Abstract:", 12);
            lblAbstract.Location = new Point(50, 150);

            txtAbstract = new TextBox();
            txtAbstract.Multiline = true;
            txtAbstract.Location = new Point(50, 180);
            txtAbstract.Size = new Size(700, 100);
            txtAbstract.BackColor = UIHelpers.SecondaryColor;
            txtAbstract.ForeColor = UIHelpers.TextColor;
            txtAbstract.Font = new Font("Segoe UI", 11F);

            var btnSave = UIHelpers.CreateButton("Upload Research");
            btnSave.Location = new Point(50, 300);
            btnSave.Click += BtnSave_Click;

            var btnCancel = UIHelpers.CreateSecondaryButton("Cancel");
            btnCancel.Location = new Point(300, 300);
            btnCancel.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new HomeView());

            this.Controls.Add(lblHeader);
            this.Controls.Add(lblTitle);
            this.Controls.Add(txtTitle);
            this.Controls.Add(lblAbstract);
            this.Controls.Add(txtAbstract);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtAbstract.Text))
            {
                MessageBox.Show("Title and Abstract cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool success = PaperService.UploadPaper(txtTitle.Text, txtAbstract.Text);
            if (success)
            {
                MessageBox.Show(Messages.Get("confirm.uploaded"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                (this.ParentForm as Form1)?.NavigateTo(new HomeView());
            }
            else
            {
                MessageBox.Show(Messages.Get("permission.denied"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
