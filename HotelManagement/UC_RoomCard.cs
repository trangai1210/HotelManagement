using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagement
{
    public partial class UC_RoomCard : UserControl
    {
        public string MaPhong { get; set; }
        public string TrangThai { get; set; }
        public string TenKhachHang { get; set; }
        public string ThoiGian { get; set; }

        // [MỚI] Thêm thuộc tính lưu mốc thời gian khách vào
        public DateTime? ThoiGianNhanPhong { get; set; }
        public DateTime? ThoiGianTraPhong { get; set; } // Giờ Out dự kiến
        public string LoaiHinhThue { get; set; }        // Để biết là Giờ hay Ngày

        // [MỚI] Bộ đếm thời gian tự động
        private Timer timerDemGio;

        public UC_RoomCard()
        {
            InitializeComponent();
            DangKySuKienClick(this);

            // Cài đặt đồng hồ chạy ngầm (nhảy số sau mỗi 60 giây)
            timerDemGio = new Timer();
            timerDemGio.Interval = 60000;
            timerDemGio.Tick += Timer1_Tick;
        }

        // Hàm này tự động chạy mỗi phút để cập nhật chữ trên Label
       
            private void Timer1_Tick(object sender, EventArgs e)
            {
                if (this.TrangThai == "Đang thuê" && ThoiGianNhanPhong.HasValue)
                {
                    TimeSpan thoiGianO = DateTime.Now - ThoiGianNhanPhong.Value;
                    string daO = $"Đã ở: {(int)thoiGianO.TotalHours}h {thoiGianO.Minutes}p";

                    // Nếu có giờ Check-out, hiện kèm lên để Lễ tân canh giờ đuổi khách / phụ thu
                    if (ThoiGianTraPhong.HasValue)
                    {
                        lblThoiGian.Text = $"{daO} | Out: {ThoiGianTraPhong.Value.ToString("HH:mm")}";
                    }
                    else
                    {
                        lblThoiGian.Text = daO;
                    }
                }
            }
        

        private void DangKySuKienClick(Control ctrl)
        {
            if (ctrl != this)
            {
                ctrl.Click += (sender, e) => this.OnClick(e);
            }

            foreach (Control child in ctrl.Controls)
            {
                DangKySuKienClick(child);
            }
        }

        public void CapNhatGiaoDien()
        {
            // Gán chữ vào các Label
            lblMaPhong.Text = this.MaPhong;
            lblTrangThai.Text = this.TrangThai;

            if (this.TrangThai == "Đang thuê" || this.TrangThai == "Đã đặt")
            {
                lblTenKhach.Text = this.TenKhachHang.ToUpper();
            }
            else if (this.TrangThai == "Đang dọn")
            {
                lblTenKhach.Text = "ĐANG DỌN";
            }
            else
            {
                lblTenKhach.Text = "PHÒNG TRỐNG";
            }

            // Xử lý hiện thời gian & Kích hoạt đồng hồ
            // Xử lý hiện thời gian & Kích hoạt đồng hồ
            if (this.TrangThai == "Đang thuê")
            {
                if (ThoiGianNhanPhong.HasValue)
                {
                    TimeSpan thoiGianO = DateTime.Now - ThoiGianNhanPhong.Value;
                    lblThoiGian.Text = $"{(int)thoiGianO.TotalHours} giờ {thoiGianO.Minutes} phút";
                    timerDemGio.Start();
                }
                else
                {
                    // [MỚI] Báo chữ lên thẻ phòng nếu Database bị thiếu giờ
                    lblThoiGian.Text = "Lỗi: Thiếu giờ Check-in";
                    timerDemGio.Stop();
                }
            }
            else
            {
                lblThoiGian.Text = this.ThoiGian;
                timerDemGio.Stop();
            }

            // Xử lý đổi màu nền 
            if (this.TrangThai == "Trống")
            {
                pnlBackground.BackColor = Color.FromArgb(46, 204, 113); // Màu xanh lá
            }
            else if (this.TrangThai == "Đang thuê")
            {
                pnlBackground.BackColor = Color.FromArgb(231, 76, 60); // Màu đỏ
            }
            else if (this.TrangThai == "Đã đặt")
            {
                pnlBackground.BackColor = Color.FromArgb(127, 140, 141); // Màu xám chuột
            }
            else if (this.TrangThai == "Đang dọn")
            {
                pnlBackground.BackColor = Color.Orange; // Màu vàng cam
            }
        }

      
       
    }
}