namespace Hantek_UI
{
    partial class FrmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            BtnOK = new Button();
            BtnCancel = new Button();
            LblWindowColor = new Label();
            PnlSettings = new FlowLayoutPanel();
            TblPanel = new TableLayoutPanel();
            label2 = new Label();
            LblTextColor = new Label();
            ChkTransparent = new CheckBox();
            BtnWindowColor = new Button();
            BtnTextColor = new Button();
            LblUnitColor = new Label();
            BtnUnitColor = new Button();
            BtnModeColor = new Button();
            panel1 = new Panel();
            LblPrecHelp = new Label();
            LblPrecision = new Label();
            LblShowWIndowBorder = new Label();
            CboPrecision = new ComboBox();
            ChkWindowBorder = new CheckBox();
            PnlSettings.SuspendLayout();
            TblPanel.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnOK
            // 
            BtnOK.AutoSize = true;
            BtnOK.DialogResult = DialogResult.OK;
            BtnOK.Location = new Point(213, 221);
            BtnOK.Name = "BtnOK";
            BtnOK.Size = new Size(33, 25);
            BtnOK.TabIndex = 0;
            BtnOK.Text = "OK";
            BtnOK.UseVisualStyleBackColor = true;
            // 
            // BtnCancel
            // 
            BtnCancel.AutoSize = true;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(252, 221);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(59, 25);
            BtnCancel.TabIndex = 1;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // LblWindowColor
            // 
            LblWindowColor.Anchor = AnchorStyles.Right;
            LblWindowColor.AutoSize = true;
            LblWindowColor.Location = new Point(14, 17);
            LblWindowColor.Name = "LblWindowColor";
            LblWindowColor.Size = new Size(83, 15);
            LblWindowColor.TabIndex = 2;
            LblWindowColor.Text = "Window Color";
            // 
            // PnlSettings
            // 
            PnlSettings.Controls.Add(TblPanel);
            PnlSettings.Controls.Add(panel1);
            PnlSettings.Dock = DockStyle.Top;
            PnlSettings.Location = new Point(0, 0);
            PnlSettings.Name = "PnlSettings";
            PnlSettings.Size = new Size(601, 215);
            PnlSettings.TabIndex = 3;
            PnlSettings.WrapContents = false;
            // 
            // TblPanel
            // 
            TblPanel.Anchor = AnchorStyles.Right;
            TblPanel.ColumnCount = 3;
            TblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            TblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
            TblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
            TblPanel.Controls.Add(label2, 0, 3);
            TblPanel.Controls.Add(LblTextColor, 0, 1);
            TblPanel.Controls.Add(LblWindowColor, 0, 0);
            TblPanel.Controls.Add(ChkTransparent, 2, 0);
            TblPanel.Controls.Add(BtnWindowColor, 1, 0);
            TblPanel.Controls.Add(BtnTextColor, 1, 1);
            TblPanel.Controls.Add(LblUnitColor, 0, 2);
            TblPanel.Controls.Add(BtnUnitColor, 1, 2);
            TblPanel.Controls.Add(BtnModeColor, 1, 3);
            TblPanel.Location = new Point(3, 3);
            TblPanel.Name = "TblPanel";
            TblPanel.RowCount = 4;
            TblPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            TblPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            TblPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            TblPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            TblPanel.Size = new Size(243, 206);
            TblPanel.TabIndex = 5;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(3, 170);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 8;
            label2.Text = "Mode Text Color";
            // 
            // LblTextColor
            // 
            LblTextColor.Anchor = AnchorStyles.Right;
            LblTextColor.AutoSize = true;
            LblTextColor.Location = new Point(6, 67);
            LblTextColor.Name = "LblTextColor";
            LblTextColor.Size = new Size(91, 15);
            LblTextColor.TabIndex = 4;
            LblTextColor.Text = "Value Text Color";
            // 
            // ChkTransparent
            // 
            ChkTransparent.Anchor = AnchorStyles.Left;
            ChkTransparent.AutoSize = true;
            ChkTransparent.Location = new Point(148, 15);
            ChkTransparent.Name = "ChkTransparent";
            ChkTransparent.Size = new Size(87, 19);
            ChkTransparent.TabIndex = 6;
            ChkTransparent.Text = "Transparent";
            ChkTransparent.UseVisualStyleBackColor = true;
            ChkTransparent.CheckedChanged += ChkTransparent_CheckedChanged;
            // 
            // BtnWindowColor
            // 
            BtnWindowColor.Anchor = AnchorStyles.Left;
            BtnWindowColor.BackColor = Color.Gray;
            BtnWindowColor.FlatStyle = FlatStyle.Flat;
            BtnWindowColor.Location = new Point(103, 12);
            BtnWindowColor.Name = "BtnWindowColor";
            BtnWindowColor.Size = new Size(31, 26);
            BtnWindowColor.TabIndex = 3;
            BtnWindowColor.UseVisualStyleBackColor = false;
            BtnWindowColor.Click += BtnWindowColor_Click;
            // 
            // BtnTextColor
            // 
            BtnTextColor.Anchor = AnchorStyles.Left;
            BtnTextColor.BackColor = Color.Yellow;
            BtnTextColor.FlatStyle = FlatStyle.Flat;
            BtnTextColor.Location = new Point(103, 61);
            BtnTextColor.Name = "BtnTextColor";
            BtnTextColor.Size = new Size(31, 27);
            BtnTextColor.TabIndex = 5;
            BtnTextColor.UseVisualStyleBackColor = false;
            BtnTextColor.Click += BtnTextColor_Click;
            // 
            // LblUnitColor
            // 
            LblUnitColor.Anchor = AnchorStyles.Right;
            LblUnitColor.AutoSize = true;
            LblUnitColor.Location = new Point(12, 117);
            LblUnitColor.Name = "LblUnitColor";
            LblUnitColor.Size = new Size(85, 15);
            LblUnitColor.TabIndex = 7;
            LblUnitColor.Text = "Unit Text Color";
            // 
            // BtnUnitColor
            // 
            BtnUnitColor.Anchor = AnchorStyles.Left;
            BtnUnitColor.BackColor = Color.Yellow;
            BtnUnitColor.FlatStyle = FlatStyle.Flat;
            BtnUnitColor.Location = new Point(103, 111);
            BtnUnitColor.Name = "BtnUnitColor";
            BtnUnitColor.Size = new Size(31, 27);
            BtnUnitColor.TabIndex = 9;
            BtnUnitColor.UseVisualStyleBackColor = false;
            BtnUnitColor.Click += BtnUnitColor_Click;
            // 
            // BtnModeColor
            // 
            BtnModeColor.Anchor = AnchorStyles.Left;
            BtnModeColor.BackColor = Color.Yellow;
            BtnModeColor.FlatStyle = FlatStyle.Flat;
            BtnModeColor.Location = new Point(103, 164);
            BtnModeColor.Name = "BtnModeColor";
            BtnModeColor.Size = new Size(31, 27);
            BtnModeColor.TabIndex = 10;
            BtnModeColor.UseVisualStyleBackColor = false;
            BtnModeColor.Click += BtnModeColor_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(LblPrecHelp);
            panel1.Controls.Add(LblPrecision);
            panel1.Controls.Add(LblShowWIndowBorder);
            panel1.Controls.Add(CboPrecision);
            panel1.Controls.Add(ChkWindowBorder);
            panel1.Location = new Point(252, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(345, 206);
            panel1.TabIndex = 7;
            // 
            // LblPrecHelp
            // 
            LblPrecHelp.Location = new Point(201, 62);
            LblPrecHelp.Name = "LblPrecHelp";
            LblPrecHelp.Size = new Size(130, 41);
            LblPrecHelp.TabIndex = 10;
            LblPrecHelp.Text = "(a.k.a The number of decimal places)";
            // 
            // LblPrecision
            // 
            LblPrecision.Location = new Point(24, 62);
            LblPrecision.Name = "LblPrecision";
            LblPrecision.Size = new Size(121, 15);
            LblPrecision.TabIndex = 9;
            LblPrecision.Text = "Precision";
            LblPrecision.TextAlign = ContentAlignment.TopRight;
            // 
            // LblShowWIndowBorder
            // 
            LblShowWIndowBorder.AutoSize = true;
            LblShowWIndowBorder.Location = new Point(24, 19);
            LblShowWIndowBorder.Name = "LblShowWIndowBorder";
            LblShowWIndowBorder.Size = new Size(121, 15);
            LblShowWIndowBorder.TabIndex = 8;
            LblShowWIndowBorder.Text = "Show Window Border";
            // 
            // CboPrecision
            // 
            CboPrecision.FormattingEnabled = true;
            CboPrecision.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6" });
            CboPrecision.Location = new Point(151, 58);
            CboPrecision.Name = "CboPrecision";
            CboPrecision.Size = new Size(44, 23);
            CboPrecision.TabIndex = 7;
            CboPrecision.SelectedIndexChanged += CboPrecision_SelectedIndexChanged;
            // 
            // ChkWindowBorder
            // 
            ChkWindowBorder.Anchor = AnchorStyles.Left;
            ChkWindowBorder.Location = new Point(151, 17);
            ChkWindowBorder.Name = "ChkWindowBorder";
            ChkWindowBorder.Size = new Size(30, 20);
            ChkWindowBorder.TabIndex = 6;
            ChkWindowBorder.UseVisualStyleBackColor = true;
            ChkWindowBorder.CheckedChanged += ChkWindowBorder_CheckedChanged;
            // 
            // FrmSettings
            // 
            AcceptButton = BtnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(601, 249);
            ControlBox = false;
            Controls.Add(PnlSettings);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmSettings";
            Text = "Settings";
            TopMost = true;
            Load += FrmSettings_Load;
            PnlSettings.ResumeLayout(false);
            TblPanel.ResumeLayout(false);
            TblPanel.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnOK;
        private Button BtnCancel;
        private Label LblWindowColor;
        private FlowLayoutPanel PnlSettings;
        private Button BtnWindowColor;
        private SplitContainer splitContainer1;
        private TableLayoutPanel TblPanel;
        private Button BtnTextColor;
        private Label LblTextColor;
        private CheckBox ChkTransparent;
        private Label label2;
        private Label LblUnitColor;
        private Button BtnUnitColor;
        private Button BtnModeColor;
        private CheckBox ChkWindowBorder;
        private Panel panel1;
        private ComboBox CboPrecision;
        private Label LblPrecision;
        private Label LblShowWIndowBorder;
        private Label LblPrecHelp;
    }
}