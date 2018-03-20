using System.Diagnostics.Contracts;
using Pixonic.TestTask.DataContracts;

namespace Pixonic.TestTask.Client.Messages
{
    public class ConnectMessage
    {
        #region Private Fields

        private readonly string _clientName;
        private readonly string _roomName;

        #endregion Private Fields

        #region Public Constructors

        public ConnectMessage(string clientName, string roomName)
        {
            Contract.Assert(!string.IsNullOrEmpty(clientName), "!string.IsNullOrEmpty(clientName)");
            Contract.Assert(!string.IsNullOrEmpty(roomName), "!string.IsNullOrEmpty(roomName)");

            _clientName = clientName;
            _roomName = roomName;
        }

        #endregion Public Constructors

        #region Public Methods

        public override string ToString()
        {
            return $"{(byte)MessageType.Connect},{_clientName},{_roomName}";
        }

        #endregion Public Methods
    }
}