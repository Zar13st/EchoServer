using System.Diagnostics.Contracts;
using Pixonic.TestTask.DataContracts;

namespace Pixonic.TestTask.Client.Messages
{
    public class HeartBitMessage
    {
        #region Private Fields

        private readonly string _clientName;

        #endregion Private Fields

        #region Public Constructors

        public HeartBitMessage(string clientName)
        {
            Contract.Assert(!string.IsNullOrEmpty(clientName), "!string.IsNullOrEmpty(clientName)");

            _clientName = clientName;
        }

        #endregion Public Constructors

        #region Public Methods

        public override string ToString()
        {
            return $"{(byte)MessageType.HeartBit},{_clientName}";
        }

        #endregion Public Methods
    }
}