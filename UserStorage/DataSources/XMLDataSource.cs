namespace UserStorage.DataSources
{
    #region Usings

    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Linq;
    using System.Configuration;

    using UserStorage.Entities;
    using UserStorage.Interfaces;
    using UserStorage.Services;

    #endregion
    
    [Serializable]
    public class XMLDataSource : IDataSource<User>
    {
        #region Fields

        private LoggingService loggerService;

        private readonly string filePath;

        #endregion
        
        #region Conctructors

        public XMLDataSource()
            : this(ConfigurationManager.AppSettings["XMLFilePath"])
        { }

        public XMLDataSource(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Null or empty file path string.");
            }

            loggerService = LoggingService.Service;
            this.filePath = filePath;
        }

        #endregion

        #region Public methods

        public void WriteDataFrom(IDataStorage<User> dataStorage)
        {
            if (dataStorage == null)
            {
                throw new ArgumentNullException("Null dataStorage object.");
            }

            loggerService.LogInfo("Starting writing data to an XML file...");
            
            try
            {
                using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    new DataContractSerializer(dataStorage.GetType()).WriteObject(fs, dataStorage);
                }

                loggerService.LogInfo("Successfully finished writing data to an XML file...");
            }
            catch
            {
                loggerService.LogError("Some exception has occured while writing to an XML file.");
                throw;
            }
        }

        public IDataStorage<User> ReadDataTo(IDataStorage<User> dataStorage)
        {
            if (dataStorage == null)
            {
                throw new ArgumentNullException("Null dataStorage object.");
            }

            loggerService.LogInfo("Starting reading data from an XML file...");

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                {
                    if (new FileInfo(filePath).Length == 0)
                    {  
                        return Activator.CreateInstance(Type.GetType(dataStorage.GetType().ToString())) as IDataStorage<User>;
                    }

                    using(XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
                    {
                        return (IDataStorage<User>)new DataContractSerializer(dataStorage.GetType()).ReadObject(reader, true);
                    }
                }
            }
            catch
            {
                loggerService.LogError("Some exception has occured while reading from an XML file.");
                throw;
            }
        }

        #endregion

        #region Private methods

        private void CreateAnEmptyXMLDocument()
        {
            File.Create(filePath).Close();

            new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("storage", String.Empty))
            .Save(filePath);
        }

        #endregion
    }
}
