using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class QLNhanVienForm : Form
    {
        HotelManagementEntities db = new HotelManagementEntities();
        public QLNhanVienForm()
        {
            InitializeComponent();
        }

        private void menuStrip1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem form đã được mở chưa
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Name == "QLNhanVien") // Thay bằng tên Form thực tế của bạn
                {
                    frm.Activate(); // Nếu mở rồi thì focus (đưa lên trên cùng)
                    return;
                }
            }

            // Nếu chưa mở thì khởi tạo và mở mới
            QLNhanVienForm fNhanVien = new QLNhanVienForm();
            fNhanVien.MdiParent = this; // Xác định Form chính là cha
            fNhanVien.Show();
        }

        private void DoiTenCot()
        {
            if (dataGridView1.Columns["MaNV"] != null)
            {
                dataGridView1.Columns["MaNV"].HeaderText = "Mã NV";
                dataGridView1.Columns["HoTen"].HeaderText = "Họ và Tên";
                dataGridView1.Columns["GioiTinh"].HeaderText = "Giới tính";
                dataGridView1.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dataGridView1.Columns["ChucVu"].HeaderText = "Chức vụ";
                dataGridView1.Columns["DiaChi"].HeaderText = "Địa chỉ";
                dataGridView1.Columns["NgaySinh"].HeaderText = "Ngày sinh";
               // giấu cột trạng thái đi
        if (dataGridView1.Columns["TrangThai"] != null)
                {
                    dataGridView1.Columns["TrangThai"].Visible = false;
                }
            }
        }

       
        private void LoadDanhSachChucVu()
        {
            using (var db = new HotelManagementEntities())
            {
                var dsChucVu = db.Roles.ToList();

                // Gán dữ liệu
                comboBoxChucVu.DataSource = dsChucVu;
                comboBoxChucVu.DisplayMember = "TenChucVu";
                comboBoxChucVu.ValueMember = "MaChucVu";

                // DÒNG QUAN TRỌNG: Đưa lựa chọn về trống
                comboBoxChucVu.SelectedIndex = -1;
            }
        }
        // 1. Hàm chính xử lý toàn bộ việc Lấy dữ liệu, Lọc và Hiển thị
        private void HienThiDanhSach()
        {
            string keyword = txtTimKiem.Text.Trim();

            // GIẢ SỬ: Bạn đã kéo 1 CheckBox lên giao diện tên là chkHienNhanVienNghi
            // Nếu bạn chưa kéo, hãy tạo nó. Nếu tạm thời chưa muốn làm tính năng này, hãy set biến này = false cứng.
            bool hienCaNguoiNghi = chkHienNhanVienNghi.Checked;

            using (var db = new HotelManagementEntities())
            {
                var query = db.Employees.AsQueryable();

                // Lớp lọc 1: Trạng thái (Chỉ lấy người đang làm nếu không tick chọn "Hiện cả người nghỉ")
                if (hienCaNguoiNghi == false)
                {
                    query = query.Where(nv => nv.TrangThai == true);
                }

                // Lớp lọc 2: Tìm kiếm theo từ khóa
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(nv => nv.MaNV.Contains(keyword) || nv.HoTen.Contains(keyword));
                }

                // Lấy dữ liệu ra
                var ketQua = query.Select(nv => new {
                    MaNV = nv.MaNV,
                    HoTen = nv.HoTen,
                    GioiTinh = nv.GioiTinh,
                    SoDienThoai = nv.SoDienThoai,
                    ChucVu = nv.Role.TenChucVu,
                    CCCD = nv.CCCD,
                    DiaChi = nv.DiaChi,
                    NgaySinh = nv.NgaySinh,
                    TrangThai = nv.TrangThai // Giữ nguyên True/False để dùng làm điều kiện tô màu
                }).ToList();

                dataGridView1.DataSource = ketQua;
                DoiTenCot();

                // ==========================================
                // VÒNG LẶP ĐỔI MÀU (Nằm gọn trong hàm này)
                // ==========================================
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Kiểm tra xem cột ẩn TrangThai có mang giá trị false (Nghỉ việc) không
                    if (row.Cells["TrangThai"].Value != null && (bool)row.Cells["TrangThai"].Value == false)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Strikeout);
                    }
                }
            }
        }

    
        private void LoadDanhSachNhanVien()
        {
            HienThiDanhSach();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            HienThiDanhSach();
        }

        // (Sự kiện phụ) Khi Lễ tân bấm tick/bỏ tick ô "Hiện người đã nghỉ", lưới cũng tự động lọc lại
        private void chkHienNhanVienNghi_CheckedChanged(object sender, EventArgs e)
        {// Bất cứ khi nào Lễ tân tick hoặc bỏ tick, tự động kích hoạt hàm tìm kiếm chạy lại
         //MessageBox.Show("Đã nhận lệnh Click!");
            txtTimKiem_TextChanged(null, null);
        }
        private void DinhDangLuoi()
        {
            // 1. Cài đặt chung cho lưới
            dataGridView1.BackgroundColor = Color.White; // Đổi nền xám xịt thành trắng
            dataGridView1.BorderStyle = BorderStyle.None; // Bỏ cái viền 3D cổ điển
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Chỉ kẻ vạch ngang mờ (chuẩn Web)
            dataGridView1.RowHeadersVisible = false; // GIẤU CỘT MŨI TÊN XÁM THỪA THÃI BÊN TRÁI
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Click vào ô nào cũng bôi đen cả dòng

            // 2. Chỉnh cột tự động giãn đều (XÓA KHOẢNG TRỐNG BÊN PHẢI)
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 3. Tuốt lại nhan sắc cho dòng Tiêu đề (Header)
            dataGridView1.EnableHeadersVisualStyles = false; // BẮT BUỘC CÓ DÒNG NÀY thì mới đổi màu được
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None; // Bỏ viền
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185); // Nền xanh dương đậm hiện đại
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Chữ trắng
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold); // Chữ to, in đậm
            dataGridView1.ColumnHeadersHeight = 40; // Tiêu đề cao lên cho thoáng

            // 4. Tuốt lại nhan sắc cho các Dòng dữ liệu (Rows)
            dataGridView1.RowTemplate.Height = 35; // Nâng chiều cao dòng lên (Mặc định lùn tịt làm chữ dính vào nhau)
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 9); // Đổi font mượt

            // Màu khi Lễ tân click vào 1 dòng (Thay màu xanh dương chói lóa bằng màu xám nhạt tinh tế)
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 240, 241);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            // 5. Hiệu ứng sọc ngựa vằn (Zebra striping) - Dòng trắng, dòng xám cực nhạt xen kẽ giúp cực kỳ dễ đọc
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }


        private void QLNhanVienForm_Load(object sender, EventArgs e)
        {
            DinhDangLuoi(); 
            chckTrangThai.Checked = true;
            LoadDanhSachChucVu(); // Tải chức vụ vào ComboBox trước
            LoadDanhSachNhanVien(); // Tải lưới nhân viên sau
        }

        private void quảnLýHồSơToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Ẩn các Panel khác (nếu bạn đã tạo chúng)
            // pnlTaiKhoan.Visible = false;
            // pnlCaLamViec.Visible = false;

            // 2. Bật Panel Quản lý hồ sơ lên
            pnlQLHoSo.Visible = true;

            // 3. Đưa Panel này lên lớp trên cùng để đảm bảo không bị Panel nào khác đè lên
            pnlQLHoSo.BringToFront();

            // 4. (Tùy chọn) Gọi hàm load dữ liệu để làm mới danh sách mỗi khi mở tab này
            // LoadDuLieuNhanVien();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            //chỉ lấy mã NV từ lưới (vì lưới có hiển thị cột này
           string maNV = dataGridView1.Rows[e.RowIndex].Cells["MaNV"].Value.ToString();
            // 2. Dùng Mã NV để gọi Database lấy FULL hồ sơ
            using (var db = new HotelManagementEntities())
            {
                var nhanVien = db.Employees.FirstOrDefault(nv=>nv.MaNV == maNV);

                if(nhanVien!=null)
                {
                    txtMaNV.Text = nhanVien.MaNV;
                    txtHoTen.Text = nhanVien.HoTen;
                    txtSdt.Text = nhanVien.SoDienThoai;
                    txtDiaChi.Text = nhanVien.DiaChi;
                    txtCCCD.Text = nhanVien.CCCD;
             
                    dateTimePicker.Value = nhanVien.NgaySinh.Date;
                    comboBoxChucVu.SelectedValue = nhanVien.MaChucVu;

                    chckTrangThai.Checked = nhanVien.TrangThai ?? true;
                    // XỬ LÝ RADIO BUTTON GIỚI TÍNH
                    if (nhanVien.GioiTinh == "Nam")
                    {
                        radNam.Checked = true; // Bật nút Nam lên (nút Nữ sẽ tự động tắt)
                    }
                    else if (nhanVien.GioiTinh == "Nữ")
                    {
                        radNu.Checked = true; // Bật nút Nữ lên
                    }
                    else
                    {
                        // Đề phòng trường hợp Database bị null hoặc rỗng
                        radNam.Checked = false;
                        radNu.Checked = false;
                    }
                }
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            // Lấy và cắt bỏ khoảng trắng thừa ở hai đầu
            string maNV = txtMaNV.Text.Trim();

            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Chưa nhập mã nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var employees = new HotelManagementEntities())
            {
                // 1. Kiểm tra trùng mã nhân viên TRƯỚC khi thêm để báo lỗi thân thiện hơn
                bool daTonTai = employees.Employees.Any(x => x.MaNV == maNV);
                if (daTonTai)
                {
                    MessageBox.Show("Mã nhân viên này đã tồn tại. Vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNV.Focus();
                    return;
                }

                // 2. Tạo nhân viên mới
                Employee em = new Employee();

                em.MaNV = maNV;
                em.HoTen = txtHoTen.Text.Trim();
                em.CCCD = txtCCCD.Text.Trim();
                em.SoDienThoai = txtSdt.Text.Trim();
                em.DiaChi = txtDiaChi.Text.Trim();
                em.MaChucVu = (int)comboBoxChucVu.SelectedValue;
                em.NgaySinh = dateTimePicker.Value;
                em.NgayVaoLam = DateTime.Now; // Tự động lấy ngày hôm nay
                em.TrangThai = true;          // Mặc định người mới là đang làm việc (True)

                // 3. Rút gọn triệt để phần Giới tính (Xóa bỏ các khối if-else .ToString() bị sai)
                em.GioiTinh = radNam.Checked ? "Nam" : "Nữ";

                try
                {
                    employees.Employees.Add(em);
                    employees.SaveChanges(); // Lưu xuống Database

                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Tải lại lưới
                    txtTimKiem_TextChanged(null, null);
                    ClearForm();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    // Bắt lỗi rỗng, lỗi sai quy tắc dữ liệu từ Entity Framework
                    string chiTietLoi = "";
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            chiTietLoi += $"- Cột [{validationError.PropertyName}]: {validationError.ErrorMessage}\n";
                        }
                    }
                    MessageBox.Show("Lỗi ràng buộc dữ liệu:\n\n" + chiTietLoi, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // Bắt các lỗi hệ thống hoặc SQL khác
                    string loiThucSu = ex.Message;

                    if (ex.InnerException != null)
                    {
                        loiThucSu += "\n\nNguyên nhân gốc: " + ex.InnerException.Message;

                        if (ex.InnerException.InnerException != null)
                        {
                            loiThucSu += "\n\nChi tiết SQL: " + ex.InnerException.InnerException.Message;
                        }
                    }

                    MessageBox.Show("Lỗi hệ thống:\n" + loiThucSu, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btSua_Click(object sender, EventArgs e)
        {
           
                // Lấy mã nhân viên hiện tại từ textbox và loại bỏ khoảng trắng thừa
                string maNV = txtMaNV.Text.Trim();

                if (string.IsNullOrEmpty(maNV))
                {
                    MessageBox.Show("Vui lòng chọn một nhân viên từ danh sách để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var db = new HotelManagementEntities())
                {
                    // 1. Tìm nhân viên trong CSDL dựa vào MaNV
                    var em = db.Employees.FirstOrDefault(x => x.MaNV == maNV);

                    // Kiểm tra xem có tìm thấy nhân viên không
                    if (em == null)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên này trong CSDL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 2. Gán các giá trị mới từ giao diện (Không sửa MaNV vì thường nó là Khóa chính)
                    em.HoTen = txtHoTen.Text.Trim();
                    em.CCCD = txtCCCD.Text.Trim();
                    em.SoDienThoai = txtSdt.Text.Trim();
                    em.DiaChi = txtDiaChi.Text.Trim();
                    em.NgaySinh = dateTimePicker.Value;
                    em.MaChucVu = (int)comboBoxChucVu.SelectedValue;
                    em.TrangThai = chckTrangThai.Checked;
                // Chỉ cần 1 dòng này là đủ cho RadioButton
                em.GioiTinh = radNam.Checked ? "Nam" : "Nữ";

                    // 3. Lưu các thay đổi xuống Cơ sở dữ liệu
                    db.SaveChanges();

                    MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiem_TextChanged(null, null);
                ClearForm();
            }
        }
        private void ClearForm()
        {
            txtTimKiem.Text = "";
            txtMaNV.Text = "";
            txtHoTen.Text = "";
            txtSdt.Text = "";
            txtDiaChi.Text = "";
            txtCCCD.Text = "";
            dateTimePicker.Value = DateTime.Now;
            comboBoxChucVu.SelectedIndex = -1;
            radNam.Checked = true;
            radNu.Checked = false;
            chckTrangThai.Checked = true;

        }
        private void btLamMoi_Click(object sender, EventArgs e)
        {
                ClearForm();
          
            
        }
    }
}
