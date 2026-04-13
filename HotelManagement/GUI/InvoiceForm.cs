using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class InvoiceForm : Form
    {
        private string GetConnectionString()
        {
            string efString = ConfigurationManager
                .ConnectionStrings["HotelManagementEntities"]
                .ConnectionString;

            int start = efString.IndexOf("provider connection string=\"")
                        + "provider connection string=\"".Length;

            int end = efString.LastIndexOf("\"");

            string sqlString = efString.Substring(start, end - start);

            return sqlString.Replace("&quot;", "\"");
        }
            public InvoiceForm()
        {
            InitializeComponent();
            LoadInvoices();
            LoadComboSearch();
        }

        private void lblMaHD_Click(object sender, EventArgs e)
        {

        }
        private void LoadInvoices()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string searchKey = txtTimKiem.Text.Trim();
                    string selectedFilter = cboLoc.SelectedItem?.ToString();


                    string sql = @"SELECT inv.MaHD, inv.NgayLap, 
                                  cus.TenKH, emp.HoTen AS TenNV, 
                                  rm.RoomName, inv.TongTien
                           FROM Invoice inv
                           JOIN Customer cus ON inv.MaKH = cus.MaKH
                           JOIN Employees emp ON inv.MaNV = emp.MaNV
                           JOIN Room rm ON inv.MaPhong = rm.RoomID
                           WHERE 1=1";

                    if (!string.IsNullOrEmpty(searchKey))
                    {
                        if (selectedFilter == "Mã hóa đơn")
                            sql += " AND inv.MaHD LIKE @searchKey";
                        else if (selectedFilter == "Tên khách hàng")
                            sql += " AND cus.TenKH LIKE @searchKey";
                        else if (selectedFilter == "Tên nhân viên")
                            sql += " AND emp.HoTen LIKE @searchKey";
                    }

                    sql += " ORDER BY inv.NgayLap DESC";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    if (!string.IsNullOrEmpty(searchKey))
                    {
                        cmd.Parameters.AddWithValue("@searchKey", "%" + searchKey + "%");
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Thêm cột nút "Chi tiết"
                    if (!dt.Columns.Contains("ChiTiet"))
                    {
                        dt.Columns.Add("ChiTiet", typeof(string));
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        row["ChiTiet"] = "📋 Xem chi tiết";
                    }

                    dgvHoaDon.DataSource = dt;

                    // Định dạng cột
                    if (dgvHoaDon.Columns["MaHD"] != null)
                        dgvHoaDon.Columns["MaHD"].HeaderText = "Mã HD";
                    if (dgvHoaDon.Columns["NgayLap"] != null)
                        dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày lập";
                    if (dgvHoaDon.Columns["TenKH"] != null)
                        dgvHoaDon.Columns["TenKH"].HeaderText = "Khách hàng";
                    if (dgvHoaDon.Columns["TenNV"] != null)
                        dgvHoaDon.Columns["TenNV"].HeaderText = "Nhân viên";
                    if (dgvHoaDon.Columns["RoomName"] != null)
                        dgvHoaDon.Columns["RoomName"].HeaderText = "Phòng";
                    if (dgvHoaDon.Columns["TongTien"] != null)
                    {
                        dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                        dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền (VNĐ)";
                    }
                    if (dgvHoaDon.Columns["ChiTiet"] != null)
                    {
                        dgvHoaDon.Columns["ChiTiet"].HeaderText = "Chi tiết";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== 2. LOAD COMBOBOX TÌM KIẾM ==========
        private void LoadComboSearch()
        {
            cboLoc.Items.Add("Mã hóa đơn");
            cboLoc.Items.Add("Tên khách hàng");
            cboLoc.Items.Add("Tên nhân viên");
            cboLoc.SelectedIndex = 0;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvHoaDon.Columns[e.ColumnIndex].Name == "ChiTiet")
            {
                string invoiceId = dgvHoaDon.Rows[e.RowIndex].Cells["MaHD"].Value.ToString();
                InvoiceDetailForm detailForm = new InvoiceDetailForm(invoiceId, GetConnectionString());
                detailForm.ShowDialog();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadInvoices();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            cboLoc.SelectedIndex = 0;
            LoadInvoices();
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTim_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}
