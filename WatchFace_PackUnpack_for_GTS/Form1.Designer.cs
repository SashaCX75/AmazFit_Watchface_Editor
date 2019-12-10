namespace WatchFace_PackUnpack_for_GTS
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.checkBox_Watchface_Path = new System.Windows.Forms.CheckBox();
            this.button_unpack = new System.Windows.Forms.Button();
            this.button_pack = new System.Windows.Forms.Button();
            this.radioButton_32 = new System.Windows.Forms.RadioButton();
            this.radioButton_64 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // checkBox_Watchface_Path
            // 
            this.checkBox_Watchface_Path.AutoSize = true;
            this.checkBox_Watchface_Path.Checked = true;
            this.checkBox_Watchface_Path.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Watchface_Path.Location = new System.Drawing.Point(12, 12);
            this.checkBox_Watchface_Path.Name = "checkBox_Watchface_Path";
            this.checkBox_Watchface_Path.Size = new System.Drawing.Size(331, 17);
            this.checkBox_Watchface_Path.TabIndex = 0;
            this.checkBox_Watchface_Path.Text = "Помещать распакованный циферблат в папку \"WatchFace\"";
            this.checkBox_Watchface_Path.UseVisualStyleBackColor = true;
            // 
            // button_unpack
            // 
            this.button_unpack.Location = new System.Drawing.Point(12, 59);
            this.button_unpack.Name = "button_unpack";
            this.button_unpack.Size = new System.Drawing.Size(75, 23);
            this.button_unpack.TabIndex = 1;
            this.button_unpack.Text = "Распаковать";
            this.button_unpack.UseVisualStyleBackColor = true;
            this.button_unpack.Click += new System.EventHandler(this.button_unpack_Click);
            // 
            // button_pack
            // 
            this.button_pack.Location = new System.Drawing.Point(93, 59);
            this.button_pack.Name = "button_pack";
            this.button_pack.Size = new System.Drawing.Size(75, 23);
            this.button_pack.TabIndex = 2;
            this.button_pack.Text = "Запаковать";
            this.button_pack.UseVisualStyleBackColor = true;
            this.button_pack.Click += new System.EventHandler(this.button_pack_Click);
            // 
            // radioButton_32
            // 
            this.radioButton_32.AutoSize = true;
            this.radioButton_32.Location = new System.Drawing.Point(12, 36);
            this.radioButton_32.Name = "radioButton_32";
            this.radioButton_32.Size = new System.Drawing.Size(112, 17);
            this.radioButton_32.TabIndex = 3;
            this.radioButton_32.TabStop = true;
            this.radioButton_32.Text = "32-разрядная ОС";
            this.radioButton_32.UseVisualStyleBackColor = true;
            // 
            // radioButton_64
            // 
            this.radioButton_64.AutoSize = true;
            this.radioButton_64.Location = new System.Drawing.Point(130, 35);
            this.radioButton_64.Name = "radioButton_64";
            this.radioButton_64.Size = new System.Drawing.Size(112, 17);
            this.radioButton_64.TabIndex = 4;
            this.radioButton_64.TabStop = true;
            this.radioButton_64.Text = "64-разрядная ОС";
            this.radioButton_64.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 87);
            this.Controls.Add(this.radioButton_64);
            this.Controls.Add(this.radioButton_32);
            this.Controls.Add(this.button_pack);
            this.Controls.Add(this.button_unpack);
            this.Controls.Add(this.checkBox_Watchface_Path);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Распаковка/упаковка циферблатов";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_Watchface_Path;
        private System.Windows.Forms.Button button_unpack;
        private System.Windows.Forms.Button button_pack;
        private System.Windows.Forms.RadioButton radioButton_32;
        private System.Windows.Forms.RadioButton radioButton_64;
    }
}

