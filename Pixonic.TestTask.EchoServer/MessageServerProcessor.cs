using Pixonic.TestTask.EchoServer.Interfaces;
using System;

namespace Pixonic.TestTask.EchoServer
{
    internal class MessageServerProcessor : IMessageFromTcpProcessor
    {
        #region Public Events

        public event Action OnHeartBitFromPlayer;

        public event Action OnPlayerDisconnected;

        public event Action<string, string> OnPlayerToRoomEnter;

        #endregion Public Events

        #region Public Methods

        public void Process(string message)
        {
            var messageParts = message.Split(',');

            switch (messageParts[0])
            {
                case "0":
                    {
                        if (message.Length < 3)
                        {
                            Console.WriteLine($"Server can't process player to room connection message with Length < 3");
                            return;
                        }

                        var playerName = messageParts[1];
                        var roomName = messageParts[2];

                        if (string.IsNullOrWhiteSpace(playerName))
                        {
                            Console.WriteLine($"Server can't process player with playerName - IsNullOrWhiteSpace");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(roomName))
                        {
                            Console.WriteLine($"Server can't process player with roomName - IsNullOrWhiteSpace");
                            return;
                        }

                        OnPlayerToRoomEnter?.Invoke(playerName, roomName);
                        break;
                    }

                case "1":
                    {
                        OnHeartBitFromPlayer?.Invoke();
                        break;
                    }

                case "2":
                    {
                        OnPlayerDisconnected?.Invoke();
                        break;
                    }

                default:
                    {
                        Console.WriteLine($"Server can't process message {message}. MessageType - unknown");
                        break;
                    }
            }
        }

        #endregion Public Methods
    }
}