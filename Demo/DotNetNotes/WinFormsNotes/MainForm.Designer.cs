namespace WinFormsNotes
{
    partial class MainForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            List = new Button();
            New = new Button();
            Display = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // Refresh
            // 
            List.Image = (Image)resources.GetObject("Refresh.Image");
            List.Location = new Point(15, 15);
            List.Name = "Refresh";
            List.Size = new Size(125, 125);
            List.TabIndex = 0;
            List.UseVisualStyleBackColor = true;
            // 
            // New
            // 
            New.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            New.Location = new Point(1280, 1190);
            New.Name = "New";
            New.Size = new Size(190, 60);
            New.TabIndex = 1;
            New.Text = "&New";
            New.UseVisualStyleBackColor = true;
            // 
            // Display
            // 
            Display.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Display.Location = new Point(25, 160);
            Display.Name = "Display";
            Display.Size = new Size(1435, 1010);
            Display.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1484, 1265);
            Controls.Add(Display);
            Controls.Add(New);
            Controls.Add(List);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "WinFormsNotes";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
        }

        #endregion

        private Button List;
        private Button New;
        private FlowLayoutPanel Display;
    }
}
