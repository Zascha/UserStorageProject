namespace UserStorage.Services.UserServices
{
    #region Usings

    using System;
    using System.Collections.Generic;

    using UserStorage.Entities;

    #endregion
    
    /// <summary>
    /// Singleton
    /// </summary>
    [Serializable]
    internal class NotificatorUserService
    {
        #region Fields & Properties

        private static readonly Lazy<NotificatorUserService> service = new Lazy<NotificatorUserService>(() => new NotificatorUserService());

        internal static NotificatorUserService Service { get { return service.Value; } }
        
        #endregion

        #region Constructors

        private NotificatorUserService()  { }

        #endregion

        #region Public methods, delegates & events

        internal delegate IEnumerable<User> DataChangedHandler();
        internal delegate void SlaveCreationHandler();

        internal event DataChangedHandler StorageChangeEvent;
        internal event SlaveCreationHandler SlaveCreationEvent;

        internal IEnumerable<User> OnStorageChange()
        {
            if (StorageChangeEvent != null)
            {
                return StorageChangeEvent();
            }

            return null;
        }

        internal void OnSlaveCreation()
        {
            if (SlaveCreationEvent != null)
            {
                SlaveCreationEvent();
            }
        }

        #endregion
    }
}
