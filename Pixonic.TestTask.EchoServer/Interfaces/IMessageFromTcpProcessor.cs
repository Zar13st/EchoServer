namespace Pixonic.TestTask.EchoServer.Interfaces
{
    public interface IMessageFromTcpProcessor
    {
        #region Public Methods

        void Process(string message);

        #endregion Public Methods
    }
}