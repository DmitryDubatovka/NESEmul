using System.Windows.Forms;
using SkiaSharp.Views.Desktop;

namespace NESEmul.PatternTablesViewer
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
            this._skControl = new SkiaSharp.Views.Desktop.SKControl();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openFile = new System.Windows.Forms.ToolStripMenuItem();
            this.openROMFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _skControl
            // 
            this._skControl.Dock = System.Windows.Forms.DockStyle.Left;
            this._skControl.Location = new System.Drawing.Point(0, 24);
            this._skControl.Name = "_skControl";
            this._skControl.Size = new System.Drawing.Size(1027, 438);
            this._skControl.TabIndex = 0;
            this._skControl.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.OnSkControlOnPaint);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1424, 24);
            this.menu.TabIndex = 2;
            this.menu.Text = "menu";
            // 
            // fileMenu
            // 
            this.fileMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFile});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // openFile
            // 
            this.openFile.Name = "openFile";
            this.openFile.Size = new System.Drawing.Size(180, 22);
            this.openFile.Text = "&Open";
            this.openFile.Click += new System.EventHandler(this.openFileMenuItem_Click);
            // 
            // openROMFileDialog
            // 
            this.openROMFileDialog.DefaultExt = "rom";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1424, 462);
            this.Controls.Add(this._skControl);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "Form1";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private SkiaSharp.Views.Desktop.SKControl _skControl;
        private MenuStrip menu;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem openFile;
        private OpenFileDialog openROMFileDialog;
    }
}

