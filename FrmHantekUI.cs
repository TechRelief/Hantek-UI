// ***********************************************************************
// Assembly         : Hantek-UI
// Author           : Ed Paay @ Tech-Relief LLC
// Created          : 07-13-2024
//
// Last Modified By : Ed
// Last Modified On : 07-13-2024
// ***********************************************************************
// <summary>
// An Application for displaying and controlling a Hantek HDM3000 series
// Digital Multi-meter.
// Some icons sourced from: https://icons8.com
// </summary>
// ***********************************************************************
using System.CodeDom;

using SCPI;

using Windows.Security.EnterpriseData;

namespace Hantek_UI
{
    /// <summary>
    /// Class FrmHantekUI.
    /// Implements the <see cref="System.Windows.Forms.Form" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class FrmHantekUI : Form
    {
        #region Local Variables

        /// <summary>
        /// The device name
        /// </summary>
        const string deviceName = "Hantek";

        /// <summary>
        /// The precision
        /// </summary>
        private int precision = 5; //Default precision
        /// <summary>
        /// The hantek object is the layer where communications with the SCPI device happens.
        /// </summary>
        private ScpiDevice? hantek = null;
        /// <summary>
        /// The QueryValue is an object where the data read from the SCPI device is stored and processed.
        /// </summary>
        private ScpiQueryValue qryValue = new("0", MeasureMode.None, 5);
        /// <summary>
        /// The lastVvalue is to keep track of the value currently displayed and is used to avoid overwriting values that have not changed.
        /// </summary>
        private string lastValue = string.Empty;

        #endregion

        #region MeasureMode Management

        /// <summary>
        /// Gets the measurement mode from its text based name.
        /// </summary>
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

        /// <summary>
        /// Sets the mode specified or if not specified will retrieve it from the Mode Setting value.
        /// </summary>
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
        /// <exception cref="System.InvalidOperationException">Mode could not be determined.</exception>
        private MeasureMode SetMode(ToolStripButton? btn = null)
        {
            MeasureMode mode;
            bool timerOn = UpdateTimer.Enabled;
            try
            {
                if (timerOn)
                {
                    Pause();
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
                ReportError(ex.Message);
                //MessageBox.Show($"SetMode(btn) returned an error: {ex.Message}");
                return MeasureMode.None;
            }
            finally
            {
                if (timerOn) { Resume(); }
            }
        }

        #endregion MeasureMode Management

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmHantekUI"/> class.
        /// </summary>
        public FrmHantekUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the FrmHantekUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FrmHantekUI_Load(object sender, EventArgs e)
        {
            HideToolBar();
            ResetDisplay();
            InitializeDisplay();
        }

        /// <summary>
        /// Handles the Layout event of the FrmHantekUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void FrmHantekUI_Layout(object sender, LayoutEventArgs e)
        {
            //If it desirable to fudge with the layout like control sizes and positions etc.  Do it here...
        }

        #endregion Initialization

        #region Window Movement

        //These variables are used to provide a means to allow a window without a border to be moved on screen.
        private bool mouseDown;
        private Point lastLocation;

        /// <summary>
        /// Handles the MouseDown event of the LblHandle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void LblHandle_MouseDown(object sender, MouseEventArgs e)
        {
            PnlMain_MouseDown(sender, e);
        }

        /// <summary>
        /// Handles the MouseMove event of the LblHandle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void LblHandle_MouseMove(object sender, MouseEventArgs e)
        {
            PnlMain_MouseMove(sender, e);
        }

        /// <summary>
        /// Handles the MouseUp event of the LblHandle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void LblHandle_MouseUp(object sender, MouseEventArgs e)
        {
            PnlMain_MouseUp(sender, e);
        }

        /// <summary>
        /// Handles the MouseDown event of the PnlMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void PnlMain_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        /// <summary>
        /// Handles the MouseMove event of the PnlMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void PnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the PnlMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void PnlMain_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        #endregion Window Movement

        #region Display Setup and Management

        /// <summary>
        /// Reports an error by adding it to the Errors list and displayes the error icon so the user can click it to view any accumulated errors.
        /// </summary>
        /// <param name="errorMsg">The error message.</param>
        private void ReportError(string errorMsg)
        {
            if (hantek == null)
                MessageBox.Show(this, $"Error: {errorMsg}", "Error Message");
            else
            {
                hantek.SetError(errorMsg);
                ImgError.Show();
            }
        }

        /// <summary>
        /// Resets the display by clearing out any data displayed.
        /// </summary>
        private void ResetDisplay()
        {
            TxtValue.Text = "---.-----";
            TxtMode.Text = string.Empty;
            TxtUnit.Text = string.Empty;
            lastValue = string.Empty;
        }

        /// <summary>
        /// Displays the text values on the display like value, mode and unit.
        /// </summary>
        private void DisplayValues()
        {
            qryValue.GetDisplayValues(out string svalu, out string sunit);
            TxtValue.Text = svalu.TrimEnd();
            TxtUnit.Text = sunit;
            TxtMode.Text = qryValue.MeasureDesc;
        }

        /// <summary>
        /// Initializes the display and attemps to connect to the Hantek DMM.
        /// </summary>
        /// <exception cref="System.Exception">Could not find the {deviceName} device. Make sure it is on and connected to the USB port...</exception>
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
                ReportError(ex.Message);
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
                TxtValue.ForeColor = Settings.Default.TextColor;
                TxtMode.ForeColor = Settings.Default.ModeColor;
                TxtUnit.ForeColor = Settings.Default.UnitColor;
                UpdateTimer.Interval = Convert.ToInt32(Settings.Default.Interval);
            }
            catch (Exception ex)
            {
                ReportError(ex.Message);
            }
        }

        /// <summary>
        /// Updates the display by reading the devices' value and measure mode.
        /// </summary>
        private void UpdateDisplay()
        {
            bool timerOn = UpdateTimer.Enabled;
            try
            {
                if (timerOn) Pause();

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
                //MessageBox.Show($"Error: {ex.Message}");
                ReportError(ex.Message);
            }
            finally
            {
                if (timerOn) Resume();
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

        /// <summary>Pauses the UpdateTimer.</summary>
        private void Pause()
        {
            UpdateTimer.Stop();
        }

        /// <summary>Resumes the UpdateTimer.</summary>
        private void Resume()
        { 
            UpdateTimer.Start(); 
        }

        /// <summary>
        /// Stops the update timer and optionally reset the display.
        /// </summary>
        /// <param name="Reset">If set to <c>true</c> will reset the display.</param>
        private void Stop(bool Reset = false)
        {
            UpdateTimer.Stop();
            PicRunning.Hide();
            PicNotRunning.Show();
            if (Reset)
                ResetDisplay();
        }

        /// <summary>
        /// Starts the update timer.
        /// </summary>
        private void Start()
        {
            UpdateTimer.Start();
            PicRunning.Show();
            PicNotRunning.Hide();
        }

        /// <summary>Handles the Click event of the PicRunning control and stops the Update Timer.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PicRunning_Click(object sender, EventArgs e)
        {
            Stop();
        }

        /// <summary>Handles the Click event of the PicNotRunning control and starts the UpdateTimer.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PicNotRunning_Click(object sender, EventArgs e)
        {
            Start();
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
                    //MessageBox.Show(this, $"Start Error: {ex.Message}", "Error Message");
                    ReportError(ex.Message);
                    return;
                }
            }
            if (hantek != null && hantek.IsReady && UpdateTimer.Enabled) Stop(); else Start();
        }

        /// <summary>
        /// Handles the Tick event of the UpdateTimer control.
        /// It is used to trigger periodic readings from the SCPI DMM device.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        #endregion Update Timer

        #region Menu Commands

        /// <summary>
        /// Handles the Click event of the BtnMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnMenu_Click(object sender, EventArgs e)
        {
            MnuPopup.Show(BtnMenu, 0, 0);
        }

        /// <summary>
        /// Handles the Click event of the ItmExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ItmExit_Click(object sender, EventArgs e)
        {
            hantek?.Close();
            this.Close();
        }

        /// <summary>
        /// Sets the color of the window.
        /// </summary>
        /// <param name="color">The color.</param>
        private void SetWindowColor(Color color)
        {
            bool isTransparent = (color == Color.Transparent);
            if (isTransparent)
            {
                Settings.Default.WindowColor = Color.Transparent;
                color = Color.Black;
                TransparencyKey = color;
                BackColor = color;
            }
            else
            {
                if (color == Color.White) //If White some of the controls won't show up!
                    color = Color.Black;
                TransparencyKey = Color.White;
                BackColor = color;
            }
            Settings.Default.Save();
        }

        #endregion Menu Commands

        #region Error Management

        /// <summary>
        /// Handles the Click event of the ImgError control.
        /// It is only active if there are errors in the devices error queue.
        /// In that case it will display a list of errors and clear them out.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ImgError_Click(object sender, EventArgs e)
        {
            if (hantek != null)
            {
                MessageBox.Show(this, hantek.Errors.ToString(), "Error List");
                hantek.Errors.ClearList();
                ImgError.Hide();
            }
        }

        #endregion Error Management

        #region Toolbar

        /// <summary>
        /// Hides the tool bar.
        /// </summary>
        private void HideToolBar()
        {
            if (ToolBar.Visible)
            {
                ToolBar.Hide();
                this.Height -= ToolBar.Height;
            }
            LblExpand.Text = "▼";
        }

        /// <summary>
        /// Shows the tool bar.
        /// </summary>
        private void ShowToolBar()
        {
            if (!ToolBar.Visible)
            {
                this.Height += ToolBar.Height;
                ToolBar.Show();
            }
            LblExpand.Text = "▲";
        }

        /// <summary>
        /// Toggles the tool bar.
        /// </summary>
        private void ToggleToolBar()
        {
            if (ToolBar.Visible)
                HideToolBar();
            else
                ShowToolBar();
        }

        /// <summary>
        /// Handles the Click event of the LblExpand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LblExpand_Click(object sender, EventArgs e)
        {
            ToggleToolBar();
        }
        /// <summary>
        /// Hilites the specified BTN.
        /// </summary>
        /// <param name="btn">The BTN.</param>
        private static void Hilite(ToolStripButton btn)
        {
            btn.ForeColor = Color.Firebrick;
            btn.BackColor = Color.Snow;
        }

        /// <summary>
        /// Lowlites the specified BTN.
        /// </summary>
        /// <param name="btn">The BTN.</param>
        private static void Lowlite(ToolStripButton btn)
        {
            btn.ForeColor = Color.Snow;
            btn.BackColor = Color.FromArgb(64, 64, 64);
        }

        /// <summary>
        /// Gets the button.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>ToolStripButton.</returns>
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

        /// <summary>
        /// Handles the Click event of the BtnDCV control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnDCV_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnACV control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnACV_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnDCI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnDCI_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnACI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnACI_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnR2W control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnR2W_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnR4W control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnR4W_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnFreq control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnFreq_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnCap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnCap_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnCont control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnCont_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        /// <summary>
        /// Handles the Click event of the BtnDiode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnDiode_Click(object sender, EventArgs e)
        {
            SetMode(sender as ToolStripButton);
        }

        #endregion ToolBar Buttons

        #endregion Toolbar

        #region Settings

        /// <summary>
        /// Handles the Click event of the ItmSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ItmSettings_Click(object sender, EventArgs e)
        {
            bool timerOn = (UpdateTimer.Enabled);
            try
            {
                if (timerOn) Pause();
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
                    //Timer Interval
                    if (frmSettings.Interval != null)
                    {
                        Settings.Default.Interval = frmSettings.Interval.Value;
                        UpdateTimer.Interval = Convert.ToInt32(frmSettings.Interval);
                    }
                    Settings.Default.Save();
                    DisplayValues();
                }
            }
            finally
            {
                if (timerOn) Resume();
            }
        }

        #endregion Settings

    }
}
