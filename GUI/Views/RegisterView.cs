using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Utils;
using Lib.services;
using Lib.common;

namespace GUI.Views
{
    public class RegisterView : UserControl
    {
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;

        public RegisterView()
        {
            UIHelpers.StyleControl(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var lblTitle = UIHelpers.CreateLabel(Messages.Get("gui.title.register"), 20, true);
            lblTitle.Location = new Point((1000 - 140) / 2, 60);

            int startX = (1000 - 250) / 2;

            var lblName = UIHelpers.CreateLabel(Messages.Get("gui.label.name"));
            lblName.Location = new Point(startX, 130);
            
            txtName = UIHelpers.CreateTextBox();
            txtName.Location = new Point(startX, 155);

            var lblEmail = UIHelpers.CreateLabel(Messages.Get("gui.label.email"));
            lblEmail.Location = new Point(startX, 205);
            
            txtEmail = UIHelpers.CreateTextBox();
            txtEmail.Location = new Point(startX, 230);

            var lblPassword = UIHelpers.CreateLabel(Messages.Get("gui.label.password"));
            lblPassword.Location = new Point(startX, 280);
            
            txtPassword = UIHelpers.CreateTextBox(isPassword: true);
            txtPassword.Location = new Point(startX, 305);

            var lblConfirmPassword = UIHelpers.CreateLabel(Messages.Get("gui.label.confirm_password"));
            lblConfirmPassword.Location = new Point(startX, 355);
            
            txtConfirmPassword = UIHelpers.CreateTextBox(isPassword: true);
            txtConfirmPassword.Location = new Point(startX, 380);

            var btnRegister = UIHelpers.CreateButton(Messages.Get("gui.btn.register"));
            btnRegister.Location = new Point((1000 - btnRegister.Width) / 2, 440);
            btnRegister.Click += BtnRegister_Click;

            var btnBack = UIHelpers.CreateSecondaryButton(Messages.Get("gui.btn.back"));
            btnBack.Location = new Point((1000 - btnBack.Width) / 2, 490);
            btnBack.Click += (s, e) => (this.ParentForm as Form1)?.NavigateTo(new WelcomeView());

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnBack);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(Messages.Get("register.password_mismatch"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AuthService.Register(txtName.Text, txtEmail.Text, txtPassword.Text);
            MessageBox.Show(Messages.Get("register.success"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            (this.ParentForm as Form1)?.NavigateTo(new WelcomeView());
        }
    }
}
