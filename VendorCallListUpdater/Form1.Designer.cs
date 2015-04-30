namespace VendorCallListUpdater
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        this.Load_Button = new System.Windows.Forms.Button();
        this.Append_Button = new System.Windows.Forms.Button();
        this.openFile = new System.Windows.Forms.OpenFileDialog();
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.undoButton = new System.Windows.Forms.Button();
        this.tablePicker = new System.Windows.Forms.ComboBox();
        this.SuspendLayout();
        // 
        // Load_Button
        // 
        this.Load_Button.Location = new System.Drawing.Point(12, 43);
        this.Load_Button.Name = "Load_Button";
        this.Load_Button.Size = new System.Drawing.Size(180, 179);
        this.Load_Button.TabIndex = 0;
        this.Load_Button.Text = "Load!";
        this.Load_Button.UseVisualStyleBackColor = true;
        this.Load_Button.Click += new System.EventHandler(this.Load_Button_Click);
        // 
        // Append_Button
        // 
        this.Append_Button.Location = new System.Drawing.Point(198, 43);
        this.Append_Button.Name = "Append_Button";
        this.Append_Button.Size = new System.Drawing.Size(172, 178);
        this.Append_Button.TabIndex = 1;
        this.Append_Button.Text = "Append!";
        this.Append_Button.UseVisualStyleBackColor = true;
        this.Append_Button.Click += new System.EventHandler(this.Append_Button_Click);
        // 
        // openFile
        // 
        this.openFile.FileName = "openFile";
        this.openFile.Filter = "CSV|*.csv";
        // 
        // progressBar
        // 
        this.progressBar.Location = new System.Drawing.Point(12, 228);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(358, 25);
        this.progressBar.TabIndex = 3;
        // 
        // undoButton
        // 
        this.undoButton.Enabled = false;
        this.undoButton.Location = new System.Drawing.Point(198, 10);
        this.undoButton.Name = "undoButton";
        this.undoButton.Size = new System.Drawing.Size(165, 25);
        this.undoButton.TabIndex = 4;
        this.undoButton.Text = "Undo!";
        this.undoButton.UseVisualStyleBackColor = true;
        this.undoButton.Click += new System.EventHandler(this.button1_Click);
        // 
        // tablePicker
        // 
        this.tablePicker.FormattingEnabled = true;
        this.tablePicker.Location = new System.Drawing.Point(13, 10);
        this.tablePicker.Name = "tablePicker";
        this.tablePicker.Size = new System.Drawing.Size(179, 22);
        this.tablePicker.TabIndex = 5;
        this.tablePicker.SelectedIndexChanged += new System.EventHandler(this.tablePicker_SelectedIndexChanged);
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(384, 272);
        this.Controls.Add(this.tablePicker);
        this.Controls.Add(this.undoButton);
        this.Controls.Add(this.progressBar);
        this.Controls.Add(this.Append_Button);
        this.Controls.Add(this.Load_Button);
        this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "Form1";
        this.Text = "Vendor Call List Updater";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Load_Button;
        private System.Windows.Forms.Button Append_Button;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.ComboBox tablePicker;
    }
}

