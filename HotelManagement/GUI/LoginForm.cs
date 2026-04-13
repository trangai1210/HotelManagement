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

        HotelManagementEntities db = new HotelManagementEntities();
        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPass.Text;

            var user = db.EmployeeAccounts
        .FirstOrDefault(a => a.Username == username && a.Password == password);

            if (user != null)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (chkRemember.Checked)
                {

                    config.AppSettings.Settings["username"].Value = username;
                    config.AppSettings.Settings["password"].Value = password;

                }
                else
                {

                    config.AppSettings.Settings["username"].Value = "";
                    config.AppSettings.Settings["password"].Value = "";

                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                // Lấy quyền từ user. (Lưu ý: Nếu bảng của bạn cột quyền tên khác như Role, ChucVu... thì sửa lại chữ QuyenHan nhé)
                string quyenCuaNguoiNay = user.QuyenHan;

                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Khởi tạo MainForm và "bơm" cái quyền vào trong ngoặc tròn
                 MainForm frmMain = new MainForm(quyenCuaNguoiNay, username);
                //MainForm frmMain = new MainForm(acc.QuyenHan, user);

                this.Hide();            // Tạm giấu màn hình đăng nhập đi
                frmMain.ShowDialog();   // Hiện MainForm lên (Lúc này MainForm sẽ tự động giấu nút đi nhờ code bài trước)
                this.Close();           // Chỉ khi nào tắt
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

       
    }
}
