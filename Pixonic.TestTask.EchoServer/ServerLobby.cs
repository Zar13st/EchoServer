using Pixonic.TestTask.EchoServer.Interfaces;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;

namespace Pixonic.TestTask.EchoServer
{
    public class ServerLobby : IServerLobby
    {
        #region Private Fields

        private readonly ConcurrentDictionary<long, IPlayer> _playersInLobby = new ConcurrentDictionary<long, IPlayer>();
        private IRoomManager _roomManager;

        #endregion Private Fields

        #region Public Constructors

        public ServerLobby(IRoomManager roomManager)
        {
            Contract.Assert(roomManager != null, "roomManager != null");

            _roomManager = roomManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddPlayer(IPlayer player)
        {
            Contract.Assert(player != null, "player != null");

            _playersInLobby[player.Id] = player;

            player.OnPlayerToRoomEnter += PlayerToRoomEnterHandler;

            player.Start();
        }

        #endregion Public Methods

        #region Private Methods

        private void PlayerToRoomEnterHandler(IPlayer player)
        {
            Contract.Assert(player != null, "player != null");

            player.OnPlayerToRoomEnter -= PlayerToRoomEnterHandler;

            var isPlayerRemoveFromLobby = _playersInLobby.TryRemove(player.Id, out _);
            if (isPlayerRemoveFromLobby)
            {
                _roomManager.PlayerToRoomEnterHandler(player);
            }
        }

        #endregion Private Methods
    }
}