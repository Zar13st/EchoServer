using Pixonic.TestTask.EchoServer.Interfaces;
using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;

namespace Pixonic.TestTask.EchoServer.TcpConnection
{
    public class IncomeConnectionListener : IIncomeConnectionListener
    {
        #region Private Fields

        private readonly int _port;
        private long _nextConnectionId;
        private TcpListener _tcpListener;

        #endregion Private Fields

        #region Public Events

        public event Action<long, TcpClient> OnNewConnection;

        #endregion Public Events

        #region Public Constructors

        public IncomeConnectionListener(int port)
        {
            Contract.Assert(port > 0, "port > null");

            _port = port;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Start()
        {
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _tcpListener.Start();

            _tcpListener.BeginAcceptTcpClient(CreateNewConnection, null);
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateNewConnection(IAsyncResult ar)
        {
            var newTcpClient = _tcpListener.EndAcceptTcpClient(ar);

            OnNewConnection?.Invoke(_nextConnectionId, newTcpClient);

            _nextConnectionId++;

            _tcpListener.BeginAcceptTcpClient(CreateNewConnection, null);
        }

        #endregion Private Methods
    }
}