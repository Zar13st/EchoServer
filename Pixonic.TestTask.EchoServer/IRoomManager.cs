using Pixonic.TestTask.EchoServer.Interfaces;

namespace Pixonic.TestTask.EchoServer
{
    public interface IRoomManager
    {
        #region Public Methods

        void EmptyRoomDetectingHandler(string roomName);

        void PlayerToRoomEnterHandler(IPlayer player);

        #endregion Public Methods
    }
}