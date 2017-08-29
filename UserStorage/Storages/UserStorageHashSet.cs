namespace UserStorage.Storages
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;

    using UserStorage.Entities;
    using UserStorage.Interfaces;
    using UserStorage.Services;

    #endregion

    [Serializable]
    [DataContract(Name = "Storage")]
    public class UserStorageHashSet : IDataStorage<User>
    {
        #region Fields & Properties

        [DataMember]
        private HashSet<User> storage;

        private LoggingService loggerService;

        public bool IsDataLoaded { get; set; }

        #endregion

        #region Constructors

        public UserStorageHashSet()
        {
            loggerService = LoggingService.Service;
            storage = new HashSet<User>();
        }

        #endregion
                
        #region Public methods

        public void Add(User obj)
        {
            if (obj == null)
            {
                loggerService.LogError("Null user object was padded to be added to storage.");
                throw new ArgumentNullException("Null passed user object.");
            }

            if (!obj.IsValid)
            {
                loggerService.LogError("Invalid user object was padded to be added to storage.");
                throw new ArgumentException("The passed user object is invalid.");
            }

            if (!storage.Add(obj))
            {
                loggerService.LogError("Trying to add a user who is already in the storage");
                throw new InvalidOperationException("Trying to add a user who is already in the storage");
            }
        }

        public void Remove(int id)
        {
            if (id < 0)
            {
                loggerService.LogError("Passed User's id is negative.");
                throw new ArgumentException("Passed User's id is negative.");
            }

            if (!storage.Remove(GetById(id)))
            {
                loggerService.LogError("Can not remove the user with such an id from the storage");
                throw new InvalidOperationException("Can not remove the user with such an id from the storage");
            }
        }

        public IEnumerable<User> GetAll()
        {
            return storage.Select(u => u).ToList();
        }

        public User GetById(int id)
        {
            if (id < 0)
            {
                loggerService.LogError("Passed User's id is negative.");
                throw new ArgumentException("Passed User's id is negative.");
            }

            var result = storage.FirstOrDefault(u => u.Id == id);

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

        public IEnumerable<User> GetByPredicate(Expression<Func<User, bool>> predicate)
        {
            if (predicate == null)
            {
                loggerService.LogError("Passed predicate is null while searching procedure.");
                throw new InvalidOperationException("Passed predicate is null.");
            }

            var result = storage.Select(predicate.Compile()) as IEnumerable<User>;

            if (result == null)
            {
                loggerService.LogError("Can not find users by the passed predicate.");
                throw new InvalidOperationException("Can not find users by the passed predicate.");
            }
            else
            {
                return result;
            }
        }

        #endregion
    }
}
