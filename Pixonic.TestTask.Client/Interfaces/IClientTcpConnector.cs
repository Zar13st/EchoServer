using System;
using System.Threading.Tasks;

namespace Pixonic.TestTask.Client.Interfaces
{
    public interface IClientTcpConnector : IDisposable
    {
        #region Public Methods

        bool IsConnected { get; }

        Task<bool> ConnectAsync();

        Task<bool> SendMessage(string command);

        #endregion Public Methods
    }
}