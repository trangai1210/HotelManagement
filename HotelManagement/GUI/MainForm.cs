using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//namespace HotelManagement.GUI
//{
//    public partial class MainForm : Form
//    {
//        public MainForm()
//        {
//            InitializeComponent();
//        }
//        bool menuOpen = false; //đóng mở menu
//                               // nút ba gạch

//        int index = 0;



//        string path = Application.StartupPath + @"\Resources\";

//        string[] images =
//        {
//         Application.StartupPath + @"\Resources\quanlyks.jpg",
//        Application.StartupPath + @"\Resources\hinhnhanvien.jpg",
//        Application.StartupPath + @"\Resources\sanh.jpg",
//        Application.StartupPath + @"\Resources\phongngu.jpg",
//        Application.StartupPath + @"\Resources\hoboi.jpg"

//        };
//        //chạy hình
//        private void MainForm_Load(object sender, EventArgs e)
//        {
//            panel1.Location = new Point(-220, 50);
//            pictureSlide.Image = Image.FromFile(images[0]);
//            slideTimer.Start();
//        }
//        private void slideTimer_Tick(object sender, EventArgs e)
//        {
//            index++;

//            if (index >= images.Length)
//                index = 0;

//            pictureSlide.Image = Image.FromFile(images[index]);
//        }

//        private void OpenChildForm(Form childForm)
//        {
//            panelContent.Controls.Clear();

//            childForm.TopLevel = false;
//            childForm.FormBorderStyle = FormBorderStyle.None;
//            childForm.Dock = DockStyle.Fill;

//            panelContent.Controls.Add(childForm);
//            childForm.Show();
//        }

//        private void btRoom_Click(object sender, EventArgs e)
//        {
//            OpenChildForm(new RoomForm());
//        }

//        private void btCustomer_Click(object sender, EventArgs e)
//        {
//            OpenChildForm(new CustomerForm());
//        }

//        private void btnMenu_Click(object sender, EventArgs e)
//        {
//            if (menuOpen == false)
//            {
//                panel1.Location = new Point(0, 50);
//                menuOpen = true;
//            }
//            else
//            {
//                panel1.Location = new Point(-220, 50);
//                menuOpen = false;
//            }
//        }
//    }
//}

namespace HotelManagement.GUI
{
    public partial class MainForm : Form
    {
        bool menuOpen = false;

        int index = 0;

        string[] images =
{
    @"D:\quanlyks.jpg",
    @"D:\hinhnhanvien.jpg",
    @"D:\sanh.jpg",
    @"D:\phongngu.jpg",
    @"D:\hoboi.jpg"
};

        public MainForm()
        {
            InitializeComponent();
        }

        // Khi form load
        private void MainForm_Load(object sender, EventArgs e)
        {
            

            //panel1.Left = -panel1.Width;

            pictureSlide.Image = Image.FromFile(images[0]);

            slideTimer.Start();

            panel1.Visible = false;
            
        }

        // Slide hình
        private void slideTimer_Tick(object sender, EventArgs e)
        {

            try
            {
                index++;

                if (index >= images.Length)
                    index = 0;

                pictureSlide.Image = Image.FromFile(images[index]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // mở form con
        private void OpenChildForm(Form childForm)
        {
            panelContent.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelContent.Controls.Add(childForm);
            childForm.Show();
        }

        // nút phòng
        private void btRoom_Click(object sender, EventArgs e)
        {
            OpenChildForm(new RoomForm());
        }

        // nút khách hàng
        private void btCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CustomerForm());
        }

        // nút ☰
        private void btnMenu_Click(object sender, EventArgs e)
        {
            //if (!menuOpen)
            //{
            //    panel1.Left = 0;
            //    menuOpen = true;
            //}
            //else
            //{
            //    panel1.Left = -panel1.Width;
            //    menuOpen = false;
            //}
            panel1.Visible = !panel1.Visible;

            // TRƯỜNG HỢP 1: Đang hiển thị Form (chuẩn bị hiện ảnh)
            if (pictureSlide.Visible == false)
            {
                // Ẩn nội dung cũ
                panelContent.Visible = false;
                panel1.Visible = false; // Ẩn luôn thanh menu bên trái

                // Cấu hình và hiện PictureBox
                pictureSlide.Dock = DockStyle.Fill;
                pictureSlide.Visible = true;
                pictureSlide.BringToFront(); // Đưa ảnh lên trên cùng để chắc chắn nhìn thấy
            }
            // TRƯỜNG HỢP 2: Đang hiển thị ảnh (nhấn lần nữa để quay lại)
            else
            {
                // Ẩn ảnh đi
                pictureSlide.Visible = false;

                // Hiện lại các panel cũ
                panel1.Visible = true;
                panelContent.Visible = true;

                // Đưa nội dung về lại phía trước
                panel1.BringToFront();
                panelContent.BringToFront();
            }
        }
    }
}

