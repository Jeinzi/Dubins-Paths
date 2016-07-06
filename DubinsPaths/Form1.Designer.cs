namespace DubinsPaths
{
	partial class Form1
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.labelTrajectoryBox = new System.Windows.Forms.Label();
			this.trajectoryList = new System.Windows.Forms.ListBox();
			this.checkBoxUseShortest = new System.Windows.Forms.CheckBox();
			this.helpBox = new System.Windows.Forms.RichTextBox();
			this.groupBoxHelp = new System.Windows.Forms.GroupBox();
			this.buttonReset = new System.Windows.Forms.Button();
			this.labelLengthCaption = new System.Windows.Forms.Label();
			this.textBoxRadius = new System.Windows.Forms.TextBox();
			this.labelRadius = new System.Windows.Forms.Label();
			this.labelLength = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.groupBoxHelp.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox.Location = new System.Drawing.Point(12, 12);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(607, 437);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
			this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
			this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
			// 
			// labelTrajectoryBox
			// 
			this.labelTrajectoryBox.AutoSize = true;
			this.labelTrajectoryBox.Location = new System.Drawing.Point(625, 12);
			this.labelTrajectoryBox.Name = "labelTrajectoryBox";
			this.labelTrajectoryBox.Size = new System.Drawing.Size(139, 13);
			this.labelTrajectoryBox.TabIndex = 1;
			this.labelTrajectoryBox.Text = "Possible Dubins trajectories:";
			// 
			// trajectoryList
			// 
			this.trajectoryList.FormattingEnabled = true;
			this.trajectoryList.Items.AddRange(new object[] {
            "RSR",
            "RSL",
            "LSR",
            "LSL",
            "RLR",
            "LRL"});
			this.trajectoryList.Location = new System.Drawing.Point(628, 28);
			this.trajectoryList.Name = "trajectoryList";
			this.trajectoryList.Size = new System.Drawing.Size(144, 82);
			this.trajectoryList.TabIndex = 1;
			// 
			// checkBoxUseShortest
			// 
			this.checkBoxUseShortest.AutoSize = true;
			this.checkBoxUseShortest.Location = new System.Drawing.Point(628, 116);
			this.checkBoxUseShortest.Name = "checkBoxUseShortest";
			this.checkBoxUseShortest.Size = new System.Drawing.Size(151, 17);
			this.checkBoxUseShortest.TabIndex = 2;
			this.checkBoxUseShortest.Text = "Always show shortest path";
			this.checkBoxUseShortest.UseVisualStyleBackColor = true;
			this.checkBoxUseShortest.CheckedChanged += new System.EventHandler(this.checkBoxUseShortest_CheckedChanged);
			// 
			// helpBox
			// 
			this.helpBox.BackColor = System.Drawing.SystemColors.Control;
			this.helpBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.helpBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.helpBox.Location = new System.Drawing.Point(6, 19);
			this.helpBox.Name = "helpBox";
			this.helpBox.ReadOnly = true;
			this.helpBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.helpBox.Size = new System.Drawing.Size(135, 115);
			this.helpBox.TabIndex = 4;
			this.helpBox.TabStop = false;
			this.helpBox.Text = "";
			// 
			// groupBoxHelp
			// 
			this.groupBoxHelp.Controls.Add(this.helpBox);
			this.groupBoxHelp.Location = new System.Drawing.Point(625, 278);
			this.groupBoxHelp.Name = "groupBoxHelp";
			this.groupBoxHelp.Size = new System.Drawing.Size(147, 142);
			this.groupBoxHelp.TabIndex = 5;
			this.groupBoxHelp.TabStop = false;
			this.groupBoxHelp.Text = "Help";
			// 
			// buttonReset
			// 
			this.buttonReset.Location = new System.Drawing.Point(625, 426);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(147, 23);
			this.buttonReset.TabIndex = 4;
			this.buttonReset.Text = "Reset";
			this.buttonReset.UseVisualStyleBackColor = true;
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// labelLengthCaption
			// 
			this.labelLengthCaption.AutoSize = true;
			this.labelLengthCaption.Location = new System.Drawing.Point(625, 250);
			this.labelLengthCaption.Name = "labelLengthCaption";
			this.labelLengthCaption.Size = new System.Drawing.Size(43, 13);
			this.labelLengthCaption.TabIndex = 7;
			this.labelLengthCaption.Text = "Length:";
			// 
			// textBoxRadius
			// 
			this.textBoxRadius.Location = new System.Drawing.Point(628, 163);
			this.textBoxRadius.Name = "textBoxRadius";
			this.textBoxRadius.Size = new System.Drawing.Size(144, 20);
			this.textBoxRadius.TabIndex = 3;
			this.textBoxRadius.TextChanged += new System.EventHandler(this.textBoxRadius_TextChanged);
			// 
			// labelRadius
			// 
			this.labelRadius.AutoSize = true;
			this.labelRadius.Location = new System.Drawing.Point(625, 147);
			this.labelRadius.Name = "labelRadius";
			this.labelRadius.Size = new System.Drawing.Size(117, 13);
			this.labelRadius.TabIndex = 9;
			this.labelRadius.Text = "Minimum turning radius:";
			// 
			// labelLength
			// 
			this.labelLength.AutoSize = true;
			this.labelLength.Location = new System.Drawing.Point(674, 250);
			this.labelLength.Name = "labelLength";
			this.labelLength.Size = new System.Drawing.Size(0, 13);
			this.labelLength.TabIndex = 10;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 461);
			this.Controls.Add(this.labelLength);
			this.Controls.Add(this.labelRadius);
			this.Controls.Add(this.textBoxRadius);
			this.Controls.Add(this.labelLengthCaption);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.groupBoxHelp);
			this.Controls.Add(this.checkBoxUseShortest);
			this.Controls.Add(this.trajectoryList);
			this.Controls.Add(this.labelTrajectoryBox);
			this.Controls.Add(this.pictureBox);
			this.Name = "Form1";
			this.Text = "Dubins paths";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.groupBoxHelp.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Label labelTrajectoryBox;
		private System.Windows.Forms.ListBox trajectoryList;
		private System.Windows.Forms.CheckBox checkBoxUseShortest;
		private System.Windows.Forms.RichTextBox helpBox;
		private System.Windows.Forms.GroupBox groupBoxHelp;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.Label labelLengthCaption;
		private System.Windows.Forms.TextBox textBoxRadius;
		private System.Windows.Forms.Label labelRadius;
		private System.Windows.Forms.Label labelLength;
	}
}

