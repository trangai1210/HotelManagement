namespace HotelManagement.GUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btReport = new System.Windows.Forms.Button();
            this.btInvoice = new System.Windows.Forms.Button();
            this.btBooking = new System.Windows.Forms.Button();
            this.btCustomer = new System.Windows.Forms.Button();
            this.btRoom = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScrollMargin = new System.Drawing.Size(220, 0);
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.panel1.Controls.Add(this.btReport);
            this.panel1.Controls.Add(this.btInvoice);
            this.panel1.Controls.Add(this.btBooking);
            this.panel1.Controls.Add(this.btCustomer);
            this.panel1.Controls.Add(this.btRoom);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(178, 661);
            this.panel1.TabIndex = 0;
            // 
            // btReport
            // 
            this.btReport.BackColor = System.Drawing.Color.CadetBlue;
            this.btReport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btReport.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btReport.ForeColor = System.Drawing.Color.White;
            this.btReport.Location = new System.Drawing.Point(0, 380);
            this.btReport.Name = "btReport";
            this.btReport.Size = new System.Drawing.Size(178, 98);
            this.btReport.TabIndex = 4;
            this.btReport.Text = "Báo Cáo";
            this.btReport.UseVisualStyleBackColor = false;
            // 
            // btInvoice
            // 
            this.btInvoice.BackColor = System.Drawing.Color.CadetBlue;
            this.btInvoice.Dock = System.Windows.Forms.DockStyle.Top;
            this.btInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btInvoice.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btInvoice.ForeColor = System.Drawing.Color.White;
            this.btInvoice.Location = new System.Drawing.Point(0, 285);
            this.btInvoice.Name = "btInvoice";
            this.btInvoice.Size = new System.Drawing.Size(178, 95);
            this.btInvoice.TabIndex = 3;
            this.btInvoice.Text = "Thanh Toán";
            this.btInvoice.UseVisualStyleBackColor = false;
            // 
            // btBooking
            // 
            this.btBooking.BackColor = System.Drawing.Color.CadetBlue;
            this.btBooking.Dock = System.Windows.Forms.DockStyle.Top;
            this.btBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBooking.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBooking.ForeColor = System.Drawing.Color.White;
            this.btBooking.Location = new System.Drawing.Point(0, 190);
            this.btBooking.Name = "btBooking";
            this.btBooking.Size = new System.Drawing.Size(178, 95);
            this.btBooking.TabIndex = 2;
            this.btBooking.Text = "Đặt Phòng";
            this.btBooking.UseVisualStyleBackColor = false;
            // 
            // btCustomer
            // 
            this.btCustomer.BackColor = System.Drawing.Color.CadetBlue;
            this.btCustomer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCustomer.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCustomer.ForeColor = System.Drawing.Color.White;
            this.btCustomer.Location = new System.Drawing.Point(0, 95);
            this.btCustomer.Name = "btCustomer";
            this.btCustomer.Size = new System.Drawing.Size(178, 95);
            this.btCustomer.TabIndex = 1;
            this.btCustomer.Text = "Khách Hàng";
            this.btCustomer.UseVisualStyleBackColor = false;
            // 
            // btRoom
            // 
            this.btRoom.BackColor = System.Drawing.Color.CadetBlue;
            this.btRoom.Dock = System.Windows.Forms.DockStyle.Top;
            this.btRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRoom.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRoom.ForeColor = System.Drawing.Color.White;
            this.btRoom.Location = new System.Drawing.Point(0, 0);
            this.btRoom.Name = "btRoom";
            this.btRoom.Size = new System.Drawing.Size(178, 95);
            this.btRoom.TabIndex = 0;
            this.btRoom.Text = "Phòng";
            this.btRoom.UseVisualStyleBackColor = false;
            this.btRoom.Click += new System.EventHandler(this.btRoom_Click);
            // 
            // panelContent
            // 
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(178, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1029, 661);
            this.panelContent.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1207, 661);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btRoom;
        private System.Windows.Forms.Button btReport;
        private System.Windows.Forms.Button btInvoice;
        private System.Windows.Forms.Button btBooking;
        private System.Windows.Forms.Button btCustomer;
        private System.Windows.Forms.Panel panelContent;
    }
}