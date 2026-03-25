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

namespace HotelManagement.GUI
{
    public partial class CustomerForm : Form
    {
        string connectionString =
        "Server=.;Database=HotelManagement;Trusted_Connection=True;";
        public CustomerForm()
        {
            InitializeComponent();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            // Placeholder
            SetPlaceholder(txtTenKH, "Họ và tên khách hàng");
            SetPlaceholder(txtCccd, "Mã căn cước công dân");
            SetPlaceholder(txtSDT, "Số điện thoại");
            SetPlaceholder(txtDiaChi, "Địa chỉ");
            SetPlaceholder(txtQuocTich, "Quốc tịch");

            // Giới tính
            cbGioiTinh.Items.Add("Nam");
            cbGioiTinh.Items.Add("Nữ");
            cbGioiTinh.Text = "Giới tính";

            txtCccd.MaxLength = 12;
            txtSDT.MaxLength = 10;
        }

        // ===== PLACEHOLDER FUNCTION =====

        void SetPlaceholder(TextBox txt, string text)
        {
            txt.Text = text;
            txt.ForeColor = Color.Gray;

            txt.Enter += (s, e) =>
            {
                if (txt.ForeColor == Color.Gray)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.Leave += (s, e) =>
            {
                if (txt.Text == "")
                {
                    txt.Text = text;
                    txt.ForeColor = Color.Gray;
                }
            };
        }

        void RemovePlaceholder(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (txt.ForeColor == Color.Gray)
            {
                txt.Text = "";
                txt.ForeColor = Color.Black;
            }
        }

        //void AddPlaceholder(object sender, EventArgs e)
        //{
        //    TextBox txt = (TextBox)sender;

        //    if (txt.Text == "")
        //    {
        //        if (txt == txtTenKH)
        //            SetPlaceholder(txt, "Họ và tên khách hàng");

        //        else if (txt == txtCccd)
        //            SetPlaceholder(txt, "Mã căn cước công dân");

        //        else if (txt == txtSDT)
        //            SetPlaceholder(txt, "Số điện thoại");

        //        else if (txt == txtDiaChi)
        //            SetPlaceholder(txt, "Địa chỉ");

        //        else if (txt == txtQuocTich)
        //            SetPlaceholder(txt, "Quốc tịch");
        //    }
        //}

        // ===== NÚT THÊM =====

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtTenKH.ForeColor == Color.Gray ||
                txtCccd.ForeColor == Color.Gray)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // kiểm tra CCCD trùng
                string check = "SELECT COUNT(*) FROM Customer WHERE CCCD=@cccd";

                SqlCommand checkCmd = new SqlCommand(check, conn);
                checkCmd.Parameters.AddWithValue("@cccd", txtCccd.Text);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("CCCD đã tồn tại!");
                    return;
                }

                // thêm khách hàng
                string query = @"INSERT INTO Customer
                                (TenKH,GioiTinh,CCCD,SDT,DiaChi,QuocTich)
                                VALUES
                                (@TenKH,@GioiTinh,@CCCD,@SDT,@DiaChi,@QuocTich)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("@CCCD", txtCccd.Text);
                cmd.Parameters.AddWithValue("@SDT", txtSDT.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@QuocTich", txtQuocTich.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Thêm khách hàng thành công");

                this.Close();
            }
        }

        // ===== NÚT HỦY =====

        private void btnHuy_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hủy thành công");
            this.Close();
        }

        private void txtTenKH_Enter(object sender, EventArgs e)
        {

        }

        private void txtTenKH_Leave(object sender, EventArgs e)
        {

        }
    }
}