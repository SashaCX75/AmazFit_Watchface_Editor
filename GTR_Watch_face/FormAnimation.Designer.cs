namespace GTR_Watch_face
{
    partial class FormAnimation
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAnimation));
            this.pictureBox_AnimatiomPreview = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton_xlarge = new System.Windows.Forms.RadioButton();
            this.radioButton_large = new System.Windows.Forms.RadioButton();
            this.radioButton_normal = new System.Windows.Forms.RadioButton();
            this.button_SaveAnimation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_NumberOfFrames = new System.Windows.Forms.NumericUpDown();
            this.progressBar_SaveAnimation = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_AnimatiomPreview)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_NumberOfFrames)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_AnimatiomPreview
            // 
            this.pictureBox_AnimatiomPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_AnimatiomPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_AnimatiomPreview.Location = new System.Drawing.Point(2, 2);
            this.pictureBox_AnimatiomPreview.Name = "pictureBox_AnimatiomPreview";
            this.pictureBox_AnimatiomPreview.Size = new System.Drawing.Size(456, 456);
            this.pictureBox_AnimatiomPreview.TabIndex = 0;
            this.pictureBox_AnimatiomPreview.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numericUpDown_NumberOfFrames);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button_SaveAnimation);
            this.panel1.Controls.Add(this.radioButton_xlarge);
            this.panel1.Controls.Add(this.radioButton_large);
            this.panel1.Controls.Add(this.radioButton_normal);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 461);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 57);
            this.panel1.TabIndex = 1;
            // 
            // radioButton_xlarge
            // 
            this.radioButton_xlarge.AutoSize = true;
            this.radioButton_xlarge.Location = new System.Drawing.Point(97, 4);
            this.radioButton_xlarge.Name = "radioButton_xlarge";
            this.radioButton_xlarge.Size = new System.Drawing.Size(36, 17);
            this.radioButton_xlarge.TabIndex = 2;
            this.radioButton_xlarge.Text = "x2";
            this.radioButton_xlarge.UseVisualStyleBackColor = true;
            this.radioButton_xlarge.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton_large
            // 
            this.radioButton_large.AutoSize = true;
            this.radioButton_large.Location = new System.Drawing.Point(46, 4);
            this.radioButton_large.Name = "radioButton_large";
            this.radioButton_large.Size = new System.Drawing.Size(45, 17);
            this.radioButton_large.TabIndex = 1;
            this.radioButton_large.Text = "x1,5";
            this.radioButton_large.UseVisualStyleBackColor = true;
            this.radioButton_large.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton_normal
            // 
            this.radioButton_normal.AutoSize = true;
            this.radioButton_normal.Checked = true;
            this.radioButton_normal.Location = new System.Drawing.Point(4, 4);
            this.radioButton_normal.Name = "radioButton_normal";
            this.radioButton_normal.Size = new System.Drawing.Size(36, 17);
            this.radioButton_normal.TabIndex = 0;
            this.radioButton_normal.TabStop = true;
            this.radioButton_normal.Text = "x1";
            this.radioButton_normal.UseVisualStyleBackColor = true;
            this.radioButton_normal.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // button_SaveAnimation
            // 
            this.button_SaveAnimation.Location = new System.Drawing.Point(199, 27);
            this.button_SaveAnimation.Name = "button_SaveAnimation";
            this.button_SaveAnimation.Size = new System.Drawing.Size(134, 23);
            this.button_SaveAnimation.TabIndex = 3;
            this.button_SaveAnimation.Text = "Сохранить анимацию";
            this.button_SaveAnimation.UseVisualStyleBackColor = true;
            this.button_SaveAnimation.Click += new System.EventHandler(this.button_SaveAnimation_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 29);
            this.label1.TabIndex = 4;
            this.label1.Text = "Количество кадров для сохранения";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown_NumberOfFrames
            // 
            this.numericUpDown_NumberOfFrames.Location = new System.Drawing.Point(127, 30);
            this.numericUpDown_NumberOfFrames.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_NumberOfFrames.Name = "numericUpDown_NumberOfFrames";
            this.numericUpDown_NumberOfFrames.Size = new System.Drawing.Size(53, 20);
            this.numericUpDown_NumberOfFrames.TabIndex = 5;
            this.numericUpDown_NumberOfFrames.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // progressBar_SaveAnimation
            // 
            this.progressBar_SaveAnimation.Location = new System.Drawing.Point(52, 219);
            this.progressBar_SaveAnimation.Name = "progressBar_SaveAnimation";
            this.progressBar_SaveAnimation.Size = new System.Drawing.Size(356, 23);
            this.progressBar_SaveAnimation.Step = 1;
            this.progressBar_SaveAnimation.TabIndex = 2;
            this.progressBar_SaveAnimation.Value = 50;
            this.progressBar_SaveAnimation.Visible = false;
            // 
            // FormAnimation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(460, 518);
            this.Controls.Add(this.progressBar_SaveAnimation);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox_AnimatiomPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAnimation";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview animation";
            this.Shown += new System.EventHandler(this.radioButton_CheckedChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_AnimatiomPreview)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_NumberOfFrames)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_AnimatiomPreview;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_normal;
        private System.Windows.Forms.RadioButton radioButton_xlarge;
        private System.Windows.Forms.RadioButton radioButton_large;
        private System.Windows.Forms.NumericUpDown numericUpDown_NumberOfFrames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_SaveAnimation;
        private System.Windows.Forms.ProgressBar progressBar_SaveAnimation;
    }
}