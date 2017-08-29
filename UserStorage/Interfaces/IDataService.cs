namespace UserStorage.Interfaces
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    #endregion

    public interface IDataService<T>
    {
        IEnumerable<T> SelectAll();

        T SelectById(int id);

        IEnumerable<T> SelectByPredicate(Expression<Func<T, bool>> predicate);
    }
}




