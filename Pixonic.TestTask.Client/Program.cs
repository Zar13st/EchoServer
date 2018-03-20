using Pixonic.TestTask.Client.TcpConnection;
using Pixonic.TestTask.DataContracts;
using System;
using System.Threading;

namespace Pixonic.TestTask.Client
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            string clientName;
            do
            {
                Console.WriteLine("Client Starting... \nEnter Client name");

                clientName = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(clientName));

            string roomName;
            do
            {
                Console.WriteLine("Enter Room name for connect");

                roomName = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(roomName));

            var configProvider = new TcpConfigProvider();
            var config = configProvider.GetConfig();

            var messageProcessor = new MessageClientProcessor();

            var tcpConnector = new TcpConnector(messageProcessor, config.IpAddress, config.Port);

            var hearBitSender = new HeartBitSender(tcpConnector, clientName);

            var clientManager = new ClientManager(tcpConnector, hearBitSender, roomName, clientName);

            clientManager.Start().GetAwaiter().GetResult();

            var mainImmortalEvent = new AutoResetEvent(false);
            mainImmortalEvent.WaitOne();
        }

        #endregion Private Methods
    }
}