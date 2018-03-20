using System;
using System.Net.Sockets;

namespace Pixonic.TestTask.EchoServer.Interfaces
{
    public interface IIncomeConnectionListener
    {
        #region Public Events

        event Action<long, TcpClient> OnNewConnection;

        #endregion Public Events

        #region Public Methods

        void Start();

        #endregion Public Methods
    }
}