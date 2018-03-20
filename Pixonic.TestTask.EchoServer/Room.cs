using Pixonic.TestTask.EchoServer.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;

namespace Pixonic.TestTask.EchoServer
{
    public class Room
    {
        #region Private Fields

        private ConcurrentDictionary<long, IPlayer> _playersById = new ConcurrentDictionary<long, IPlayer>();

        #endregion Private Fields

        #region Public Constructors

        public Room(string name)
        {
            Contract.Assert(!string.IsNullOrEmpty(name), "!string.IsNullOrEmpty(name)");

            Name = name;
        }

        #endregion Public Constructors

        #region Public Events

        public event Action<string> OnRoomEmpty;

        #endregion Public Events

        #region Public Properties

        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public void AddPlayer(IPlayer player)
        {
            Contract.Assert(player != null, "player != null");

            _playersById[player.Id] = player;

            player.OnPlayerDisconnected += RemovePlayer;
            player.OnHeartBitMessage += SendToAll;
        }

        public void RemovePlayer(IPlayer player)
        {
            Contract.Assert(player != null, "player != null");

            player.OnPlayerDisconnected -= RemovePlayer;
            player.OnHeartBitMessage -= SendToAll;

            _playersById.TryRemove(player.Id, out _);
        }

        #endregion Public Methods

        #region Private Methods

        private void SendToAll(long id)
        {
            foreach (var player in _playersById)
            {
                player.Value.SendMessageToClient($"HeartBit from palyer Name: {_playersById[id].Name}, Id: {id}");
            }
        }

        #endregion Private Methods
    }
}