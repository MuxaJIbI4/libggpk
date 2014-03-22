namespace VPatchGGPK
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
			this.buttonSelectPOE = new System.Windows.Forms.Button();
			this.buttonTranslate = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonExit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonSelectPOE
			// 
			this.buttonSelectPOE.Location = new System.Drawing.Point(581, 12);
			this.buttonSelectPOE.Name = "buttonSelectPOE";
			this.buttonSelectPOE.Size = new System.Drawing.Size(75, 23);
			this.buttonSelectPOE.TabIndex = 0;
			this.buttonSelectPOE.Text = "選擇 POE";
			this.buttonSelectPOE.UseVisualStyleBackColor = true;
			this.buttonSelectPOE.Click += new System.EventHandler(this.button1_Click);
			// 
			// buttonTranslate
			// 
			this.buttonTranslate.Location = new System.Drawing.Point(581, 41);
			this.buttonTranslate.Name = "buttonTranslate";
			this.buttonTranslate.Size = new System.Drawing.Size(75, 23);
			this.buttonTranslate.TabIndex = 1;
			this.buttonTranslate.Text = "中文化";
			this.buttonTranslate.UseVisualStyleBackColor = true;
			this.buttonTranslate.Click += new System.EventHandler(this.button2_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(117, 13);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(447, 22);
			this.textBox1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "Content.ggpk 路徑";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(14, 41);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(550, 209);
			this.textBox3.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(623, 238);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 12);
			this.label2.TabIndex = 7;
			this.label2.Text = "statue";
			// 
			// buttonExit
			// 
			this.buttonExit.Location = new System.Drawing.Point(581, 214);
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.Size = new System.Drawing.Size(75, 23);
			this.buttonExit.TabIndex = 8;
			this.buttonExit.Text = "關閉";
			this.buttonExit.UseVisualStyleBackColor = true;
			this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(664, 262);
			this.Controls.Add(this.buttonExit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.buttonTranslate);
			this.Controls.Add(this.buttonSelectPOE);
			this.Name = "Form1";
			this.Text = "VPatchGGPK";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonSelectPOE;
		private System.Windows.Forms.Button buttonTranslate;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonExit;
	}
}

