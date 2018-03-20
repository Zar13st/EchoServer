using System;
using System.IO;

namespace Pixonic.TestTask.DataContracts
{
    public class TcpConfigProvider
    {
        #region Public Methods

        public EchoServerConfig GetConfig()
        {
            var configPath = $@"{Environment.CurrentDirectory}\{SystemConsts.ConfigName}";

            var config = EchoServerConfig.Default();

            if (File.Exists(configPath))
            {
                var configParts = File.ReadAllLines(configPath);

                foreach (var part in configParts)
                {
                    if (!part.Contains("=")) continue;

                    var nameValue = part.Replace(" ", "").Split('=');

                    switch (nameValue[0])
                    {
                        case "IpAdress":
                            {
                                config.IpAddress = nameValue[1];
                                break;
                            }
                        case "Port":
                            {
                                var isParsed = int.TryParse(nameValue[1], out var port);
                                if (isParsed)
                                {
                                    config.Port = port;
                                }
                                break;
                            }
                    }
                }
            }

            return config;
        }

        #endregion Public Methods
    }
}