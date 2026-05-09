using HotelManagement.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class InvoiceForm : Form
    {
        public InvoiceForm()
        {
            InitializeComponent();
            this.Load += InvoiceForm_Load;

            // --- ĐĂNG KÝ SỰ KIỆN THỜI GIAN THỰC ---
            // 1. Nhập chữ đến đâu lọc đến đó
            txtTimKiem.TextChanged += (s, e) => LoadInvoices();

            // 2. Ấn tick lọc theo ngày là load lại luôn
            chkLocNgay.CheckedChanged += (s, e) => {
                // Mở/Khóa dtpNgay tùy theo trạng thái tick
                dtpNgay.Enabled = chkLocNgay.Checked;
                LoadInvoices();
            };

            // 3. Thay đổi ngày trên DateTimePicker cũng load lại luôn
            dtpNgay.ValueChanged += (s, e) => {
                if (chkLocNgay.Checked) LoadInvoices();
            };
        }

        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            LoadComboSearch();
            dtpNgay.Enabled = false; // Mặc định chưa tick thì khóa chọn ngày
            LoadInvoices();
        }

        private void LoadComboSearch()
        {
            cboLoc.Items.Clear();
            cboLoc.Items.Add("Mã hóa đơn");
            cboLoc.Items.Add("Tên khách hàng");
            cboLoc.SelectedIndex = 0;
        }

        private void LoadInvoices()
        {
            try
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    string searchKey = txtTimKiem.Text.Trim().ToLower();
                    string selectedFilter = cboLoc.SelectedItem?.ToString();

                    var query = db.Invoices
                                  .Include(inv => inv.Booking)
                                  .Include(inv => inv.Booking.Customer)
                                  .Include("Booking.BookingDetails.Room")
                                  .AsQueryable();

                    if (chkLocNgay.Checked)
                    {
                        DateTime targetDate = dtpNgay.Value.Date;
                        query = query.Where(inv => DbFunctions.TruncateTime(inv.PaymentDate) == targetDate);
                    }

                    if (!string.IsNullOrEmpty(searchKey))
                    {
                        if (selectedFilter == "Mã hóa đơn")
                            query = query.Where(inv => inv.InvoiceID.ToString().Contains(searchKey));
                        else if (selectedFilter == "Tên khách hàng")
                            query = query.Where(inv => inv.Booking.Customer.TenKH.ToLower().Contains(searchKey));
                    }

                   // Trong hàm LoadInvoices của InvoiceForm
var dsHoaDon = query.OrderByDescending(inv => inv.InvoiceID) // Sắp xếp mã hóa đơn mới nhất lên đầu
                    .ToList()
                    .Select(inv => new
                    {
                        inv.InvoiceID,
                        NgayLap = inv.PaymentDate,
                        // Truy vấn lấy tên khách từ bảng liên kết
                        TenKH = inv.Booking?.Customer?.TenKH ?? "N/A",
                        RoomName = inv.Booking?.BookingDetails?.FirstOrDefault()?.Room?.RoomName ?? "N/A",
                        inv.TotalAmount,
                        // Hiển thị trạng thái thanh toán
                        TrangThai = inv.Booking?.PaymentStatus ?? "Đã thanh toán",
                        ChiTiet = "📋 Xem chi tiết"
                    }).ToList();

dgvHoaDon.DataSource = dsHoaDon;
                    FormatGrid();
                }
            }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); }
        }

        private void FormatGrid()
        {
            if (dgvHoaDon.Columns["InvoiceID"] != null) dgvHoaDon.Columns["InvoiceID"].HeaderText = "Mã HD";
            if (dgvHoaDon.Columns["TotalAmount"] != null)
            {
                dgvHoaDon.Columns["TotalAmount"].HeaderText = "Tổng tiền";
                dgvHoaDon.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
            }
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvHoaDon.Columns[e.ColumnIndex].Name == "ChiTiet")
            {
                int currentInvoiceId = Convert.ToInt32(dgvHoaDon.Rows[e.RowIndex].Cells["InvoiceID"].Value);
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var hoaDon = db.Invoices.FirstOrDefault(inv => inv.InvoiceID == currentInvoiceId);
                    if (hoaDon?.BookingID != null)
                    {
                        // Mở form chi tiết và truyền BookingID
                        InvoiceDetailForm detailForm = new InvoiceDetailForm(hoaDon.BookingID.Value);
                        detailForm.ShowDialog();
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            chkLocNgay.Checked = false;
            cboLoc.SelectedIndex = 0;
            LoadInvoices();
        }
    }
}