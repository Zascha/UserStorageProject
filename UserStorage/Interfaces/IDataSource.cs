namespace UserStorage.Interfaces
{
    public interface IDataSource<T>
    {
        void WriteDataFrom(IDataStorage<T> dataStorage);

        IDataStorage<T> ReadDataTo(IDataStorage<T> dataStorage);
    }
}
