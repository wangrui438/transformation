namespace Transformation
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
            this.button2 = new System.Windows.Forms.Button();
            this.txtfileName = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnTf = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(645, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(131, 37);
            this.button2.TabIndex = 0;
            this.button2.Text = "浏览...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtfileName
            // 
            this.txtfileName.Location = new System.Drawing.Point(99, 102);
            this.txtfileName.Name = "txtfileName";
            this.txtfileName.Size = new System.Drawing.Size(516, 35);
            this.txtfileName.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnTf
            // 
            this.btnTf.Location = new System.Drawing.Point(835, 93);
            this.btnTf.Name = "btnTf";
            this.btnTf.Size = new System.Drawing.Size(161, 45);
            this.btnTf.TabIndex = 2;
            this.btnTf.Text = " 转换";
            this.btnTf.UseVisualStyleBackColor = true;
            this.btnTf.Click += new System.EventHandler(this.btnTf_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1143, 289);
            this.Controls.Add(this.btnTf);
            this.Controls.Add(this.txtfileName);
            this.Controls.Add(this.button2);
            this.Name = "Form1";
            this.Text = "文件转换工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtfileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnTf;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

