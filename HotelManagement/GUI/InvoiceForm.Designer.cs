namespace HotelManagement.GUI
{
    partial class InvoiceForm
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
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.cboLoc = new System.Windows.Forms.ComboBox();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.dgvHoaDon = new System.Windows.Forms.DataGridView();
            this.chkLocNgay = new System.Windows.Forms.CheckBox();
            this.dtpNgay = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(72, 40);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(198, 22);
            this.txtTimKiem.TabIndex = 0;
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
            // 
            // btnTim
            // 
            this.btnTim.Location = new System.Drawing.Point(0, 40);
            this.btnTim.Name = "btnTim";
            this.btnTim.Size = new System.Drawing.Size(64, 26);
            this.btnTim.TabIndex = 1;
            this.btnTim.Text = "🔍 Tìm";
            this.btnTim.UseVisualStyleBackColor = true;
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
            // 
            // cboLoc
            // 
            this.cboLoc.FormattingEnabled = true;
            this.cboLoc.Location = new System.Drawing.Point(304, 38);
            this.cboLoc.Name = "cboLoc";
            this.cboLoc.Size = new System.Drawing.Size(160, 24);
            this.cboLoc.TabIndex = 2;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(490, 40);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(119, 23);
            this.btnLamMoi.TabIndex = 3;
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // dgvHoaDon
            // 
            this.dgvHoaDon.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoaDon.Location = new System.Drawing.Point(29, 92);
            this.dgvHoaDon.Name = "dgvHoaDon";
            this.dgvHoaDon.RowHeadersWidth = 51;
            this.dgvHoaDon.RowTemplate.Height = 24;
            this.dgvHoaDon.Size = new System.Drawing.Size(945, 404);
            this.dgvHoaDon.TabIndex = 4;
            this.dgvHoaDon.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHoaDon_CellContentClick);
            // 
            // chkLocNgay
            // 
            this.chkLocNgay.AutoSize = true;
            this.chkLocNgay.Location = new System.Drawing.Point(644, 42);
            this.chkLocNgay.Name = "chkLocNgay";
            this.chkLocNgay.Size = new System.Drawing.Size(113, 20);
            this.chkLocNgay.TabIndex = 12;
            this.chkLocNgay.Text = "Lọc theo ngày";
            this.chkLocNgay.UseVisualStyleBackColor = true;
            // 
            // dtpNgay
            // 
            this.dtpNgay.Location = new System.Drawing.Point(774, 40);
            this.dtpNgay.Name = "dtpNgay";
            this.dtpNgay.Size = new System.Drawing.Size(200, 22);
            this.dtpNgay.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpNgay);
            this.groupBox1.Controls.Add(this.chkLocNgay);
            this.groupBox1.Controls.Add(this.dgvHoaDon);
            this.groupBox1.Controls.Add(this.btnLamMoi);
            this.groupBox1.Controls.Add(this.cboLoc);
            this.groupBox1.Controls.Add(this.btnTim);
            this.groupBox1.Controls.Add(this.txtTimKiem);
            this.groupBox1.Location = new System.Drawing.Point(10, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1017, 547);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hóa đơn";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // InvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 580);
            this.Controls.Add(this.groupBox1);
            this.Name = "InvoiceForm";
            this.Text = "InvoiceForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.ComboBox cboLoc;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.CheckBox chkLocNgay;
        private System.Windows.Forms.DateTimePicker dtpNgay;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}