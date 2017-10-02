namespace DirectX9Demo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem_File = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog_XFile = new System.Windows.Forms.OpenFileDialog();
            this.panel_Disp = new System.Windows.Forms.Panel();
            this.menuStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip_Main
            // 
            this.menuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_File});
            this.menuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Main.Name = "menuStrip_Main";
            this.menuStrip_Main.Size = new System.Drawing.Size(784, 25);
            this.menuStrip_Main.TabIndex = 0;
            this.menuStrip_Main.Text = "menuStrip1";
            // 
            // toolStripMenuItem_File
            // 
            this.toolStripMenuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_File_Open});
            this.toolStripMenuItem_File.Name = "toolStripMenuItem_File";
            this.toolStripMenuItem_File.Size = new System.Drawing.Size(44, 21);
            this.toolStripMenuItem_File.Text = "文件";
            // 
            // toolStripMenuItem_File_Open
            // 
            this.toolStripMenuItem_File_Open.Name = "toolStripMenuItem_File_Open";
            this.toolStripMenuItem_File_Open.Size = new System.Drawing.Size(109, 22);
            this.toolStripMenuItem_File_Open.Text = "打开...";
            this.toolStripMenuItem_File_Open.Click += new System.EventHandler(this.MenuItemClickEventHandler);
            // 
            // openFileDialog_XFile
            // 
            this.openFileDialog_XFile.Filter = "(*.x)|*.x";
            // 
            // panel_Disp
            // 
            this.panel_Disp.BackColor = System.Drawing.Color.White;
            this.panel_Disp.Location = new System.Drawing.Point(13, 29);
            this.panel_Disp.Name = "panel_Disp";
            this.panel_Disp.Size = new System.Drawing.Size(759, 520);
            this.panel_Disp.TabIndex = 1;
            this.panel_Disp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel_Disp);
            this.Controls.Add(this.menuStrip_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip_Main;
            this.Name = "Form1";
            this.Text = "X Static Viewer";
            this.menuStrip_Main.ResumeLayout(false);
            this.menuStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_File;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_File_Open;
        private System.Windows.Forms.OpenFileDialog openFileDialog_XFile;
        private System.Windows.Forms.Panel panel_Disp;
    }
}

