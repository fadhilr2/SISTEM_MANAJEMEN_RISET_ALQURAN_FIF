using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class ResearchDetailView : UserControl
    {
        private Paper currentPaper;

        public ResearchDetailView(Paper paper)
        {
            currentPaper = paper;
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel(currentPaper.Title, 16, true);
            lblTitle.Location = new Point(50, 30);
            lblTitle.MaximumSize = new Size(700, 0);

            var lblAuthor = UIHelpers.CreateLabel($"By: {currentPaper.Author} | Date: {currentPaper.Date}", 12);
            lblAuthor.Location = new Point(50, 70);

            var txtAbstract = new TextBox();
            txtAbstract.Multiline = true;
            txtAbstract.ReadOnly = true;
            txtAbstract.Text = currentPaper.Paper_Abstract;
            txtAbstract.Location = new Point(50, 110);
            txtAbstract.Size = new Size(700, 150);
            txtAbstract.BackColor = UIHelpers.SecondaryColor;
            txtAbstract.ForeColor = UIHelpers.TextColor;
            txtAbstract.Font = new Font("Segoe UI", 11F);
            txtAbstract.ScrollBars = ScrollBars.Vertical;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblAuthor);
            this.Controls.Add(txtAbstract);

            int startX = 50;
            var btnViewProfile = UIHelpers.CreateButton("View Author Profile");
            btnViewProfile.Location = new Point(startX, 280);
            btnViewProfile.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new ProfileView(currentPaper.Author));
            this.Controls.Add(btnViewProfile);
            startX += 220;

            if (PaperService.CanEdit(currentPaper))
            {
                var btnEdit = UIHelpers.CreateButton("Edit");
                btnEdit.Location = new Point(startX, 280);
                btnEdit.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new EditResearchView(currentPaper));
                this.Controls.Add(btnEdit);
                startX += 220;

                var btnDelete = UIHelpers.CreateButton("Delete");
                btnDelete.BackColor = Color.IndianRed;
                btnDelete.Location = new Point(startX, 280);
                btnDelete.Click += (s, e) => 
                {
                    if (MessageBox.Show("Are you sure you want to delete this research?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        PaperService.DeletePaper(currentPaper);
                        MessageBox.Show(Messages.Get("confirm.deleted"), "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());
                    }
                };
                this.Controls.Add(btnDelete);
            }

            var btnBack = UIHelpers.CreateSecondaryButton("Back");
            btnBack.Location = new Point(50, 340);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());
            this.Controls.Add(btnBack);
        }
    }
}
