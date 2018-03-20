using Pixonic.TestTask.EchoServer.Interfaces;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pixonic.TestTask.DataContracts;

namespace Pixonic.TestTask.EchoServer.TcpConnection
{
    public class TcpConnector : IServerTcpConnector
    {
        #region Private Fields

        private readonly IMessageFromTcpProcessor _messageProcessor;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private readonly TcpClient _tcpClient;
        private bool _isConnected;
        private StreamReader _reader;
        private StreamWriter _writer;

        #endregion Private Fields

        #region Public Constructors

        public TcpConnector(IMessageFromTcpProcessor messageProcessor, TcpClient tcpClient)
        {
            Contract.Assert(messageProcessor != null, "messageProcessor != null");
            Contract.Assert(tcpClient != null, "tcpClient != null");

            _messageProcessor = messageProcessor;
            _tcpClient = tcpClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool Connect()
        {
            try
            {
                var stream = _tcpClient.GetStream();
                _writer = new StreamWriter(stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };
                _reader = new StreamReader(stream, Encoding.ASCII);

                _reader.BaseStream.BeginRead(new byte[0], 0, 0, ReadFromTcpCallBack, _reader);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ConnectAsync Error. Message: {e.Message}. Error: {e}");

                return _isConnected = false;
            }

            return _isConnected = true;
        }

        public void Dispose()
        {
            _isConnected = false;

            if (_reader != null)
            {
                var reader = _reader;
                _reader = null;
                reader.Dispose();
            }

            if (_writer != null)
            {
                var writer = _writer;
                _writer = null;
                writer.Dispose();
            }

            _tcpClient?.Close();
        }

        public async Task<bool> SendMessage(string message)
        {
            Contract.Assert(!string.IsNullOrEmpty(message), "!string.IsNullOrEmpty(message)");

            bool isCommandSended;

            if (!_isConnected)
            {
                Console.WriteLine($"Try Sending message then !_isConnected. Message to send: {message}");

                return false;
            }

            await _semaphore.WaitAsync();

            try
            {
                await _writer.WriteLineAsync(message);
                isCommandSended = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"SendMessage Warn - Client Disconnected. Message to send: {message}. Message: {e.Message}.");

                isCommandSended = false;
            }
            finally
            {
                _semaphore.Release();
            }
            return isCommandSended;
        }

        #endregion Public Methods

        #region Private Methods

        private void ReadFromTcpCallBack(IAsyncResult ar)
        {
            if (!_isConnected)
            {
                return;
            }

            try
            {
                var reader = (StreamReader)ar.AsyncState;

                reader.BaseStream.EndRead(ar);

                while (!reader.EndOfStream)
                {
                    var message = reader.ReadLine();

                    _messageProcessor.Process(message);
                }

                reader.BaseStream.BeginRead(new byte[0], 0, 0, ReadFromTcpCallBack, reader);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ReadFromTcpCallBack Error: Client Disconnected. Message: {e.Message}.");

                _messageProcessor.Process($"{(byte)MessageType.Disconnect}, PlayerDisconnected");

                _isConnected = false;
            }
        }

        #endregion Private Methods
    }
}