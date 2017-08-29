namespace UserStorage.Services.TCPServices
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization;
    using System.ComponentModel;

    using UserStorage.Services;

    #endregion
    
    [Serializable]
    public class TCPServerService<T> : MarshalByRefObject
    {
        #region Fields

        private LoggingService loggerService;

        private TcpListener server;

        private BackgroundWorker backgroundWorker;

        private List<TcpClient> clients;

        #endregion

        #region Constructors & Finalizator

        protected TCPServerService()
        {
            loggerService = LoggingService.Service;
            server = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000));
            backgroundWorker = new BackgroundWorker();
            clients = new List<TcpClient>();

            server.Start();

            backgroundWorker.DoWork += ListenClients;

            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }
                
        ~TCPServerService()
        {
            if (server != null)
            {
                server.Stop();
            }
        }

        #endregion

        #region Public methods
        
        protected void WriteDataToNetworkStream(IEnumerable<T> data)
        {
            foreach (var client in clients)
            {
                BinaryFormatter formatter = new BinaryFormatter();

                try 
                {
                    formatter.Serialize(client.GetStream(), data);
                }
                catch (SerializationException) 
                {
                    loggerService.LogError("SerializationException has occured while writing data to a NetworkStream.");
                    throw;
                }
                catch
                {
                    loggerService.LogError("Some exception has occured while writing data to a NetworkStream.");
                    throw new InvalidOperationException("Some exception has occured while writing data to a NetworkStream.");
                }
            }
        }
        
        #endregion

        #region Private methods

        private void ListenClients(object sender, EventArgs e)
        {
            while (true)
            {
                clients.Add(server.AcceptTcpClient());
            }
        }

        #endregion
    }
}
