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
            this.buttonApplyZIP = new System.Windows.Forms.Button();
            this.textBoxContentGGPK = new System.Windows.Forms.TextBox();
            this.labelContentGGPKPath = new System.Windows.Forms.Label();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonApplyChinese = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSelectPOE
            // 
            this.buttonSelectPOE.Location = new System.Drawing.Point(414, 41);
            this.buttonSelectPOE.Name = "buttonSelectPOE";
            this.buttonSelectPOE.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectPOE.TabIndex = 0;
            this.buttonSelectPOE.Text = "Select POE";
            this.buttonSelectPOE.UseVisualStyleBackColor = true;
            this.buttonSelectPOE.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonApplyZIP
            // 
            this.buttonApplyZIP.Location = new System.Drawing.Point(414, 71);
            this.buttonApplyZIP.Name = "buttonApplyZIP";
            this.buttonApplyZIP.Size = new System.Drawing.Size(75, 23);
            this.buttonApplyZIP.TabIndex = 1;
            this.buttonApplyZIP.Text = "Apply ZIP";
            this.buttonApplyZIP.UseVisualStyleBackColor = true;
            this.buttonApplyZIP.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxContentGGPK
            // 
            this.textBoxContentGGPK.Location = new System.Drawing.Point(117, 13);
            this.textBoxContentGGPK.Name = "textBoxContentGGPK";
            this.textBoxContentGGPK.Size = new System.Drawing.Size(371, 22);
            this.textBoxContentGGPK.TabIndex = 2;
            // 
            // labelContentGGPKPath
            // 
            this.labelContentGGPKPath.AutoSize = true;
            this.labelContentGGPKPath.Location = new System.Drawing.Point(12, 17);
            this.labelContentGGPKPath.Name = "labelContentGGPKPath";
            this.labelContentGGPKPath.Size = new System.Drawing.Size(92, 12);
            this.labelContentGGPKPath.TabIndex = 4;
            this.labelContentGGPKPath.Text = "Content.ggpk Path";
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(14, 41);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(394, 209);
            this.textBoxOutput.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(450, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "poedb";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(413, 227);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonApplyChinese
            // 
            this.buttonApplyChinese.Location = new System.Drawing.Point(415, 100);
            this.buttonApplyChinese.Name = "buttonApplyChinese";
            this.buttonApplyChinese.Size = new System.Drawing.Size(75, 36);
            this.buttonApplyChinese.TabIndex = 9;
            this.buttonApplyChinese.Text = "Apply Chinese";
            this.buttonApplyChinese.UseVisualStyleBackColor = true;
            this.buttonApplyChinese.Click += new System.EventHandler(this.buttonApplyChinese_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 260);
            this.Controls.Add(this.buttonApplyChinese);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.labelContentGGPKPath);
            this.Controls.Add(this.textBoxContentGGPK);
            this.Controls.Add(this.buttonApplyZIP);
            this.Controls.Add(this.buttonSelectPOE);
            this.Name = "Form1";
            this.Text = "VPatchGGPK  v0.4";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonSelectPOE;
		private System.Windows.Forms.Button buttonApplyZIP;
		private System.Windows.Forms.TextBox textBoxContentGGPK;
		private System.Windows.Forms.Label labelContentGGPKPath;
		private System.Windows.Forms.TextBox textBoxOutput;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonApplyChinese;
    }
}

