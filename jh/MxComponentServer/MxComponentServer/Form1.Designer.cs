namespace MxComponentServer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ConnectPLC = new Button();
            DisconnectPLC = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // ConnectPLC
            // 
            ConnectPLC.Font = new Font("맑은 고딕", 15F, FontStyle.Bold);
            ConnectPLC.Location = new Point(12, 12);
            ConnectPLC.Name = "ConnectPLC";
            ConnectPLC.Size = new Size(230, 88);
            ConnectPLC.TabIndex = 0;
            ConnectPLC.Text = "Connect PLC";
            ConnectPLC.UseVisualStyleBackColor = true;
            ConnectPLC.Click += ConnectPLC_Click;
            // 
            // DisconnectPLC
            // 
            DisconnectPLC.Font = new Font("맑은 고딕", 15F, FontStyle.Bold);
            DisconnectPLC.Location = new Point(260, 12);
            DisconnectPLC.Name = "DisconnectPLC";
            DisconnectPLC.Size = new Size(230, 88);
            DisconnectPLC.TabIndex = 1;
            DisconnectPLC.Text = "Disconnect PLC";
            DisconnectPLC.UseVisualStyleBackColor = true;
            DisconnectPLC.Click += DisconnectPLC_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 124);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 2;
            label1.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(510, 256);
            Controls.Add(label1);
            Controls.Add(DisconnectPLC);
            Controls.Add(ConnectPLC);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ConnectPLC;
        private Button DisconnectPLC;
        private Label label1;
    }
}
