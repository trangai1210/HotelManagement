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
    public partial class FrmDatPhong : Form
    {
        public FrmDatPhong()
        {
            InitializeComponent();

            // Tắt giao diện cũ của Windows để nút bấm nhận màu
            dgvDanhSach.EnableHeadersVisualStyles = false;

            // TỰ ĐỘNG GẮN SỰ KIỆN TÔ MÀU VÀO BẢNG (Rất quan trọng)
            dgvDanhSach.CellFormatting += DgvDanhSach_CellFormatting;

            LoadDanhSachDatPhong();
        }

        // ========================================================
        // 1. HÀM LOAD DỮ LIỆU TỪ DATABASE
        // ========================================================
        private void LoadDanhSachDatPhong()
        {
            string tuKhoa = txtTimCCCD.Text.Trim().ToLower();

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var query = db.Bookings.AsQueryable();

                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query = query.Where(b => b.Customer != null &&
                                            (b.Customer.CCCD.StartsWith(tuKhoa) || b.Customer.TenKH.ToLower().Contains(tuKhoa)));
                }

                var rawData = query.Select(b => new
                {
                    b.BookingID,
                    TenKhach = b.Customer.TenKH,
                    CCCD = b.Customer.CCCD,
                    b.CheckInDate,
                    b.CheckOutDate,
                    b.LoaiHinhThue,
                    b.Status,
                    DanhSachPhong = b.BookingDetails.Select(bd => bd.Room.RoomName),
                    DanhSachLoaiPhong = b.BookingDetails.Select(bd => bd.Room.RoomType.TypeName)
                }).ToList();

                var danhSachHienThi = rawData.Select(b => new
                {
                    Mã_Đơn = b.BookingID,
                    Tên_Khách = b.TenKhach,
                    CCCD = b.CCCD,
                    Phòng = string.Join(", ", b.DanhSachPhong),
                    Loại_Phòng = string.Join(", ", b.DanhSachLoaiPhong.Distinct()),
                    Ngày_Giờ_Vào = b.CheckInDate,
                    Ngày_Giờ_Ra = b.CheckOutDate,
                    Loại_Hình = b.LoaiHinhThue,
                    Trạng_Thái = b.Status
                }).ToList();

                dgvDanhSach.DataSource = danhSachHienThi;

                if (dgvDanhSach.Columns.Contains("Ngày_Giờ_Vào"))
                {
                    dgvDanhSach.Columns["Ngày_Giờ_Vào"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvDanhSach.Columns["Ngày_Giờ_Ra"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }

                CauHinhCotChucNang();
            }
        }

        // ========================================================
        // 2. KẺ CỘT CHỨC NĂNG (Chỉ tạo khung, chưa tô màu)
        // ========================================================
        private void CauHinhCotChucNang()
        {
            if (!dgvDanhSach.Columns.Contains("ColDuyetDon"))
            {
                DataGridViewButtonColumn btnDuyet = new DataGridViewButtonColumn();
                btnDuyet.Name = "ColDuyetDon";
                btnDuyet.HeaderText = "Duyệt Web";
                btnDuyet.Text = "Duyệt";
                btnDuyet.UseColumnTextForButtonValue = true;
                btnDuyet.FlatStyle = FlatStyle.Flat;
                dgvDanhSach.Columns.Add(btnDuyet);
            }

            if (!dgvDanhSach.Columns.Contains("ColCheckIn"))
            {
                DataGridViewButtonColumn btnCheckIn = new DataGridViewButtonColumn();
                btnCheckIn.Name = "ColCheckIn";
                btnCheckIn.HeaderText = "Nhận Phòng";
                btnCheckIn.Text = "Check-In";
                btnCheckIn.UseColumnTextForButtonValue = true;
                btnCheckIn.FlatStyle = FlatStyle.Flat;
                dgvDanhSach.Columns.Add(btnCheckIn);
            }
            if (!dgvDanhSach.Columns.Contains("ColCheckOut"))
            {
                DataGridViewButtonColumn btnCheckOut = new DataGridViewButtonColumn();
                btnCheckOut.Name = "ColCheckOut";
                btnCheckOut.HeaderText = "Trả Phòng";
                btnCheckOut.Text = "Check-Out";
                btnCheckOut.UseColumnTextForButtonValue = true;
                btnCheckOut.FlatStyle = FlatStyle.Flat;
                dgvDanhSach.Columns.Add(btnCheckOut);
            }
        }

        // ========================================================
        // 3. THUẬT TOÁN TÔ MÀU TỰ ĐỘNG THEO THỜI GIAN THỰC
        // ========================================================
        private void DgvDanhSach_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDanhSach.Columns.Count == 0) return;

            // Lấy trạng thái của dòng đang được vẽ
            string trangThai = dgvDanhSach.Rows[e.RowIndex].Cells["Trạng_Thái"].Value?.ToString();

            // ---- TÔ MÀU CỘT DUYỆT ĐƠN ----
            if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColDuyetDon")
            {
                if (trangThai == "Chờ duyệt" || trangThai == "Pending")
                {
                    // Hợp lệ: Tô xanh lá
                    e.CellStyle.BackColor = Color.MediumSeaGreen;
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.SelectionBackColor = Color.SeaGreen;
                }
                else
                {
                    // Không hợp lệ: Tô xám mờ
                    e.CellStyle.BackColor = Color.LightGray;
                    e.CellStyle.ForeColor = Color.DimGray;
                    e.CellStyle.SelectionBackColor = Color.LightGray;
                }
            }

            // ---- TÔ MÀU CỘT CHECK-IN ----
            else if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColCheckIn")
            {
                if (trangThai == "Đã đặt")
                {
                    // Hợp lệ: Tô xanh dương
                    e.CellStyle.BackColor = Color.DodgerBlue;
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.SelectionBackColor = Color.RoyalBlue;
                }
                else
                {
                    // Không hợp lệ: Tô xám mờ
                    e.CellStyle.BackColor = Color.LightGray;
                    e.CellStyle.ForeColor = Color.DimGray;
                    e.CellStyle.SelectionBackColor = Color.LightGray;
                }
            }
            else if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColCheckOut")
            {
                if (trangThai == "Đang Thuê") // Chỉ phòng đang có khách mới được trả
                {
                    // Hợp lệ: Tô màu Cam (DarkOrange)
                    e.CellStyle.BackColor = Color.DarkOrange;
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.SelectionBackColor = Color.Orange;
                }
                else
                {
                    // Không hợp lệ: Tô xám mờ
                    e.CellStyle.BackColor = Color.LightGray;
                    e.CellStyle.ForeColor = Color.DimGray;
                    e.CellStyle.SelectionBackColor = Color.LightGray;
                }

            }
        }

        private void btDatPhong_Click(object sender, EventArgs e)
        {
            ttDatPhong fm = new ttDatPhong();
            fm.ShowDialog();
            LoadDanhSachDatPhong();
        }

        private void txtTimCCCD_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachDatPhong();
        }

        // ========================================================
        // 4. XỬ LÝ CLICK: CHẶN CLICK VÀO NÚT XÁM CHUẨN XÁC NHẤT
        // ========================================================
        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];
            string trangThaiHienTai = row.Cells["Trạng_Thái"].Value?.ToString();

            // NẾU CLICK VÀO CỘT DUYỆT
            if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColDuyetDon")
            {
                // CHẶN NGAY: Nếu trạng thái không phải chờ duyệt thì không cho click (dù nút có bị click cũng im lìm)
                if (trangThaiHienTai != "Chờ duyệt" && trangThaiHienTai != "Pending") return;

                int bookingId = Convert.ToInt32(row.Cells["Mã_Đơn"].Value);
                string tenKhach = row.Cells["Tên_Khách"].Value.ToString();

                DialogResult rs = MessageBox.Show($"Bạn muốn Duyệt đơn cho khách {tenKhach} không?", "Duyệt Đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    using (HotelManagementEntities db = new HotelManagementEntities())
                    {
                        var phieuDat = db.Bookings.Find(bookingId);
                        if (phieuDat != null)
                        {
                            phieuDat.Status = "Đã đặt";
                            db.SaveChanges();
                            MessageBox.Show("Duyệt đơn thành công!");
                            LoadDanhSachDatPhong();
                        }
                    }
                }
            }

            // NẾU CLICK VÀO CỘT CHECK-IN
            else if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColCheckIn")
            {
                // CHẶN NGAY: Nếu trạng thái không phải "Đã đặt" thì không cho thao tác
                if (trangThaiHienTai != "Đã đặt") return;

                int bookingId = Convert.ToInt32(row.Cells["Mã_Đơn"].Value);
                string tenKhach = row.Cells["Tên_Khách"].Value.ToString();

                DialogResult rs = MessageBox.Show($"Xác nhận Giao phòng cho khách {tenKhach}?", "Check-in", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    using (HotelManagementEntities db = new HotelManagementEntities())
                    {
                        var phieuDat = db.Bookings.Find(bookingId);
                        if (phieuDat != null)
                        {
                            phieuDat.Status = "Đang Thuê";
                            phieuDat.CheckInDate = DateTime.Now;
                            db.SaveChanges();
                            MessageBox.Show("Check-in thành công!");
                            LoadDanhSachDatPhong();
                        }
                    }
                }
            }

            else if (dgvDanhSach.Columns[e.ColumnIndex].Name == "ColCheckOut")
            {
                // CHẶN NGAY: Chỉ phòng "Đang Thuê" mới được trả
                if (trangThaiHienTai != "Đang Thuê") return;

                int bookingId = Convert.ToInt32(row.Cells["Mã_Đơn"].Value);
                string tenKhach = row.Cells["Tên_Khách"].Value.ToString();

                // 1. MỞ FORM THANH TOÁN (Form mà chúng ta vừa hoàn thiện xong)
                InvoiceDetailForm frmThanhToan = new InvoiceDetailForm(bookingId);
                frmThanhToan.ShowDialog();

                // 2. SAU KHI ĐÓNG FORM (Lễ tân đã in bill và thu tiền xong)
                // Cập nhật lại danh sách để dòng này biến mất (nếu lọc) hoặc chuyển thành "Hoàn thành"
                LoadDanhSachDatPhong();
            }

        }
    }
}