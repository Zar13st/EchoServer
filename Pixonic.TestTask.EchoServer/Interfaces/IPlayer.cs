using System;

namespace Pixonic.TestTask.EchoServer.Interfaces
{
    public interface IPlayer
    {
        #region Public Events

        event Action<long> OnHeartBitMessage;

        event Action<IPlayer> OnPlayerDisconnected;

        event Action<IPlayer> OnPlayerToRoomEnter;

        #endregion Public Events

        #region Public Properties

        long Id { get; }
        string Name { get; }
        string RoomName { get; }

        #endregion Public Properties

        #region Public Methods

        void HeartBitMessageHandler();

        void OnPlayerToRoomEnterHandler(string name, string roomName);

        void PlayerDisconnectHandler();

        void SendMessageToClient(string message);

        void Start();

        #endregion Public Methods
    }
}