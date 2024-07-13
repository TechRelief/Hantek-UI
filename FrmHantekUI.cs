using SCPI;

namespace Hantek_UI
{
    public partial class FrmHantekUI : Form
    {
        #region Local Variables

        const string deviceName = "Hantek";

        private int precision = 5; //Default precision
        /// <summary>The hantek object is the layer where communications with the SCPI device happens.</summary>
        private ScpiDevice? hantek = null;
        /// <summary>The QueryValue is an object where the data read from the SCPI device is stored and processed.</summary>
        private ScpiQueryValue qryValue = new("0", MeasureMode.None, 5);
        /// <summary>The lastVvalue is to keep track of the value currently displayed and is used to avoid overwriting values that have not changed.</summary>
        private string lastValue = string.Empty;

        #endregion

        #region MeasureMode Management

        /// <summary>Gets the measurement mode from its text based name.</summary>
        /// <param name="smode">The mode name.</param>
        /// <returns>MeasureMode enumerated mode value.</returns>
        private static MeasureMode GetModeFromString(string smode)
        {
            try
            {
                return (MeasureMode)Enum.Parse(typeof(MeasureMode), smode);
            }
            catch { return MeasureMode.None; }
        }

        /// <summary>Sets the mode specified or if not specified will retrieve it from the Mode Setting value.</summary>
        /// <param name="mode">The mode or null if the Mode setting should be used.</param>
        /// <returns>MeasureMode enumerated value.</returns>
        private MeasureMode SetMode(MeasureMode? mode)
        {
            mode ??= GetModeFromString(Settings.Default.Mode);
            ToolStripButton btn = GetButton(mode.ToString() ?? MeasureMode.DCV.ToString());
            return SetMode(btn);
        }

        /// <summary>
        /// Sets the mode associated with the specified tool bar button.
        /// If not specified (null) it will get and set the mode according to the Mode setting.  
        /// It will save the mode to the Mode setting for future use.
        /// </summary>
        /// <param name="btn">The Tool Bar button associated with the Mode to be set..</param>
        /// <returns>MeasureMode enumerated value.</returns>
        private MeasureMode SetMode(ToolStripButton? btn = null)
        {
            MeasureMode mode;
            bool timerOn = UpdateTimer.Enabled;
            try
            {
                if (timerOn)
                {
                    Stop();
                }

                //If a button was not provided determine mode from Settings
                if (btn == null)
                {
                    string smode = Settings.Default.Mode;
                    mode = GetModeFromString(smode);
                    btn = GetButton(smode);
                }
                else
                {
                    //So the btn.Text is equivalent to the string value of the enumerated MeasureMode
                    mode = GetModeFromString(btn.Text ?? String.Empty);
                }
                if (mode == MeasureMode.None)
                    throw new InvalidOperationException($"Mode could not be determined.");

                //Check if we need to remove the hilight from a button first
                if (ToolBar.Tag != null) //The tool bar tag is used to store the active button control
                {
                    ToolStripButton? prevButton = (ToolStripButton)ToolBar.Tag;
                    if (prevButton != null) Lowlite(prevButton);
                }

                ToolBar.Tag = btn; //Record which button is highlighted
                if (btn != null) Hilite(btn);
                Settings.Default.Mode = mode.ToString();
                Settings.Default.Save();
                if (hantek == null) //Should not happen in normal operation but you never know...
                {
                    return MeasureMode.None;
                }
                hantek.SetDeviceMode(mode, out int errCount);
                qryValue.Mode = mode;
                ImgError.Visible = (hantek.Errors.Count > 0);
                Thread.Sleep(3000); //So the Hantek DMM seems to become unresponsive if you change its mode and then immediately ask for a reading, so a delay was needed here.
                DisplayValues();
                return mode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SetMode(btn) returned an error: {ex.Message}");
                return MeasureMode.None;
            }
            finally
            {
                if (timerOn) { Start(); }
            }
        }

        #endregion MeasureMode Management

        #region Initialization

        public FrmHantekUI()
        {
            InitializeComponent();
        }

        private void FrmHantekUI_Load(object sender, EventArgs e)
        {
            HideToolBar();
            ResetDisplay();
            InitializeDisplay();
        }

        private void FrmHantekUI_Layout(object sender, LayoutEventArgs e)
        {
            //If it desirable to fudge with the layout like control sizes and positions etc.  Do it here...
        }

        #endregion Initialization

        #region Window Movement

        //These variables are used to provide a means to allow a window without a border to be moved on screen.
        private bool mouseDown;
        private Point lastLocation;

        private void LblHandle_MouseDown(object sender, MouseEventArgs e)
        {
            PnlMain_MouseDown(sender, e);
        }

        private void LblHandle_MouseMove(object sender, MouseEventArgs e)
        {
            PnlMain_MouseMove(sender, e);
        }

        private void LblHandle_MouseUp(object sender, MouseEventArgs e)
        {
            PnlMain_MouseUp(sender, e);
        }

        private void PnlMain_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void PnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void PnlMain_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        #endregion Window Movement

        #region Display Setup and Management

        /// <summary>Resets the display by clearing out any data displayed.</summary>
        private void ResetDisplay()
        {
            TxtValue.Text = "---.-----";
            TxtMode.Text = string.Empty;
            TxtUnit.Text = string.Empty;
            lastValue = string.Empty;
        }

        /// <summary>Displays the text values on the display like value, mode and unit.</summary>
        private void DisplayValues()
        {
            qryValue.GetDisplayValues(out string svalu, out string sunit);
            TxtValue.Text = svalu.TrimEnd();
            TxtUnit.Text = sunit;
            TxtMode.Text = qryValue.MeasureDesc;
        }

        /// <summary>Initializes the display and attemps to connect to the Hantek DMM.</summary>
        private void InitializeDisplay()
        {
            try
            {
                hantek = new ScpiDevice();
                if (!hantek.FindAndOpenDevice(deviceName))
                {
                    throw new Exception($"Could not find the {deviceName} device. Make sure it is on and connected to the USB port...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error: {ex.Message}", "Error Message");
            }
            try
            {
                bool showBorder = Settings.Default.HasBorder; //Check if the border should be displayed
                precision = Settings.Default.Precision; //Number for places after the decimal point
                Color windowColor = Settings.Default.WindowColor;
                SetWindowColor(windowColor);
                if (showBorder)
                    FormBorderStyle = FormBorderStyle.FixedDialog;
                else
                    FormBorderStyle = FormBorderStyle.None;
                string smode = Settings.Default.Mode;
                MeasureMode mode = SetMode(GetModeFromString(smode));
                //Settings.Default.Mode = mode.ToString();
                //Settings.Default.Save();
                TxtValue.ForeColor = Settings.Default.TextColor;
                TxtMode.ForeColor = Settings.Default.ModeColor;
                TxtUnit.ForeColor = Settings.Default.UnitColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>Updates the display by reading the devices' value and measure mode.</summary>
        private void UpdateDisplay()
        {
            bool timerOn = UpdateTimer.Enabled;
            try
            {
                if (timerOn) Stop();

                if (hantek == null || !hantek.IsReady)
                {
                    ResetDisplay();
                    return;
                }
                _ = hantek.ReadValue(ref qryValue, precision);
                ImgError.Visible = (hantek.Errors.Count > 0);
                // qryValue.Mode = hantek.GetMeasureMode(); //Superfluous if the DMM stays in remote mode
                if (lastValue != qryValue.Value)
                {
                    DisplayValues();
                }
                lastValue = qryValue.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                if (timerOn) Start();
            }
        }

#endregion Display Setup and Management

        #region Update Timer

        /****************************************************************************
         * When the DMM is put in remote mode it is necesary to send a READ? command
         * each time to trigger it to do a reading and return the reading value.
         * Becuase of this it is necessary to have an update timer to trigger periodic
         * readings so the display will be updated.  The default is to trigger a reading
         * every 250 mS but this can be changed by changing the setting in the UpdateTimer 
         * control.
         *****************************************************************************/

        /// <summary>Stops the update timer and optionally reset the display.</summary>
        /// <param name="Reset">If set to <c>true</c> will reset the display.</param>
        private void Stop(bool Reset = false)
        {
            UpdateTimer.Stop();
            if (Reset)
                ResetDisplay();
        }

        /// <summary>Starts the update timer.</summary>
        private void Start()
        {
            UpdateTimer.Start();
        }

        /// <summary>
        /// Handles the Click event of the BtnStart control.
        /// If there is no connection to the Hantek device, it will attempt to locate the Hantek device and open a session on it.  
        /// If successful or already connected the update timer will be started. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.Exception">Could not find the {deviceName} device. Make sure it is on and connected to the USB port...</exception>
        private void ItmStart_Click(object sender, EventArgs e)
        {
            hantek ??= new();
            if (!hantek.IsReady)
            {
                try
                {
                    if (!hantek.FindAndOpenDevice(deviceName))
                    {
                        throw new Exception($"Could not find the {deviceName} device. Make sure it is on and connected to the USB port...");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Start Error: {ex.Message}", "Error Message");
                    return;
                }
            }
            if (hantek != null && hantek.IsReady && UpdateTimer.Enabled) Stop(); else Start();
        }

        /// <summary>Handles the Tick event of the UpdateTimer control.
        /// It is used to trigger periodic readings from the SCPI DMM device.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        #endregion Update Timer

        #region Menu Commands

        private void BtnMenu_Click(object sender, EventArgs e)
        {
            MnuPopup.Show(BtnMenu, 0, 0);
        }

        private void ItmExit_Click(object sender, EventArgs e)
        {
            hantek?.Close();
            this.Close();
        }

        private void SetWindowColor(Color color)
        {
            bool isTransparent = (color == Color.Transparent);
            Settings.Default.WindowColor = color;
            Settings.Default.Save();

            if (isTransparent)
            {
                TransparencyKey = BackColor;
            }
            else
            {
                TransparencyKey = Color.LimeGreen;
                BackColor = color;
            }
        }

        #endregion Menu Commands

        #region Error Management

        /// <summary>Handles the Click event of the ImgError control.
        /// It is only active if there are errors in the devices error queue.  
        /// In that case it will display a list of errors and clear them out.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ImgError_Click(object sender, EventArgs e)
        {
            if (hantek != null)
            {
                MessageBox.Show(this, hantek.Errors.ToString(), "Error List");
                hantek.Errors.ClearList();
            }
        }

        #endregion Error Management

        #region Toolbar

        private void HideToolBar()
        {
            if (ToolBar.Visible)
            {
                ToolBar.Hide();
                this.Height -= ToolBar.Height;
            }
            LblExpand.Text = "▼";
        }

        private void ShowToolBar()
        {
            if (!ToolBar.Visible)
            {
                this.Height += ToolBar.Height;
                ToolBar.Show();
            }
            LblExpand.Text = "▲";
        }

        private void ToggleToolBar()
        {
            if (ToolBar.Visible)
                HideToolBar();
            else
                ShowToolBar();
        }

        private void LblExpand_Click(object sender, EventArgs e)
        {
            ToggleToolBar();
        }
        private static void Hilite(ToolStripButton btn)
        {
            btn.ForeColor = Color.Firebrick;
            btn.BackColor = Color.Linen;
        }

        private static void Lowlite(ToolStripButton btn)
        {
            btn.ForeColor = Color.White;
            btn.BackColor = Color.DimGray;
        }

        private ToolStripButton GetButton(string text)
        {
            return text.ToUpperInvariant() switch
            {
                "DCV" => BtnDCV,
                "ACV" => BtnACV,
                "DCI" => BtnDCI,
                "ACI" => BtnACI,
                "R2W" => BtnR2W,
                "R4W" => BtnR4W,
                "CONT" => BtnCont,
                "DIODE" => BtnDiode,
                "CAP" => BtnCap,
                "FREQ" => BtnFreq,
                _ => BtnDCV,
            };
        }

        #region ToolBar Buttons

        private void BtnDCV_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnACV_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnDCI_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnACI_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnR2W_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnR4W_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnFreq_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnCap_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnCont_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        private void BtnDiode_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        #endregion ToolBar Buttons

        #endregion Toolbar

        #region Settings

        private void ItmSettings_Click(object sender, EventArgs e)
        {
            bool timerOn = (UpdateTimer.Enabled);
            if (timerOn) Stop();
            //Open the Settings Form
            using FrmSettings frmSettings = new();
            DialogResult rslt = frmSettings.ShowDialog(this);
            if (rslt == DialogResult.OK)
            {
                // Text Color  
                if (frmSettings.TextColor != null)
                {
                    Settings.Default.TextColor = frmSettings.TextColor.Value;
                    TxtValue.ForeColor = Settings.Default.TextColor;
                }
                // Window Color
                if (frmSettings.WindowColor != null)
                {
                    Settings.Default.WindowColor = frmSettings.WindowColor.Value;
                    SetWindowColor(frmSettings.WindowColor.Value);
                }
                // Mode Color
                if (frmSettings.ModeColor != null)
                {
                    Settings.Default.ModeColor = frmSettings.ModeColor.Value;
                    TxtMode.ForeColor = Settings.Default.ModeColor;
                }
                // Unit Color
                if (frmSettings.UnitColor != null)
                {
                    Settings.Default.UnitColor = frmSettings.UnitColor.Value;
                    TxtUnit.ForeColor = Settings.Default.UnitColor;
                }
                // Window Border
                if (frmSettings.WindowBorder != null)
                {
                    bool hasBorder = frmSettings.WindowBorder.Value;
                    Settings.Default.HasBorder = hasBorder;
                    if (hasBorder)
                        FormBorderStyle = FormBorderStyle.FixedDialog;
                    else
                        FormBorderStyle = FormBorderStyle.None;
                }
                // Precision
                if (frmSettings.Precision != null)
                {
                    precision = frmSettings.Precision.Value;
                    if (precision >= 10) precision = 5;
                    qryValue?.SetFormatMask(precision);
                    Settings.Default.Precision = precision;
                }
                Settings.Default.Save();
                DisplayValues();
            }
            if (timerOn) Start();
        }

        #endregion Settings
    }
}
