using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class EditResearchView : UserControl
    {
        private Paper currentPaper;
        private TextBox txtTitle;
        private TextBox txtAbstract;

        public EditResearchView(Paper paper)
        {
            currentPaper = paper;
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblHeader = UIHelpers.CreateLabel(Messages.Get("gui.title.edit_research"), 20, true);
            lblHeader.Location = new Point(50, 30);

            var lblTitle = UIHelpers.CreateLabel(Messages.Get("gui.label.title"), 12);
            lblTitle.Location = new Point(50, 80);

            txtTitle = UIHelpers.CreateTextBox();
            txtTitle.Text = currentPaper.Title;
            txtTitle.Location = new Point(50, 110);
            txtTitle.Size = new Size(900, 30);

            var lblAbstract = UIHelpers.CreateLabel(Messages.Get("gui.label.abstract"), 12);
            lblAbstract.Location = new Point(50, 150);

            txtAbstract = new TextBox();
            txtAbstract.Multiline = true;
            txtAbstract.Text = currentPaper.Paper_Abstract;
            txtAbstract.Location = new Point(50, 180);
            txtAbstract.Size = new Size(900, 200);
            txtAbstract.BackColor = UIHelpers.SecondaryColor;
            txtAbstract.ForeColor = UIHelpers.TextColor;
            txtAbstract.Font = new Font("Segoe UI", 11F);

            var btnSave = UIHelpers.CreateButton(Messages.Get("gui.btn.save"));
            btnSave.Location = new Point(50, 410);
            btnSave.Click += BtnSave_Click;

            var btnCancel = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.cancel"));
            btnCancel.Location = new Point(270, 410);
            btnCancel.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new ResearchDetailView(currentPaper));

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
            if (PaperService.EditTitle(currentPaper, txtTitle.Text) &&
                PaperService.EditAbstract(currentPaper, txtAbstract.Text))
            {
                MessageBox.Show(Messages.Get("gui.msg.research_updated"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                (this.ParentForm as Form1)?.NavigateTo(new ResearchDetailView(currentPaper));
            }
            else
            {
                MessageBox.Show(Messages.Get("permission.denied"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
