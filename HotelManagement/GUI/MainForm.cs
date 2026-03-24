using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenChildForm(Form childForm)
        {
            panelContent.Controls.Clear();

            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;

            panelContent.Controls.Add(childForm);
            childForm.Show();
        }

        private void btRoom_Click(object sender, EventArgs e)
        {
            OpenChildForm(new RoomForm());
        }

        private void btCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CustomerForm());
        }
    }
}
