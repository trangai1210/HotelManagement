using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class ttDatPhong : Form
    {
        public ttDatPhong()
        {
            InitializeComponent();
        }

        // ========================================================
        // 1. LOAD FORM VÀ CẤU HÌNH GIAO DIỆN
        // ========================================================
        private void ttDatPhong_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- Cấu hình bảng Đã Chọn ---
            dgvPhongDaChon.Columns.Clear();
            dgvPhongDaChon.Columns.Add("RoomID", "Mã phòng");
            dgvPhongDaChon.Columns.Add("RoomName", "Tên phòng");
            dgvPhongDaChon.Columns.Add("LoaiPhong", "Loại phòng");

            DataGridViewTextBoxColumn colSoNguoi = new DataGridViewTextBoxColumn();
            colSoNguoi.Name = "SoNguoi";
            colSoNguoi.HeaderText = "Số người";
            colSoNguoi.DefaultCellStyle.BackColor = Color.LightYellow;
            dgvPhongDaChon.Columns.Add(colSoNguoi);

            dgvPhongDaChon.AllowUserToAddRows = false;
            dgvPhongDaChon.ReadOnly = false;
            dgvPhongDaChon.Columns["RoomID"].ReadOnly = true;
            dgvPhongDaChon.Columns["RoomName"].ReadOnly = true;
            dgvPhongDaChon.Columns["LoaiPhong"].ReadOnly = true;
            dgvPhongDaChon.Columns["SoNguoi"].ReadOnly = false;

            // --- Cấu hình bảng Phòng Trống ---
            dgvPhongTrong.Columns.Clear();
            dgvPhongTrong.Columns.Add("RoomID", "Mã phòng");
            dgvPhongTrong.Columns.Add("RoomName", "Tên phòng");
            dgvPhongTrong.Columns.Add("LoaiPhong", "Loại phòng");

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol.Name = "ActionAdd";
            imgCol.HeaderText = "Thêm";
            imgCol.DefaultCellStyle.NullValue = CreateAddIcon(24, Color.FromArgb(76, 175, 80), Color.White);
            imgCol.Width = 60;
            dgvPhongTrong.Columns.Add(imgCol);

            dgvPhongTrong.AllowUserToAddRows = false;
            dgvPhongTrong.RowHeadersVisible = false;
            dgvPhongTrong.ReadOnly = true;

            // Gắn sự kiện Leave cho ô CCCD để tự động tìm khách cũ
            //    txtCCCD.Leave += TxtCCCD_Leave;
            // Gắn sự kiện để khi bấm đổi RadioButton, máy sẽ tự nhảy giờ
            radTheoDem.CheckedChanged += radTheoDem_CheckedChanged;
            radTheoGio.CheckedChanged += radTheoDem_CheckedChanged;

            // Set mặc định chọn "Thuê theo đêm" lúc vừa mở Form
            radTheoDem.Checked = true;

            LoadPhongTrong();
        }

        private void LoadPhongTrong()
        {
            // Bắt lỗi nếu Form chưa load xong
            if (dtpNgayVao == null || dtpNgayRa == null) return;

            // 1. Lấy thời gian khách đang muốn đặt trên Form
            DateTime tgVao = dtpNgayVao.Value.Date.Add(dtpGioVao.Value.TimeOfDay);
            DateTime tgRa = dtpNgayRa.Value.Date.Add(dtpGioRa.Value.TimeOfDay);

            // Chặn lỗi: Nếu giờ ra nhỏ hơn hoặc bằng giờ vào thì không tìm kiếm
            if (tgVao >= tgRa) return;

            // =======================================================
            // THÊM MỚI: CÀI ĐẶT THỜI GIAN DỌN PHÒNG 1 TIẾNG (60 PHÚT)
            // =======================================================
            int thoiGianDonDep = 60;

            // Tạo thời gian "ảo" để kiểm tra đụng độ (Trừ 60p lúc vào, cộng 60p lúc ra)
            DateTime tgVao_Ao = tgVao.AddMinutes(-thoiGianDonDep);
            DateTime tgRa_Ao = tgRa.AddMinutes(thoiGianDonDep);

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                // 2. TÌM DANH SÁCH PHÒNG BỊ KẸT LỊCH (OVERLAP)
                // Trong hàm LoadPhongTrong
                var danhSachPhongBiKet = db.BookingDetails
                    .Where(bd => bd.Booking.Status != "Đã hủy" && bd.Booking.Status != "Hoàn thành")
                    // Hệ thống sẽ hiểu: Cứ đơn nào không phải Đã hủy hay Hoàn thành thì là đang kẹt (Đã đặt/Đang thuê)
                    .Where(bd => tgVao_Ao < bd.Booking.CheckOutDate && tgRa_Ao > bd.Booking.CheckInDate)
                    .Select(bd => bd.RoomID)
                    .ToList();

                // 3. LẤY DANH SÁCH PHÒNG TRỐNG = TẤT CẢ PHÒNG TRỪ ĐI PHÒNG BỊ KẸT
                // 3. LẤY DANH SÁCH PHÒNG TRỐNG = TẤT CẢ PHÒNG TRỪ ĐI PHÒNG BỊ KẸT VÀ PHÒNG ĐANG DỌN/BẢO TRÌ
                var dsPhong = db.Rooms
                    .Where(r => !danhSachPhongBiKet.Contains(r.RoomID) && r.Status != "Đang dọn" && r.Status != "Bảo trì")
                    .Select(r => new
                    {
                        r.RoomID,
                        r.RoomName,
                        LoaiPhong = r.RoomType.TypeName
                    }).ToList();

                // 4. Đổ lên DataGridView
                dgvPhongTrong.Rows.Clear();
                foreach (var phong in dsPhong)
                {
                    dgvPhongTrong.Rows.Add(phong.RoomID, phong.RoomName, phong.LoaiPhong);
                }
            }
        }

        // ========================================================
        // 2. XỬ LÝ CHUYỂN PHÒNG QUA LẠI
        // ========================================================
        private void dgvPhongTrong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPhongTrong.Columns[e.ColumnIndex].Name == "ActionAdd")
            {
                var row = dgvPhongTrong.Rows[e.RowIndex];

                int roomID = Convert.ToInt32(row.Cells["RoomID"].Value);
                string roomName = row.Cells["RoomName"].Value?.ToString();
                string loaiPhong = row.Cells["LoaiPhong"].Value?.ToString();

                // Chuyển sang bảng Đã Chọn, gán mặc định số người = 1
                dgvPhongDaChon.Rows.Add(roomID, roomName, loaiPhong, 1);
                dgvPhongTrong.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void dgvPhongDaChon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvPhongDaChon.Rows[e.RowIndex];
                dgvPhongTrong.Rows.Add(row.Cells["RoomID"].Value, row.Cells["RoomName"].Value, row.Cells["LoaiPhong"].Value);
                dgvPhongDaChon.Rows.RemoveAt(e.RowIndex);
            }
        }


        private string TaoMaKhachHangMoi(HotelManagementEntities db)
        {
            // Thêm .Where(c => c.MaKH.StartsWith("AKH")) để CHỈ LỌC CÁC MÃ CỦA WINFORMS
            var khCuoi = db.Customers
                           .Where(c => c.MaKH.StartsWith("AKH"))
                           .OrderByDescending(c => c.MaKH)
                           .FirstOrDefault();

            if (khCuoi == null || string.IsNullOrEmpty(khCuoi.MaKH))
            {
                return "AKH001";
            }

            string maSoStr = khCuoi.MaKH.Replace("AKH", "");
            if (int.TryParse(maSoStr, out int soMoi))
            {
                soMoi++;
                return "AKH" + soMoi.ToString("D3");
            }

            return "AKH001";
        }

        // ========================================================
        // 4. LƯU XUỐNG DATABASE
        // ========================================================
        private void btLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu rỗng
            if (string.IsNullOrWhiteSpace(txtCCCD.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên và CCCD của khách hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvPhongDaChon.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 phòng để đặt!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. KẾT HỢP NGÀY VÀ GIỜ THÀNH KIỂU DATETIME HOÀN CHỈNH
            // Lấy ngày (cắt bỏ phần giờ mặc định) và lấy giờ (cắt bỏ phần ngày) rồi cộng lại
            DateTime checkInHoanChinh = dtpNgayVao.Value.Date.Add(dtpGioVao.Value.TimeOfDay);
            DateTime checkOutHoanChinh = dtpNgayRa.Value.Date.Add(dtpGioRa.Value.TimeOfDay);

            // Kiểm tra tính hợp lý của thời gian
            if (checkOutHoanChinh <= checkInHoanChinh)
            {
                MessageBox.Show("Thời gian trả phòng phải lớn hơn thời gian nhận phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 3. Xử lý Khách hàng
                        string cccd = txtCCCD.Text.Trim();
                        Customer khachHang = db.Customers.FirstOrDefault(c => c.CCCD == cccd);

                        if (khachHang == null)
                        {
                            khachHang = new Customer();
                            khachHang.MaKH = TaoMaKhachHangMoi(db);
                            khachHang.TenKH = txtHoTen.Text;
                            khachHang.CCCD = txtCCCD.Text;
                            khachHang.SDT = txtSdt.Text; 
                            khachHang.DiaChi = txtDiaChi.Text;
                            khachHang.QuocTich = txtQuocTich.Text;
                            khachHang.GioiTinh = radNam.Checked ? "Nam" : "Nữ";

                            db.Customers.Add(khachHang);
                        }
                        else
                        {
                            khachHang.TenKH = txtHoTen.Text;
                            khachHang.SDT = txtSdt.Text;
                            khachHang.DiaChi = txtDiaChi.Text;
                            khachHang.QuocTich = txtQuocTich.Text;
                            khachHang.GioiTinh = radNam.Checked ? "Nam" : "Nữ";
                        }

                        // 4. Tạo Đơn đặt phòng
                        HotelManagement.Models.Booking newBooking = new HotelManagement.Models.Booking();
                        newBooking.CustomerID = khachHang.MaKH;
                        newBooking.BookingDate = DateTime.Now;

                        // Gán thời gian đã gộp vào Database
                        newBooking.CheckInDate = checkInHoanChinh;
                        newBooking.CheckOutDate = checkOutHoanChinh;
                        // =================================================================
                        // TỰ ĐỘNG XẾP TRẠNG THÁI DỰA VÀO NGÀY KHÁCH ĐẾN
                        // =================================================================
                        // Nếu ngày khách chọn nhận phòng chính là ngày hôm nay -> Cho nhận phòng luôn
                        if (chkNhanPhongNgay.Checked)
                        {
                            newBooking.Status = "Đang Thuê";
                        }
                        else
                        {   
                            newBooking.Status = "Đã đặt";
                        }
                        newBooking.Deposit = 0;
                        newBooking.NguonDat = "Trực tiếp";
                        newBooking.LoaiHinhThue = radTheoDem.Checked ? "Theo đêm" : "Theo giờ";
                      

                        db.Bookings.Add(newBooking);

                        // 5. Lưu chi tiết các phòng được chọn
                        foreach (DataGridViewRow row in dgvPhongDaChon.Rows)
                        {
                            int rId = Convert.ToInt32(row.Cells["RoomID"].Value);

                            int soNguoi = 1;
                            if (row.Cells["SoNguoi"].Value != null)
                            {
                                int.TryParse(row.Cells["SoNguoi"].Value.ToString(), out soNguoi);
                            }

                            BookingDetail detail = new BookingDetail();
                            detail.Booking = newBooking;
                            detail.RoomID = rId;
                            detail.SoNguoi = soNguoi;

                            db.BookingDetails.Add(detail);
                        }

                        // 6. Thực thi xuống Database
                        db.SaveChanges();
                        transaction.Commit();

                        MessageBox.Show("Đặt phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close(); // Hoặc reset form tùy bạn
                    }
                    catch (Exception ex)
                    {
                      
                        transaction.Rollback();
                        // Lấy lỗi sâu nhất (thường là lỗi vi phạm ràng buộc SQL)
                        var innerEx = ex.InnerException;
                        while (innerEx.InnerException != null) innerEx = innerEx.InnerException;

                        MessageBox.Show("Lỗi thực sự từ Database: " + innerEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                
                }
            }
        }

        // ========================================================
        // HÀM HỖ TRỢ VẼ ĐỒ HOẠ
        // ========================================================
        private Bitmap CreateAddIcon(int size, Color circleColor, Color plusColor)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using (Brush brush = new SolidBrush(circleColor))
                {
                    g.FillEllipse(brush, 0, 0, size - 1, size - 1);
                }

                using (Pen pen = new Pen(plusColor, 2))
                {
                    g.DrawLine(pen, size / 2, size / 4, size / 2, size * 3 / 4);
                    g.DrawLine(pen, size / 4, size / 2, size * 3 / 4, size / 2);
                }
            }
            return bmp;
        }

        private void radTheoDem_CheckedChanged(object sender, EventArgs e)
        {
            if (radTheoDem.Checked)
            {
                // 1. NẾU THUÊ THEO ĐÊM
                dtpGioVao.Value = new DateTime(2000, 1, 1, 14, 0, 0); // Mặc định 14:00
                dtpGioRa.Value = new DateTime(2000, 1, 1, 12, 0, 0);  // Mặc định 12:00

                dtpNgayRa.Value = dtpNgayVao.Value.AddDays(1); // Trả phòng vào hôm sau

                // Khóa không cho sửa giờ`
                dtpGioVao.Enabled = false;
                dtpGioRa.Enabled = false;
            }
            else if (radTheoGio.Checked)
            {
                // 2. NẾU THUÊ THEO GIỜ
                dtpGioVao.Enabled = true;
                dtpGioRa.Enabled = true;

                // ========================================================
                // ĐIỂM MẤU CHỐT LÀ 2 DÒNG NÀY: Reset về ngay thời điểm hiện tại
                // ========================================================
                dtpNgayVao.Value = DateTime.Now; // Ép về ngày hôm nay
                dtpGioVao.Value = DateTime.Now;  // Lấy đúng giờ phút giây hiện tại trên máy tính

                // Mặc định ngày ra bằng ngày vào
                dtpNgayRa.Value = dtpNgayVao.Value;

                // Cộng 2 tiếng vào giờ hiện tại để làm "Giờ ra dự kiến"
                dtpGioRa.Value = dtpGioVao.Value.AddHours(2);
            }
        }

        private void dtpNgayVao_ValueChanged(object sender, EventArgs e)
        {
            // Nếu đang chọn thuê theo đêm, khi ngày vào thay đổi -> ngày ra tự động +1
            if (radTheoDem.Checked)
            {
                dtpNgayRa.Value = dtpNgayVao.Value.AddDays(1);
            }
            // Nếu thuê theo giờ, ngày ra trượt theo bằng đúng ngày vào
            else if (radTheoGio.Checked)
            {
                dtpNgayRa.Value = dtpNgayVao.Value;
            }
            LoadPhongTrong();
            
        }

        private void btHuy_Click(object sender, EventArgs e)
        {
            // Hiện hộp thoại hỏi lại cho chắc chắn
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy bỏ thao tác và đóng cửa sổ này không? Các thông tin vừa nhập sẽ không được lưu.",
                                                  "Xác nhận Hủy",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            // Nếu người dùng chọn Yes (Đồng ý)
            if (result == DialogResult.Yes)
            {
                this.Close(); // Đóng Form hiện tại
            }
        }

        private void dtpNgayRa_ValueChanged(object sender, EventArgs e)
        {
            LoadPhongTrong();
        }

        private void dtpGioVao_ValueChanged(object sender, EventArgs e)
        {
            LoadPhongTrong();

        }

        private void dtpGioRa_ValueChanged(object sender, EventArgs e)
        {
            LoadPhongTrong();

        }
    }
}