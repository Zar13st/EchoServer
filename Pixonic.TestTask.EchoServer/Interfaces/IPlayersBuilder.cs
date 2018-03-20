using System.Net.Sockets;

namespace Pixonic.TestTask.EchoServer.Interfaces
{
    public interface IPlayersBuilder
    {
        #region Public Methods

        IPlayer Build(long connectionId, TcpClient tcpClient);

        #endregion Public Methods
    }
}