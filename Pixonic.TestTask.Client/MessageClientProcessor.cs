using Pixonic.TestTask.Client.Interfaces;
using System;

namespace Pixonic.TestTask.Client
{
    public class MessageClientProcessor : IMessageFromTcpProcessor
    {
        #region Public Methods

        public void Process(string message)
        {
            Console.WriteLine(message);
        }

        #endregion Public Methods
    }
}