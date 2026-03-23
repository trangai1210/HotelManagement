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
            bool onlyActive = checkBoxActive.Checked;

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

            // lọc trạng thái
            if (onlyActive)
            {
                query = query.Where(r => r.Status.ToLower() == "trống");
            }

            dataGridView1.DataSource = query
                .Select(r => new
                {
                    r.RoomID,
                    r.RoomName,
                    TypeName = r.RoomType.TypeName,
                    r.Status
                })
                .ToList();
        }
        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            SearchRoom();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            SearchRoom();
        }
    }
}
