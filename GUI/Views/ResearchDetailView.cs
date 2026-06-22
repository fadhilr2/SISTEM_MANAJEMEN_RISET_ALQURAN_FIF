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
            lblTitle.MaximumSize = new Size(900, 0);

            var lblAuthor = UIHelpers.CreateLabel($"{Messages.Get("gui.label.author")} {currentPaper.Author} | Date: {currentPaper.Date}", 12);
            lblAuthor.Location = new Point(50, 80);

            var txtAbstract = new TextBox();
            txtAbstract.Multiline = true;
            txtAbstract.ReadOnly = true;
            txtAbstract.Text = currentPaper.Paper_Abstract;
            txtAbstract.Location = new Point(50, 120);
            txtAbstract.Size = new Size(900, 240);
            txtAbstract.BackColor = UIHelpers.SecondaryColor;
            txtAbstract.ForeColor = UIHelpers.TextColor;
            txtAbstract.Font = new Font("Segoe UI", 11F);
            txtAbstract.ScrollBars = ScrollBars.Vertical;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblAuthor);
            this.Controls.Add(txtAbstract);

            int startX = 50;
            var btnViewProfile = UIHelpers.CreateButton(Messages.Get("gui.btn.view_profile"));
            btnViewProfile.Location = new Point(startX, 390);
            btnViewProfile.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new ProfileView(currentPaper.Author));
            this.Controls.Add(btnViewProfile);
            startX += 220;

            if (PaperService.CanEdit(currentPaper))
            {
                var btnEdit = UIHelpers.CreateButton(Messages.Get("gui.btn.edit_research"));
                btnEdit.Location = new Point(startX, 390);
                btnEdit.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new EditResearchView(currentPaper));
                this.Controls.Add(btnEdit);
                startX += 220;

                var btnDelete = UIHelpers.CreateButton(Messages.Get("gui.btn.delete_research"));
                btnDelete.BackColor = Color.IndianRed;
                btnDelete.Location = new Point(startX, 390);
                btnDelete.Click += (s, e) => 
                {
                    if (MessageBox.Show(Messages.Get("gui.msg.confirm_delete"), "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        PaperService.DeletePaper(currentPaper);
                        MessageBox.Show(Messages.Get("confirm.deleted"), "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());
                    }
                };
                this.Controls.Add(btnDelete);
            }

            var btnBack = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.back"));
            btnBack.Location = new Point(50, 460);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new AllResearchView());
            this.Controls.Add(btnBack);
        }
    }
}
