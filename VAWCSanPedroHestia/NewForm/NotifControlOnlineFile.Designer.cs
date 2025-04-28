namespace VAWCSanPedroHestia.NewForm
{
    partial class NotifControlOnlineFile
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnViewComplaint = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(132, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(398, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Complainant has reached out through a formal complaint.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // btnViewComplaint
            // 
            this.btnViewComplaint.BackColor = System.Drawing.Color.Red;
            this.btnViewComplaint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewComplaint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewComplaint.ForeColor = System.Drawing.Color.White;
            this.btnViewComplaint.Location = new System.Drawing.Point(358, 72);
            this.btnViewComplaint.Name = "btnViewComplaint";
            this.btnViewComplaint.Size = new System.Drawing.Size(163, 39);
            this.btnViewComplaint.TabIndex = 3;
            this.btnViewComplaint.Text = "VIEW COMPLAINT";
            this.btnViewComplaint.UseVisualStyleBackColor = false;
            this.btnViewComplaint.Click += new System.EventHandler(this.btnViewComplaint_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::VAWCSanPedroHestia.Properties.Resources.sanpedro14;
            this.pictureBox1.Location = new System.Drawing.Point(24, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(91, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // NotifControlOnlineFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnViewComplaint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "NotifControlOnlineFile";
            this.Size = new System.Drawing.Size(549, 140);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnViewComplaint;
    }
}