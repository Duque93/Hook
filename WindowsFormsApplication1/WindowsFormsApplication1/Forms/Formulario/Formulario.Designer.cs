namespace OverlayDrawingTest
{
    partial class Formulario
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
            this.checkBoxOverlay = new System.Windows.Forms.CheckBox();
            this.textBoxWindowNameTarget = new System.Windows.Forms.TextBox();
            this.labelWindowName = new System.Windows.Forms.Label();
            this.buttonBuscar = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox_Debug = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // checkBoxOverlay
            // 
            this.checkBoxOverlay.AutoSize = true;
            this.checkBoxOverlay.Location = new System.Drawing.Point(16, 38);
            this.checkBoxOverlay.Name = "checkBoxOverlay";
            this.checkBoxOverlay.Size = new System.Drawing.Size(104, 17);
            this.checkBoxOverlay.TabIndex = 0;
            this.checkBoxOverlay.Text = "Activate Overlay";
            this.checkBoxOverlay.UseVisualStyleBackColor = true;
            this.checkBoxOverlay.Visible = false;
            this.checkBoxOverlay.CheckedChanged += new System.EventHandler(this.checkBoxOverlay_CheckedChanged);
            // 
            // textBoxWindowNameTarget
            // 
            this.textBoxWindowNameTarget.Location = new System.Drawing.Point(107, 12);
            this.textBoxWindowNameTarget.Name = "textBoxWindowNameTarget";
            this.textBoxWindowNameTarget.Size = new System.Drawing.Size(170, 20);
            this.textBoxWindowNameTarget.TabIndex = 1;
            this.textBoxWindowNameTarget.Text = "Administrador de tareas";
            this.textBoxWindowNameTarget.TextChanged += new System.EventHandler(this.textBoxWindowNameTarget_TextChanged);
            // 
            // labelWindowName
            // 
            this.labelWindowName.AutoSize = true;
            this.labelWindowName.Location = new System.Drawing.Point(13, 15);
            this.labelWindowName.Name = "labelWindowName";
            this.labelWindowName.Size = new System.Drawing.Size(88, 13);
            this.labelWindowName.TabIndex = 2;
            this.labelWindowName.Text = "Nombre proceso:";
            // 
            // buttonBuscar
            // 
            this.buttonBuscar.Location = new System.Drawing.Point(290, 9);
            this.buttonBuscar.Name = "buttonBuscar";
            this.buttonBuscar.Size = new System.Drawing.Size(72, 23);
            this.buttonBuscar.TabIndex = 3;
            this.buttonBuscar.Text = "Buscar";
            this.buttonBuscar.UseVisualStyleBackColor = true;
            this.buttonBuscar.Click += new System.EventHandler(this.buttonBuscar_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(16, 209);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(346, 40);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Coordenadas del raton:";
            // 
            // richTextBox_Debug
            // 
            this.richTextBox_Debug.Location = new System.Drawing.Point(16, 61);
            this.richTextBox_Debug.Name = "richTextBox_Debug";
            this.richTextBox_Debug.Size = new System.Drawing.Size(346, 69);
            this.richTextBox_Debug.TabIndex = 6;
            this.richTextBox_Debug.Text = "";
            // 
            // Formulario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 261);
            this.Controls.Add(this.richTextBox_Debug);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.buttonBuscar);
            this.Controls.Add(this.labelWindowName);
            this.Controls.Add(this.textBoxWindowNameTarget);
            this.Controls.Add(this.checkBoxOverlay);
            this.Name = "Formulario";
            this.Text = "Formulario";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Formulario_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxOverlay;
        private System.Windows.Forms.TextBox textBoxWindowNameTarget;
        private System.Windows.Forms.Label labelWindowName;
        private System.Windows.Forms.Button buttonBuscar;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_Debug;
    }
}

