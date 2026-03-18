using System.Configuration;
using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace HotelManagement.GUI
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // Định nghĩa vùng vẽ và hai màu bắt đầu/kết thúc
            Rectangle rect = panel1.ClientRectangle;
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Blue, Color.Black, 45F))
            {
                g.FillRectangle(brush, rect);
            }
        }
        HotelManagementEntities db = new HotelManagementEntities();
        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPass.Text;

            var user = db.Accounts
        .FirstOrDefault(a => a.Username == username && a.Password == password);

            if (user != null)
            {
                if (chkRemember.Checked)
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    config.AppSettings.Settings["username"].Value = username;
                    config.AppSettings.Settings["password"].Value = password;

                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    MessageBox.Show("Login success");
                    this.Hide();
                    this.Close();
                }
                else
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    config.AppSettings.Settings["username"].Value = "";
                    config.AppSettings.Settings["password"].Value = "";

                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                   
                }
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Text = ConfigurationManager.AppSettings["username"];
            txtPass.Text = ConfigurationManager.AppSettings["password"];

            if (txtUsername.Text != "")
                chkRemember.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btDangKi_Click(object sender, EventArgs e)
        {
            FormRegister register = new FormRegister();
            register.Show();
            this.Hide();
        }
    }
}
