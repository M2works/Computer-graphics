namespace Polygon_Editor
{
    partial class Angle_Form
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
            this.SetDegreeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.degreeTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SetDegreeButton
            // 
            this.SetDegreeButton.Location = new System.Drawing.Point(28, 67);
            this.SetDegreeButton.Name = "SetDegreeButton";
            this.SetDegreeButton.Size = new System.Drawing.Size(75, 23);
            this.SetDegreeButton.TabIndex = 0;
            this.SetDegreeButton.Text = "OK";
            this.SetDegreeButton.UseVisualStyleBackColor = true;
            this.SetDegreeButton.Click += new System.EventHandler(this.SetDegreeButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(22, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Angle degrees: ";
            // 
            // degreeTextBox
            // 
            this.degreeTextBox.Location = new System.Drawing.Point(109, 28);
            this.degreeTextBox.Name = "degreeTextBox";
            this.degreeTextBox.Size = new System.Drawing.Size(100, 20);
            this.degreeTextBox.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(124, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Angle_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 111);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.degreeTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SetDegreeButton);
            this.MaximumSize = new System.Drawing.Size(250, 150);
            this.MinimumSize = new System.Drawing.Size(250, 150);
            this.Name = "Angle_Form";
            this.Text = "Set angle";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SetDegreeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox degreeTextBox;
        private System.Windows.Forms.Button button1;
    }
}