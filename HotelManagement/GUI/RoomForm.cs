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
using System.Data.Entity;
using System.Security.Policy;
using System.Net.NetworkInformation;

namespace HotelManagement.GUI
{
    public partial class RoomForm : Form
    {
        public RoomForm()
        {
            InitializeComponent();
        }

        private void SearchRoom()
        {
            string keyword = txtTimKiem.Text.ToLower();
         

            HotelManagementEntities db = new HotelManagementEntities();

            var query = db.Rooms
                          .Include(r => r.RoomType)
                          .AsQueryable();

            // tìm theo tên phòng hoặc loại phòng
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(r =>
                    r.RoomName.ToLower().Contains(keyword) ||
                    r.RoomType.TypeName.ToLower().Contains(keyword)
                );
            }

      

            dataGridView1.DataSource = query
                .Select(r => new
                {
                    r.RoomID,
                    r.RoomName,
                    r.RoomTypeID,
                    TypeName = r.RoomType.TypeName,
                    r.Status,
                    r.Floor
                })
                .ToList();

            // Ẩn cột RoomTypeID vì không muốn người dùng nhìn thấy
            if (dataGridView1.Columns["RoomTypeID"] != null)
                dataGridView1.Columns["RoomTypeID"].Visible = false;
        }
     

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            SearchRoom();
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {
            // hiên dữ liệu phòng lên grid
            SearchRoom();

            using ( var rooms = new HotelManagementEntities())
            {
                var list = rooms.RoomTypes.ToList();

                comboBoxLoaiPhong.DataSource = list;

                comboBoxLoaiPhong.DisplayMember = "TypeName";
                comboBoxLoaiPhong.ValueMember = "RoomTypeID";

            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
           if(string.IsNullOrEmpty(txtMaPhong.Text))
            {
                MessageBox.Show("Chưa nhập tên phòng");
                return;
            }

           using (var rooms = new HotelManagementEntities())
            {
                //tạo phòng mới
                Room r = new Room();

                r.RoomName = txtMaPhong.Text;
                // lấy mẫ loại phòng
                r.RoomTypeID = (int)comboBoxLoaiPhong.SelectedValue;
                r.Status = "Trống";

                //lấy giá trị tâng\]U
                r.Floor=(int)numericUpDownTang.Value;

                //Thêm vào database
                rooms.Rooms.Add(r);
                rooms.SaveChanges();

                MessageBox.Show("Thêm thành công!");
                SearchRoom();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
         
            //chặn click vào header
            if (e.RowIndex < 0) return;

            // lấy object từ dòng được chọn
            //var phong = dataGridView1.Rows[e.RowIndex].DataBoundItem as Room; // do là dạng data table nên không dùng bounditem được
            //MessageBox.Show(phong == null ? "phong = null" : phong.GetType().ToString());
            //if (phong != null)
            //{
            //    txtMaPhong.Text = phong.RoomName;
            //    comboBoxLoaiPhong.SelectedValue = phong.RoomTypeID;
            //    numericUpDownTang.Value = (int) phong.Floor;
            //    comboBoxTrangThai.Text = phong.Status;

            //}

            var row = dataGridView1.Rows[e.RowIndex];
            txtMaPhong.Text = row.Cells["RoomName"].Value.ToString();
            int roomTypeID = 0;
            if (row.Cells["RoomTypeID"].Value != DBNull.Value)
                roomTypeID = Convert.ToInt32(row.Cells["RoomTypeID"].Value);
            comboBoxLoaiPhong.SelectedValue = roomTypeID;
            comboBoxTrangThai.Text = row.Cells["Status"].Value.ToString() ;
            numericUpDownTang.Value = Convert.ToDecimal(row.Cells["Floor"].Value);
        }

        
        private void btSua_Click(object sender, EventArgs e)
        {
            // lấy mã phòng hiện tại từ textbox
            string maPhong = txtMaPhong.Text;

            if (string.IsNullOrEmpty(maPhong))
            {
                MessageBox.Show("Vui lòng chọn một phòng từ danh sách để sửa!", "Thông báo");
                return;
            }

            using (var db = new HotelManagementEntities())
            {
                // 2. Tìm phòng trong CSDL bằng LINQ
                // Thay SingleOrDefault bằng FirstOrDefault
                var phong = db.Rooms.FirstOrDefault(p => p.RoomName == maPhong);
                if (phong != null)
                {
                    try
                    {
                        // 3. Cập nhật thông tin từ các Control bên trái vào đối tượng
                        phong.RoomTypeID = (int)comboBoxLoaiPhong.SelectedValue; // Nếu dùng ValueMember
                        phong.Floor = (int)numericUpDownTang.Value; // NumericUpDown dùng .Value
                        phong.Status =comboBoxTrangThai.Text;
                       

                        // 4. Lưu thay đổi xuống Database
                        db.SaveChanges();

                        MessageBox.Show("Cập nhật thông tin phòng thành công!", "Thành công");

                        // 5. Làm mới lại DataGridView
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phòng có mã này!");
                }
            }
        }

        private void LoadData()
        {
            using (var db = new HotelManagementEntities())
            {
                // Lấy danh sách phòng từ Database và gán vào Grid
                var danhSach = db.Rooms.Select(p => new {
                    p.RoomID,
                    p.RoomName,
                    p.RoomTypeID,
                    TypeName = p.RoomType.TypeName, // Lấy tên loại phòng từ bảng liên kết
                    p.Status,
                    p.Floor
                }).ToList();

                dataGridView1.DataSource = danhSach;
            }
        }

        private void ClearInput()
        {
            txtMaPhong.Text = string.Empty;
            comboBoxLoaiPhong.SelectedIndex = -1;
            comboBoxTrangThai.SelectedIndex = -1;
            numericUpDownTang.Value = 0;
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            string maPhong = txtMaPhong.Text;

            if (string.IsNullOrEmpty(maPhong))
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa từ danh sách!", "Thông báo");
                return;
            }
            // 2. Hiện hộp thoại xác nhận
            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa phòng {maPhong} không?",
                                       "Xác nhận xóa",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                using (var db = new HotelManagementEntities())
                {
                    try
                    {
                        var phong = db.Rooms.FirstOrDefault(p => p.RoomName == maPhong);
                        if (phong != null)
                        {
                        // 4. Lệnh xóa đối tượng
                        db.Rooms.Remove(phong);

                        // 5. Lưu thay đổi xuống Database
                        db.SaveChanges();

                        MessageBox.Show("Xóa phòng thành công!", "Thành công");

                        // 6. Làm mới lại bảng và xóa trắng các ô nhập liệu
                        LoadData();
                        ClearInput();
                        }
                        else
                        {
                        MessageBox.Show("Không tìm thấy dữ liệu để xóa.");
                        }
                    }
                     catch (Exception ex)
                    {
                        // Lỗi này thường xảy ra nếu phòng này đang có hóa đơn hoặc khách ở (ràng buộc khóa ngoại)
                        string thongBaoLoi = "LỖI THỰC SỰ LÀ:\n" + ex.Message;
                            //MessageBox.Show("Không thể xóa phòng này vì dữ liệu đang được sử dụng ở bảng khác!");
                        MessageBox.Show(thongBaoLoi);
                    }
                }
            }
        }

        private void groupboxTimKiem_Enter(object sender, EventArgs e)
        {

        }
    }
}
