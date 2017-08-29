namespace UserStorage.Services.TCPServices
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization;

    using UserStorage.Services;

    #endregion

    [Serializable]
    public class TCPClientService<T> : MarshalByRefObject
    {
        #region Fields

        private LoggingService loggerService;

        private TcpClient client;

        #endregion

        #region Constructors & Finalizator

        protected TCPClientService() 
        { 
            loggerService = LoggingService.Service;

            client = new TcpClient("127.0.0.1", 5000);
        }

        ~TCPClientService()
        {
            if (client != null)
            {
                client.Close();
            }
        }

        #endregion

        #region Public methods

        protected IEnumerable<T> ReadDataFromNetworkStream()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                return (List<T>)new BinaryFormatter().Deserialize(client.GetStream());
            }
            catch (SerializationException)
            {
                loggerService.LogError("SerializationException has occured while reading data from a NetworkStream.");
                throw;
            }
            catch
            {
                loggerService.LogError("Some exception has occured whilereading data from a NetworkStream.");
                throw new InvalidOperationException("Some exception has occured while reading data from a NetworkStream.");
            }
        }

        #endregion
    }
}
