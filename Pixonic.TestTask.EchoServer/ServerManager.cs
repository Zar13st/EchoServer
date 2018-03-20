using Pixonic.TestTask.EchoServer.Interfaces;
using System.Diagnostics.Contracts;
using System.Net.Sockets;

namespace Pixonic.TestTask.EchoServer
{
    public class ServerManager
    {
        #region Private Fields

        private IIncomeConnectionListener _incomeConnectionListener;
        private IPlayersBuilder _playersBuilder;
        private IServerLobby _serverLobby;

        #endregion Private Fields

        #region Public Constructors

        public ServerManager(IIncomeConnectionListener incomeConnectionListener, IServerLobby serverLobby, IPlayersBuilder playersBuilder)
        {
            Contract.Assert(incomeConnectionListener != null, "incomeConnectionListener != null");
            Contract.Assert(serverLobby != null, "serverLobby != null");
            Contract.Assert(playersBuilder != null, "playersBuilder != null");

            _incomeConnectionListener = incomeConnectionListener;
            _serverLobby = serverLobby;
            _playersBuilder = playersBuilder;

            _incomeConnectionListener.OnNewConnection += CreatePlayer;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CreatePlayer(long connectionId, TcpClient tcpClient)
        {
            Contract.Assert(tcpClient != null, "tcpClient != null");

            var newPlayer = _playersBuilder.Build(connectionId, tcpClient);

            _serverLobby.AddPlayer(newPlayer);
        }

        public void Start()
        {
            _incomeConnectionListener.Start();
        }

        #endregion Public Methods
    }
}