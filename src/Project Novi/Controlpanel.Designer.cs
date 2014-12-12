namespace Project_Novi
{
    partial class Controlpanel
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
            this.textboxUsernameTwitter1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textboxUsernameTwitter2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textboxUsernameTwitter3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textboxUsernameTwitter1
            // 
            this.textboxUsernameTwitter1.Location = new System.Drawing.Point(152, 77);
            this.textboxUsernameTwitter1.Name = "textboxUsernameTwitter1";
            this.textboxUsernameTwitter1.Size = new System.Drawing.Size(100, 22);
            this.textboxUsernameTwitter1.TabIndex = 0;
            this.textboxUsernameTwitter1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textboxUsernameTwitter1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controlpanel_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(30, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Feed username 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(28, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "Twitter settings ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(30, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Feed username 2:";
            // 
            // textboxUsernameTwitter2
            // 
            this.textboxUsernameTwitter2.Location = new System.Drawing.Point(152, 105);
            this.textboxUsernameTwitter2.Name = "textboxUsernameTwitter2";
            this.textboxUsernameTwitter2.Size = new System.Drawing.Size(100, 22);
            this.textboxUsernameTwitter2.TabIndex = 4;
            this.textboxUsernameTwitter2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controlpanel_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(30, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Feed username 3:";
            // 
            // textboxUsernameTwitter3
            // 
            this.textboxUsernameTwitter3.Location = new System.Drawing.Point(152, 133);
            this.textboxUsernameTwitter3.Name = "textboxUsernameTwitter3";
            this.textboxUsernameTwitter3.Size = new System.Drawing.Size(100, 22);
            this.textboxUsernameTwitter3.TabIndex = 6;
            this.textboxUsernameTwitter3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controlpanel_KeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(166, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 33);
            this.button1.TabIndex = 8;
            this.button1.Text = "Enter to save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controlpanel_KeyDown);
            // 
            // Controlpanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.ClientSize = new System.Drawing.Size(640, 392);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textboxUsernameTwitter3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textboxUsernameTwitter2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textboxUsernameTwitter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Controlpanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Controlpanel";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controlpanel_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textboxUsernameTwitter1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textboxUsernameTwitter2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textboxUsernameTwitter3;
        private System.Windows.Forms.Button button1;
    }
}