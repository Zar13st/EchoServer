using Pixonic.TestTask.DataContracts;
using Pixonic.TestTask.EchoServer.TcpConnection;
using System;
using System.Threading;

namespace Pixonic.TestTask.EchoServer
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            var uniqueKeeper = new UniquenessServerKeeper(SystemConsts.ServerName);
            var isUniqueModulKey = uniqueKeeper.CheckUniquenessModule();
            if (!isUniqueModulKey)
            {
                Console.WriteLine("The app has already been launched. And now it is closed.");
                Thread.Sleep(4000);
                return;
            }
            else
            {
                Console.WriteLine($"Server Starting...");
            }

            var configProvider = new TcpConfigProvider();
            var config = configProvider.GetConfig();

            var listner = new IncomeConnectionListener(config.Port);

            var roomManager = new RoomManager();
            var serverLobby = new ServerLobby(roomManager);

            var playerBuilder = new PlayersBuilder();

            var serverManager = new ServerManager(listner, serverLobby, playerBuilder);
            serverManager.Start();

            var mainImmortalEvent = new AutoResetEvent(false);
            mainImmortalEvent.WaitOne();
        }

        #endregion Private Methods
    }
}