using Pixonic.TestTask.Client.Interfaces;
using Pixonic.TestTask.Client.Messages;
using Pixonic.TestTask.DataContracts;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Pixonic.TestTask.Client
{
    public class HeartBitSender : IHeartBitSender
    {
        #region Private Fields

        private string _heartBitMessage;
        private Timer _heartBitTimer;
        private IClientTcpConnector _tcpConnector;

        #endregion Private Fields

        #region Public Constructors

        public HeartBitSender(IClientTcpConnector tcpConnector, string clientName)
        {
            Contract.Assert(tcpConnector != null, "tcpConnector != null");
            Contract.Assert(!string.IsNullOrEmpty(clientName), "!string.IsNullOrEmpty(clientName)");

            _tcpConnector = tcpConnector;
            _heartBitMessage = new HeartBitMessage(clientName).ToString();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Start()
        {
            _heartBitTimer = new Timer(Send, null, SystemConsts.HeartBitTimeSpan, SystemConsts.PseudoInfinity);
        }

        #endregion Public Methods

        #region Private Methods

        private void Send(object state)
        {
            var watch = new Stopwatch();
            watch.Start();

            if (_tcpConnector.IsConnected)
            {
                _tcpConnector.SendMessage(_heartBitMessage).GetAwaiter().GetResult();
            }

            var nextSendTimeSpan = SystemConsts.HeartBitTimeSpan - new TimeSpan(0, 0, 0, 0, (int)watch.ElapsedMilliseconds);

            if (nextSendTimeSpan.TotalMilliseconds < 0)
            {
                nextSendTimeSpan = TimeSpan.Zero;
            }

            _heartBitTimer.Change(nextSendTimeSpan, SystemConsts.PseudoInfinity);
        }

        #endregion Private Methods
    }
}