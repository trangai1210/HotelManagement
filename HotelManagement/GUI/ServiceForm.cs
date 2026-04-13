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
    public partial class ServiceForm : Form
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

        public ServiceForm()
        {
            InitializeComponent();
            LoadDataGridView();   // Hiển thị lên bảng
            LoadComboLoaiDV();
        }
       
             // ========== LOAD DATA GRID VIEW ==========
        private void LoadDataGridView(string timKiem = "")
        {
                try
                {
                    using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                    {
                        conn.Open();

                        // Lấy từ khóa tìm kiếm
                        string searchName = txtTimKiem.Text.Trim();

                        // Lấy loại được chọn
                        string selectedCategory = cboLoaiDV.SelectedValue?.ToString();

                        // Xây dựng câu lệnh SQL
                        string sql = @"SELECT ServiceId, ServiceName, Price, Unit, Category
                                   FROM Service WHERE 1=1";

                        // Thêm điều kiện lọc theo tên (không phân biệt hoa thường)
                        if (!string.IsNullOrEmpty(searchName))
                        {
                            sql += " AND LOWER(ServiceName) LIKE @timKiem";
                        }

                        // Thêm điều kiện lọc theo loại (nếu không chọn "Tất cả")
                        if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "Tất cả")
                        {
                            sql += " AND Category = @Category";
                        }

                        sql += " ORDER BY ServiceId";

                        SqlCommand cmd = new SqlCommand(sql, conn);

                        // Thêm tham số tìm kiếm
                        if (!string.IsNullOrEmpty(searchName))
                        {
                            cmd.Parameters.AddWithValue("@timKiem", "%" + searchName.ToLower() + "%");
                        }

                        // Thêm tham số loại
                        if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "Tất cả")
                        {
                            cmd.Parameters.AddWithValue("@Category", selectedCategory);
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvDanhSach.DataSource = dt;

                        // Định dạng cột Price
                        if (dgvDanhSach.Columns["Price"] != null)
                        {
                            dgvDanhSach.Columns["Price"].DefaultCellStyle.Format = "N0";
                            dgvDanhSach.Columns["Price"].HeaderText = "Đơn giá (VNĐ)";
                        }

                        // Đặt tên cột hiển thị
                        if (dgvDanhSach.Columns["ServiceId"] != null)
                            dgvDanhSach.Columns["ServiceId"].HeaderText = "Mã";
                        if (dgvDanhSach.Columns["ServiceName"] != null)
                            dgvDanhSach.Columns["ServiceName"].HeaderText = "Tên dịch vụ";
                        if (dgvDanhSach.Columns["Unit"] != null)
                            dgvDanhSach.Columns["Unit"].HeaderText = "Đơn vị";
                        if (dgvDanhSach.Columns["Category"] != null)
                            dgvDanhSach.Columns["Category"].HeaderText = "Loại";

                        // Hiển thị thông báo nếu không tìm thấy
                        if (dt.Rows.Count == 0 && !string.IsNullOrEmpty(searchName))
                        {
                            MessageBox.Show("Không tìm thấy dịch vụ nào!", "Thông báo",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void LoadComboLoaiDV()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT Category FROM Service WHERE Category IS NOT NULL AND Category != '' ORDER BY Category";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Thêm dòng "Tất cả" vào đầu
                    DataRow row = dt.NewRow();
                    row["Category"] = "Tất cả";
                    dt.Rows.InsertAt(row, 0);

                    cboLoaiDV.DataSource = dt;
                    cboLoaiDV.DisplayMember = "Category";
                    cboLoaiDV.ValueMember = "Category";
                    cboLoaiDV.DropDownStyle = ComboBoxStyle.DropDownList;
                    cboLoaiDV.SelectedIndex = 0; // Mặc định chọn "Tất cả"
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải loại dịch vụ: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ServiceForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDV.Text))
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần sửa!", "Cảnh báo");
                return;
            }// nhi

            if (string.IsNullOrWhiteSpace(txtTenDV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên dịch vụ!", "Cảnh báo");
                return;
            }

            decimal price = 0;
            if (!string.IsNullOrWhiteSpace(txtDonGia.Text))
                decimal.TryParse(txtDonGia.Text, out price);

            // Lấy loại được chọn
            string selectedCategory = cboLoaiDV.SelectedValue?.ToString();
            if (selectedCategory == "Tất cả")
            {
                selectedCategory = "";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string sql = @"UPDATE Service 
                                   SET ServiceName = @ServiceName, 
                                       Price = @Price, 
                                       Unit = @Unit, 
                                       Category = @Category
                                   WHERE ServiceId = @ServiceId";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ServiceId", int.Parse(txtMaDV.Text));
                    cmd.Parameters.AddWithValue("@ServiceName", txtTenDV.Text.Trim());
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Unit", txtDonViTinh.Text.Trim());
                    cmd.Parameters.AddWithValue("@Category", selectedCategory);

                    cmd.ExecuteNonQuery();

                    LoadComboLoaiDV();
                    LoadDataGridView();
                    ClearForm();

                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(txtTenDV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên dịch vụ!", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDV.Focus();
                return;
            }

            decimal price = 0;
            if (!string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                if (!decimal.TryParse(txtDonGia.Text, out price))
                {
                    MessageBox.Show("Đơn giá không hợp lệ!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                    return;
                }
            }

            // Lấy loại được chọn
            string selectedCategory = cboLoaiDV.SelectedValue?.ToString();
            if (selectedCategory == "Tất cả")
            {
                selectedCategory = "";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string sql = @"INSERT INTO Service (ServiceName, Price, Unit, Category) 
                                   VALUES (@ServiceName, @Price, @Unit, @Category)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ServiceName", txtTenDV.Text.Trim());
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Unit", txtDonViTinh.Text.Trim());
                    cmd.Parameters.AddWithValue("@Category", selectedCategory);

                    cmd.ExecuteNonQuery();

                    LoadComboLoaiDV();  // Reload combobox để cập nhật loại mới
                    LoadDataGridView();
                    ClearForm();

                    MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dữ liệu: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearForm()
        {
            txtMaDV.Text = "";
            txtTenDV.Text = "";
            txtDonGia.Text = "";
            txtDonViTinh.Text = "";
            txtTimKiem.Text = "";
            if (cboLoaiDV.Items.Count > 0)
                cboLoaiDV.SelectedIndex = 0;
            txtTenDV.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDV.Text))
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!", "Cảnh báo");
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa dịch vụ này?",
                                                "Xác nhận xóa",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                    {
                        conn.Open();

                        string sql = "DELETE FROM Service WHERE ServiceId = @ServiceId";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@ServiceId", int.Parse(txtMaDV.Text));
                        cmd.ExecuteNonQuery();

                        LoadComboLoaiDV();
                        LoadDataGridView();
                        ClearForm();

                        MessageBox.Show("Xóa thành công!", "Thông báo");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa dữ liệu: " + ex.Message, "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            cboLoaiDV.SelectedIndex = 0;
            LoadDataGridView();
            ClearForm();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];

                txtMaDV.Text = row.Cells["ServiceId"].Value.ToString();
                txtTenDV.Text = row.Cells["ServiceName"].Value.ToString();
                txtDonGia.Text = row.Cells["Price"].Value.ToString();
                txtDonViTinh.Text = row.Cells["Unit"].Value?.ToString() ?? "";

                // Chọn category trong combobox
                string category = row.Cells["Category"].Value?.ToString() ?? "";
                if (string.IsNullOrEmpty(category))
                {
                    cboLoaiDV.SelectedIndex = 0;
                }
                else
                {
                    for (int i = 0; i < cboLoaiDV.Items.Count; i++)
                    {
                        DataRowView drv = cboLoaiDV.Items[i] as DataRowView;
                        if (drv != null && drv["Category"].ToString() == category)
                        {
                            cboLoaiDV.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadDataGridView();
        }

        private void txtTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnTim_Click(sender, e);
            }
        }
    }
}
