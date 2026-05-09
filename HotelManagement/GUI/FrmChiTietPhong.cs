using HotelManagement.Models;
using System;
using System.Data.Entity;
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
    public partial class FrmChiTietPhong : Form
    {
        private int _bookingId;
        public FrmChiTietPhong(int bookingId)
        {
            InitializeComponent();
            _bookingId = bookingId;
        }
    
         private void FrmChiTietPhong_Load(object sender, EventArgs e)
            {
                LoadThongTinKhach();
                LoadDichVuDaDung();
            }

        // 1. Load thông tin khách hàng và phòng
        private void LoadThongTinKhach()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var info = db.BookingDetails
                             .Include(bd => bd.Booking.Customer)
                             .Include(bd => bd.Room)
                             .FirstOrDefault(bd => bd.BookingID == _bookingId);

                if (info != null)
                {
                    lblPhong.Text = info.Room.RoomName;
                    lblTenKhach.Text = info.Booking.Customer.TenKH;
                    lblNgayDen.Text = info.Booking.CheckInDate?.ToString("dd/MM/yyyy HH:mm");

                    // Tính số ngày ở tạm tính
                    var soNgay = (DateTime.Now - info.Booking.CheckInDate.Value).Days;
                    lblSoNgay.Text = (soNgay == 0 ? 1 : soNgay).ToString() + " ngày";
                    //số người
                    lblSoNguoi.Text = info.SoNguoi.ToString() + " người";
                }
            }
        }

        // 2. Load danh sách dịch vụ khách đã gọi (Hiện lên bảng nhỏ)
        // 2. Load danh sách dịch vụ khách đã gọi (Hiện lên bảng nhỏ)
        public void LoadDichVuDaDung()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var ds = db.BookingServices
                           .Where(bs => bs.BookingID == _bookingId)
                           .Select(bs => new
                           {
                               bs.Service.ServiceName,
                               bs.Quantity,
                               ThanhTien = bs.Quantity * bs.Price
                           }).ToList();

                // Nạp dữ liệu vào bảng
                dgvDichVuLuu.DataSource = ds;

                // =========================================================
                // ĐỔI TÊN CỘT SANG TIẾNG VIỆT & LÀM ĐẸP BẢNG
                // =========================================================
                if (dgvDichVuLuu.Columns["ServiceName"] != null)
                {
                    dgvDichVuLuu.Columns["ServiceName"].HeaderText = "Dịch vụ";
                }

                if (dgvDichVuLuu.Columns["Quantity"] != null)
                {
                    dgvDichVuLuu.Columns["Quantity"].HeaderText = "Số lượng";
                }

                if (dgvDichVuLuu.Columns["ThanhTien"] != null)
                {
                    dgvDichVuLuu.Columns["ThanhTien"].HeaderText = "Thành tiền";
                    // Định dạng tiền tệ có dấu phẩy (VD: 50,000)
                    dgvDichVuLuu.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                }

                // Chỉnh giao diện bảng cho gọn gàng (Giống dgvDsDaChon)
                dgvDichVuLuu.RowHeadersVisible = false;           // Ẩn cột mũi tên trống bên trái
                dgvDichVuLuu.AllowUserToAddRows = false;          // Ẩn dòng trắng thừa ở dưới cùng
                dgvDichVuLuu.ReadOnly = true;                     // Khóa bảng, chỉ cho xem không cho gõ bậy
                dgvDichVuLuu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động kéo giãn cột cho vừa khít
            }
        }

        // 3. NÚT THÊM DỊCH VỤ: Mở Form mà chúng ta làm nãy giờ
        private void btnThemDV_Click(object sender, EventArgs e)
        {
            FrmThemDichVu frm = new FrmThemDichVu(_bookingId);
            frm.ShowDialog();

            // SAU KHI FORM DỊCH VỤ ĐÓNG -> LOAD LẠI BẢNG TẠI ĐÂY
            LoadDichVuDaDung();
        }

        // 4. NÚT THANH TOÁN (Check-out)
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            // Ở đây bạn sẽ gọi Form hóa đơn (Invoice)
            // Truyền _bookingId sang đó để tính tổng tiền cuối cùng
            InvoiceDetailForm frm = new InvoiceDetailForm(_bookingId); // Chuyền _bookingId sang
            frm.ShowDialog();
        }
    }
}
