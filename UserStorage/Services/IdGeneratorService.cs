namespace UserStorage.Services
{
    #region Usings

    using System;

    using UserStorage.Entities;

    #endregion
    
    internal static class IdGeneratorService
    {
        #region Fields

        private static LoggingService loggerService = LoggingService.Service;

        #endregion

        #region Public methods

        internal static void SetUniqueId(this User user)
        {
            loggerService.LogInfo("Starting generating id for a passed user...");

            if (user == null)
            {
                loggerService.LogError("Trying to set id for a null user object.");
                throw new ArgumentNullException("Trying to set id for a null user object.");
            }

            if (!user.IsValid)
            {
                loggerService.LogError("Trying to set id for an invalid user object.");
                throw new ArgumentException("Trying to set id for an invalid user object.");
            }

            var id = user.ToString().GetHashCode();

            user.Id = id < 0 ? -id : id;

            loggerService.LogInfo("Successfully finished generating id for a passed user.");
        }

        #endregion
    }
}
