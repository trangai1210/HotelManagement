using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void btQuayLai_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void btXacNhan_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPass.Text; 
            string confirm = txtConfirm.Text;   

            if(username ==""||password=="")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            if(password!=confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không đúng");
                return;
            }
            HotelManagementEntities db = new HotelManagementEntities();

            var user = db.Accounts.FirstOrDefault(u=>u.Username==username);
            if(user!=null)
            {
                MessageBox.Show("Tài khoản đã tồn tại");
                return;
            }

            Account newAccount = new Account();

            newAccount.Username = username;
            newAccount.Password = password;
            newAccount.Role = "Staff";

            db.Accounts.Add(newAccount);
            db.SaveChanges();

            MessageBox.Show("Đăng ký thành công");
        }
    }
}
