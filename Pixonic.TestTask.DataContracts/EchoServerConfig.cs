namespace Pixonic.TestTask.DataContracts
{
    public class EchoServerConfig
    {
        #region Public Properties

        public string IpAddress { get; set; } = TcpConsts.LocalIpAddress;

        public int Port { get; set; } = TcpConsts.Port;

        #endregion Public Properties

        #region Public Methods

        public static EchoServerConfig Default()
        {
            return new EchoServerConfig();
        }

        #endregion Public Methods
    }
}