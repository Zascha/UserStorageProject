namespace UserStorage.Services
{
    #region Usings

    using System;
    using System.Diagnostics;

    using Logger;

    #endregion
    
    /// <summary>
    /// Singleton logger class.
    /// The default logger is nLogger.
    /// The using logger can be set by SetLogger method.
    /// </summary>
    [Serializable]
    internal sealed class LoggingService
    {
        #region Fields & Properties

        private static readonly Lazy<LoggingService> service = new Lazy<LoggingService>(() => new LoggingService());

        private static BooleanSwitch boolSwitch;
        
        private static ILogger logger;

        public static LoggingService Service { get { return service.Value; } }

        #endregion

        #region Conctructors

        private LoggingService() 
        {
            boolSwitch = new BooleanSwitch("loggingSwitch", "A switch for logging defined in the app.config file");
            logger = new NLogger();
        }

        #endregion

        #region Public methods

        internal void LogInfo(string message)
        {
            if (boolSwitch.Enabled)
            {
                logger.LogInfo(message);
            }
        }

        internal void LogError(string message)
        {
            if (boolSwitch.Enabled)
            {
                logger.LogError(message);
            }
        }

        internal static void SetLogger(ILogger loggerToSet)
        {
            if (logger == null)
            {
                logger = loggerToSet;
            }
        }

        #endregion
    }
}
