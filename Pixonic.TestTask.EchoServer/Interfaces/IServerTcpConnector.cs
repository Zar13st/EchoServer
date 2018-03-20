using System;
using System.Threading.Tasks;

namespace Pixonic.TestTask.EchoServer.Interfaces
{
    public interface IServerTcpConnector : IDisposable
    {
        #region Public Methods

        bool Connect();

        Task<bool> SendMessage(string command);

        #endregion Public Methods
    }
}