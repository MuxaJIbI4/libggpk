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
			this.textBoxContentGGPK = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxOutput = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonExit = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxSmallFont = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxNormalFont = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxLargeFont = new System.Windows.Forms.TextBox();
			this.buttonApplyFont = new System.Windows.Forms.Button();
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
			// textBoxContentGGPK
			// 
			this.textBoxContentGGPK.Location = new System.Drawing.Point(117, 13);
			this.textBoxContentGGPK.Name = "textBoxContentGGPK";
			this.textBoxContentGGPK.Size = new System.Drawing.Size(447, 22);
			this.textBoxContentGGPK.TabIndex = 2;
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
			// textBoxOutput
			// 
			this.textBoxOutput.Location = new System.Drawing.Point(14, 41);
			this.textBoxOutput.Multiline = true;
			this.textBoxOutput.Name = "textBoxOutput";
			this.textBoxOutput.Size = new System.Drawing.Size(550, 209);
			this.textBoxOutput.TabIndex = 6;
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
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 260);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 12);
			this.label3.TabIndex = 9;
			this.label3.Text = "小字";
			// 
			// textBoxSmallFont
			// 
			this.textBoxSmallFont.Location = new System.Drawing.Point(50, 257);
			this.textBoxSmallFont.Name = "textBoxSmallFont";
			this.textBoxSmallFont.Size = new System.Drawing.Size(100, 22);
			this.textBoxSmallFont.TabIndex = 10;
			this.textBoxSmallFont.Text = "26";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(157, 260);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 12);
			this.label4.TabIndex = 11;
			this.label4.Text = "中字";
			// 
			// textBoxNormalFont
			// 
			this.textBoxNormalFont.Location = new System.Drawing.Point(192, 256);
			this.textBoxNormalFont.Name = "textBoxNormalFont";
			this.textBoxNormalFont.Size = new System.Drawing.Size(100, 22);
			this.textBoxNormalFont.TabIndex = 12;
			this.textBoxNormalFont.Text = "33";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(298, 260);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(29, 12);
			this.label5.TabIndex = 13;
			this.label5.Text = "大字";
			// 
			// textBoxLargeFont
			// 
			this.textBoxLargeFont.Location = new System.Drawing.Point(333, 256);
			this.textBoxLargeFont.Name = "textBoxLargeFont";
			this.textBoxLargeFont.Size = new System.Drawing.Size(100, 22);
			this.textBoxLargeFont.TabIndex = 14;
			this.textBoxLargeFont.Text = "45";
			// 
			// buttonApplyFont
			// 
			this.buttonApplyFont.Location = new System.Drawing.Point(439, 256);
			this.buttonApplyFont.Name = "buttonApplyFont";
			this.buttonApplyFont.Size = new System.Drawing.Size(75, 23);
			this.buttonApplyFont.TabIndex = 15;
			this.buttonApplyFont.Text = "修改字體";
			this.buttonApplyFont.UseVisualStyleBackColor = true;
			this.buttonApplyFont.Click += new System.EventHandler(this.buttonApplyFont_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(664, 295);
			this.Controls.Add(this.buttonApplyFont);
			this.Controls.Add(this.textBoxLargeFont);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBoxNormalFont);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxSmallFont);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.buttonExit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxOutput);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxContentGGPK);
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
		private System.Windows.Forms.TextBox textBoxContentGGPK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxOutput;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonExit;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxSmallFont;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxNormalFont;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxLargeFont;
		private System.Windows.Forms.Button buttonApplyFont;
	}
}

