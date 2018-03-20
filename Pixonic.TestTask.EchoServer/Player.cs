using Pixonic.TestTask.EchoServer.Interfaces;
using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Pixonic.TestTask.EchoServer
{
    public class Player : IPlayer
    {
        #region Private Fields

        private readonly AutoResetEvent _immortalEvent = new AutoResetEvent(false);
        private readonly IServerTcpConnector _tcpConnector;

        #endregion Private Fields

        #region Public Constructors

        public Player(long connectionId, IServerTcpConnector tcpConnector)
        {
            Contract.Assert(tcpConnector != null, "tcpConnector != null");

            _tcpConnector = tcpConnector;
            Id = connectionId;
        }

        #endregion Public Constructors

        #region Public Events

        public event Action<long> OnHeartBitMessage;

        public event Action<IPlayer> OnPlayerDisconnected;

        public event Action<IPlayer> OnPlayerToRoomEnter;

        #endregion Public Events

        #region Public Properties

        public long Id { get; }

        public string Name { get; private set; }

        public string RoomName { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void HeartBitMessageHandler()
        {
            OnHeartBitMessage?.Invoke(Id);
        }

        public void OnPlayerToRoomEnterHandler(string name, string roomName)
        {
            Contract.Assert(!string.IsNullOrEmpty(name), "!string.IsNullOrEmpty(name)");
            Contract.Assert(!string.IsNullOrEmpty(roomName), "!string.IsNullOrEmpty(roomName)");

            Name = name;
            RoomName = roomName;

            OnPlayerToRoomEnter?.Invoke(this);
        }

        public void PlayerDisconnectHandler()
        {
            OnPlayerDisconnected?.Invoke(this);

            _immortalEvent.Set();
        }

        public void SendMessageToClient(string message)
        {
            _tcpConnector.SendMessage(message).GetAwaiter().GetResult();
        }

        public void Start()
        {
            var newevent = new AutoResetEvent(false);
            new Thread(() =>
            {
                _tcpConnector.Connect();

                newevent.Set();

                _immortalEvent.WaitOne();
            }).Start();

            newevent.WaitOne();
        }

        #endregion Public Methods
    }
}