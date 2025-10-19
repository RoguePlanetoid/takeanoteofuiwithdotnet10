namespace WinFormsNotes;

partial class UpsertDialog
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TextBox Title;
    private System.Windows.Forms.TextBox Content;
    private System.Windows.Forms.ComboBox Background;
    private System.Windows.Forms.Button PrimaryButton;
    private System.Windows.Forms.Button SecondaryButton;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        ComponentResourceManager resources = new ComponentResourceManager(typeof(UpsertDialog));
        Title = new TextBox();
        Content = new TextBox();
        Background = new ComboBox();
        PrimaryButton = new Button();
        SecondaryButton = new Button();
        TitleLabel = new Label();
        ContentLabel = new Label();
        BackgroundLabel = new Label();
        SuspendLayout();
        // 
        // Title
        // 
        Title.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        Title.Location = new Point(20, 75);
        Title.Name = "Title";
        Title.Size = new Size(725, 47);
        Title.TabIndex = 0;
        // 
        // Content
        // 
        Content.Location = new Point(20, 190);
        Content.Multiline = true;
        Content.Name = "Content";
        Content.Size = new Size(725, 260);
        Content.TabIndex = 1;
        // 
        // Background
        // 
        Background.Location = new Point(20, 525);
        Background.Name = "Background";
        Background.Size = new Size(725, 49);
        Background.TabIndex = 2;
        // 
        // PrimaryButton
        // 
        PrimaryButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        PrimaryButton.Location = new Point(340, 640);
        PrimaryButton.Name = "PrimaryButton";
        PrimaryButton.Size = new Size(190, 60);
        PrimaryButton.TabIndex = 3;
        PrimaryButton.Click += PrimaryButton_Click;
        // 
        // SecondaryButton
        // 
        SecondaryButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        SecondaryButton.Location = new Point(555, 640);
        SecondaryButton.Name = "SecondaryButton";
        SecondaryButton.Size = new Size(190, 60);
        SecondaryButton.TabIndex = 4;
        SecondaryButton.Click += SecondaryButton_Click;
        // 
        // TitleLabel
        // 
        TitleLabel.AutoSize = true;
        TitleLabel.Location = new Point(20, 20);
        TitleLabel.Name = "TitleLabel";
        TitleLabel.Size = new Size(74, 41);
        TitleLabel.TabIndex = 5;
        TitleLabel.Text = "Title";
        // 
        // ContentLabel
        // 
        ContentLabel.AutoSize = true;
        ContentLabel.Location = new Point(20, 135);
        ContentLabel.Name = "ContentLabel";
        ContentLabel.Size = new Size(125, 41);
        ContentLabel.TabIndex = 6;
        ContentLabel.Text = "Content";
        // 
        // BackgroundLabel
        // 
        BackgroundLabel.AutoSize = true;
        BackgroundLabel.Location = new Point(20, 465);
        BackgroundLabel.Name = "BackgroundLabel";
        BackgroundLabel.Size = new Size(177, 41);
        BackgroundLabel.TabIndex = 7;
        BackgroundLabel.Text = "Background";
        // 
        // UpsertDialog
        // 
        AcceptButton = PrimaryButton;
        CancelButton = SecondaryButton;
        ClientSize = new Size(768, 712);
        Controls.Add(BackgroundLabel);
        Controls.Add(ContentLabel);
        Controls.Add(TitleLabel);
        Controls.Add(Title);
        Controls.Add(Content);
        Controls.Add(Background);
        Controls.Add(PrimaryButton);
        Controls.Add(SecondaryButton);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        Name = "UpsertDialog";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Note";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label TitleLabel;
    private Label ContentLabel;
    private Label BackgroundLabel;
}
