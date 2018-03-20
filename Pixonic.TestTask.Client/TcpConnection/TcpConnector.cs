using Pixonic.TestTask.Client.Interfaces;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pixonic.TestTask.Client.TcpConnection
{
    public class TcpConnector : IClientTcpConnector
    {
        #region Private Fields

        private readonly string _ipAddress;
        private readonly IMessageFromTcpProcessor _messageProcessor;
        private readonly int _port;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private readonly TcpClient _tcpClient;
        private StreamReader _reader;
        private StreamWriter _writer;

        #endregion Private Fields

        #region Public Properties

        public bool IsConnected { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public TcpConnector(IMessageFromTcpProcessor messageProcessor, string ipAddress, int port)
        {
            Contract.Assert(messageProcessor != null, "messageProcessor != null");
            Contract.Assert(!string.IsNullOrEmpty(ipAddress), "!string.IsNullOrEmpty(ipAddress)");
            Contract.Assert(port > 0, "port > 0");

            _ipAddress = ipAddress;
            _port = port;
            _messageProcessor = messageProcessor;

            _tcpClient = new TcpClient();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> ConnectAsync()
        {
            try
            {
                await _tcpClient.ConnectAsync(_ipAddress, _port);

                var stream = _tcpClient.GetStream();
                _writer = new StreamWriter(stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };
                _reader = new StreamReader(stream, Encoding.ASCII);

                _reader.BaseStream.BeginRead(new byte[0], 0, 0, ReadFromTcpCallBack, _reader);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ConnectAsync Error. Message: {e.Message}. Error: {e}");

                return IsConnected = false;
            }

            return IsConnected = true;
        }

        public void Dispose()
        {
            IsConnected = false;

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

            if (!IsConnected)
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
                Console.WriteLine($"SendMessage Error - Server Disconnected. Message to send: {message}. Message: {e.Message}. Need to restart Clien");

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
            if (!IsConnected)
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
                Console.WriteLine($"ReadFromTcpCallBack Error: Server Disconnected. Message: {e.Message}. Need to restart Client");

                IsConnected = false;
            }
        }

        #endregion Private Methods
    }
}