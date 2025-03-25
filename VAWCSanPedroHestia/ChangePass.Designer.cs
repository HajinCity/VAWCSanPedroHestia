namespace VAWCSanPedroHestia
{
    partial class ChangePass
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
            this.Oldpasstxtbox = new System.Windows.Forms.TextBox();
            this.Newpasstxtbox = new System.Windows.Forms.TextBox();
            this.Chngpasstxtbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Changpassbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(51)))), ((int)(((byte)(140)))));
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(912, 75);
            this.panel1.TabIndex = 0;
            // 
            // Oldpasstxtbox
            // 
            this.Oldpasstxtbox.Location = new System.Drawing.Point(257, 135);
            this.Oldpasstxtbox.Multiline = true;
            this.Oldpasstxtbox.Name = "Oldpasstxtbox";
            this.Oldpasstxtbox.Size = new System.Drawing.Size(404, 51);
            this.Oldpasstxtbox.TabIndex = 1;
            // 
            // Newpasstxtbox
            // 
            this.Newpasstxtbox.Location = new System.Drawing.Point(257, 237);
            this.Newpasstxtbox.Multiline = true;
            this.Newpasstxtbox.Name = "Newpasstxtbox";
            this.Newpasstxtbox.Size = new System.Drawing.Size(404, 51);
            this.Newpasstxtbox.TabIndex = 2;
            // 
            // Chngpasstxtbox
            // 
            this.Chngpasstxtbox.Location = new System.Drawing.Point(257, 335);
            this.Chngpasstxtbox.Multiline = true;
            this.Chngpasstxtbox.Name = "Chngpasstxtbox";
            this.Chngpasstxtbox.Size = new System.Drawing.Size(404, 51);
            this.Chngpasstxtbox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 11.25F);
            this.label1.Location = new System.Drawing.Point(256, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 22);
            this.label1.TabIndex = 4;
            this.label1.Text = "OLD PASSWORD";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 11.25F);
            this.label2.Location = new System.Drawing.Point(256, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 22);
            this.label2.TabIndex = 5;
            this.label2.Text = "NEW PASSWORD";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 11.25F);
            this.label3.Location = new System.Drawing.Point(256, 309);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 22);
            this.label3.TabIndex = 6;
            this.label3.Text = "CHANGE PASSWORD";
            // 
            // Changpassbtn
            // 
            this.Changpassbtn.BackgroundImage = global::VAWCSanPedroHestia.Properties.Resources.rect9;
            this.Changpassbtn.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.Changpassbtn.Location = new System.Drawing.Point(303, 417);
            this.Changpassbtn.Name = "Changpassbtn";
            this.Changpassbtn.Size = new System.Drawing.Size(308, 79);
            this.Changpassbtn.TabIndex = 7;
            this.Changpassbtn.Text = "Change Password";
            this.Changpassbtn.UseVisualStyleBackColor = false;
            // 
            // ChangePass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 538);
            this.Controls.Add(this.Changpassbtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Chngpasstxtbox);
            this.Controls.Add(this.Newpasstxtbox);
            this.Controls.Add(this.Oldpasstxtbox);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChangePass";
            this.Text = "ChangePass";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox Oldpasstxtbox;
        private System.Windows.Forms.TextBox Newpasstxtbox;
        private System.Windows.Forms.TextBox Chngpasstxtbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Changpassbtn;
    }
}