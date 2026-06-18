using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.models;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class AllResearchView : UserControl
    {
        private ListBox lstResearch;

        public AllResearchView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel("ALL RESEARCH", 20, true);
            lblTitle.Location = new Point(50, 50);

            lstResearch = new ListBox();
            lstResearch.Location = new Point(50, 100);
            lstResearch.Size = new Size(700, 200);
            lstResearch.Font = new Font("Segoe UI", 12F);
            lstResearch.BackColor = UIHelpers.SecondaryColor;
            lstResearch.ForeColor = UIHelpers.TextColor;

            var btnOpen = UIHelpers.CreateButton("Open Selected Research");
            btnOpen.Location = new Point(50, 320);
            btnOpen.Click += BtnOpen_Click;

            var btnBack = UIHelpers.CreateSecondaryButton("Back");
            btnBack.Location = new Point(300, 320);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new HomeView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lstResearch);
            this.Controls.Add(btnOpen);
            this.Controls.Add(btnBack);
        }

        private void LoadData()
        {
            lstResearch.Items.Clear();
            foreach (Paper paper in Lib.services.DataContext.Papers.GetAll())
            {
                lstResearch.Items.Add(paper.Title);
            }
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if (lstResearch.SelectedItem != null)
            {
                string title = lstResearch.SelectedItem.ToString();
                Paper? paper = PaperService.SearchPaper(title);
                if (paper != null)
                {
                    (this.ParentForm as Form1)?.NavigateTo(new ResearchDetailView(paper));
                }
            }
            else
            {
                MessageBox.Show("Please select a research paper.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
