namespace Logger
{
    #region Usings

    using NLog;

    #endregion

    public class NLogger : ILogger
    {
        #region Fields & Properties

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public methods

        public void LogTrace(string message)
        {
            logger.Trace(message);
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogFatal(string message)
        {
            logger.Fatal(message);
        }

        #endregion
    }
}
