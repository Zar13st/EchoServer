using Pixonic.TestTask.EchoServer.Interfaces;
using Pixonic.TestTask.EchoServer.TcpConnection;
using System.Diagnostics.Contracts;
using System.Net.Sockets;

namespace Pixonic.TestTask.EchoServer
{
    public class PlayersBuilder : IPlayersBuilder
    {
        #region Public Methods

        public IPlayer Build(long connectionId, TcpClient tcpClient)
        {
            Contract.Assert(tcpClient != null, "tcpClient != null");

            var messageProcessor = new MessageServerProcessor();

            var tcpConnector = new TcpConnector(messageProcessor, tcpClient);

            var player = new Player(connectionId, tcpConnector);

            messageProcessor.OnPlayerToRoomEnter += player.OnPlayerToRoomEnterHandler;
            messageProcessor.OnHeartBitFromPlayer += player.HeartBitMessageHandler;
            messageProcessor.OnPlayerDisconnected += player.PlayerDisconnectHandler;

            return player;
        }

        #endregion Public Methods
    }
}