namespace WindowsFormsApplication1.Forms.ProcessForm.DetallesProcessForm
{
    partial class DetallesProcessForm
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
            this.tabContenidoDetalles = new System.Windows.Forms.TabControl();
            this.localDetailsTab = new System.Windows.Forms.TabPage();
            this.tabModulos = new System.Windows.Forms.TabPage();
            this.labelBasePriorityData = new System.Windows.Forms.Label();
            this.labelHandlersData = new System.Windows.Forms.Label();
            this.labelOutput = new System.Windows.Forms.Label();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.labelErrorOutput = new System.Windows.Forms.Label();
            this.richTextBoxErrorOutput = new System.Windows.Forms.RichTextBox();
            this.listViewModules = new System.Windows.Forms.ListView();
            this.tabContenidoDetalles.SuspendLayout();
            this.localDetailsTab.SuspendLayout();
            this.tabModulos.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabContenidoDetalles
            // 
            this.tabContenidoDetalles.Controls.Add(this.localDetailsTab);
            this.tabContenidoDetalles.Controls.Add(this.tabModulos);
            this.tabContenidoDetalles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabContenidoDetalles.HotTrack = true;
            this.tabContenidoDetalles.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabContenidoDetalles.Location = new System.Drawing.Point(-3, 4);
            this.tabContenidoDetalles.Name = "tabContenidoDetalles";
            this.tabContenidoDetalles.SelectedIndex = 0;
            this.tabContenidoDetalles.Size = new System.Drawing.Size(517, 259);
            this.tabContenidoDetalles.TabIndex = 0;
            // 
            // localDetailsTab
            // 
            this.localDetailsTab.BackColor = System.Drawing.Color.WhiteSmoke;
            this.localDetailsTab.Controls.Add(this.richTextBoxErrorOutput);
            this.localDetailsTab.Controls.Add(this.labelErrorOutput);
            this.localDetailsTab.Controls.Add(this.richTextBoxOutput);
            this.localDetailsTab.Controls.Add(this.labelOutput);
            this.localDetailsTab.Controls.Add(this.labelHandlersData);
            this.localDetailsTab.Controls.Add(this.labelBasePriorityData);
            this.localDetailsTab.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.localDetailsTab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.localDetailsTab.Location = new System.Drawing.Point(4, 22);
            this.localDetailsTab.Name = "localDetailsTab";
            this.localDetailsTab.Padding = new System.Windows.Forms.Padding(3);
            this.localDetailsTab.Size = new System.Drawing.Size(509, 233);
            this.localDetailsTab.TabIndex = 0;
            this.localDetailsTab.Text = "Local";
            // 
            // tabModulos
            // 
            this.tabModulos.Controls.Add(this.listViewModules);
            this.tabModulos.Location = new System.Drawing.Point(4, 22);
            this.tabModulos.Name = "tabModulos";
            this.tabModulos.Padding = new System.Windows.Forms.Padding(3);
            this.tabModulos.Size = new System.Drawing.Size(509, 233);
            this.tabModulos.TabIndex = 1;
            this.tabModulos.Text = "Modulos";
            this.tabModulos.UseVisualStyleBackColor = true;
            // 
            // labelBasePriorityData
            // 
            this.labelBasePriorityData.AutoSize = true;
            this.labelBasePriorityData.Location = new System.Drawing.Point(11, 17);
            this.labelBasePriorityData.Name = "labelBasePriorityData";
            this.labelBasePriorityData.Size = new System.Drawing.Size(74, 13);
            this.labelBasePriorityData.TabIndex = 3;
            this.labelBasePriorityData.Text = "Base Priority : ";
            // 
            // labelHandlersData
            // 
            this.labelHandlersData.AutoSize = true;
            this.labelHandlersData.Location = new System.Drawing.Point(91, 17);
            this.labelHandlersData.Name = "labelHandlersData";
            this.labelHandlersData.Size = new System.Drawing.Size(94, 13);
            this.labelHandlersData.TabIndex = 5;
            this.labelHandlersData.Text = "Handler Address : ";
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.Location = new System.Drawing.Point(11, 42);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(45, 13);
            this.labelOutput.TabIndex = 6;
            this.labelOutput.Text = "Output :";
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Enabled = false;
            this.richTextBoxOutput.Location = new System.Drawing.Point(14, 59);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.Size = new System.Drawing.Size(251, 164);
            this.richTextBoxOutput.TabIndex = 7;
            this.richTextBoxOutput.Text = "--En desarrollo";
            // 
            // labelErrorOutput
            // 
            this.labelErrorOutput.AutoSize = true;
            this.labelErrorOutput.Location = new System.Drawing.Point(268, 42);
            this.labelErrorOutput.Name = "labelErrorOutput";
            this.labelErrorOutput.Size = new System.Drawing.Size(70, 13);
            this.labelErrorOutput.TabIndex = 8;
            this.labelErrorOutput.Text = "Error Output :";
            // 
            // richTextBoxErrorOutput
            // 
            this.richTextBoxErrorOutput.Enabled = false;
            this.richTextBoxErrorOutput.Location = new System.Drawing.Point(271, 59);
            this.richTextBoxErrorOutput.Name = "richTextBoxErrorOutput";
            this.richTextBoxErrorOutput.Size = new System.Drawing.Size(238, 164);
            this.richTextBoxErrorOutput.TabIndex = 9;
            this.richTextBoxErrorOutput.Text = "--En desarrollo";
            // 
            // listViewModules
            // 
            this.listViewModules.Location = new System.Drawing.Point(-4, 0);
            this.listViewModules.Name = "listViewModules";
            this.listViewModules.Size = new System.Drawing.Size(517, 237);
            this.listViewModules.TabIndex = 0;
            this.listViewModules.UseCompatibleStateImageBehavior = false;
            this.listViewModules.View = System.Windows.Forms.View.Details;
            // 
            // DetallesProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 261);
            this.Controls.Add(this.tabContenidoDetalles);
            this.Name = "DetallesProcessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DetallesProcessForm_FormClosed);
            this.tabContenidoDetalles.ResumeLayout(false);
            this.localDetailsTab.ResumeLayout(false);
            this.localDetailsTab.PerformLayout();
            this.tabModulos.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabModulos;
        private System.Windows.Forms.Label labelBasePriorityData;
        private System.Windows.Forms.Label labelHandlersData;
        private System.Windows.Forms.TabControl tabContenidoDetalles;
        private System.Windows.Forms.TabPage localDetailsTab;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.RichTextBox richTextBoxErrorOutput;
        private System.Windows.Forms.Label labelErrorOutput;
        private System.Windows.Forms.ListView listViewModules;
    }
}