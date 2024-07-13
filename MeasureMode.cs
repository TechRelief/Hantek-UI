// ***********************************************************************
// Assembly         : SCPI
// Author           : Ed Paay
// Created          : 06-30-2024
//
// Last Modified By : Ed Paay
// Last Modified On : 07-04-2024
// ***********************************************************************
// <summary>
//  The MeasureMode enum is used to conveniently store the measurement mode
//  the DMM is in.  SCPI will return various strings like "VOLT", "VOLT:AC"
//  these are better managed by converting them to an enumerated value.
// </summary>
// ***********************************************************************

namespace SCPI
{
    /// <summary>Enum MeasureMode specifies the mode the DMM is in currently, like DC Voltage etc.</summary>
    public enum MeasureMode
    {
        /// <summary>The None indicates the mode has not been setr yet.</summary>
        None,
        /// <summary>DC Voltage</summary>
        DCV,
        /// <summary>AC Voltage</summary>
        ACV,
        /// <summary>Resistance 2 Wire</summary>
        R2W,
        /// <summary>Resistance 4 Wire</summary>
        R4W,
        /// <summary>DC Current</summary>
        DCI,
        /// <summary>AC Current</summary>
        ACI,
        /// <summary>Frequency</summary>
        FREQ,
        /// <summary>Capacitance</summary>
        CAP,
        /// <summary>Continuity</summary>
        CONT,
        /// <summary>Diode</summary>
        DIODE,
        /// <summary>Temperature</summary>
        TEMP
    }
}
