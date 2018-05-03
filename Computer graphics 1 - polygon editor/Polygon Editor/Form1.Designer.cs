namespace Polygon_Editor
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.horizontalButton = new System.Windows.Forms.ToolStripButton();
            this.verticalButton = new System.Windows.Forms.ToolStripButton();
            this.angleButton = new System.Windows.Forms.ToolStripButton();
            this.addVertexButton = new System.Windows.Forms.ToolStripButton();
            this.deleteConstraintsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteVertexButton = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.drawingBox = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bresenhamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xiaolinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drawingBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 437);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.horizontalButton,
            this.verticalButton,
            this.angleButton,
            this.addVertexButton,
            this.deleteConstraintsButton,
            this.toolStripSeparator2,
            this.deleteVertexButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(484, 20);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // horizontalButton
            // 
            this.horizontalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.horizontalButton.Enabled = false;
            this.horizontalButton.Image = ((System.Drawing.Image)(resources.GetObject("horizontalButton.Image")));
            this.horizontalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.horizontalButton.Name = "horizontalButton";
            this.horizontalButton.Size = new System.Drawing.Size(23, 17);
            this.horizontalButton.Text = "Horizontal line";
            this.horizontalButton.Click += new System.EventHandler(this.horizontalButton_Click);
            // 
            // verticalButton
            // 
            this.verticalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.verticalButton.Enabled = false;
            this.verticalButton.Image = ((System.Drawing.Image)(resources.GetObject("verticalButton.Image")));
            this.verticalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.verticalButton.Name = "verticalButton";
            this.verticalButton.Size = new System.Drawing.Size(23, 17);
            this.verticalButton.Text = "Vertical line";
            this.verticalButton.Click += new System.EventHandler(this.verticalButton_Click);
            // 
            // angleButton
            // 
            this.angleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.angleButton.Enabled = false;
            this.angleButton.Image = ((System.Drawing.Image)(resources.GetObject("angleButton.Image")));
            this.angleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.angleButton.Name = "angleButton";
            this.angleButton.Size = new System.Drawing.Size(23, 17);
            this.angleButton.Text = "Set angle";
            this.angleButton.Click += new System.EventHandler(this.angleButton_Click);
            // 
            // addVertexButton
            // 
            this.addVertexButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addVertexButton.Enabled = false;
            this.addVertexButton.Image = ((System.Drawing.Image)(resources.GetObject("addVertexButton.Image")));
            this.addVertexButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addVertexButton.Name = "addVertexButton";
            this.addVertexButton.Size = new System.Drawing.Size(23, 17);
            this.addVertexButton.Text = "Add vertex";
            this.addVertexButton.Click += new System.EventHandler(this.addVertexButton_Click);
            // 
            // deleteConstraintsButton
            // 
            this.deleteConstraintsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteConstraintsButton.Enabled = false;
            this.deleteConstraintsButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteConstraintsButton.Image")));
            this.deleteConstraintsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteConstraintsButton.Name = "deleteConstraintsButton";
            this.deleteConstraintsButton.Size = new System.Drawing.Size(23, 17);
            this.deleteConstraintsButton.Text = "Delete constraints";
            this.deleteConstraintsButton.Click += new System.EventHandler(this.deleteConstraintsButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 20);
            // 
            // deleteVertexButton
            // 
            this.deleteVertexButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteVertexButton.Enabled = false;
            this.deleteVertexButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteVertexButton.Image")));
            this.deleteVertexButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteVertexButton.Name = "deleteVertexButton";
            this.deleteVertexButton.Size = new System.Drawing.Size(23, 17);
            this.deleteVertexButton.Text = "Delete vertex";
            this.deleteVertexButton.Click += new System.EventHandler(this.deleteVertexButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.drawingBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(478, 411);
            this.panel1.TabIndex = 2;
            // 
            // drawingBox
            // 
            this.drawingBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingBox.Location = new System.Drawing.Point(0, 0);
            this.drawingBox.Name = "drawingBox";
            this.drawingBox.Size = new System.Drawing.Size(478, 411);
            this.drawingBox.TabIndex = 0;
            this.drawingBox.TabStop = false;
            this.drawingBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel_MD);
            this.drawingBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel_MM);
            this.drawingBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Panel_MU);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.bresenhamToolStripMenuItem,
            this.xiaolinToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(484, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // bresenhamToolStripMenuItem
            // 
            this.bresenhamToolStripMenuItem.Name = "bresenhamToolStripMenuItem";
            this.bresenhamToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.bresenhamToolStripMenuItem.Text = "Bresenham";
            this.bresenhamToolStripMenuItem.Click += new System.EventHandler(this.bresenhamToolStripMenuItem_Click);
            // 
            // xiaolinToolStripMenuItem
            // 
            this.xiaolinToolStripMenuItem.Name = "xiaolinToolStripMenuItem";
            this.xiaolinToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.xiaolinToolStripMenuItem.Text = "Xiaolin";
            this.xiaolinToolStripMenuItem.Click += new System.EventHandler(this.xiaolinToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Polygon Editor";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.drawingBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton horizontalButton;
        private System.Windows.Forms.ToolStripButton verticalButton;
        private System.Windows.Forms.ToolStripButton angleButton;
        private System.Windows.Forms.ToolStripButton addVertexButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton deleteVertexButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox drawingBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripButton deleteConstraintsButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bresenhamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xiaolinToolStripMenuItem;
    }
}

