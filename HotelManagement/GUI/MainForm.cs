using HotelManagement.Models;
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



namespace HotelManagement.GUI
{
    public partial class MainForm : Form
    {
        //Biến toàn cục lưu trữ quyền của người vưaf đăng nhập
        private string quyenHanHienTai = "";
        private string usernameHienTai = "";


        bool menuOpen = false;

        int index = 0;
        Button currentButton = null;

        Image[] images =
        {
            Properties.Resources.quanlyks,
            Properties.Resources.hinhnhanvien,
            Properties.Resources.sanh,
            Properties.Resources.phongngu,
            Properties.Resources.hoboi
        };

        public MainForm(string quyenHanHienTai, string usernameHienTai)
        {
            InitializeComponent();
            this.quyenHanHienTai = quyenHanHienTai;
            this.usernameHienTai = usernameHienTai;
        }

        private void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(8, 30, 50);
            btn.ForeColor = Color.Gold;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Height = 45;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);

            // Hover
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(0, 100, 180);
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(8, 30, 50);
            };
        }

        // Khi form load
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (quyenHanHienTai == "Lễ tân") // (Lưu ý: Chữ này phải khớp 100% với chữ bạn lưu trong SQL)
            {
                // 1. Giấu nút Quản lý Nhân sự / Phân quyền
                btQLNhanVien.Visible = false; // Nhớ đổi tên btn này thành tên thật của bạn nhé

                // 2. Giấu nút Xem báo cáo Doanh thu
                btReport.Visible = false;

                
            }
            else if (quyenHanHienTai == "Quản lý" || quyenHanHienTai == "Admin")
            {
                // Là Quản lý thì không bị giấu gì cả, full quyền!
            }

            panelContent.AutoScroll = true; // bật scroll
            // Ẩn menu ban đầu
            panel1.Visible = false;

            // Cấu hình slide
            pictureSlide.Image = images[0];
            pictureSlide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureSlide.Dock = DockStyle.Fill;
            pictureSlide.Visible = true;
            //pictureSlide.BringToFront();

            // Timer chạy slide
            slideTimer.Interval = 2000; // 2 giây
            slideTimer.Start();

            panelContent.Visible = false;
            pictureSlide.Visible = true;

            //pictureSlide.BringToFront();
            panelTop.BringToFront();

            // ====== GIAO DIỆN TỔNG ======
            this.BackColor = Color.FromArgb(10, 25, 40); // nền tối

            panelTop.BackColor = Color.FromArgb(5, 20, 35); // thanh trên
            panel1.BackColor = Color.FromArgb(8, 30, 50);   // sidebar
            panelContent.BackColor = Color.FromArgb(220, 230, 240); // nền form

            this.Font = new Font("Segoe UI", 10F);
            //this.ForeColor = Color.Gold;

            StyleButton(btRoom);
            StyleButton(btBooking);
            StyleButton(btCustomer);
            StyleButton(btDichVu);
            StyleButton(btInvoice);
            StyleButton(btReport);
            StyleButton(btQLNhanVien);

            //CHỈNH SIDEBAR (panel1)
            panel1.Width = 200;
            panel1.Padding = new Padding(0, 10, 0, 0);

            panelContent.Padding = new Padding(0);

            Panel overlay = new Panel();
            overlay.BackColor = Color.FromArgb(120, 0, 20, 40);
            overlay.Dock = DockStyle.Fill;

            pictureSlide.Controls.Add(overlay);
            overlay.SendToBack();

            //tiêu đề menutop
            Label title = new Label();
            title.Text = "          HOTEL MANAGEMENT";
            title.ForeColor = Color.Gold;
            title.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(60, 15);

            panelTop.Controls.Add(title);

            // 
            //panelTop.Dock = DockStyle.Top;
            //panel1.Dock = DockStyle.Left;
            //panelContent.Dock = DockStyle.Fill;

            // ===== FIX KHOẢNG CÁCH BUTTON =====
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.Height = 45; // chỉnh chiều cao
                    btn.Dock = DockStyle.Top; // xếp dọc
                    btn.Margin = new Padding(0); // bỏ khoảng cách dư
                    btn.Padding = new Padding(10, 0, 0, 0); // cách chữ bên trái
                }
            }

            // Cho phép scroll nếu nhiều nút
            panel1.AutoScroll = true;

            panelTop.Dock = DockStyle.Top;
            panel1.Dock = DockStyle.Left;
            panelContent.Dock = DockStyle.Fill;
            pictureSlide.Dock = DockStyle.Fill;


            //panelContent.Dock = DockStyle.Fill;
            //panelContent.Padding = new Padding(0); // ❗ bỏ lệch

            //panelTop.Dock = DockStyle.Top;     // thanh trên
            //panel1.Dock = DockStyle.Left;      // menu trái
            //panelContent.Dock = DockStyle.Fill; // phần còn lại

            // ===== LAYOUT CHUẨN (CHỈ GIỮ 1 LẦN) =====
            panelTop.Dock = DockStyle.Top;
            panel1.Dock = DockStyle.Left;
            panelContent.Dock = DockStyle.Fill;
            pictureSlide.Dock = DockStyle.Fill; 

            // ===== FIX PADDING (tránh lệch / tràn) =====
            panelContent.Padding = new Padding(0);
            panel1.Padding = new Padding(0);

            panelTop.BringToFront();
            panel1.BringToFront();
            LoadCaLamViec();

        }

        // Slide hình
        private void slideTimer_Tick(object sender, EventArgs e)
        {
            
                index++;

                if (index >= images.Length)
                    index = 0;

               pictureSlide.Image = images[index];
            
        }
        private void SetActiveButton(Button btn)
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(8, 30, 50);
            }

            currentButton = btn;
            currentButton.BackColor = Color.FromArgb(0, 120, 215);
        }
        // mở form con
        private void OpenChildForm(Form childForm)
        {
            panelContent.Visible = true;

            //Ẩn slide hoàn toàn
            pictureSlide.Visible = false;

            panelContent.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;

            // FIX LỆCH + TRÀN
            childForm.Dock = DockStyle.Fill;
            childForm.Margin = new Padding(0);
            childForm.Padding = new Padding(0);
            childForm.Location = new Point(0, 0);

            panelContent.Controls.Add(childForm);

            // đưa panelContent lên trên
            panelContent.BringToFront();
            childForm.AutoScroll = true; // cho phép scroll bên trong form con
            childForm.Show();
        }

        // nút phòng
        private void btRoom_Click(object sender, EventArgs e)
        {
            SetActiveButton(btRoom);
            OpenChildForm(new RoomForm());
        }

        // nút khách hàng
        private void btCustomer_Click(object sender, EventArgs e)
        {
            SetActiveButton(btCustomer);
            OpenChildForm(new CustomerForm());
        }
        // nút dịch vụ
        private void btDichVu_Click(object sender, EventArgs e)
        {
            SetActiveButton(btCustomer);
            OpenChildForm(new ServiceForm());
        }

        // nút ☰
        
        private async void btnMenu_Click(object sender, EventArgs e)
        {
            if (!menuOpen)
            {
                panel1.Visible = true;

                for (int i = 0; i <= 200; i += 20)
                {
                    panel1.Width = i;
                    await Task.Delay(10);
                }

                pictureSlide.Visible = false;
                menuOpen = true;
            }
            else
            {
                for (int i = panel1.Width; i >= 0; i -= 20)
                {
                    panel1.Width = i;
                    await Task.Delay(10);
                }

                panel1.Visible = false;

                panelContent.Controls.Clear();
                panelContent.Visible = false;

                pictureSlide.Visible = true;
                //pictureSlide.BringToFront();

                menuOpen = false;
            }
        }

        private void btQLNhanVien_Click(object sender, EventArgs e)
        {
           
            OpenChildForm(new QLNhanVienForm());
        }



        //=======CHẤM CÔNG ========
        private string LayMaNhanVienHienTai()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var acc = db.EmployeeAccounts.FirstOrDefault(a => a.Username == this.usernameHienTai);
                return acc != null ? acc.MaNV : "";
            }
        }

        private void btCheckin_Click(object sender, EventArgs e)
        {
            string maNV = LayMaNhanVienHienTai();
            if (maNV == "") return;
            //// Bắt lỗi nếu quên chưa chọn Ca
            if (cbbCaLamViec.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Ca làm việc trước khi Check-in!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                DateTime homNay = DateTime.Now.Date;
               
                // kiểm tra hôm nay đã checkin chưa
                bool daVao = db.ChamCongs.Any(cc => cc.MaNV == maNV && cc.NgayCC == homNay);
                if (daVao)
                {
                    MessageBox.Show("Bạn đã Check-in hôm nay rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ChamCong ccMoi = new ChamCong();
                // LẤY MÃ CA TỪ COMBOBOX
                // Sửa dòng 345 thành như thế này:
                int maCaDuocChon = (int)cbbCaLamViec.SelectedValue; ccMoi.MaNV = maNV;
                ccMoi.NgayCC = homNay;
                ccMoi.GioVao = DateTime.Now;
                ccMoi.TrangThai = "Đang làm việc";
                ccMoi.MaCa = maCaDuocChon;

                db.ChamCongs.Add(ccMoi);
                db.SaveChanges();

                MessageBox.Show($"Check-in thành công lúc {DateTime.Now:HH:mm}!", "Báo danh");
            }
        }

        private void btCheckout_Click(object sender, EventArgs e)
        {
            string maNV = LayMaNhanVienHienTai();
            if (maNV == "") return;

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                DateTime homNay = DateTime.Now.Date;

                // Tìm bản ghi Check-in sáng nay mà chưa có giờ về (GioRa == null)
                var caHienTai = db.ChamCongs.FirstOrDefault(cc => cc.MaNV == maNV && cc.NgayCC == homNay && cc.GioRa == null);

                if (caHienTai != null)
                {
                    caHienTai.GioRa = DateTime.Now; // Chốt giờ về
                    caHienTai.TrangThai = "Hoàn thành";
                    db.SaveChanges();

                    MessageBox.Show($"Check-out thành công lúc {DateTime.Now:HH:mm}! Nghỉ ngơi thôi.", "Báo danh");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu Check-in của bạn hoặc bạn đã Check-out rồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LoadCaLamViec()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                // Lấy danh sách ca từ CSDL
                var dsCa = db.CaLamViecs.Select(c => new
                {
                    MaCa = c.MaCa,
                    TenCa = c.TenCa
                }).ToList();

                // Đổ lên ComboBox
                cbbCaLamViec.DataSource = dsCa;
                cbbCaLamViec.DisplayMember = "TenCa"; // Hiện chữ (Ca Sáng, Ca Chiều)
                cbbCaLamViec.ValueMember = "MaCa";    // Giấu số (1, 2, 3) ở dưới

                cbbCaLamViec.SelectedIndex = -1;
            }
        }
    }
}

