namespace UserStorage.Interfaces
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    #endregion

    public interface IDataStorage<T>
    {
        bool IsDataLoaded { get; set; }

        void Add(T obj);

        void Remove(int id);

        IEnumerable<T> GetAll();

        T GetById(int id);

        IEnumerable<T> GetByPredicate(Expression<Func<T, bool>> predicate);
    }
}
