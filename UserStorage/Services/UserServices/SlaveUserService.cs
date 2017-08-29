namespace UserStorage.Services.UserServices
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using UserStorage.Services;
    using UserStorage.Services.TCPServices;
    using UserStorage.Entities;
    using UserStorage.Interfaces;
    using UserStorage.Exceptions;

    using Logger;

    #endregion

    [Serializable]
    public class SlaveUserService : TCPClientService<User>, IDataService<User>
    {
        #region Fields & Properties

        private NotificatorUserService notificationService;

        private LoggingService loggerService;

        public IEnumerable<User> data;
        
        private static int slavesQuantity;

        #endregion

        #region Constructors

        public SlaveUserService() 
            : this(new NLogger())
        { }   
     
        public SlaveUserService(ILogger logger)
        {
            if (logger == null)
            {
                loggerService.LogError("Logger is null.");
                throw new InvalidOperationException("Logger is null.");
            }

            LoggingService.SetLogger(logger);
            loggerService = LoggingService.Service;
            notificationService = NotificatorUserService.Service;
            

            string slaveName = String.Format("Slave{0}", ++slavesQuantity);
            AppDomain slaveDomain = AppDomain.CreateDomain("Domain" + slaveName);
            slaveDomain.SetData(slaveName, this);

            notificationService.StorageChangeEvent += ReadDataFromNetworkStream;
            notificationService.OnSlaveCreation();
            data = ReadDataFromNetworkStream();
        }

        #endregion

        #region Public methods
        
        public IEnumerable<User> SelectAll()
        {
            CheckIfTemporaryStorageIsFilled();

            return data;
        }

        public User SelectById(int id)
        {
            if (id < 0)
            {
                loggerService.LogError("Passed User's id is negative while searching by id procedure.");
                throw new ArgumentException("Passed User's id is negative.");
            }

            CheckIfTemporaryStorageIsFilled();

            var result = data.Where(user => user.Id == id).SingleOrDefault();

            if (result == null)
            {
                loggerService.LogError("Can not find a user with such an id");
                throw new InvalidOperationException("Can not find a user with such an id");
            }
            else
            {
                return result;
            }
        }

        public IEnumerable<User> SelectByPredicate(Expression<Func<User, bool>> predicate)
        {
            if (predicate == null)
            {
                loggerService.LogError("Passed predicate is null while searching by id procedure.");
                throw new InvalidOperationException("Passed predicate is null.");
            }

            CheckIfTemporaryStorageIsFilled();

            var result = data.Select(predicate.Compile()) as IEnumerable<User>;

            if (result == null)
            {
                loggerService.LogError("Can not find users with the passed predicate.");
                throw new InvalidOperationException("Can not find users with the passed predicate.");
            }
            else
            {
                return result;
            }
        }

        #endregion

        #region Private methods

        private void CheckIfTemporaryStorageIsFilled()
        {
            if (data == null)
            {
                loggerService.LogError("No data has been received from any MasterService yet.");
                throw new NoDataFromMasterServiceWasRecievedException("No data has been received from any MasterService yet.");
            }
        }

        #endregion
    }
}
