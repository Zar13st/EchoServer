using Pixonic.TestTask.Client.Interfaces;
using Pixonic.TestTask.Client.Messages;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Pixonic.TestTask.Client
{
    public class ClientManager
    {
        #region Private Fields

        private string _clientName;
        private IHeartBitSender _heartBitSender;
        private string _roomName;
        private IClientTcpConnector _tcpConnector;

        #endregion Private Fields

        #region Public Constructors

        public ClientManager(IClientTcpConnector tcpConnector, IHeartBitSender heartBitSender, string roomName, string clientName)
        {
            Contract.Assert(tcpConnector != null, "tcpConnector != null");
            Contract.Assert(heartBitSender != null, "heartBitSender != null");
            Contract.Assert(!string.IsNullOrEmpty(roomName), "!string.IsNullOrEmpty(roomName)");
            Contract.Assert(!string.IsNullOrEmpty(clientName), "!string.IsNullOrEmpty(clientName)");

            _tcpConnector = tcpConnector;
            _heartBitSender = heartBitSender;
            _roomName = roomName;
            _clientName = clientName;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> Start()
        {
            var isConnectedToServer = await _tcpConnector.ConnectAsync();
            if (!isConnectedToServer)
            {
                Console.WriteLine($"Can't Create tcpConnector. Client starting aborted");
                return false;
            }

            var connectMessage = new ConnectMessage(_clientName, _roomName).ToString();

            var isRoomNameSended = await _tcpConnector.SendMessage(connectMessage);
            if (!isRoomNameSended)
            {
                Console.WriteLine($"Can't send room name. Client starting aborted");
                return false;
            }

            _heartBitSender.Start();

            return true;
        }

        #endregion Public Methods
    }
}