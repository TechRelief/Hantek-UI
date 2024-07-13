// ***********************************************************************
// Assembly         : SCPI
// Author           : Ed Paay
// Created          : 06-30-2024
//
// Last Modified By : Ed Paay
// Last Modified On : 07-04-2024
// ***********************************************************************
// <summary>
// This class was developed for a Hantek 3065 DDM but may be used as a
// template for other SCPI devices with some changes if need be.
// The Hantek DMM device connects to a PC via National Instruments
// "VISA" drivers via the USB port and uses the
// "Standard Commands for Programmable Instruments" API and commands
// (Abrevviated SCPI often promounced skippy) 
// Hantek directs a user to download the "Keysight Instrument Control
// Bundle" from the Keysight website:
// https://www.keysight.com/us/en/lib/software-detail/computer-software/io-libraries-suite-downloads-2175637.html
// It appears that Hantek not only cloned the physical device but also
// tried to make the firmware compatible.  There are settings on the device
// to emulate older DMMs from HP or Keithly but this software assumes it is
// set to respond as a Hantek device.
// The Hantek DMM does not use a COM port in Windows; many smaller devices use
// a USB to Serial converter chip which means it looks like a COM port; the
// Keysight drivers talk directly to the USB port via the Keysight/VISA drivers.
//
// SCPI command usage:
// So the SCPI commands will vary depending on the device addressed.
// For instance an Oscilloscope, DMM and Power Supply would have different
// capabilities and therefore different commands.
//
// The Hantek Multimeters use several commands and the usual order is as
// follows:
// CONF:... is used to set what is going to be measured; CONF:VOLT:DC
// then to set sub settings like selecting a range or a secondary
// display SENS:... is used.  SENS:VOLT:DC:SEC "VOLT:AC" for instance
// would select AC as the secondary display, which could display ripple.
// READ? is used to take a reading and will return a string like:
// +1.11650501E-02, note that +9.90000000E+37 is returned for Overload
// (Resistance etc.) or OPEN (Continuity).  
// After submitting a SCPI command to the device use:
// *STB? to read the errorMsg byte, there are two bits that will tell you
// if there was an error or data is waiting to be returned.
// Bit 2 of the errorMsg byte tells you there are messages on the Error Queue
// to be read.  Read each message using the command: SYST:ERR? at the end
// of the list +0, "No error" will be returned.  If there were errors a
// message Error will display on the DMM screen.  After the last error was
// read it will disappear.
// Bit 4 of the errorMsg byte if set means there is data availabe to be read.
// 
// </summary>
// ***********************************************************************

using System.Text;

namespace SCPI
{
    /// <summary>
    /// Class ScpiDevice is used to encapsulate methods and properties pertaining to the SCPI device being addressed.
    /// </summary>
    public class ScpiDevice
    {
        private int resourceManager;
        private int session;
        private string deviceAddr = string.Empty;
        private string deviceName = string.Empty;
        private bool isReady = false;
        private readonly ErrorList errors = [];

        /// <summary>The error message list, errors will accumulate here.</summary>
        /// <value>The error list.</value>
        public ErrorList Errors { get => errors; /*set => errors = value;*/ }

        /// <summary>Gets the measure mode description.</summary>
        /// <param name="mode">The mode.</param>
        /// <returns>System.String: Description.</returns>
        public static string GetMeasureDesc(MeasureMode mode)
        {
            return mode switch
            {
                MeasureMode.None => string.Empty,
                MeasureMode.DCV => "Voltage DC",
                MeasureMode.ACV => "Voltage AC",
                MeasureMode.R2W => "Resistance 2W",
                MeasureMode.R4W => "Resistance 4W",
                MeasureMode.DCI => "Current DC",
                MeasureMode.ACI => "Current AC",
                MeasureMode.FREQ => "Frequency",
                MeasureMode.CAP => "Capacitance",
                MeasureMode.CONT => "Continuity",
                MeasureMode.DIODE => "Diode",
                MeasureMode.TEMP => "Temperature",
                _ => "* Undefined *",
            };
        }

        /// <summary>Sets the mode on the SCPI device, like DCV, ACV etc.</summary>
        /// <param name="mode">The mode to set the device to.</param>
        /// <param name="errCount">The error count, zero if none.  Errors will be in the error list..</param>
        /// <exception cref="System.ArgumentException">ScpiDevice.SetMode(mode): MeasureMode.None is invalid...</exception>
        /// <exception cref="System.ArgumentException">ScpiDevice.SetMode(mode): Unknown Mode Specified...</exception>
        public void SetDeviceMode(MeasureMode mode, out int errCount)
        {
            switch (mode)
            {
                case MeasureMode.None:
                    throw new ArgumentException("ScpiDevice.SetMode(mode): MeasureMode.None is invalid...");
                case MeasureMode.DCV:
                    SendCommand("CONF:VOLT:DC", out errCount);
                    break;
                case MeasureMode.ACV:
                    SendCommand("CONF:VOLT:AC", out errCount);
                    break;
                case MeasureMode.R2W:
                    SendCommand("CONF:RES", out errCount);
                    break;
                case MeasureMode.R4W:
                    SendCommand("CONF:FRES", out errCount);
                    break;
                case MeasureMode.DCI:
                    SendCommand("CONF:CURR:DC", out errCount);
                    break;
                case MeasureMode.ACI:
                    SendCommand("CONF:CURR:AC", out errCount);
                    break;
                case MeasureMode.FREQ:
                    SendCommand("CONF:FREQ", out errCount);
                    break;
                case MeasureMode.CAP:
                    SendCommand("CONF:CAP", out errCount);
                    break;
                case MeasureMode.CONT:
                    SendCommand("CONF:CONT", out errCount);
                    break;
                case MeasureMode.DIODE:
                    SendCommand("CONF:DIOD", out errCount);
                    break;
                case MeasureMode.TEMP:
                    SendCommand("CONF:TEMP", out errCount);  //Don't have a temperature probe..  Will have to add FRTD|RTD|FTH|THER to tell it which kind of probe to use.
                    break;
                default:
                    throw new ArgumentException("ScpiDevice.SetMode(mode): Unknown Mode Specified..."); ;
            }
        }

        /// <summary>
        /// Gets or sets the device address.
        /// </summary>
        /// <value>The device address.</value>
        public string DeviceAddr { get => deviceAddr; set => deviceAddr = value; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string DeviceName { get => deviceName; set => deviceName = value; }

        /// <summary>
        /// Will return true if the device is ready for action.
        /// </summary>
        /// <value>True if ready.</value>
        public bool IsReady { get => isReady; /*set => isReady = value;*/ }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScpiDevice" /> class.
        /// </summary>
        public ScpiDevice()
        {
            if (!OpenResourceManager())
                throw new InvalidOperationException("ScpiDevice: Could not open the VISA ResourceManager.");
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ScpiDevice" /> class.
        /// </summary>
        ~ScpiDevice()
        {
            Close();
        }

        /// <summary>
        /// Closes the ScpiDevice instance and closes any resources it has in use.
        /// </summary>
        public void Close()
        {
            isReady = false;
            try
            {
                if (session != 0) _ = Visa32.viClose(session);
            }
            catch { }
            try
            {
                if (resourceManager != 0) _ = Visa32.viClose(resourceManager);
            }
            catch { }
        }

        /// <summary>Reads the measure mode from the device like DCV, ACV, DCA, ACA etc..</summary>
        /// <returns>MeasureMode.</returns>
        public MeasureMode GetMeasureMode()
        {
            const string modeCmd = $"SENS:FUNC?\n";  //The status byte read command
            string sMode = SendAndRead(modeCmd, out _);
            switch (sMode)
            {
                case "VOLT":
                    return MeasureMode.DCV;
                case "VOLT:AC":
                    return MeasureMode.ACV;
                case "DIOD":
                    return MeasureMode.DIODE;
                case "CURR":
                    return MeasureMode.DCI;
                case "CURR:AC":
                    return MeasureMode.ACI;
                case "CONT":
                    return MeasureMode.CONT;
                case "CAP":
                    return MeasureMode.CAP;
                case "FREQ":
                    return MeasureMode.FREQ;
                case "RES":
                    return MeasureMode.R2W;
                case "FRES":
                    return MeasureMode.R4W;
                default:
                    return MeasureMode.None;
            }
        }

        /// <summary>Reads the status byte and sets errorBitIsSet boolean output to reflect if 
        /// there are error messages on the queue.</summary>
        /// <param name="errorBitIsOn">if set to <c>true</c> the error message bit is on.</param>
        /// <returns>System.Byte: The status byte.</returns>
        /// <exception cref="System.Exception">ReadStatusByte failed on write or read.</exception>
        public byte ReadStatusByte(out bool errorBitIsOn)
        {
            const string statCmd = $"*STB?\n";  //The status byte read command
            const byte ErrQBit = 0b00000100;  //Bit 2 of the status byte is on if there are errors

            try
            {
                WriteBytes(statCmd); //Send the *STB? command
            }
            catch (Exception e)
            {
                throw new Exception($"ReadStatusByte: WriteBytes({statCmd}) failed with message: {e.Message}", e);
            }
            string stbByteStr;
            try
            {
                stbByteStr = ReadString(); //Try and get the Status Byte String
            }
            catch (Exception e)
            {
                throw new Exception($"ReadStatusByte: ReadString() (Status Byte) failed with message: {e.Message}", e);
            }
            // Convert the status byte string to an actual byte so we can test for the error message bit.
            if (!string.IsNullOrWhiteSpace(stbByteStr))
            {
                if (byte.TryParse(stbByteStr, out byte stbByte))
                    errorBitIsOn = ((stbByte & ErrQBit) != 0);
                else
                    throw new Exception($"ReadStatusByte: statusByte: {stbByteStr} is not a byte value and is invalid.");
                return stbByte;
            }
            errorBitIsOn = false;
            return 0;
        }

        /// <summary>Gets any error messages available on the error message queue and adds them to the Errors list.</summary>
        /// <returns>The number of error added to the list, 0 if no errors.</returns>
        /// <exception cref="System.Exception">GetErrorMsgs: WriteBytes({errCmd}) failed with message: {e.Message}</exception>
        /// <exception cref="System.Exception">GetErrorMsgs: ReadString() (Error Message) failed with message: {e.Message}</exception>
        public int GetErrorMsgs()
        {
            //The SCPI Error Command 
            const string errCmd = $"SYST:ERR?\n";
            //There can be multiple error items so we will put them in a list
            ErrorList errs = [];
            bool done = false; //Flag to know when to stop
            string errMsg;
            do
            {
                //Send command
                try
                {
                    WriteBytes(errCmd);
                }
                catch (Exception e)
                {
                    throw new Exception($"GetErrorMsgs: WriteBytes({errCmd}) failed with message: {e.Message}", e);
                }
                //Read response
                try
                {
                    errMsg = ReadString(); //Try and get an error message from the error queue
                }
                catch (Exception e)
                {
                    throw new Exception($"GetErrorMsgs: ReadString() (Error Message) failed with message: {e.Message}", e);
                }
                ErrorItem errItem = new(errMsg);
                if (errItem.EndOfList)
                    done = true;
                else
                    errs.Add(errItem);
            } while (done == false);
            errors.AddRange(errs);
            return errs.Count;
        }

        /// <summary>
        /// Writes a SCPI command as an array of bytes to the SCPI device.
        /// An exception is thrown if not all the characters could be written to the SCPI device for some reason.
        /// </summary>
        /// <param name="DataArray">The command byte array.</param>
        /// <exception cref="System.Exception">WriteBytes: Write Failure...</exception>
        private void WriteBytes(byte[] DataArray)
        {
            _ = Visa32.viWrite(session, DataArray, DataArray.Length, out var nBytesWritten);
            if (DataArray.Length != nBytesWritten)
                throw new Exception($"WriteBytes: Write Failure...");
        }

        /// <summary>
        /// Writes a SCPI command to the SCPI device.
        /// An exception is thrown if not all the characters could be written to the SCPI device for some reason.
        /// </summary>
        /// <param name="str">The string to write.</param>
        /// <exception cref="System.Exception">WriteBytes: Write Failure...</exception>
        private void WriteBytes(string str)
        {
            var dataArray = Encoding.Default.GetBytes(str);
            WriteBytes(dataArray);
        }

        /// <summary>
        /// Reads a message/data string from the SCPI device. 
        /// Usually done after issuing a SCPI query command (The ones ending with a ?).
        /// If there was no data (some commands do not return any data) an empty string is returned.
        /// </summary>
        /// <returns>The message or data as a string.  Empty string if none.</returns>
        private string ReadString()
        {
            //This code based on the Hantek demo.cs not analyzed but it works so it was left alone...
            List<byte[]> byteList = [];
            int readValue = -1;
            bool readResult = false;
            do
            {
                int maxcount = 1024;
                int retCount = 0;
                byte[] buff = new byte[maxcount];
                readValue = Visa32.viRead(session, buff, maxcount, out retCount);
                if (retCount > 0)
                {
                    byte[] invalidBuff = new byte[retCount];
                    Array.Copy(buff, 0, invalidBuff, 0, retCount);
                    byteList.Add(invalidBuff);
                }
                if (readValue == Visa32.VI_SUCCESS || readValue == Visa32.VI_SUCCESS_TERM_CHAR)
                {
                    readResult = true;
                    break;
                }
            }
            while (readValue == Visa32.VI_SUCCESS_MAX_CNT);

            if (readResult == false)
                return string.Empty; // Happens when the command does not have a return value like CONF:VOLT:AC etc.

            if (byteList.Count > 0)
            {
                List<int> allCount = byteList.Select(o => o.Length).ToList();
                var count = allCount.Sum();
                var allBytes = new byte[count];
                int index = 0;
                for (int i = 0; i < byteList.Count; i++)
                {
                    var byteArray = byteList[i];
                    byteArray.CopyTo(allBytes, index);
                    index += byteArray.Length;
                }
                var stringValue = Encoding.Default.GetString(allBytes);
                return stringValue;
            }
            return string.Empty;
        }

        /// <summary>
        /// Sends a SCPI command and if it was a query cmd (i.e. ending with ?) it will read the result.
        /// If there were errors they will be returned in the errors out parameter.
        /// </summary>
        /// <param name="cmd">The SCPI command.</param>        
        /// <param name="errors">The error message list if any.</param>
        /// <returns>System.String: The result if it was a query or an empty string if there was no result.</returns>
        /// <exception cref="System.Exception">SendAndRead: WriteBytes({cmd}) failed with message: {e.Message}</exception>
        /// <exception cref="System.Exception">SendAndRead: ReadString() failed with message: {e.Message}</exception>
        /// <exception cref="System.Exception">SendAndRead: {ex.Message}</exception>
        private string SendAndRead(string cmd, out int errCount) //ErrorList errors)
        {
            string readStr = string.Empty;
            //The SCPI interpreter seems to need an \n character at the end.
            if (!cmd.EndsWith('\n')) cmd += "\n";
            
            //Send the specified SCPI command
            try
            {
                WriteBytes(cmd); //Send the SCPI command
            }
            catch (Exception e)
            {
                throw new Exception($"SendAndRead: WriteBytes({cmd}) failed with message: {e.Message}", e);
            }
            //Check if a result needs to be fetched.
            try
            {
                if (cmd.Contains('?')) //Was it a query?
                    readStr = ReadString(); //If a Query a result should be returned so fetch it...
            }
            catch (Exception e)
            {
                throw new Exception($"SendAndRead: ReadString() failed with message: {e.Message}", e);
            }
            //Ok so now that we have send the command and optionally got the result, lets check the status byte to see if there were any errors.
            bool errorBitIsOn;
            try
            {
                _ = ReadStatusByte(out errorBitIsOn);
            }
            catch (Exception ex)
            {
                throw new Exception($"SendAndRead: {ex.Message}", ex);
            }
            //If the error message bit 2 is set we need to retrieve the error messages which will also clear the error fifo stack
            if (errorBitIsOn)
                errCount = GetErrorMsgs();
            else
                errCount = 0;
            return readStr.Replace("\"", "").Replace("\n", "");  //Don't want those quotes or \n
        }

        /// <summary>Reads the device's current reading.  Use: SendAndRead(cmd..) if a command should be sent first.</summary>
        /// <param name="qryValue">The Query Value object to be updated with the value read.</param>
        /// <param name="precision">Used to specify the number of digits after the decimal point.  null means it has not yet been defined and should be set later before getting any display values.</param>
        /// <returns>The number of new error messages added to the ScpiDevice.Errors list, 0 if no errors.</returns>
        /// <exception cref="System.Exception">ReadValue-&gt;GetMeasureMode: Could not read the mode from the device. {ex.Message}</exception>
        /// <exception cref="System.Exception">ReadValue-&gt;SendAndRead({readCmd}): Error: {ex.Message}</exception>
        public int ReadValue(ref ScpiQueryValue qryValue, int? precision = null)
        {
            int errCount = 0;
            const string readCmd = $"READ?\n";  //The value read command
            string sValue;
            try
            {
                sValue = SendAndRead(readCmd, out errCount);
            }
            catch (Exception ex)
            {
                throw new Exception($"ReadValue->SendAndRead({readCmd}): Error: {ex.Message}", ex);
            }
            qryValue.Update(sValue, precision);
            return errCount;
        }
        /// <summary>Sends the specified SCPI command and reads the result if any.
        /// If there were any errors they will be available in the ScpiDevice.Errors property.</summary>
        /// <param name="cmd">The SCPI command.</param>
        /// <param name="qryValue">The Query Value object to update with new values.</param>
        /// <param name="precision"></param>
        /// <returns>The Error Message Count, zero if no errors or warnings.</returns>
        /// <exception cref="System.Exception">SendAndRead({cmd}): Error: {ex.Message}</exception>
        /// <exception cref="System.Exception">ReadValue-&gt;GetMeasureMode: Could not read the mode from the device. {ex.Message}</exception>
        public int SendAndRead(string cmd, ref ScpiQueryValue qryValue, int? precision = null)
        {
            int errCount = 0;
            string sValue;
            try
            {
                sValue = SendAndRead(cmd, out errCount);
            }
            catch (Exception ex)
            {
                throw new Exception($"SendAndRead({cmd}): Error: {ex.Message}", ex);
            }
            qryValue.Update(sValue, precision);
            return errCount;
        }

        /// <summary>Sends the specified command, used for a command that does not return any data like "CONF:VOLT:DC" to simply set the mode.  Use SendAndRead() for commands that ilicit a responce.</summary>
        /// <param name="cmd">The command to send to the Scpi Device..</param>
        /// <param name="errCount">The error count will be non-zero if there were errors reported..</param>
        /// <exception cref="System.Exception">SendCommand: WriteBytes({cmd}) failed with message: {e.Message}</exception>
        /// <exception cref="System.Exception">SendAndRead: {ex.Message}</exception>
        private void SendCommand(string cmd, out int errCount)
        {
            //The SCPI interpreter seems to need an \n character at the end.
            if (!cmd.EndsWith('\n')) cmd += "\n";

            //Send the specified SCPI command
            try
            {
                WriteBytes(cmd); //Send the SCPI command
            }
            catch (Exception e)
            {
                throw new Exception($"SendCommand: WriteBytes({cmd}) failed with message: {e.Message}", e);
            }
            //Ok so now that we have send the command and optionally got the result, lets check the status byte to see if there were any errors.
            bool errorBitIsOn;
            try
            {
                _ = ReadStatusByte(out errorBitIsOn);
            }
            catch (Exception ex)
            {
                throw new Exception($"SendAndRead: {ex.Message}", ex);
            }
            //If the error message bit 2 is set we need to retrieve the error messages which will also clear the error fifo stack
            if (errorBitIsOn)
                errCount = GetErrorMsgs();
            else
                errCount = 0;
        }

        /// <summary>
        /// Opens the VISA resource manager.
        /// </summary>
        /// <returns>True if successful.</returns>
        private bool OpenResourceManager()
        {
            var stat = Visa32.viOpenDefaultRM(out this.resourceManager);
            return stat == Visa32.VI_SUCCESS;
        }

        /// <summary>
        /// Attempts to open a session using the provided device address.
        /// </summary>
        /// <param name="deviceAddress">The device address to use.</param>
        /// <returns>Returns true if successful.</returns>
        public bool OpenSession(string deviceAddress)
        {
            var stat = Visa32.viOpen(this.resourceManager, deviceAddress, Visa32.VI_NO_LOCK, Visa32.VI_TMO_IMMEDIATE, out this.session);
            return stat == Visa32.VI_SUCCESS;
        }

        /// <summary>
        /// Finds any SCPI devices and returns their information in a list.
        /// </summary>
        /// <returns>A list of available SCPI devices.</returns>
        public List<string> FindDevices()
        {
            var exprList = new List<string>() { "?*INSTR", "?*SOCKET" };
            var deviceList = new HashSet<string>();
            foreach (var expr in exprList)
            {
                int retCount;
                var sb = new StringBuilder();
                _ = Visa32.viFindRsrc(resourceManager, expr, out int vi, out retCount, sb);
                if (retCount > 0)
                {
                    if (sb.Length > 0)
                        deviceList.Add(sb.ToString());
                    for (int i = 0; i < retCount; i++)
                    {
                        var sbb = new StringBuilder(50);
                        _ = Visa32.viFindNext(vi, sbb);
                        deviceList.Add(sbb.ToString());
                    }
                }
            }
            return [.. deviceList];
        }

        /// <summary>
        /// Tries to find the named device and opens a session with it if found.
        /// An exception is thrown if there was a read failure from the SCPI device.
        /// </summary>
        /// <param name="deviceID">ID of the device you want to open like "Hantek".</param>
        /// <returns>True if a SCPI device was found with the specified name and it was opened.</returns>
        /// <exception cref="System.Exception">FindAndOpenDevice: Failed with message: {ex.Message}</exception>
        public bool FindAndOpenDevice(string deviceID)
        {
            isReady = false;
            var addresses = FindDevices();
            if (addresses.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (var address in addresses)
                {
                    if (OpenSession(address))
                    {
                        deviceAddr = address;
                        string readValue;
                        try
                        {
                            WriteBytes("*IDN? \n");
                            readValue = ReadString();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"FindAndOpenDevice: Failed with message: {ex.Message}", ex);
                        }
                        if (!string.IsNullOrEmpty(readValue))
                        {
                            if (readValue.Contains(deviceID, StringComparison.InvariantCultureIgnoreCase))
                            {
                                deviceName = readValue;
                                isReady = true;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool SetATTR_TERMCHAR_EN()
        {
            int nViStatus = Visa32.viSetAttribute(session, Visa32.VI_ATTR_TERMCHAR_EN, 1);
            return nViStatus == Visa32.VI_SUCCESS;
        }

        /// <summary>
        /// Gets the specified Visa instrument attribute string value.
        /// </summary>
        /// <param name="attr">The attribute.</param>
        /// <returns>Attribute value as a string.</returns>
        public string GetDeviceAttribute(int attr = Visa32.VI_ATTR_RSRC_CLASS)
        {
            StringBuilder sb = new(100);
            var state = Visa32.viGetAttribute(session, attr, sb);
            if (state == Visa32.VI_SUCCESS)
            {
                return sb.ToString().ToUpper();
            }
            return string.Empty;
        }

    }
}
