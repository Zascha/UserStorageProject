namespace UserStorage.Exceptions
{
    #region Usings
        using System;
    #endregion

    /// <summary>
    /// This exception is thrown if a SlaveService object has not recieved any data from any MasterService yet
    /// </summary>
    public class NoDataFromMasterServiceWasRecievedException : Exception
    {
        public NoDataFromMasterServiceWasRecievedException()
        {
        }

        public NoDataFromMasterServiceWasRecievedException(string message)
            : base(message)
        {
        }

        public NoDataFromMasterServiceWasRecievedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
