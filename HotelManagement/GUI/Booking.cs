using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class Booking : Form
    {
        public Booking()
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered",
        System.Reflection.BindingFlags.SetProperty |
        System.Reflection.BindingFlags.Instance |
        System.Reflection.BindingFlags.NonPublic,
        null, flpSoDoPhong, new object[] { true });
            this.Resize += new System.EventHandler(this.Booking_Resize);

        }

        private void Booking_Resize(object sender, EventArgs e)
        {
            if (flpSoDoPhong == null) return;

            flpSoDoPhong.SuspendLayout(); // Tạm dừng vẽ để tránh chớp

            int chieuRongThucTe = flpSoDoPhong.Width - 35; // Trừ hao thanh cuộn
            if (chieuRongThucTe > 0)
            {
                // Đi qua từng dòng Tiêu đề và Panel đang có sẵn trên màn hình
                foreach (Control ctrl in flpSoDoPhong.Controls)
                {
                    if (ctrl is Label)
                    {
                        ctrl.Width = chieuRongThucTe;
                    }
                    else if (ctrl is FlowLayoutPanel flpCon)
                    {
                        // CỰC KỲ QUAN TRỌNG: Phải mở khóa MaximumSize và MinimumSize 
                        // thì các thẻ phòng mới chịu tràn ra lấp đầy khoảng trống bên phải!
                        flpCon.MaximumSize = new Size(chieuRongThucTe, 0);
                        flpCon.MinimumSize = new Size(chieuRongThucTe, 0);
                        flpCon.Width = chieuRongThucTe;
                    }
                }
            }

            flpSoDoPhong.ResumeLayout(); // Bật lại vẽ giao diện
        }
        private void Booking_Load(object sender, EventArgs e)
        {
            LoadSoDoPhong();
        }

        // ======================================================================
        // HÀM CHÍNH: TẢI VÀ VẼ SƠ ĐỒ PHÒNG
        // ======================================================================
        private void LoadSoDoPhong()
        {
            // Tạm dừng vẽ giao diện để tránh giật lag màn hình khi load nhiều thẻ
            flpSoDoPhong.SuspendLayout();
            flpSoDoPhong.Controls.Clear();

            // Cài đặt chiều cơ bản cho Panel chính
            flpSoDoPhong.FlowDirection = FlowDirection.TopDown;
            flpSoDoPhong.WrapContents = false;
            flpSoDoPhong.AutoScroll = true;

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                DateTime bayGio = DateTime.Now;

                // 1. TÌM CÁC PHÒNG ĐANG CÓ KHÁCH Ở (Trạng thái hóa đơn: Đang Thuê)
                var danhSachPhongDangThue = db.BookingDetails
                    .Where(bd => bd.Booking.Status == "Đang Thuê")
                    .Select(bd => bd.RoomID.ToString())
                    .ToList();

                // 2. TÌM CÁC PHÒNG ĐÃ CÓ KHÁCH ĐẶT TRƯỚC (Cho hôm nay hoặc tương lai)
                // Lấy những đơn chưa trả phòng, và thời gian ra phải lớn hơn hiện tại
                var cacDonDaDat = db.BookingDetails
                    .Where(bd => (bd.Booking.Status == "Đã đặt" || bd.Booking.Status == "Chờ duyệt")
                              && bd.Booking.CheckOutDate > bayGio)
                    .ToList();

                // 3. LẤY DANH SÁCH LOẠI PHÒNG ĐỂ NHÓM (Phòng Đơn, Phòng Đôi...)
                var danhSachLoaiPhong = db.RoomTypes.ToList();

                foreach (var loaiPhong in danhSachLoaiPhong)
                {
                    // --- A. VẼ TIÊU ĐỀ LOẠI PHÒNG ---
                    Label lblHeader = new Label();
                    lblHeader.Text = string.IsNullOrEmpty(loaiPhong.TypeName) ? "Chưa phân loại" : loaiPhong.TypeName;
                    lblHeader.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                    lblHeader.ForeColor = Color.DeepSkyBlue;
                    lblHeader.AutoSize = false;

                    int chieuRongLabel = flpSoDoPhong.Width - 30;
                    lblHeader.Width = chieuRongLabel > 0 ? chieuRongLabel : 800;
                    lblHeader.Height = 40;
                    lblHeader.TextAlign = ContentAlignment.BottomLeft;
                    lblHeader.Margin = new Padding(10, 20, 0, 5);
                    flpSoDoPhong.Controls.Add(lblHeader);

                    // --- B. VẼ PANEL CHỨA THẺ PHÒNG CỦA LOẠI ĐÓ ---
                    FlowLayoutPanel flpCon = new FlowLayoutPanel();
                    flpCon.FlowDirection = FlowDirection.LeftToRight;
                    flpCon.WrapContents = true;
                    flpCon.AutoSize = true;

                    int scrollBarWidth = 35;
                    int chieuRongThucTe = flpSoDoPhong.Width - scrollBarWidth;
                    if (chieuRongThucTe > 0)
                    {
                        flpCon.Width = chieuRongThucTe;
                        flpCon.MinimumSize = new Size(chieuRongThucTe, 0);
                        flpCon.MaximumSize = new Size(chieuRongThucTe, 0);
                    }

                    // --- C. XỬ LÝ TỪNG PHÒNG TRONG LOẠI NÀY ---
                    var danhSachPhongTheoLoai = db.Rooms.Where(r => r.RoomTypeID == loaiPhong.RoomTypeID).ToList();

                    foreach (var phong in danhSachPhongTheoLoai)
                    {
                        UC_RoomCard thePhong = new UC_RoomCard();
                        thePhong.MaPhong = phong.RoomName;
                        thePhong.ThoiGian = "";
                        thePhong.Margin = new Padding(10);
                        string trangThai = "Trống"; // Mặc định ban đầu

                        // -------------------------------------------------------------
                        // THUẬT TOÁN XẾP HẠNG ƯU TIÊN MÀU SẮC (ĐỎ -> XÁM -> VÀNG -> XANH)
                        // -------------------------------------------------------------

                        // ƯU TIÊN 1: Phòng đang có khách ở (MÀU ĐỎ)
                        // ƯU TIÊN 1: Phòng đang có khách ở (MÀU ĐỎ)
                        if (danhSachPhongDangThue.Contains(phong.RoomID.ToString()))
                        {
                            trangThai = "Đang thuê";
                            var phieuThue = db.BookingDetails.FirstOrDefault(bd => bd.RoomID == phong.RoomID && bd.Booking.Status == "Đang Thuê");

                            if (phieuThue != null && phieuThue.Booking.Customer != null)
                            {
                                thePhong.TenKhachHang = phieuThue.Booking.Customer.TenKH;

                                // [SỬA Ở ĐÂY] Thay vì nối chuỗi tĩnh, truyền thẳng mốc CheckIn vào cho thẻ tự đếm
                                if (phieuThue.Booking.CheckInDate.HasValue)
                                {
                                    thePhong.ThoiGianNhanPhong = phieuThue.Booking.CheckInDate.Value;
                                }

                            }
                            else { thePhong.TenKhachHang = "Khách vãng lai"; }
                        }
                        // ƯU TIÊN 2: Phòng chưa ai ở, nhưng dơ đang dọn dẹp (MÀU XÁM)
                        else if (phong.Status == "Đang dọn")
                        {
                            trangThai = "Đang dọn";
                            thePhong.TenKhachHang = "Đang dọn dẹp...";
                            thePhong.ThoiGian = "Cấm bán";
                        }
                        // ƯU TIÊN 3: Phòng sạch, nhưng đang có người chờ tới lấy (MÀU VÀNG)
                        else
                        {
                            // Lọc ra ông khách sắp tới gần với thời điểm hiện tại nhất
                            var khachSapToi = cacDonDaDat
                                .Where(bd => bd.RoomID == phong.RoomID)
                                .OrderBy(bd => bd.Booking.CheckInDate)
                                .FirstOrDefault();

                            if (khachSapToi != null)
                            {
                                trangThai = "Đã đặt";
                                thePhong.TenKhachHang = khachSapToi.Booking.Customer.TenKH;
                                if (khachSapToi.Booking.CheckInDate.HasValue)
                                {
                                    thePhong.ThoiGian = $"Đến lúc: {khachSapToi.Booking.CheckInDate.Value.ToString("HH:mm")}";
                                }
                            }
                            // ƯU TIÊN 4: Đích thị là phòng trống hoàn toàn (MÀU XANH)
                            else
                            {
                                trangThai = "Trống";
                                thePhong.TenKhachHang = "Phòng Sẵn Sàng";
                            }
                        }

                        // Cập nhật lên UI
                        thePhong.TrangThai = trangThai;
                        thePhong.Click += pnlBackground_Click; // Gắn sự kiện Click
                        thePhong.CapNhatGiaoDien();

                        flpCon.Controls.Add(thePhong);
                    }

                    flpSoDoPhong.Controls.Add(flpCon);
                }
            }

            // Mở lại việc vẽ giao diện sau khi đã nạp xong data
            flpSoDoPhong.ResumeLayout();
        }

        // ======================================================================
        // SỰ KIỆN: CLICK VÀO THẺ PHÒNG TRÊN SƠ ĐỒ
        // ======================================================================
        private void pnlBackground_Click(object sender, EventArgs e)
        {
            UC_RoomCard clickedCard = sender as UC_RoomCard;

            if (clickedCard != null)
            {
                string maPhong = clickedCard.MaPhong;
                string trangThai = clickedCard.TrangThai;

                // 1. NHẤN VÀO PHÒNG TRỐNG -> Mở form đặt phòng
                if (trangThai == "Trống")
                {
                    // Lời khuyên: Nếu FrmDatPhong của bạn có hàm tạo nhận tham số mã phòng
                    // thì gọi: FrmDatPhong frm = new FrmDatPhong(maPhong); 
                    FrmDatPhong frm = new FrmDatPhong();
                    frm.ShowDialog();

                    // Khách đặt xong đóng Form -> Load lại sơ đồ
                    LoadSoDoPhong();
                }

                // 2. NHẤN VÀO PHÒNG ĐÃ ĐẶT (MÀU VÀNG) -> Tiến hành Check-in
                else if (trangThai == "Đã đặt")
                {
                    DialogResult rs = MessageBox.Show($"Khách đã đến và muốn Nhận phòng {maPhong} đúng không?", "Xác nhận Check-in", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (rs == DialogResult.Yes)
                    {
                        // Ở ĐÂY BẠN VIẾT CODE UPDATE DATABASE:
                        // Tìm Booking của phòng này có trạng thái "Đã đặt", đổi nó thành "Đang Thuê".
                        // ...
                        MessageBox.Show("Check-in thành công!");
                        LoadSoDoPhong(); // Load lại để thẻ Vàng thành thẻ Đỏ
                    }
                }

                // 3. NHẤN VÀO PHÒNG ĐANG THUÊ (MÀU ĐỎ) -> Thêm dịch vụ hoặc Check-out
                // 3. NHẤN VÀO PHÒNG ĐANG THUÊ (MÀU ĐỎ) -> Thêm dịch vụ hoặc Check-out
                else if (trangThai == "Đang thuê")
                {
                    using (HotelManagementEntities db = new HotelManagementEntities())
                    {
                        // 3.1. Tìm ID của căn phòng thông qua Tên phòng (maPhong)
                        var room = db.Rooms.FirstOrDefault(r => r.RoomName == maPhong);

                        if (room != null)
                        {
                            // 3.2. Dò tìm Đơn đặt phòng (Booking) đang active (Đang Thuê) của căn phòng này
                            var phieuThue = db.BookingDetails
                                              .FirstOrDefault(bd => bd.RoomID == room.RoomID && bd.Booking.Status == "Đang Thuê");

                            if (phieuThue != null)
                            {
                                int bookingIdHienTai = phieuThue.BookingID; // Đây chính là cái INT ta cần!

                                // 3.3. Truyền BookingID sang Form Thêm Dịch Vụ
                                FrmChiTietPhong frm = new FrmChiTietPhong(bookingIdHienTai);
                                frm.ShowDialog();

                                // (Không cần LoadSoDoPhong() ở đây vì thêm dịch vụ không làm đổi màu phòng)
                            }
                            else
                            {
                                MessageBox.Show("Lỗi: Không tìm thấy hóa đơn đang thuê của phòng này!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

                // 4. NHẤN VÀO PHÒNG ĐANG DỌN (MÀU XÁM) -> Cập nhật dọn xong
                else if (trangThai == "Đang dọn")
                {
                    DialogResult rs = MessageBox.Show($"Bộ phận Buồng phòng đã dọn xong phòng {maPhong}?", "Cập nhật dọn phòng", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (rs == DialogResult.Yes)
                    {
                        using (HotelManagementEntities db = new HotelManagementEntities())
                        {
                            var r = db.Rooms.FirstOrDefault(x => x.RoomName == maPhong);
                            if (r != null)
                            {
                                r.Status = "Available"; // Hoặc "Trống" tùy CSDL của bạn
                                db.SaveChanges();
                            }
                        }
                        LoadSoDoPhong(); // Load lại để thẻ Xám thành thẻ Xanh (Sẵn sàng bán)
                    }
                }
            }
        }
    }
}