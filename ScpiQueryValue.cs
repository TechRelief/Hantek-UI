// ***********************************************************************
// Assembly         : SCPI
// Author           : Ed Paay
// Created          : 06-30-2024
//
// Last Modified By : Ed Paay
// Last Modified On : 07-04-2024
// ***********************************************************************
// <summary>
// This class is used to store and process data received from the SCPI
// device usually in response to a SCPI query or read command.
// </summary>
// ***********************************************************************

namespace SCPI
{
    /// <summary>
    /// Class ScpiQueryValue is used to store a query string returned from a SCPI query command. 
    /// The ToString() method will convert the value suitable for display depending on the measurement mode.
    /// </summary>
    public class ScpiQueryValue
    {
        private string formatMask = "###0.00000";
        private string value;
        private MeasureMode mode;

        /// <summary>Initializes a new instance of the <see cref="T:SCPI.ScpiQueryValue" /> class.</summary>
        /// <param name="qryValue">The value read from the SCPI device.</param>
        /// <param name="mode">The measurement  mode.</param>
        /// <param name="precision">The precision, null if undefined, will default to 5.
        /// Value 5 will be used if undefined until changed.</param>
        public ScpiQueryValue(string qryValue, MeasureMode mode, int? precision = null)
        {
            this.value = qryValue;
            this.mode = mode;   
            formatMask = GetFormatMask(precision ?? 5);
        }

        /// <summary>
        /// Gets the format mask. The default is: ###0.#####
        /// this gives a precision of up to five digits after the decimal point.  
        /// You can use the SetFormatMask() method to set this.
        /// </summary>
        /// <txtValue>The format mask.</txtValue>
        public string FormatMask { get => formatMask; }


        /// <summary>Gets the format mask derived from the number of digits after the decimal point.</summary>
        /// <param name="digits">The number of digits after the decimal point.</param>
        /// <returns>The format mask suitable for use with the .Format functions.</returns>
        /// <exception cref="System.ArgumentException">ScpiQueryValue.SetFormatMask({digits}), txtValue should be less than 10.</exception>
        public static string GetFormatMask(int digits)
        {
            if (digits > 9) throw new ArgumentException($"ScpiQueryValue.SetFormatMask({digits}), txtValue should be less than 10.");
            string s = "000000000"[..digits];
            return $"##0.{s}";
        }

        /// <summary>Builds a format mask based on the precision, i.e. the number of digits after the decimal point for output of numeric values.</summary>
        /// <param name="digits">The number of digits after the decimal point.</param>
        /// <exception cref="System.ArgumentException">ScpiQueryValue.SetFormatMask({digits}), txtValue should be less than 10.</exception>
        public void SetFormatMask(int digits)
        {
            formatMask = GetFormatMask(digits);
        }

        /// <summary>The measurement mode, like ACV, DCV, etc..</summary>
        public MeasureMode Mode { get => mode; set => mode = value; }

        /// <summary>Gets the measurement mode description like Voltage DC, Resistance, etc. Used to display the mode.</summary>
        public string MeasureDesc => ScpiDevice.GetMeasureDesc(mode);

        /// <summary>Gets or sets the query value string.</summary>
        /// <value>The value.</value>
        public string Value { get => value; set => this.value = value; }

        /// <summary>
        /// Gets the numeric txtValue of the Query Result.  Attempts to convert the QueryValue string to a Double txtValue.  
        /// If the string is not a numeric txtValue it returns False and the txtValue returned will be zero.
        /// </summary>
        /// <param name="value">The QueryValue converted to a numeric Double txtValue.</param>
        /// <returns>
        ///   <c>true</c> if the query txtValue string could be converted, <c>false</c> otherwise.</returns>
        public bool GetNumericValue(out double value)
        {
            value = 0D;
            if (!string.IsNullOrWhiteSpace(this.value))
            {
                if (double.TryParse(Value, out value))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>Gets the display values for this entity, like the Value and the Unit.</summary>
        /// <param name="txtValue">The text value either numeric or like "Overload" etc..</param>
        /// <param name="unit">The unit like V, Hz etc.</param>
        /// <returns>
        ///   <c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool GetDisplayValues(out string txtValue, out string unit)
        {
            txtValue = string.Empty;
            unit = string.Empty;

            if (string.IsNullOrWhiteSpace(Value))
                return false;

            if (Value == "+9.90000000E+37") //Overload condition
            {
                //Overload condition
                switch (mode)
                {
                    case MeasureMode.DCV:
                    case MeasureMode.ACV:
                    case MeasureMode.R2W:
                    case MeasureMode.R4W:
                    case MeasureMode.DCI:
                    case MeasureMode.ACI:
                    case MeasureMode.FREQ:
                    case MeasureMode.TEMP:
                    case MeasureMode.CAP:
                    {
                        txtValue = "Overload";
                        return true;
                    }
                    case MeasureMode.CONT:
                    case MeasureMode.DIODE:
                    {
                        txtValue = "Open";
                        return true;
                    }
                    default:
                        txtValue = Value;
                        return true;
                }
            }
            else //Convert the txtValue for display
            {
                if (GetNumericValue(out Double dvalu))
                {
                    double absvalu = Math.Abs(dvalu);
                    switch (mode)
                    {
                        case MeasureMode.DCV:
                        case MeasureMode.ACV:
                        case MeasureMode.DIODE:
                        {
                            unit = "V";
                            if (absvalu < 1e-3)
                            {
                                dvalu *= 1e6;
                                unit = "μV";
                            }
                            else if (absvalu < 1)
                            {
                                dvalu *= 1e3;
                                unit = "mV";
                            }
                            break;
                        }
                        case MeasureMode.R2W:
                        case MeasureMode.R4W:
                        case MeasureMode.CONT:
                        {
                            unit = "Ω";
                            dvalu = absvalu; //No such thing as negative resistance, some multimeters erroniously output negative resistance so this will take care of this...
                            if (dvalu > 999999D)
                            {
                                dvalu /= 1e6;
                                unit = "MΩ";
                            }
                            else if (dvalu > 999D)
                            {
                                dvalu /= 1000D;
                                unit = "kΩ";
                            }
                            break;
                        }
                        case MeasureMode.DCI:
                        case MeasureMode.ACI:
                        {
                            unit = "A";
                            if (absvalu < 1e-3)
                            {
                                dvalu *= 1e6;
                                unit = "μA";
                            }
                            else if (absvalu < 1)
                            {
                                dvalu *= 1e3;
                                unit = "mA";
                            }
                            break;
                        }
                        case MeasureMode.FREQ:
                        {
                            unit = "Hz";
                            dvalu = absvalu;
                            if (dvalu > 999D)
                            {
                                dvalu /= 1000D;
                                unit = "kHz";
                            }
                            break;
                        }
                        case MeasureMode.CAP:
                        {
                            unit = "μF"; //It can only measure up to 100μF, kinda useless!
                            dvalu = absvalu;
                            if (dvalu < 1e-3)
                            {
                                dvalu *= 1e6;
                                unit = "pF";
                            }
                            else if (dvalu <= 1)
                            {
                                dvalu *= 1e3;
                                unit = "nF";
                            }
                            break;
                        }
                        default:
                            unit = string.Empty;
                            break;
                    }
                    string fmt = "{0:" + formatMask + "}";
                    txtValue = String.Format(fmt, dvalu);
                    return true;
                }
                else
                {
                    txtValue = Value;
                    return true;
                }
            }
        }

        /// <summary>Returns a <see cref="T:System.String">String</see> with a message or the txtValue read from the device and its unit as appropriate.</summary>
        public override string ToString()
        {
            if (GetDisplayValues(out string sVal, out string sUnit))
            {
                return $"{sVal} {sUnit}";
            }
            else
                return string.Empty;
        }

        /// <summary>Updates this ScpiQueryValue object with the specified values.</summary>
        /// <param name="sValue">The new value.</param>
        /// <param name="measureMode">The measure mode if any.</param>
        /// <param name="precision">The precision if any.</param>
        public void Update(string sValue, MeasureMode measureMode, int? precision = null)
        {
            value = sValue;
            mode = measureMode;
            if (precision != null) SetFormatMask(precision.Value);
        }

        /// <summary>Updates this ScpiQueryValue object with the specified values.</summary>
        /// <param name="sValue">The new value.</param>
        /// <param name="precision">The precision if any.</param>
        public void Update(string sValue, int? precision = null)
        {
            value = sValue;
            if (precision != null) SetFormatMask(precision.Value);
        }

    }
}
