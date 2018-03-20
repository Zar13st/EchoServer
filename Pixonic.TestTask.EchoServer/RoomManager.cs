using Pixonic.TestTask.EchoServer.Interfaces;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;

namespace Pixonic.TestTask.EchoServer
{
    public class RoomManager : IRoomManager
    {
        #region Private Fields

        private ConcurrentDictionary<string, Room> _roomByName = new ConcurrentDictionary<string, Room>();

        #endregion Private Fields

        #region Public Methods

        public void EmptyRoomDetectingHandler(string roomName)
        {
            Contract.Assert(!string.IsNullOrEmpty(roomName), "!string.IsNullOrEmpty(roomName)");

            _roomByName.TryRemove(roomName, out _);
        }

        public void PlayerToRoomEnterHandler(IPlayer player)
        {
            Contract.Assert(player != null, "player != null");

            var roomCreated = _roomByName.TryGetValue(player.RoomName, out Room playersRoom);

            if (!roomCreated)
            {
                playersRoom = new Room(player.RoomName);

                _roomByName[player.RoomName] = playersRoom;
            }

            playersRoom.AddPlayer(player);
        }

        #endregion Public Methods
    }
}