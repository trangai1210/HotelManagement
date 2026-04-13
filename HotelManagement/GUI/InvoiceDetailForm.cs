using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace HotelManagement.GUI
{
    public partial class InvoiceDetailForm : Form
    {
        private string invoiceId;
        private string connectionString;
        public InvoiceDetailForm(string invoiceId, string connString)
        {
            InitializeComponent();
            this.invoiceId = invoiceId;
            this.connectionString = connString;
            LoadInvoiceDetails();
        }

        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadInvoiceDetails()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Thông tin hóa đơn
                    string sqlInfo = @"SELECT inv.MaHD, inv.NgayLap, 
                          cus.TenKH, cus.SDT, cus.CCCD, cus.GioiTinh, cus.QuocTich,
                          emp.HoTen AS TenNV, emp.MaNV,
                          rm.RoomName, inv.TongTien, inv.GhiChu, inv.TrangThai
                   FROM Invoice inv
                   JOIN Customer cus ON inv.MaKH = cus.MaKH
                   JOIN Employees emp ON inv.MaNV = emp.MaNV
                   JOIN Room rm ON inv.MaPhong = rm.RoomID
                   WHERE inv.MaHD = @MaHD";

                    SqlCommand cmdInfo = new SqlCommand(sqlInfo, conn);
                    cmdInfo.Parameters.AddWithValue("@MaHD", invoiceId);
                    SqlDataAdapter daInfo = new SqlDataAdapter(cmdInfo);
                    DataTable dtInfo = new DataTable();
                    daInfo.Fill(dtInfo);

                    if (dtInfo.Rows.Count > 0)
                    {
                        lblMaHD.Text = dtInfo.Rows[0]["MaHD"].ToString();
                        lblNgayLap.Text = Convert.ToDateTime(dtInfo.Rows[0]["NgayLap"]).ToString("dd/MM/yyyy HH:mm");                        
                        lblTenNV.Text = dtInfo.Rows[0]["TenNV"].ToString();
                        lblPhong.Text = dtInfo.Rows[0]["RoomName"].ToString();
                        lblTenKH.Text = dtInfo.Rows[0]["TenKH"].ToString();
                        lblSDT.Text = dtInfo.Rows[0]["SDT"].ToString();
                        lblCCCD.Text = dtInfo.Rows[0]["CCCD"].ToString();
                        lblGioiTinh.Text = dtInfo.Rows[0]["GioiTinh"].ToString();
                        lblQuocTich.Text = dtInfo.Rows[0]["QuocTich"].ToString();
                        lblTrangThai.Text = Convert.ToBoolean(dtInfo.Rows[0]["TrangThai"]) ? "Đã thanh toán" : "Chưa thanh toán";
                        lblTongTien.Text = string.Format("{0:N0} VNĐ", dtInfo.Rows[0]["TongTien"]);
                    }

                    // Chi tiết dịch vụ
                    string sqlDetail = @"SELECT s.ServiceName, id.SoLuong, s.Price, 
                            (id.SoLuong * s.Price) AS ThanhTien
                     FROM InvoiceDetail id
                     JOIN Service s ON id.MaDV = s.ServiceId
                     WHERE id.MaHD = @MaHD";

                    SqlCommand cmdDetail = new SqlCommand(sqlDetail, conn);
                    cmdDetail.Parameters.AddWithValue("@MaHD", invoiceId);
                    SqlDataAdapter daDetail = new SqlDataAdapter(cmdDetail);
                    DataTable dtDetail = new DataTable();
                    daDetail.Fill(dtDetail);

                    dgvChiTiet.DataSource = dtDetail;

                    // Tính tổng tiền dịch vụ
                    decimal tongTienDV = 0;
                    foreach (DataRow row in dtDetail.Rows)
                    {
                        tongTienDV += Convert.ToDecimal(row["ThanhTien"]);
                    }

                    decimal tongTienPhong = Convert.ToDecimal(dtInfo.Rows[0]["TongTien"]) - tongTienDV;

                    lblTongTienPhong.Text = string.Format("{0:N0} VNĐ", tongTienPhong);
                    lblTongTienDV.Text = string.Format("{0:N0} VNĐ", tongTienDV);
                    lblTongTien.Text = string.Format("{0:N0} VNĐ", dtInfo.Rows[0]["TongTien"]);

                    // Định dạng DataGridView
                    if (dgvChiTiet.Columns["ServiceName"] != null)
                        dgvChiTiet.Columns["ServiceName"].HeaderText = "Dịch vụ";
                    if (dgvChiTiet.Columns["SoLuong"] != null)
                        dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
                    if (dgvChiTiet.Columns["Price"] != null)
                    {
                        dgvChiTiet.Columns["Price"].DefaultCellStyle.Format = "N0";
                        dgvChiTiet.Columns["Price"].HeaderText = "Đơn giá";
                    }
                    if (dgvChiTiet.Columns["ThanhTien"] != null)
                    {
                        dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                        dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
                    }
                    if (dgvChiTiet.Columns["Note"] != null)
                        dgvChiTiet.Columns["Note"].HeaderText = "Ghi chú";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

                // Hiển thị hộp thoại chọn máy in trước khi in
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDoc;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Lấy font chữ
            Font fontTieuDe = new Font("Arial", 14, FontStyle.Bold);
            Font fontNormal = new Font("Arial", 10, FontStyle.Regular);
            Font fontDam = new Font("Arial", 10, FontStyle.Bold);

            // Vị trí bắt đầu vẽ
            int y = e.MarginBounds.Top;
            int x = e.MarginBounds.Left;
            int khoangCach = 25;

            // === 1. TIÊU ĐỀ HÓA ĐƠN ===
            string tieuDe = "HÓA ĐƠN THANH TOÁN";
            SizeF kichThuocTieuDe = e.Graphics.MeasureString(tieuDe, fontTieuDe);
            e.Graphics.DrawString(tieuDe, fontTieuDe, Brushes.Black,
                e.MarginBounds.Left + (e.MarginBounds.Width - kichThuocTieuDe.Width) / 2, y);
            y += khoangCach + 10;

            // === 2. THÔNG TIN HÓA ĐƠN ===
            e.Graphics.DrawString($"Mã HD: {lblMaHD.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"Ngày lập: {lblNgayLap.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"Nhân viên: {lblTenNV.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"Phòng: {lblPhong.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach + 10;

            // === 3. THÔNG TIN KHÁCH HÀNG ===
            e.Graphics.DrawString("--- THÔNG TIN KHÁCH HÀNG ---", fontDam, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"Tên KH: {lblTenKH.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"SDT: {lblSDT.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"CCCD: {lblCCCD.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"Giới tính: {lblGioiTinh.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"Quốc tịch: {lblQuocTich.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach + 10;

            // === 4. DANH SÁCH DỊCH VỤ ===
            e.Graphics.DrawString("--- CHI TIẾT DỊCH VỤ ---", fontDam, Brushes.Black, x, y);
            y += khoangCach;

            // Vẽ header bảng
            e.Graphics.DrawString("Dịch vụ", fontDam, Brushes.Black, x, y);
            e.Graphics.DrawString("SL", fontDam, Brushes.Black, x + 150, y);
            e.Graphics.DrawString("Đơn giá", fontDam, Brushes.Black, x + 220, y);
            e.Graphics.DrawString("Thành tiền", fontDam, Brushes.Black, x + 320, y);
            y += khoangCach - 5;

            // Vẽ đường kẻ
            e.Graphics.DrawLine(Pens.Black, x, y, x + 450, y);
            y += 10;

            // Duyệt từng dòng trong DataGridView
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue;

                string dichVu = row.Cells["ServiceName"].Value?.ToString() ?? "";
                string soLuong = row.Cells["SoLuong"].Value?.ToString() ?? "";
                string donGia = row.Cells["Price"].Value?.ToString() ?? "";
                string thanhTien = row.Cells["ThanhTien"].Value?.ToString() ?? "";

                e.Graphics.DrawString(dichVu, fontNormal, Brushes.Black, x, y);
                e.Graphics.DrawString(soLuong, fontNormal, Brushes.Black, x + 150, y);
                e.Graphics.DrawString(donGia, fontNormal, Brushes.Black, x + 220, y);
                e.Graphics.DrawString(thanhTien, fontNormal, Brushes.Black, x + 320, y);

                y += khoangCach;

                // Kiểm tra nếu hết trang
                if (y > e.MarginBounds.Bottom - 100)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            y += 10;
            e.Graphics.DrawLine(Pens.Black, x, y, x + 450, y);
            y += khoangCach;

            // === 5. TỔNG TIỀN ===
            e.Graphics.DrawString($"Tiền phòng: {lblTongTienPhong.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"Tiền dịch vụ: {lblTongTienDV.Text}", fontNormal, Brushes.Black, x, y);
            y += khoangCach;
            e.Graphics.DrawString($"TỔNG CỘNG: {lblTongTien.Text}", fontDam, Brushes.Black, x, y);
            y += khoangCach + 10;

            // === 6. TRẠNG THÁI ===
            e.Graphics.DrawString($"Trạng thái: {lblTrangThai.Text}", fontDam,
                lblTrangThai.Text == "Đã thanh toán" ? Brushes.Green : Brushes.Red, x, y);
            y += khoangCach + 10;

            // === 7. CHỮ KÝ ===
            e.Graphics.DrawString("Khách hàng", fontNormal, Brushes.Black, x, e.MarginBounds.Bottom - 30);
            e.Graphics.DrawString("(Ký, ghi rõ họ tên)", fontNormal, Brushes.Black, x, e.MarginBounds.Bottom - 15);
            e.Graphics.DrawString("Nhân viên", fontNormal, Brushes.Black, x + 300, e.MarginBounds.Bottom - 30);
            e.Graphics.DrawString("(Ký, ghi rõ họ tên)", fontNormal, Brushes.Black, x + 300, e.MarginBounds.Bottom - 15);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
