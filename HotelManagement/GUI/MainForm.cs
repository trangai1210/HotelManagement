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

        Image[] images =
        {
            Properties.Resources.quanlyks,
            Properties.Resources.hinhnhanvien,
            Properties.Resources.sanh,
            Properties.Resources.phongngu,
            Properties.Resources.hoboi
        };

        public MainForm()
        {
            InitializeComponent();
        }

        // Khi form load
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Ẩn menu ban đầu
            panel1.Visible = false;

            // Cấu hình slide
            pictureSlide.Image = images[0];
            pictureSlide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureSlide.Dock = DockStyle.Fill;
            pictureSlide.Visible = true;
            pictureSlide.BringToFront();

            // Timer chạy slide
            slideTimer.Interval = 2000; // 2 giây
            slideTimer.Start();

            panelContent.Visible = false;
            pictureSlide.Visible = true;

            pictureSlide.BringToFront();
            panelTop.BringToFront();

        }

        // Slide hình
        private void slideTimer_Tick(object sender, EventArgs e)
        {
            
                index++;

                if (index >= images.Length)
                    index = 0;

               pictureSlide.Image = images[index];
            
        }

        // mở form con
        private void OpenChildForm(Form childForm)
        {
            ////panelContent.Controls.Clear();

            ////childForm.TopLevel = false;
            ////childForm.FormBorderStyle = FormBorderStyle.None;
            ////childForm.Dock = DockStyle.Fill;

            ////panelContent.Controls.Add(childForm);
            ////childForm.Show();

            //// Ẩn slide khi mở form
            //pictureSlide.Visible = false;

            //panelContent.Controls.Clear();

            //childForm.TopLevel = false;
            //childForm.FormBorderStyle = FormBorderStyle.None;
            //childForm.Dock = DockStyle.Fill;

            //panelContent.Controls.Add(childForm);
            //childForm.Show();


            //  HIỆN LẠI PANEL
            panelContent.Visible = true;

            // Ẩn slide
            pictureSlide.Visible = false;

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
            if (!menuOpen) // mở menu
            {
                panel1.Visible = true;        // hiện menu
                pictureSlide.Visible = false; // ẩn slide

                menuOpen = true;
            }
            else // đóng menu
            {
                panel1.Visible = false; // ẩn menu

                //  xóa form đang mở
                panelContent.Controls.Clear();

                // Ẩn luôn panel chứa form
                panelContent.Visible = false;

                // Hiện lại slide
                pictureSlide.Visible = true;
                pictureSlide.BringToFront();

                menuOpen = false;
            }

        }

        private void btDichVu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ServiceForm());
        }
    }
}

