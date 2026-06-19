using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Utils
{
    public static class UIHelpers
    {
        public static readonly Color BackgroundColor = Color.FromArgb(30, 30, 30);
        public static readonly Color PrimaryColor = Color.FromArgb(0, 122, 204);
        public static readonly Color SecondaryColor = Color.FromArgb(45, 45, 48);
        public static readonly Color TextColor = Color.White;

        public static void StyleForm(Form form)
        {
            form.BackColor = BackgroundColor;
            form.ForeColor = TextColor;
            form.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        }

        public static void StyleControl(Control control)
        {
            control.BackColor = BackgroundColor;
            control.ForeColor = TextColor;
            control.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        }

        public static Button CreateButton(string text)
        {
            var btn = new Button();
            btn.Text = text;
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = PrimaryColor;
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Size = new Size(200, 40);
            
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(28, 151, 234);
            btn.MouseLeave += (s, e) => btn.BackColor = PrimaryColor;
            
            return btn;
        }
        
        public static Button CreateSecondaryButton(string text)
        {
            var btn = CreateButton(text);
            btn.BackColor = SecondaryColor;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(62, 62, 66);
            btn.MouseLeave += (s, e) => btn.BackColor = SecondaryColor;
            return btn;
        }

        public static TextBox CreateTextBox(bool isPassword = false)
        {
            var txt = new TextBox();
            txt.BackColor = SecondaryColor;
            txt.ForeColor = TextColor;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 12F);
            txt.UseSystemPasswordChar = isPassword;
            txt.Size = new Size(250, 30);
            return txt;
        }

        public static Label CreateLabel(string text, int fontSize = 10, bool isBold = false)
        {
            var lbl = new Label();
            lbl.Text = text;
            lbl.ForeColor = TextColor;
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", fontSize, isBold ? FontStyle.Bold : FontStyle.Regular);
            return lbl;
        }
    }
}
