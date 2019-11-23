namespace WatchFace_PackUnpack
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton_loly = new System.Windows.Forms.RadioButton();
            this.radioButton_WakeUpich = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton_42 = new System.Windows.Forms.RadioButton();
            this.radioButton_47 = new System.Windows.Forms.RadioButton();
            this.checkBox_Watchface_Path = new System.Windows.Forms.CheckBox();
            this.button_unpack = new System.Windows.Forms.Button();
            this.button_pack = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton_loly);
            this.panel1.Controls.Add(this.radioButton_WakeUpich);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 45);
            this.panel1.TabIndex = 0;
            // 
            // radioButton_loly
            // 
            this.radioButton_loly.AutoSize = true;
            this.radioButton_loly.Location = new System.Drawing.Point(4, 27);
            this.radioButton_loly.Name = "radioButton_loly";
            this.radioButton_loly.Size = new System.Drawing.Size(195, 17);
            this.radioButton_loly.TabIndex = 1;
            this.radioButton_loly.Text = "Утилита от loly (требуется Python)";
            this.radioButton_loly.UseVisualStyleBackColor = true;
            // 
            // radioButton_WakeUpich
            // 
            this.radioButton_WakeUpich.AutoSize = true;
            this.radioButton_WakeUpich.Checked = true;
            this.radioButton_WakeUpich.Location = new System.Drawing.Point(4, 4);
            this.radioButton_WakeUpich.Name = "radioButton_WakeUpich";
            this.radioButton_WakeUpich.Size = new System.Drawing.Size(204, 17);
            this.radioButton_WakeUpich.TabIndex = 0;
            this.radioButton_WakeUpich.TabStop = true;
            this.radioButton_WakeUpich.Text = "Утилита от WakeUpich (без Python)";
            this.radioButton_WakeUpich.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton_42);
            this.panel2.Controls.Add(this.radioButton_47);
            this.panel2.Location = new System.Drawing.Point(259, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(70, 45);
            this.panel2.TabIndex = 1;
            // 
            // radioButton_42
            // 
            this.radioButton_42.AutoSize = true;
            this.radioButton_42.Location = new System.Drawing.Point(4, 27);
            this.radioButton_42.Name = "radioButton_42";
            this.radioButton_42.Size = new System.Drawing.Size(56, 17);
            this.radioButton_42.TabIndex = 3;
            this.radioButton_42.Text = "42 мм";
            this.radioButton_42.UseVisualStyleBackColor = true;
            // 
            // radioButton_47
            // 
            this.radioButton_47.AutoSize = true;
            this.radioButton_47.Checked = true;
            this.radioButton_47.Location = new System.Drawing.Point(4, 4);
            this.radioButton_47.Name = "radioButton_47";
            this.radioButton_47.Size = new System.Drawing.Size(56, 17);
            this.radioButton_47.TabIndex = 2;
            this.radioButton_47.TabStop = true;
            this.radioButton_47.Text = "47 мм";
            this.radioButton_47.UseVisualStyleBackColor = true;
            // 
            // checkBox_Watchface_Path
            // 
            this.checkBox_Watchface_Path.AutoSize = true;
            this.checkBox_Watchface_Path.Checked = true;
            this.checkBox_Watchface_Path.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Watchface_Path.Location = new System.Drawing.Point(16, 70);
            this.checkBox_Watchface_Path.Name = "checkBox_Watchface_Path";
            this.checkBox_Watchface_Path.Size = new System.Drawing.Size(331, 17);
            this.checkBox_Watchface_Path.TabIndex = 2;
            this.checkBox_Watchface_Path.Text = "Помещать распакованный циферблат в папку \"WatchFace\"";
            this.checkBox_Watchface_Path.UseVisualStyleBackColor = true;
            // 
            // button_unpack
            // 
            this.button_unpack.Location = new System.Drawing.Point(178, 103);
            this.button_unpack.Name = "button_unpack";
            this.button_unpack.Size = new System.Drawing.Size(75, 23);
            this.button_unpack.TabIndex = 3;
            this.button_unpack.Text = "Распаковать";
            this.button_unpack.UseVisualStyleBackColor = true;
            this.button_unpack.Click += new System.EventHandler(this.button_unpack_Click);
            // 
            // button_pack
            // 
            this.button_pack.Location = new System.Drawing.Point(259, 103);
            this.button_pack.Name = "button_pack";
            this.button_pack.Size = new System.Drawing.Size(75, 23);
            this.button_pack.TabIndex = 4;
            this.button_pack.Text = "Запаковать";
            this.button_pack.UseVisualStyleBackColor = true;
            this.button_pack.Click += new System.EventHandler(this.button_pack_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 135);
            this.Controls.Add(this.button_pack);
            this.Controls.Add(this.button_unpack);
            this.Controls.Add(this.checkBox_Watchface_Path);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Распаковка/упаковка циферблатов";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_loly;
        private System.Windows.Forms.RadioButton radioButton_WakeUpich;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButton_42;
        private System.Windows.Forms.RadioButton radioButton_47;
        private System.Windows.Forms.CheckBox checkBox_Watchface_Path;
        private System.Windows.Forms.Button button_unpack;
        private System.Windows.Forms.Button button_pack;
    }
}

