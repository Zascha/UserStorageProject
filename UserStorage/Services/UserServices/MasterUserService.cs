namespace UserStorage.Services.UserServices
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using UserStorage.Services;
    using UserStorage.Services.TCPServices;
    using UserStorage.Storages;
    using UserStorage.DataSources;
    using UserStorage.Entities;
    using UserStorage.Interfaces;

    using Logger;

    #endregion

    public class MasterUserService : TCPServerService<User>, IDataService<User>
    {
        #region Fields

        private NotificatorUserService notificationService;

        private LoggingService loggerService;

        private IDataSource<User> dataSource;

        private IDataStorage<User> dataStorage;

        private static int mastersQuantity;

        #endregion

        #region Constructors

        public MasterUserService() 
            : this(new XMLDataSource(), new UserStorageHashSet(), new NLogger()) 
        { }

        public MasterUserService(IDataSource<User> dataSource)
            : this(dataSource, new UserStorageHashSet(), new NLogger()) 
        { }

        public MasterUserService(IDataSource<User> dataSource, ILogger logger)
            : this(dataSource, new UserStorageHashSet(), logger)
        { }

        public MasterUserService(IDataSource<User> dataSource, IDataStorage<User> dataStorage, ILogger logger)
        {
            if (dataSource == null || dataStorage == null)
            {
                loggerService.LogError("Data source or storage is null.");
                throw new InvalidOperationException("Data source or storage is null.");
            }

            if (logger == null)
            {
                loggerService.LogError("Logger is null.");
                throw new InvalidOperationException("Logger is null.");
            }

            LoggingService.SetLogger(logger);
            loggerService = LoggingService.Service;
            notificationService = NotificatorUserService.Service;            

            this.dataSource = dataSource;
            this.dataStorage = dataStorage;

            string masterName = String.Format("Master{0}", ++mastersQuantity);
            AppDomain masterDomain = AppDomain.CreateDomain("Domain" + masterName);
            masterDomain.SetData(masterName, this);

            notificationService.SlaveCreationEvent += SendDataForACreatedSlave;
        }
        
        #endregion
        
        #region Public methods
        
        public void Add(User obj)
        {
            loggerService.LogInfo("Starting adding a user for a storage...");

            if (obj == null)
            {
                loggerService.LogError("The passed user object is null.");
                throw new ArgumentNullException("The passed user object is null.");
            }

            if (!obj.IsValid)
            {
                loggerService.LogError("Passed User object is nor valid.");
                throw new ArgumentException("Passed User object is nor valid.");
            }

            CheckIfDataStorageIsFilled();

            try
            {
                obj.SetUniqueId();
                dataStorage.Add(obj);
                loggerService.LogInfo("Successfully finished adding a user for a storage...");
                NotifySlavesThatDataHasBeenChanged();
            }
            catch
            {
                throw;
            }
        }

        public void Remove(int id)
        {
            loggerService.LogInfo("Starting removing a user for a storage...");

            if (id < 0)
            {
                loggerService.LogError("Passed User's id is negative.");
                throw new ArgumentException("Passed User's id is negative.");
            }

            CheckIfDataStorageIsFilled();

            try
            {
                dataStorage.Remove(id);
                loggerService.LogInfo("Successfully finished removing a user for a storage...");
                NotifySlavesThatDataHasBeenChanged();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<User> SelectAll()
        {
            CheckIfDataStorageIsFilled();

            try
            {
                return dataStorage.GetAll();
            }
            catch
            {
                throw;
            }
        }

        public User SelectById(int id)
        {
            if (id < 0)
            {
                loggerService.LogError("Passed User's id is negative while searching by id procedure.");
                throw new ArgumentException("Passed User's id is negative.");
            }

            CheckIfDataStorageIsFilled();

            try
            {
                return dataStorage.GetById(id);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<User> SelectByPredicate(Expression<Func<User, bool>> predicate)
        {
            if (predicate == null)
            {
                loggerService.LogError("Passed predicate is null while searching procedure.");
                throw new InvalidOperationException("Passed predicate is null.");
            }

            CheckIfDataStorageIsFilled();

            try
            {
                return dataStorage.GetByPredicate(predicate);
            }
            catch
            {
                throw;
            }
        }

        public void SaveChanges()
        {
            loggerService.LogInfo("Starting saving data from the storage to the data source...");

            if (dataSource == null || dataStorage == null)
            {
                loggerService.LogError("Data source or storage is null.");
                throw new InvalidOperationException("Data source or storage is null");
            }

            if (!dataStorage.IsDataLoaded)
            {
                loggerService.LogError("Trying to save not loaded storage. This can cause data loss.");
                throw new InvalidOperationException("Trying to save not loaded storage. This can cause data loss.");
            }

            try
            {
                dataSource.WriteDataFrom(dataStorage);
                loggerService.LogInfo("Successfully finished to saving data from the storage to the data source.");
            }
            catch
            {
                throw;
            }
        }

        public void Restore()
        {
            loggerService.LogInfo("Starting restoring data from the storage to the data source...");

            if (dataSource == null || dataStorage == null)
            {
                loggerService.LogError("Data source or storage is null");
                throw new InvalidOperationException("Data source or storage is null");
            }

            try
            {
                FillDataStorage();
                loggerService.LogInfo("Successfully finished restoring data from the storage to the data source...");
            }
            catch
            {
                throw;
            }
        }
        
        #endregion

        #region Private methods

        private void FillDataStorage()
        {
            try
            {
                dataStorage = dataSource.ReadDataTo(dataStorage);
                dataStorage.IsDataLoaded = true;
            }
            catch
            {
                throw;
            }
        }

        private void CheckIfDataStorageIsFilled()
        {
            if (!dataStorage.IsDataLoaded)
            {
                FillDataStorage();
            }
        }

        private void NotifySlavesThatDataHasBeenChanged()
        {
            WriteDataToNetworkStream(SelectAll());
            notificationService.OnStorageChange();
        }

        private void SendDataForACreatedSlave()
        {
            WriteDataToNetworkStream(SelectAll());
        }

        #endregion
    }
}
