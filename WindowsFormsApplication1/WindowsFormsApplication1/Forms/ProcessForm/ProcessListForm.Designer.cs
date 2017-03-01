namespace WindowsFormsApplication1.Forms
{
    partial class ProcessListForm
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
            this.listProcessView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listProcessView
            // 
            this.listProcessView.FullRowSelect = true;
            this.listProcessView.GridLines = true;
            this.listProcessView.Location = new System.Drawing.Point(1, 1);
            this.listProcessView.MultiSelect = false;
            this.listProcessView.Name = "listProcessView";
            this.listProcessView.Scrollable = false;
            this.listProcessView.Size = new System.Drawing.Size(200, 170);
            this.listProcessView.TabIndex = 0;
            this.listProcessView.UseCompatibleStateImageBehavior = false;
            this.listProcessView.View = System.Windows.Forms.View.Details;
            // 
            // ProcessListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(213, 183);
            this.Controls.Add(this.listProcessView);
            this.Name = "ProcessListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProcessListForm";
            this.Load += new System.EventHandler(this.ProcessListForm_Load);
            this.SizeChanged += new System.EventHandler(this.ProcessListForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listProcessView;
    }
}