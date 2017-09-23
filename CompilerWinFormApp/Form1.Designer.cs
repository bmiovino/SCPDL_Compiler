namespace CompilerWinFormApp
{
    partial class Form1
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
            this.btnCompile = new System.Windows.Forms.Button();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtCompile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCompile
            // 
            this.btnCompile.BackColor = System.Drawing.Color.Coral;
            this.btnCompile.Location = new System.Drawing.Point(13, 285);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(149, 45);
            this.btnCompile.TabIndex = 0;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = false;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(13, 12);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(1119, 267);
            this.txtCode.TabIndex = 1;
            // 
            // txtCompile
            // 
            this.txtCompile.Location = new System.Drawing.Point(12, 336);
            this.txtCompile.Multiline = true;
            this.txtCompile.Name = "txtCompile";
            this.txtCompile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCompile.Size = new System.Drawing.Size(1119, 267);
            this.txtCompile.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 615);
            this.Controls.Add(this.txtCompile);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.btnCompile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtCompile;
    }
}

