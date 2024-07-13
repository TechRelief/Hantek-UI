// ***********************************************************************
// Assembly         : SCPI
// Author           : Ed Paay
// Created          : 06-30-2024
//
// Last Modified By : Ed Paay
// Last Modified On : 07-04-2024
// ***********************************************************************
// <summary>
// These clases are responsible for handling and storing SCPI error
// messages read from the SCPI message queue.
// </summary>
// ***********************************************************************

namespace SCPI
{
    /// <summary>
    /// Class ErrorItem is used to store an error messge and its error number.
    /// </summary>
    public class ErrorItem
    {
        public int ErrId = 0;
        public string ErrDesc = string.Empty;

        /// <summary>Initializes a new instance of the <see cref="T:SCPI.ErrorItem" /> class.</summary>
        /// <param name="errorMsg">The error messge as returned from the SCPI device in response to SYST:ERR?</param>
        /// <exception cref="System.IO.InvalidDataException">ErrorItem(errorMsg) was invoked with an invalid errorMsg value: {errorMsg}</exception>
        public ErrorItem(string errorMsg)
        {
            //errorMsg lines are formatted as: {-|+}nnn, "Status String" 
            //It appears that it starts with + when there is no error or - for the error code!?
            int commaOffset = errorMsg.IndexOf(',');
            string code = errorMsg[..commaOffset];
            if (!(int.TryParse(code, out ErrId)))
            {
                throw new InvalidDataException($"ErrorItem(errorMsg) was invoked with an invalid errorMsg value: {errorMsg}");
            }
            ErrDesc = errorMsg[(commaOffset + 1)..];
        }

        /// <summary>Initializes a new instance of the <see cref="T:SCPI.ErrorItem" /> class.</summary>
        /// <param name="errId">The error identifier.</param>
        /// <param name="errDesc">The error description.</param>
        public ErrorItem(int errId, string errDesc) { ErrId = errId; ErrDesc = errDesc; }

        public override string ToString()
        {
            return $"{ErrDesc} ({ErrId})";
        }

        /// <summary>
        /// Gets a value indicating whether this is an empty / end of list item.  
        /// The DMM returns a +0, "No error" item if there is no error or the error queue is empty.  
        /// So EndOfList will be true if this is the case.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is end-of-list; otherwise, <c>false</c>.</value>
        public bool EndOfList => ErrId != 0;

        /// <summary>Gets a value indicating whether this item is an error.</summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c> since it can be a success or warning item.</value>
        public bool IsError => ErrId < 0;
    }

    /// <summary>
    /// Class ErrorList is used to store a list of error messages.
    /// Implements the <see cref="System.Collections.Generic.List{SCPI.ErrorItem}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{SCPI.ErrorItem}" />
    public class ErrorList : List<ErrorItem>
    {
        public ErrorList() : base()
        {
        }

        /// <summary>Clears the errors list.</summary>
        public void ClearList()
        {
            this.Clear();
        }

        public override string ToString()
        {
            string s = string.Empty;
            foreach (ErrorItem item in this)
            {
                s += $"{item.ToString()}\n";
            }
            return s;
        }
    }
}
