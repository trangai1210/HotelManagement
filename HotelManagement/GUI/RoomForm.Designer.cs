namespace HotelManagement.GUI
{
    partial class RoomForm
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
            this.groupboxTimKiem = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxThongTinPhong = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBoxLoaiPhong = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.numericUpDownTang = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMaPhong = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.entityCommand1 = new System.Data.Entity.Core.EntityClient.EntityCommand();
            this.groupboxTimKiem.SuspendLayout();
            this.groupBoxThongTinPhong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupboxTimKiem
            // 
            this.groupboxTimKiem.Controls.Add(this.checkBox1);
            this.groupboxTimKiem.Controls.Add(this.textBox1);
            this.groupboxTimKiem.Controls.Add(this.label1);
            this.groupboxTimKiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupboxTimKiem.Location = new System.Drawing.Point(31, 43);
            this.groupboxTimKiem.Name = "groupboxTimKiem";
            this.groupboxTimKiem.Size = new System.Drawing.Size(870, 93);
            this.groupboxTimKiem.TabIndex = 0;
            this.groupboxTimKiem.TabStop = false;
            this.groupboxTimKiem.Text = "Tìm kiếm";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(441, 46);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(265, 29);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Chỉ phòng đang hoạt động";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(94, 46);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(309, 30);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tìm:";
            // 
            // groupBoxThongTinPhong
            // 
            this.groupBoxThongTinPhong.Controls.Add(this.comboBox2);
            this.groupBoxThongTinPhong.Controls.Add(this.comboBoxLoaiPhong);
            this.groupBoxThongTinPhong.Controls.Add(this.checkBox2);
            this.groupBoxThongTinPhong.Controls.Add(this.numericUpDownTang);
            this.groupBoxThongTinPhong.Controls.Add(this.label6);
            this.groupBoxThongTinPhong.Controls.Add(this.label5);
            this.groupBoxThongTinPhong.Controls.Add(this.label4);
            this.groupBoxThongTinPhong.Controls.Add(this.label3);
            this.groupBoxThongTinPhong.Controls.Add(this.txtMaPhong);
            this.groupBoxThongTinPhong.Controls.Add(this.label2);
            this.groupBoxThongTinPhong.Controls.Add(this.button4);
            this.groupBoxThongTinPhong.Controls.Add(this.button3);
            this.groupBoxThongTinPhong.Controls.Add(this.button2);
            this.groupBoxThongTinPhong.Controls.Add(this.button1);
            this.groupBoxThongTinPhong.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxThongTinPhong.Location = new System.Drawing.Point(31, 142);
            this.groupBoxThongTinPhong.Name = "groupBoxThongTinPhong";
            this.groupBoxThongTinPhong.Size = new System.Drawing.Size(455, 484);
            this.groupBoxThongTinPhong.TabIndex = 1;
            this.groupBoxThongTinPhong.TabStop = false;
            this.groupBoxThongTinPhong.Text = "Thông tin phòng";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(126, 232);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(297, 33);
            this.comboBox2.TabIndex = 13;
            // 
            // comboBoxLoaiPhong
            // 
            this.comboBoxLoaiPhong.FormattingEnabled = true;
            this.comboBoxLoaiPhong.Location = new System.Drawing.Point(126, 118);
            this.comboBoxLoaiPhong.Name = "comboBoxLoaiPhong";
            this.comboBoxLoaiPhong.Size = new System.Drawing.Size(297, 33);
            this.comboBoxLoaiPhong.TabIndex = 12;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(118, 287);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(173, 29);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "Đang hoạt động";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // numericUpDownTang
            // 
            this.numericUpDownTang.Location = new System.Drawing.Point(126, 180);
            this.numericUpDownTang.Name = "numericUpDownTang";
            this.numericUpDownTang.Size = new System.Drawing.Size(297, 30);
            this.numericUpDownTang.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 344);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 25);
            this.label6.TabIndex = 8;
            this.label6.Text = "Ghi chú:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 25);
            this.label5.TabIndex = 7;
            this.label5.Text = "Trạng thái:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Tầng:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Loại phòng:";
            // 
            // txtMaPhong
            // 
            this.txtMaPhong.Location = new System.Drawing.Point(126, 60);
            this.txtMaPhong.Name = "txtMaPhong";
            this.txtMaPhong.Size = new System.Drawing.Size(297, 30);
            this.txtMaPhong.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mã phòng:";
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(129, 426);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(108, 39);
            this.button4.TabIndex = 4;
            this.button4.Text = "Sửa";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(222, 426);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(108, 39);
            this.button3.TabIndex = 3;
            this.button3.Text = "Xóa";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(315, 426);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 39);
            this.button2.TabIndex = 2;
            this.button2.Text = "Thêm";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(36, 426);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 39);
            this.button1.TabIndex = 1;
            this.button1.Text = "Thêm";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(519, 178);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(382, 429);
            this.dataGridView1.TabIndex = 0;
            // 
            // entityCommand1
            // 
            this.entityCommand1.CommandTimeout = 0;
            this.entityCommand1.CommandTree = null;
            this.entityCommand1.Connection = null;
            this.entityCommand1.EnablePlanCaching = true;
            this.entityCommand1.Transaction = null;
            // 
            // RoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 638);
            this.Controls.Add(this.groupBoxThongTinPhong);
            this.Controls.Add(this.groupboxTimKiem);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RoomForm";
            this.Text = "RoomForm";
            this.groupboxTimKiem.ResumeLayout(false);
            this.groupboxTimKiem.PerformLayout();
            this.groupBoxThongTinPhong.ResumeLayout(false);
            this.groupBoxThongTinPhong.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupboxTimKiem;
        private System.Windows.Forms.GroupBox groupBoxThongTinPhong;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Data.Entity.Core.EntityClient.EntityCommand entityCommand1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtMaPhong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBoxLoaiPhong;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownTang;
    }
}