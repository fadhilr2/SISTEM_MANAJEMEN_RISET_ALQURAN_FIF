using System.Windows.Forms;
using GUI.Views;
using GUI.Utils;

namespace GUI
{
    public partial class Form1 : Form
    {
        private Panel mainPanel;

        public Form1()
        {
            InitializeComponent();
            UIHelpers.StyleForm(this);
            this.Text = "AL-QURAN FIF Research Management";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            InitializeMainPanel();
            NavigateTo(new WelcomeView());
        }

        private void InitializeMainPanel()
        {
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            this.Controls.Add(mainPanel);
        }

        public void NavigateTo(UserControl view)
        {
            mainPanel.Controls.Clear();
            view.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(view);
        }
    }
}
