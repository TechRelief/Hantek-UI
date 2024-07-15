namespace Hantek_UI
{
    partial class FrmHantekUI
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHantekUI));
            BtnDCV = new ToolStripButton();
            LblHandle = new Label();
            TxtMode = new Label();
            TxtValue = new Label();
            BtnMenu = new Label();
            PnlMain = new Panel();
            PicRunning = new PictureBox();
            PicNotRunning = new PictureBox();
            TxtUnit = new Label();
            LblExpand = new Label();
            ImgError = new PictureBox();
            UpdateTimer = new System.Windows.Forms.Timer(components);
            MnuPopup = new ContextMenuStrip(components);
            ItmStartStop = new ToolStripMenuItem();
            ItmSettings = new ToolStripMenuItem();
            ItmExit = new ToolStripMenuItem();
            ToolBar = new ToolStrip();
            toolStripSeparator11 = new ToolStripSeparator();
            toolStripSeparator1 = new ToolStripSeparator();
            BtnACV = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            BtnDCI = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            BtnACI = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            BtnR2W = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            BtnR4W = new ToolStripButton();
            toolStripSeparator6 = new ToolStripSeparator();
            BtnFreq = new ToolStripButton();
            toolStripSeparator7 = new ToolStripSeparator();
            BtnCap = new ToolStripButton();
            toolStripSeparator8 = new ToolStripSeparator();
            BtnCont = new ToolStripButton();
            toolStripSeparator9 = new ToolStripSeparator();
            BtnDiode = new ToolStripButton();
            toolStripSeparator10 = new ToolStripSeparator();
            PnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PicRunning).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicNotRunning).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ImgError).BeginInit();
            MnuPopup.SuspendLayout();
            ToolBar.SuspendLayout();
            SuspendLayout();
            // 
            // BtnDCV
            // 
            BtnDCV.BackColor = Color.FromArgb(64, 64, 64);
            BtnDCV.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnDCV.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDCV.ForeColor = Color.Snow;
            BtnDCV.ImageTransparentColor = Color.Magenta;
            BtnDCV.Name = "BtnDCV";
            BtnDCV.Size = new Size(35, 28);
            BtnDCV.Text = "DCV";
            BtnDCV.ToolTipText = "DC Voltage";
            BtnDCV.Click += BtnDCV_Click;
            // 
            // LblHandle
            // 
            LblHandle.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            LblHandle.BackColor = Color.Transparent;
            LblHandle.CausesValidation = false;
            LblHandle.Cursor = Cursors.Hand;
            LblHandle.FlatStyle = FlatStyle.Flat;
            LblHandle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            LblHandle.ForeColor = Color.Snow;
            LblHandle.Location = new Point(630, 159);
            LblHandle.Name = "LblHandle";
            LblHandle.Size = new Size(23, 25);
            LblHandle.TabIndex = 11;
            LblHandle.Text = "◢";
            LblHandle.TextAlign = ContentAlignment.TopRight;
            LblHandle.MouseDown += LblHandle_MouseDown;
            LblHandle.MouseMove += LblHandle_MouseMove;
            LblHandle.MouseUp += LblHandle_MouseUp;
            // 
            // TxtMode
            // 
            TxtMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            TxtMode.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtMode.ForeColor = Color.GreenYellow;
            TxtMode.Location = new Point(177, 0);
            TxtMode.Name = "TxtMode";
            TxtMode.Size = new Size(340, 74);
            TxtMode.TabIndex = 12;
            TxtMode.Text = "Resistance 2W";
            TxtMode.TextAlign = ContentAlignment.TopRight;
            // 
            // TxtValue
            // 
            TxtValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            TxtValue.CausesValidation = false;
            TxtValue.Font = new Font("Segoe UI Semibold", 63.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtValue.Location = new Point(-27, 66);
            TxtValue.Name = "TxtValue";
            TxtValue.Size = new Size(554, 105);
            TxtValue.TabIndex = 13;
            TxtValue.Text = "+123.456789";
            TxtValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // BtnMenu
            // 
            BtnMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnMenu.AutoSize = true;
            BtnMenu.CausesValidation = false;
            BtnMenu.Font = new Font("Segoe UI Black", 20.25F, FontStyle.Bold);
            BtnMenu.ForeColor = Color.Orange;
            BtnMenu.Location = new Point(613, -1);
            BtnMenu.Name = "BtnMenu";
            BtnMenu.Size = new Size(36, 37);
            BtnMenu.TabIndex = 14;
            BtnMenu.Text = "≡";
            BtnMenu.Click += BtnMenu_Click;
            // 
            // PnlMain
            // 
            PnlMain.BackColor = Color.Transparent;
            PnlMain.CausesValidation = false;
            PnlMain.Controls.Add(PicRunning);
            PnlMain.Controls.Add(PicNotRunning);
            PnlMain.Controls.Add(LblHandle);
            PnlMain.Controls.Add(TxtUnit);
            PnlMain.Controls.Add(LblExpand);
            PnlMain.Controls.Add(ImgError);
            PnlMain.Controls.Add(TxtValue);
            PnlMain.Controls.Add(BtnMenu);
            PnlMain.Controls.Add(TxtMode);
            PnlMain.Dock = DockStyle.Top;
            PnlMain.Location = new Point(0, 0);
            PnlMain.Name = "PnlMain";
            PnlMain.Size = new Size(652, 185);
            PnlMain.TabIndex = 15;
            PnlMain.MouseDown += PnlMain_MouseDown;
            PnlMain.MouseMove += PnlMain_MouseMove;
            PnlMain.MouseUp += PnlMain_MouseUp;
            // 
            // PicRunning
            // 
            PicRunning.Image = Properties.Resources.Running;
            PicRunning.ImageLocation = "";
            PicRunning.Location = new Point(583, 7);
            PicRunning.Name = "PicRunning";
            PicRunning.Size = new Size(29, 28);
            PicRunning.SizeMode = PictureBoxSizeMode.Zoom;
            PicRunning.TabIndex = 22;
            PicRunning.TabStop = false;
            PicRunning.Visible = false;
            PicRunning.Click += PicRunning_Click;
            // 
            // PicNotRunning
            // 
            PicNotRunning.Image = (Image)resources.GetObject("PicNotRunning.Image");
            PicNotRunning.ImageLocation = "";
            PicNotRunning.Location = new Point(583, 7);
            PicNotRunning.Name = "PicNotRunning";
            PicNotRunning.Size = new Size(29, 28);
            PicNotRunning.SizeMode = PictureBoxSizeMode.Zoom;
            PicNotRunning.TabIndex = 23;
            PicNotRunning.TabStop = false;
            PicNotRunning.Click += PicNotRunning_Click;
            // 
            // TxtUnit
            // 
            TxtUnit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            TxtUnit.CausesValidation = false;
            TxtUnit.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtUnit.ForeColor = Color.Cyan;
            TxtUnit.Location = new Point(513, 87);
            TxtUnit.Name = "TxtUnit";
            TxtUnit.Size = new Size(111, 72);
            TxtUnit.TabIndex = 16;
            TxtUnit.Text = "kHz";
            TxtUnit.TextAlign = ContentAlignment.MiddleLeft;
            TxtUnit.UseMnemonic = false;
            // 
            // LblExpand
            // 
            LblExpand.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LblExpand.AutoSize = true;
            LblExpand.BackColor = Color.Transparent;
            LblExpand.Cursor = Cursors.SizeNS;
            LblExpand.FlatStyle = FlatStyle.Flat;
            LblExpand.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblExpand.ForeColor = Color.Snow;
            LblExpand.Location = new Point(563, 161);
            LblExpand.Name = "LblExpand";
            LblExpand.Size = new Size(38, 21);
            LblExpand.TabIndex = 20;
            LblExpand.Text = "▲▼";
            LblExpand.UseMnemonic = false;
            LblExpand.Click += LblExpand_Click;
            // 
            // ImgError
            // 
            ImgError.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ImgError.BackgroundImageLayout = ImageLayout.Zoom;
            ImgError.Image = Properties.Resources.Bang;
            ImgError.InitialImage = Properties.Resources.Bang;
            ImgError.Location = new Point(545, 0);
            ImgError.Name = "ImgError";
            ImgError.Size = new Size(32, 32);
            ImgError.SizeMode = PictureBoxSizeMode.Zoom;
            ImgError.TabIndex = 17;
            ImgError.TabStop = false;
            ImgError.Visible = false;
            ImgError.Click += ImgError_Click;
            // 
            // UpdateTimer
            // 
            UpdateTimer.Interval = 250;
            UpdateTimer.Tick += UpdateTimer_Tick;
            // 
            // MnuPopup
            // 
            MnuPopup.ImageScalingSize = new Size(20, 20);
            MnuPopup.Items.AddRange(new ToolStripItem[] { ItmStartStop, ItmSettings, ItmExit });
            MnuPopup.Name = "MnuPopup";
            MnuPopup.Size = new Size(134, 70);
            // 
            // ItmStartStop
            // 
            ItmStartStop.Name = "ItmStartStop";
            ItmStartStop.Size = new Size(133, 22);
            ItmStartStop.Text = "&Start / Stop";
            ItmStartStop.ToolTipText = "Start or Stop the OWON display.";
            ItmStartStop.Click += ItmStart_Click;
            // 
            // ItmSettings
            // 
            ItmSettings.Name = "ItmSettings";
            ItmSettings.Size = new Size(133, 22);
            ItmSettings.Text = "Settings";
            ItmSettings.Click += ItmSettings_Click;
            // 
            // ItmExit
            // 
            ItmExit.Name = "ItmExit";
            ItmExit.Size = new Size(133, 22);
            ItmExit.Text = "&Exit";
            ItmExit.ToolTipText = "Exit the application.";
            ItmExit.Click += ItmExit_Click;
            // 
            // ToolBar
            // 
            ToolBar.BackColor = Color.Silver;
            ToolBar.BackgroundImageLayout = ImageLayout.None;
            ToolBar.CanOverflow = false;
            ToolBar.Dock = DockStyle.Fill;
            ToolBar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ToolBar.GripMargin = new Padding(0);
            ToolBar.GripStyle = ToolStripGripStyle.Hidden;
            ToolBar.ImageScalingSize = new Size(20, 20);
            ToolBar.Items.AddRange(new ToolStripItem[] { toolStripSeparator11, BtnDCV, toolStripSeparator1, BtnACV, toolStripSeparator2, BtnDCI, toolStripSeparator3, BtnACI, toolStripSeparator4, BtnR2W, toolStripSeparator5, BtnR4W, toolStripSeparator6, BtnFreq, toolStripSeparator7, BtnCap, toolStripSeparator8, BtnCont, toolStripSeparator9, BtnDiode, toolStripSeparator10 });
            ToolBar.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            ToolBar.Location = new Point(0, 185);
            ToolBar.Name = "ToolBar";
            ToolBar.Padding = new Padding(0);
            ToolBar.RenderMode = ToolStripRenderMode.Professional;
            ToolBar.Size = new Size(652, 31);
            ToolBar.Stretch = true;
            ToolBar.TabIndex = 16;
            ToolBar.Text = "ToolBar";
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 31);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 31);
            // 
            // BtnACV
            // 
            BtnACV.BackColor = Color.FromArgb(64, 64, 64);
            BtnACV.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnACV.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnACV.ForeColor = Color.Snow;
            BtnACV.ImageTransparentColor = Color.Magenta;
            BtnACV.Name = "BtnACV";
            BtnACV.Size = new Size(34, 28);
            BtnACV.Text = "ACV";
            BtnACV.ToolTipText = "AC Voltage";
            BtnACV.Click += BtnACV_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 31);
            // 
            // BtnDCI
            // 
            BtnDCI.BackColor = Color.FromArgb(64, 64, 64);
            BtnDCI.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnDCI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDCI.ForeColor = Color.Snow;
            BtnDCI.ImageTransparentColor = Color.Magenta;
            BtnDCI.Name = "BtnDCI";
            BtnDCI.Size = new Size(31, 28);
            BtnDCI.Text = "DCI";
            BtnDCI.ToolTipText = "DC Current";
            BtnDCI.Click += BtnDCI_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 31);
            // 
            // BtnACI
            // 
            BtnACI.BackColor = Color.FromArgb(64, 64, 64);
            BtnACI.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnACI.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnACI.ForeColor = Color.Snow;
            BtnACI.ImageTransparentColor = Color.Magenta;
            BtnACI.Name = "BtnACI";
            BtnACI.Size = new Size(30, 28);
            BtnACI.Text = "ACI";
            BtnACI.ToolTipText = "AC Current";
            BtnACI.Click += BtnACI_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 31);
            // 
            // BtnR2W
            // 
            BtnR2W.BackColor = Color.FromArgb(64, 64, 64);
            BtnR2W.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnR2W.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnR2W.ForeColor = Color.Snow;
            BtnR2W.ImageTransparentColor = Color.Magenta;
            BtnR2W.Name = "BtnR2W";
            BtnR2W.Size = new Size(38, 28);
            BtnR2W.Text = "R2W";
            BtnR2W.ToolTipText = "Resistance (2 Wire)";
            BtnR2W.Click += BtnR2W_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 31);
            // 
            // BtnR4W
            // 
            BtnR4W.BackColor = Color.FromArgb(64, 64, 64);
            BtnR4W.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnR4W.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnR4W.ForeColor = Color.Snow;
            BtnR4W.ImageTransparentColor = Color.Magenta;
            BtnR4W.Name = "BtnR4W";
            BtnR4W.Size = new Size(38, 28);
            BtnR4W.Text = "R4W";
            BtnR4W.ToolTipText = "Resistance (4 Wire)";
            BtnR4W.Click += BtnR4W_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 31);
            // 
            // BtnFreq
            // 
            BtnFreq.BackColor = Color.FromArgb(64, 64, 64);
            BtnFreq.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnFreq.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnFreq.ForeColor = Color.Snow;
            BtnFreq.ImageTransparentColor = Color.Magenta;
            BtnFreq.Name = "BtnFreq";
            BtnFreq.Size = new Size(40, 28);
            BtnFreq.Text = "FREQ";
            BtnFreq.ToolTipText = "Frequency";
            BtnFreq.Click += BtnFreq_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 31);
            // 
            // BtnCap
            // 
            BtnCap.BackColor = Color.FromArgb(64, 64, 64);
            BtnCap.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnCap.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnCap.ForeColor = Color.Snow;
            BtnCap.ImageTransparentColor = Color.Magenta;
            BtnCap.Name = "BtnCap";
            BtnCap.Size = new Size(33, 28);
            BtnCap.Text = "CAP";
            BtnCap.ToolTipText = "Capacitance";
            BtnCap.Click += BtnCap_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(6, 31);
            // 
            // BtnCont
            // 
            BtnCont.BackColor = Color.FromArgb(64, 64, 64);
            BtnCont.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnCont.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnCont.ForeColor = Color.Snow;
            BtnCont.ImageTransparentColor = Color.Magenta;
            BtnCont.Name = "BtnCont";
            BtnCont.Size = new Size(43, 28);
            BtnCont.Text = "CONT";
            BtnCont.ToolTipText = "Continuity";
            BtnCont.Click += BtnCont_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(6, 31);
            // 
            // BtnDiode
            // 
            BtnDiode.BackColor = Color.FromArgb(64, 64, 64);
            BtnDiode.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnDiode.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDiode.ForeColor = Color.Snow;
            BtnDiode.ImageTransparentColor = Color.Magenta;
            BtnDiode.Name = "BtnDiode";
            BtnDiode.Size = new Size(48, 28);
            BtnDiode.Text = "DIODE";
            BtnDiode.ToolTipText = "Diode Mode";
            BtnDiode.Click += BtnDiode_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(6, 31);
            // 
            // FrmHantekUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoValidate = AutoValidate.Disable;
            BackColor = Color.FromArgb(64, 64, 64);
            CausesValidation = false;
            ClientSize = new Size(652, 216);
            Controls.Add(ToolBar);
            Controls.Add(PnlMain);
            ForeColor = Color.Yellow;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(300, 45);
            Name = "FrmHantekUI";
            Text = "Hantek DMM";
            TopMost = true;
            TransparencyKey = Color.FromArgb(96, 64, 32);
            Load += FrmHantekUI_Load;
            Layout += FrmHantekUI_Layout;
            PnlMain.ResumeLayout(false);
            PnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PicRunning).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicNotRunning).EndInit();
            ((System.ComponentModel.ISupportInitialize)ImgError).EndInit();
            MnuPopup.ResumeLayout(false);
            ToolBar.ResumeLayout(false);
            ToolBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LblHandle;
        private Label TxtMode;
        private Label TxtValue;
        private Label BtnMenu;
        private Panel PnlMain;
        private System.Windows.Forms.Timer UpdateTimer;
        private ContextMenuStrip MnuPopup;
        private ToolStripMenuItem ItmStartStop;
        private ToolStripMenuItem ItmExit;
        private Label TxtUnit;
        private ToolStripMenuItem ItmWindowColor;
        private ToolStripMenuItem ItmBlack;
        private ToolStripMenuItem ItmBlue;
        private ToolStripMenuItem ItmGray;
        private ToolStripMenuItem ItmDarkBlue;
        private ToolStripMenuItem ItmTransparent;
        private PictureBox ImgError;
        //private Button BtnCtrlPanel;
        private Label LblExpand;
        private ToolStrip ToolBar;
        private ToolStripButton BtnDCV;
        private ToolStripButton BtnACV;
        private ToolStripButton BtnDCI;
        private ToolStripButton BtnACI;
        private ToolStripButton BtnR2W;
        private ToolStripButton BtnR4W;
        private ToolStripButton BtnFreq;
        private ToolStripButton BtnCap;
        private ToolStripButton BtnCont;
        private ToolStripButton BtnDiode;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem ItmTextColor;
        private ToolStripMenuItem ItmSettings;
        private PictureBox PicRunning;
        private PictureBox PicNotRunning;
    }
}