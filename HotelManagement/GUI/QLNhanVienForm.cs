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
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace HotelManagement.GUI
{

    public partial class QLNhanVienForm : Form
    {
        HotelManagementEntities db = new HotelManagementEntities();
        // Biến dùng chung để chứa danh sách lương vừa tính
        List<BangLuong> danhSachLuongThangNay = new List<BangLuong>();

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
            //QLNhanVienForm fNhanVien = new QLNhanVienForm();
            //fNhanVien.MdiParent = this; // Xác định Form chính là cha
            //fNhanVien.Show();
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
                var ketQua = query.Select(nv => new
                {
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
            dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None; // Bỏ cái viền 3D cổ điển
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
                var nhanVien = db.Employees.FirstOrDefault(nv => nv.MaNV == maNV);

                if (nhanVien != null)
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


        //======================TÀI KHOẢN VÀ PHÂN QUYỀN=========================
        //========================================================================

        // Biến toàn cục để nhớ xem Admin đang chọn ai bên lưới
        private string maNvDangChon = "";

        private void LoadNhanVienChuaCoTaiKhoan()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var nguoiCoTK = db.EmployeeAccounts.Select(a => a.MaNV).ToList();

                var dsNhanVien = db.Employees
            .Where(nv => nv.TrangThai == true)
            .Where(nv => nv.MaChucVu == 1 || nv.MaChucVu == 2)
            .Where(nv => !nguoiCoTK.Contains(nv.MaNV))
            .Select(nv => new
            {
                MaNV = nv.MaNV,
                ThongTin = nv.MaNV + " - " + nv.HoTen
            }).ToList();
                comboBoxNhanVien.DataSource = null;

                comboBoxNhanVien.DataSource = dsNhanVien;
                comboBoxNhanVien.DisplayMember = "ThongTin";
                comboBoxNhanVien.ValueMember = "MaNV";
                comboBoxNhanVien.SelectedIndex = -1;
            }
        }



        private void LamMoiForm()
        {
            maNvDangChon = "";
            txtUserName.Text = "";
            txtPass.Text = "";

            comboBoxQuyenHan.SelectedIndex = -1;
            comboBoxNhanVien.SelectedIndex = -1;

            // Mở khóa lại các ô nhập liệu 
            txtUserName.Enabled = true;
            comboBoxNhanVien.Enabled = true;
            LoadNhanVienChuaCoTaiKhoan();
            LoadDanhSachNhanVien();
        }
        private void LoadDSTaiKhoan()
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var dsTaiKhoan = db.EmployeeAccounts.Select(acc => new
                {
                    MaNV = acc.MaNV,
                    HoTen = acc.Employee.HoTen, // Lấy tên từ bảng Employees
                    Username = acc.Username,
                    QuyenHan = acc.QuyenHan
                }).ToList();

                dataGridViewTaiKhoan.DataSource = dsTaiKhoan;

                // Chỉnh lại tiêu đề cột cho chuyên nghiệp (đảm bảo tên dgvTaiKhoan khớp với thiết kế của bạn)
                if (dataGridViewTaiKhoan.Columns["MaNV"] != null)
                {
                    dataGridViewTaiKhoan.Columns["MaNV"].HeaderText = "Mã NV";
                    dataGridViewTaiKhoan.Columns["HoTen"].HeaderText = "Họ và Tên";
                    dataGridViewTaiKhoan.Columns["Username"].HeaderText = "Tên đăng nhập";
                    dataGridViewTaiKhoan.Columns["QuyenHan"].HeaderText = "Quyền hạn";
                }
            }
        }
        private void KhoiTaoTabTaiKhoan()
        {
            LoadNhanVienChuaCoTaiKhoan();
            LoadDSTaiKhoan();

            comboBoxQuyenHan.Items.Clear();
            comboBoxQuyenHan.Items.Add("Quản lý");
            comboBoxQuyenHan.Items.Add("Lễ tân");
        }

        private void comboBoxNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxNhanVien.SelectedIndex == -1 || comboBoxNhanVien.SelectedValue == null) return;
            string maNV = comboBoxNhanVien.SelectedValue.ToString();

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var nv = db.Employees.FirstOrDefault(n => n.MaNV == maNV);

            }
        }

        private void tàiKhoảnPhânQuyềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KhoiTaoTabTaiKhoan();

            pnlTaiKhoan.Visible = true;

            // 3. Đưa Panel này lên lớp trên cùng để đảm bảo không bị Panel nào khác đè lên
            pnlTaiKhoan.BringToFront();

        }

        private void btTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã nhập đủ chưa
            if (comboBoxNhanVien.SelectedValue == null || txtUserName.Text.Trim() == "" || comboBoxQuyenHan.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                string userMoi = txtUserName.Text.Trim();

                // 2. CHỐT CHẶN BẢO MẬT: Kiểm tra trùng tên đăng nhập
                if (db.EmployeeAccounts.Any(a => a.Username == userMoi))
                {
                    MessageBox.Show("Tên đăng nhập này đã có người sử dụng! Vui lòng chọn tên khác.", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label.Focus();
                    return;
                }

                // 3. Tiến hành tạo tài khoản mới
                EmployeeAccount acc = new EmployeeAccount();
                acc.MaNV = comboBoxNhanVien.SelectedValue.ToString();
                acc.Username = userMoi;
                acc.Password = "123456";
                acc.QuyenHan = comboBoxQuyenHan.SelectedItem.ToString();

                db.EmployeeAccounts.Add(acc);
                db.SaveChanges(); // Lưu xuống DB

                MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 4. Load lại giao diện và xóa trắng các ô
                LoadNhanVienChuaCoTaiKhoan();
                LoadDSTaiKhoan();
                LamMoiForm();
            }
        }

        private void btCapNhat_Click(object sender, EventArgs e)
        {
            if (maNvDangChon == "")
            {
                MessageBox.Show("Vui lòng chọn một tài khoản trên lưới để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                var acc = db.EmployeeAccounts.FirstOrDefault(a => a.MaNV == maNvDangChon);
                if (acc != null)
                {
                    acc.QuyenHan = comboBoxQuyenHan.SelectedItem.ToString();
                    db.SaveChanges();

                    MessageBox.Show("Cập nhật quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDSTaiKhoan();
                    LamMoiForm();
                }
            }
        }

        private void dataGridViewTK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewTaiKhoan.Rows[e.RowIndex];

                maNvDangChon = row.Cells["MaNV"].Value.ToString();
                string hoTen = row.Cells["HoTen"].Value != null ? row.Cells["HoTen"].Value.ToString() : "Chưa có tên";


                txtUserName.Text = row.Cells["Username"].Value.ToString();
                txtPass.Text = "******";
                comboBoxQuyenHan.SelectedItem = row.Cells["QuyenHan"].Value.ToString();

                // 1. Lấy thông tin từ dòng được click một cách an toàn (Chống lỗi Null)

                // 2. TẠO DANH SÁCH ẢO
                var dsTam = new List<object> {
                new { MaNV = maNvDangChon, ThongTin = maNvDangChon + " - " + hoTen }
                };

                comboBoxNhanVien.DataSource = dsTam;
                comboBoxNhanVien.DisplayMember = "ThongTin";
                comboBoxNhanVien.ValueMember = "MaNV";
                comboBoxNhanVien.SelectedValue = maNvDangChon;

                // Khóa các ô nhạy cảm
                txtUserName.Enabled = false;
                comboBoxNhanVien.Enabled = false;

            }
        }

        private void btDatLaiMk_Click(object sender, EventArgs e)
        {
            if (maNvDangChon == "") return;
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đặt lại mật khẩu thành '123456' cho tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var acc = db.EmployeeAccounts.FirstOrDefault(a => a.MaNV == maNvDangChon);
                    if (acc != null)
                    {
                        acc.Password = "123456";//dua ve mac dinh
                        db.SaveChanges();
                        MessageBox.Show($"Đã Reset mật khẩu của tài khoản '{acc.Username}' về: 123456", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LamMoiForm();
                    }
                }
            }
        }

        private void btThuHoi_Click(object sender, EventArgs e)
        {
            if (maNvDangChon == "") return;

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thu hồi (xóa) tài khoản này? Nhân viên sẽ không thể đăng nhập được nữa.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (HotelManagementEntities db = new HotelManagementEntities())
                {
                    var acc = db.EmployeeAccounts.FirstOrDefault(a => a.MaNV == maNvDangChon);
                    if (acc != null)
                    {
                        db.EmployeeAccounts.Remove(acc);
                        db.SaveChanges();

                        MessageBox.Show("Đã thu hồi tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadNhanVienChuaCoTaiKhoan(); // Load lại để người này hiện lại lên ComboBox
                        LoadDSTaiKhoan();
                        LamMoiForm();
                    }
                }
            }
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            LamMoiForm();

        }
        //=================BẢNG LƯƠNG==================
        private double TongGioLam(string maNV, int thang, int nam)
        {
            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                //lọc ca làm việc đã check out
                var dsChamCong = db.ChamCongs.Where(cc =>
                cc.MaNV == maNV &&
                cc.GioRa != null &&
                cc.NgayCC.Month == thang &&
                cc.NgayCC.Year == nam).ToList();

                double tongGioCoHeSo = 0;
                foreach (var cc in dsChamCong)
                {
                    TimeSpan thoiGianLam = cc.GioRa.Value - cc.GioVao.Value;
                    double giolam = thoiGianLam.TotalHours;

                    double heSo = 1.0;
                    if (cc.MaCa != null)
                    {
                        var ca = db.CaLamViecs.Find(cc.MaCa);
                        if (ca != null) heSo = (double)ca.HeSoLuong;
                    }

                    tongGioCoHeSo += (giolam * heSo);
                }
                return Math.Round(tongGioCoHeSo, 2);
            }

        }

        private void btLuong_Click(object sender, EventArgs e)
        {
            // Làm sạch danh sách nếu người dùng bấm tính lại nhiều lần
            danhSachLuongThangNay.Clear();

            int thang = dtpThangNam.Value.Month;
            int nam = dtpThangNam.Value.Year;

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                // Lấy danh sách nhân viên ĐANG LÀM VIỆC kèm theo thông tin CHỨC VỤ (Role)
                // Lưu ý: Nếu bị lỗi chữ Include, bạn thêm dòng "using System.Data.Entity;" lên đầu file nhé.
                var tatCaNV = db.Employees.Include("Role").Where(nv => nv.TrangThai == true).ToList();

                foreach (var nv in tatCaNV)
                {
                    double tongGio = TongGioLam(nv.MaNV, thang, nam);

                    // Bỏ qua những người tháng này không đi làm
                    if (tongGio >= 0)
                    {
                        // Lấy mức lương tự động từ Chức Vụ (Nếu chức vụ chưa có lương thì mặc định = 0)
                        decimal luongCuaChucVuNay = 0;
                        if (nv.Role != null && nv.Role.LuongMoiGio != null)
                        {
                            luongCuaChucVuNay = (decimal)nv.Role.LuongMoiGio;
                        }

                        BangLuong luongNV = new BangLuong();
                        luongNV.MaNV = nv.MaNV;
                        // luongNV.HoTen = nv.HoTen;
                        luongNV.TongGioLam = tongGio;
                        luongNV.LuongTheoGio = luongCuaChucVuNay;
                        luongNV.TongTien = (decimal)tongGio * luongCuaChucVuNay; // Công thức: Giờ x Lương

                        danhSachLuongThangNay.Add(luongNV);
                    }
                }

                // Đổ dữ liệu lên giao diện (DataGridView)
                dgvBangLuong.DataSource = null;
                dgvBangLuong.DataSource = danhSachLuongThangNay;

                // Mở khóa nút Lưu
                btLuu.Enabled = true;

                MessageBox.Show($"Đã tự động tính xong lương cho {danhSachLuongThangNay.Count} nhân viên!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            if (danhSachLuongThangNay.Count == 0) return;

            string thangNamChuoi = dtpThangNam.Value.ToString("MM/yyyy");

            using (HotelManagementEntities db = new HotelManagementEntities())
            {
                foreach (var item in danhSachLuongThangNay)
                {
                    // Kiểm tra chống lưu trùng 2 lần trong 1 tháng
                    bool daTonTai = db.BangLuongs.Any(b => b.MaNV == item.MaNV && b.ThangNam == thangNamChuoi);

                    if (!daTonTai)
                    {
                        BangLuong bl = new BangLuong();
                        bl.MaNV = item.MaNV;
                        bl.ThangNam = thangNamChuoi;
                        bl.TongGioLam = item.TongGioLam;
                        bl.LuongTheoGio = item.LuongTheoGio;
                        bl.TienThuong = 0; // Bản này không dùng thưởng phạt nên gắn cứng = 0
                        bl.TienPhat = 0;
                        bl.TongTien = item.TongTien;
                        bl.NgayLapPhieu = DateTime.Now;

                        db.BangLuongs.Add(bl);
                    }
                }

                db.SaveChanges();

                MessageBox.Show($"Đã lưu toàn bộ Bảng lương tháng {thangNamChuoi} vào hệ thống thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Lưu xong thì khóa nút lại để khỏi bấm nhầm
                btLuu.Enabled = false;
            }
        }
    }
}
